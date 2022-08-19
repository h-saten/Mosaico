import { CCQuoteResponse, OrdersService, ExchangeRate, PaymentCurrency, TokenBalance } from 'mosaico-wallet';
import { BehaviorSubject, Subscription, zip } from 'rxjs';
import { UserService } from '../../../user-management/services/user.service';
import { Stage } from 'mosaico-project';
import { SubSink } from 'subsink';
export type PaymentMethodType = 'MOSAICO_WALLET' | 'KANGA_EXCHANGE' | 'BANK_TRANSFER' | 'CREDIT_CARD' | 'METAMASK' | 'BINANCE';

export interface PaymentServiceSettings {
    projectId: string;
    presetTokenAmount: number;
    userId: string;
    stage: Stage;
    decimals?: number;
    [key: string]: any;
}

export abstract class Cleanable {
    protected subs = new SubSink();

    public set s(value: Subscription) {
        this.subs.add(value);
    }

    public destroy(): void {
        this.subs.unsubscribe();
    }
}

export abstract class QuoteAccessor extends Cleanable {
    public quote: CCQuoteResponse;

    public get rates(): ExchangeRate[] {
        return this.quote?.exchangeRates;
    }

    public get minimumPurchase(): number {
        return this.quote?.minimumPurchase;
    }

    public get maximumPurchase(): number {
        return this.quote?.maximumPurchase;
    }

    public get companyName(): string {
        return this.quote?.companyName;
    }

    public get projectName(): string {
        return this.quote?.projectName;
    }

    public get policyUrl(): string {
        return this.quote?.privacyPolicyUrl;
    }

    public get regulationUrl(): string {
        return this.quote?.regulationsUrl;
    }

    public get paymentCurrencies(): PaymentCurrency[] {
        return this.quote?.currencies;
    }

    public get currentBalances(): TokenBalance[] {
        return this.quote?.paymentCurrencyBalances;
    }

    protected async setQuoteAsync(q: CCQuoteResponse): Promise<void> {
        this.quote = q;
    }
}

export abstract class PaymentService extends QuoteAccessor {
    protected isLoading$ = new BehaviorSubject<boolean>(false);
    protected isDataLoaded$ = new BehaviorSubject<boolean>(false);
    protected settings$ = new BehaviorSubject<PaymentServiceSettings>(null);

    protected tokenAmount$ = new BehaviorSubject<number>(0);
    protected paymentAmount$ = new BehaviorSubject<number>(0);
    protected tokenPrice$ = new BehaviorSubject<number>(0);
    protected currency$ = new BehaviorSubject<string>('USD');
    protected isAMLCompleted$ = new BehaviorSubject<boolean>(false);
    protected isPurchaseInvalid = false;

    public onCurrencyChanged = new BehaviorSubject<string>(null);
    public onTokenAmountChanged = new BehaviorSubject<number>(null);
    public onPaymentAmountChanged = new BehaviorSubject<number>(null);
    public onDataLoaded = new BehaviorSubject<boolean>(false);
    public onValidationFailed = new BehaviorSubject<any>(null);
    public onValidationSucceeded = new BehaviorSubject<any>(null);
    public onQuoteChanged = new BehaviorSubject<CCQuoteResponse>(null);

    constructor(public paymentMethod: PaymentMethodType, protected orderService: OrdersService,
        protected userService: UserService) {
        super();
    }

    protected async getInitialDataAsync(): Promise<void> {
        const quoteRequest = this.orderService.getQuote(this.settings.projectId, this.paymentMethod);
        const userRequest = this.userService.getUser(this.userId);
        const responses = await zip(quoteRequest, userRequest)
            .toPromise();
        await this.setQuoteAsync(responses[0]?.data);
        this.onQuoteChanged.next(responses[0]?.data);
        this.currency$.next(this.paymentCurrencies[0]?.ticker);
        this.isAMLCompleted$.next(responses[1]?.data?.isAMLVerified);
        this.isDataLoaded$.next(true);
    }

    protected recalculatePaymentAmount(): void {
        const rate = this.rates.find((r) => r.currency === this.currency$.value);
        if(rate?.exchangeRate > 0){
            const val = this.round(this.tokenAmount * rate?.exchangeRate);
            if(val !== this.paymentAmount$.value) {
                this.paymentAmount$.next(val);
                this.onPaymentAmountChanged.next(this.paymentAmount$.value);
            }
        }
    }

    protected recalculateTokenPrice(): void {
        const val = this.round(this.stage.tokenPrice);
        this.tokenPrice$.next(val);
    }

    protected recalculateTokenAmount(): void {
        if(this.tokenPrice$.value > 0){
            const rate = this.rates.find((r) => r.currency === this.currency$.value);
            if(rate?.exchangeRate > 0){
                const val = this.round(this.paymentAmount$.value / rate?.exchangeRate);
                if(val !== this.tokenAmount$.value){
                    this.tokenAmount$.next(val);
                }
            }
        }
    }

    public set settings(settings: PaymentServiceSettings) {
        this.settings$.next(settings);
    }

    public get settings(): PaymentServiceSettings {
        return this.settings$.value;
    }

    public set tokenAmount(value: number) {
        this.tokenAmount$.next(this.round(value));
    }

    public get tokenAmount(): number {
        return this.tokenAmount$.value;
    }

    public get tokenPrice(): number {
        return this.tokenPrice$.value;
    }

    public set tokenPrice(value: number) {
        this.tokenPrice$.next(this.round(value));
    }

    public get stage(): Stage {
        return this.settings?.stage;
    }

    public get isAMLCompleted(): boolean {
        return this.isAMLCompleted$.value;
    }

    public get userId(): string {
        return this.settings?.userId;
    }

    public get projectId(): string {
        return this.settings?.projectId;
    }

    public set currency(value: string) {
        this.currency$.next(value);
    }

    public get currency(): string {
        return this.currency$.value;
    }

    public get isValid(): boolean {
        return !this.isPurchaseInvalid;
    }

    public get isLoading(): boolean {
        return this.isLoading$.value;
    }

    public get isDataLoaded(): boolean {
        return this.isDataLoaded$.value;
    }

    public get minimumPurchaseInUSD(): number {
        return this.round(this.minimumPurchase * this.tokenPrice);
    }

    public get currentBalance(): number {
        const balance = this.currentBalances?.find((b) => b.symbol === this.currency$.value);
        return this.round(balance?.balance);
    }

    public get paymentAmount(): number {
        return this.paymentAmount$.value;
    }

    public set paymentAmount(val: number) {
        this.paymentAmount$.next(this.round(val));
        this.recalculateTokenAmount();
        this.onPaymentAmountChanged.next(this.paymentAmount$.value);
    }

    public async initAsync(settings?: PaymentServiceSettings): Promise<void> {
        this.isLoading$.next(true);
        this.settings = settings;
        if(this.settings.decimals <= 0) this.settings = {...this.settings, decimals: 6};

        await this.getInitialDataAsync();

        this.tokenAmount = this.settings?.presetAmount > 0 && this.settings?.presetAmount > this.minimumPurchase ?
            this.settings?.presetAmount : this.minimumPurchase;

        this.recalculateTokenPrice();
        this.recalculatePaymentAmount();

        this.s = this.tokenAmount$.subscribe((t) => {
            this.recalculatePaymentAmount();
            this.onTokenAmountChanged.next(t);
        });
        this.s = this.currency$.subscribe((c) => {
            this.recalculatePaymentAmount();
            this.onCurrencyChanged.next(c);
        });

        this.isLoading$.next(false);

        this.s = this.onValidationFailed.subscribe((v) => {
            this.isPurchaseInvalid = true;
        });
        this.s = this.onValidationSucceeded.subscribe((v) => {
            this.isPurchaseInvalid = false;
        });
        this.s = this.isDataLoaded$.subscribe((d) => {
            if(d === true) {
                this.onDataLoaded.next(true);
            }
        });
        this.s = this.settings$.subscribe((s) => {
            if(s) {
                this.recalculateTokenPrice();
                this.recalculatePaymentAmount();
            }
        });
    }

    public abstract getValidationPayload(payload: any): any;

    public async validateAsync(): Promise<boolean> {
        const validationPayload = this.getValidationPayload({
            tokenAmount: this.tokenAmount,
            currency: this.currency$.value,
            payedAmount: this.paymentAmount$.value,
            paymentMethod: this.paymentMethod
        });

        try {
            const response = await this.orderService.validate(this.projectId, validationPayload).toPromise();
            if (response?.data?.status === 'OK') {
                this.onValidationSucceeded.next(response?.data);
                return true;
            }
            if (response?.data?.status !== 'OK') {
                this.onValidationFailed.next(response?.data);
                return false;
            }
        }
        catch (error) {
            return false;
        }
    }

    private round(val?: number): number{
        return val;
    }
}

export class MosaicoPaymentService extends PaymentService {
    public getValidationPayload(payload: any) {
        return payload;
    }
}