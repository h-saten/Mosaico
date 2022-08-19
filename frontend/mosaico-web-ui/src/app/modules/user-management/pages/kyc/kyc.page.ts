import { selectUserInformation, setAccountVerified } from '../../store';
import { DOCUMENT } from '@angular/common';
import { AfterContentInit, AfterViewInit, Component, ElementRef, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services';
import { SubSink } from 'subsink';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ConfigService, ErrorHandlingService } from 'mosaico-base';
import { UserInformation } from '../../models';
import { take } from 'rxjs';
import Passbase from "@passbase/button";

@Component({
    selector: 'app-kyc',
    templateUrl: './kyc.page.html',
})
export class KYCPage implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild("passbaseButton", { static: false }) passbaseButton: ElementRef;

    passbaseOrg = 'mosaico-e540f142';

    passbaseLink = '';
    showButton = false;
    window;
    isLoaded = false;
    private subs = new SubSink();
    kycProvider: string;
    passBaseApiKey: string;
    basisIdApiKey: string;
    user: UserInformation;
    redirectUrl: string;
    canMakeKyc = false;

    ngOnDestroy(): void {
        this.subs.unsubscribe();
    }

    constructor(@Inject(DOCUMENT) private document: Document, private toastr: ToastrService, private route: ActivatedRoute,
        private userService: UserService, private store: Store, private router: Router, config: ConfigService, private errorHandler: ErrorHandlingService,
        private languageService: TranslateService) {
        this.window = this.document.defaultView;
        this.kycProvider = config.getConfig()?.kycProvider;
        this.passBaseApiKey = config.getConfig()?.passbaseKey;
        this.basisIdApiKey = config.getConfig()?.basisIdKey;
    }

    ngAfterViewInit(): void {
        this.subs.sink = this.userService.getKycStatus().subscribe((s) => {
            if (s?.data === 'Pending') {
                this.languageService.get('KYC.ALREADY_IN_PROGRESS').subscribe((t) => {
                    this.toastr.error(t);
                });
                this.router.navigateByUrl(this.redirectUrl);
                this.canMakeKyc = false;
                return;
            }
            else {
                this.canMakeKyc = true;
                this.subs.sink = this.store.select(selectUserInformation).pipe(take(1)).subscribe((usr) => {
                    if (usr && usr.id && usr.id.length > 0) {
                        this.user = usr;
                        if (!this.isLoaded && this.canMakeKyc) {
                            if (this.kycProvider === 'PASSBASE') {
                                try {
                                    this.loadPassbaseWidget();
                                    this.isLoaded = true;
                                }
                                catch (error) {
                                    setTimeout(() => {
                                        if (!this.isLoaded) {
                                            this.loadPassbaseWidget();
                                            this.isLoaded = true;
                                        }
                                    }, 1000);
                                }
                            }
                        }
                    }
                });
            }
        });
    }

    ngOnInit(): void {
        this.redirectUrl = this.route.snapshot.queryParamMap.get('redirectUrl');
        if (!this.redirectUrl || this.redirectUrl.length === 0) {
            this.redirectUrl = '/user/profile';
        }

    }

    loadPassbaseWidget(): void {
        let self = this;
        let element = this.passbaseButton.nativeElement;
        try {
            this.createPassbaseLink();
            setTimeout(() => {
                this.isLoaded = true;
                this.showButton = true;
            }, 5000);
            Passbase.renderButton(
                element,
                this.passBaseApiKey,
                {
                    hidden: true,
                    onStart: () => {
                        self.isLoaded = true;
                    },
                    onSubmitted: (identityAccessKey) => {
                        self.subs.sink = self.userService.initKyc({ id: identityAccessKey, provider: "PASSBASE" }).subscribe((res) => {
                            if (res) {
                                self.toastr.success("Thank you for completing the application. You will get notified as soon as verification will be completed");
                                self.router.navigateByUrl(this.redirectUrl);
                            }
                        });
                    },
                    onError: (errorCode) => { self.toastr.error(self.languageService.instant(errorCode)); self.router.navigateByUrl(this.redirectUrl); },
                    prefillAttributes: {
                        email: this.user?.email,
                        country: this.user?.country
                    }
                }
            );
        }
        catch (error) {
            console.log(error);
        }

        setTimeout(() => {
            try {
                Passbase.start();
            }
            catch (error) {
                console.log(error);
            }
        }, 500);
    }

    private createPassbaseLink(): void {
        const prefillAttributes = {
            prefill_attributes: {
                email: this.user?.email,
                country: this.user?.country
            }
        };
        const objJsonStr = JSON.stringify(prefillAttributes);
        const objJsonB64 = Buffer.from(objJsonStr).toString("base64");
        this.passbaseLink = "https://verify.passbase.com/" + this.passbaseOrg + "/?p=" + objJsonB64;
    }

    public redirect(): void {
        this.router.navigateByUrl(this.redirectUrl);
    }
}