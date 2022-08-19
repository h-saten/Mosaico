import { Component, Input, OnDestroy, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { FormBase, validateForm, ErrorHandlingService, trim } from 'mosaico-base';
import { ToastrService } from 'ngx-toastr';
import { SubSink } from 'subsink';
import { Blockchain, CreateTokenCommand, DeploymentEstimateHubService, TokenService, TOKEN_TYPES } from 'mosaico-wallet';

@Component({
  selector: 'app-token-creation',
  templateUrl: './token-creation.component.html',
  styleUrls: ['./token-creation.component.scss']
})
export class TokenCreationComponent extends FormBase implements OnInit, OnDestroy {
  isLoading = false;
  @Input() projectId?: string;
  @Input() companyId: string;
  @Input() selectedNetwork?: string;
  @Output() created: EventEmitter<string> = new EventEmitter<string>();
  @Output() startedSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() stoppedSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() canceled = new EventEmitter<any>();

  step = 1;

  tokenTypes = TOKEN_TYPES;
  subs = new SubSink();
  @Input() networks: Blockchain[] = [];
  
  shouldBlockNetwork = false;

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
    this.subs.sink = this.startedSave.subscribe((started) => {
      this.isLoading = true;
      this.disableForm();
    });
    this.subs.sink = this.stoppedSave.subscribe((stopped) => {
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

  createForm(): void {
    this.form = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]),
      symbol: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(5)]),
      decimals: new FormControl(18, [Validators.required, Validators.min(6), Validators.max(18)]),
      network: new FormControl(this.networks[0]?.name, [Validators.required]),
      initialSupply: new FormControl(null, [Validators.required, Validators.min(1)]),
      tokenType: new FormControl('UTILITY', [Validators.required]),
      isMintable: new FormControl(false),
      isBurnable: new FormControl(false),
      isGovernance: new FormControl(false)
    });
    this.form.get('decimals')?.disable();    
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
      let command = this.form.getRawValue() as CreateTokenCommand;
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

  private async storeToken(command: CreateTokenCommand) {
    try {
      const creationResult = await this.tokenService.createToken(command).toPromise();
      if (creationResult && creationResult.data) {
        this.subs.sink = this.translateService.get('NEW_TOKEN.MESSAGES.SUCCESSFULLY_CREATED').subscribe((res) => {
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
