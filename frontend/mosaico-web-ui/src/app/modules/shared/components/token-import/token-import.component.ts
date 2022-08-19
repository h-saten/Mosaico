import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {TranslateService} from '@ngx-translate/core';
import {ErrorHandlingService, FormBase, trim, validateForm} from 'mosaico-base';
import {ToastrService} from 'ngx-toastr';
import {SubSink} from 'subsink';
import {Blockchain, DeploymentEstimateHubService, ImportTokenCommand, TOKEN_TYPES, TokenService} from 'mosaico-wallet';

@Component({
  selector: 'app-token-import',
  templateUrl: './token-import.component.html',
  styleUrls: ['./token-import.component.scss']
})
export class TokenImportComponent extends FormBase implements OnInit, OnDestroy {
  isLoading = false;
  @Input() projectId?: string;
  @Input() companyId: string;
  @Input() selectedNetwork?: string;
  @Output() created: EventEmitter<string> = new EventEmitter<string>();
  @Output() startedSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() stoppedSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() canceled = new EventEmitter<any>();

  step = 2;

  tokenTypes = TOKEN_TYPES;
  subs = new SubSink();
  @Input() networks: Blockchain[] = [];

  shouldBlockNetwork = false;

  blockImport = true;
  tokenDetailsFetched = false;
  importDenied = false;
  importDetailsError = false;

  constructor(private toastrService: ToastrService, private errorHandler: ErrorHandlingService,
    private estimateHub: DeploymentEstimateHubService, private tokenService: TokenService, private translateService: TranslateService) {
      super();
    }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
    this.estimateHub.removeListener();
  }

  ngOnInit(): void {
    this.createForm();
    this.form.controls.contractAddress.valueChanges.subscribe((value: string) => {
      if (this.form.controls.contractAddress.invalid) {
        this.blockImport = true;
        return;
      }
      this.fetchTokenDetails(value);
    });
    this.subs.sink = this.startedSave.subscribe(() => {
      this.isLoading = true;
      this.disableForm();
    });
    this.subs.sink = this.stoppedSave.subscribe(() => {
      this.isLoading = false;
      this.enableForm();
    })
    if(this.selectedNetwork && this.selectedNetwork.length > 0 && this.networks) {
      const network = this.networks.find((n) => n.name === this.selectedNetwork);
      if(network) {
        this.form.get('network').setValue(this.selectedNetwork);
        this.shouldBlockNetwork = true;
        this.form.get('network').disable();
      }
    }

  }

  private fetchTokenDetails(tokenContractAddress: string) {
    this.subs.sink = this.tokenService.getTokenDetails(this.selectedNetwork, tokenContractAddress)
      .subscribe(response => {
        const canImport = response && response.data && response.data.canImport === true;
        if (canImport == true) {
          const tokenDetails = response.data;
          this.form.get('decimals')?.setValue(tokenDetails.decimals);
          this.form.get('name')?.setValue(tokenDetails.name);
          this.form.get('symbol')?.setValue(tokenDetails.symbol);
          this.form.get('initialSupply')?.setValue(tokenDetails.totalSupply);
          this.form.get('isBurnable')?.setValue(tokenDetails.burnable);
          this.form.get('isMintable')?.setValue(tokenDetails.mintable);
          this.blockImport = false;
          this.importDenied = true;
          this.tokenDetailsFetched = true;
        } else {
          this.importDenied = true;
        }
      }, () => {
        this.importDetailsError = true;
      });
  }

  createForm(): void {
   this.form = new FormGroup({
      contractAddress: new FormControl('', [Validators.required, Validators.minLength(42), Validators.maxLength(42)]), // TODO custom contract address validator
      network: new FormControl(this.networks[0]?.name, [Validators.required]),
      name: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
      symbol: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(5)]),
      decimals: new FormControl(18, [Validators.required, Validators.min(6), Validators.max(18)]),
      initialSupply: new FormControl(null, [Validators.required, Validators.min(1)]),
      tokenType: new FormControl('UTILITY', [Validators.required]),
      isMintable: new FormControl(false),
      isBurnable: new FormControl(false),
      isGovernance: new FormControl(false)
    });
    this.disableForm();
    this.form.get('contractAddress')?.enable();
  }

  disableForm(): void {
    this.form.disable();
  }

  enableForm(): void {
    this.form.enable();
    this.form.get('decimals')?.disable();
    if(this.shouldBlockNetwork) {
      this.form.get('network').disable();
    }
  }

  async save(): Promise<void> {
    if (validateForm(this.form)) {
      let command = this.form.getRawValue() as ImportTokenCommand;
      command = trim(command);
      command.projectId = this.projectId;
      command.companyId = this.companyId;
      if(command.tokenType === 'GOVERNANCE') {
        command.tokenType = 'UTILITY';
        command.isGovernance = true;
      }
      this.startedSave.emit();
      await this.storeToken(command);
    }
    else {
      this.subs.sink = this.translateService.get('NEW_TOKEN.MESSAGES.INVALID_FORM').subscribe((res) => {
        this.toastrService.error(res);
      });
    }
  }

  private async storeToken(command: ImportTokenCommand) {
    try {
      const creationResult = await this.tokenService.importToken(command).toPromise();
      if (creationResult && creationResult.data) {
        this.subs.sink = this.translateService.get('IMPORT_TOKEN.MESSAGES.SUCCESSFULLY_CREATED').subscribe((res) => {
          this.toastrService.success(res);
        });
        this.created.emit(creationResult.data);
        this.stoppedSave.emit();
      }
    }
    catch (error) {
      this.errorHandler.handleErrorWithToastr(error);
      this.stoppedSave.emit();
    }
  }

  nextStep(): void {
    this.step++;
  }

  previousStep(): void {
    this.step--;
  }
}
