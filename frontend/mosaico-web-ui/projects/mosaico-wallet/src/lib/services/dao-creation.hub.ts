import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuthService, ConfigService, HubServiceBase } from 'mosaico-base';

export interface CompanyCreated {
    companyId: string;
    slug: string;
}

@Injectable({
    providedIn: 'root'
})
export class DaoCreationHubService extends HubServiceBase {

    public created$ = new BehaviorSubject<CompanyCreated>(null);
    public failed$ = new BehaviorSubject<string>(null);
    public proposalCreated$ = new BehaviorSubject<string>(null);
    public proposalCreationFailed$ = new BehaviorSubject<string>(null);
    public voteSubmitted$ = new BehaviorSubject<string>(null);
    public voteFailed$ = new BehaviorSubject<string>(null);
    public vaultDeployed$ = new BehaviorSubject<boolean>(false);
    public vaultDeploymentFailed$ = new BehaviorSubject<string>(null);
    public depositCreated$ = new BehaviorSubject<boolean>(false);
    public depositCreationFailed$ = new BehaviorSubject<string>(null);
    public vaultSent$ = new BehaviorSubject<boolean>(false);
    public vaultSendFailed$ = new BehaviorSubject<string>(null);
    public vestingDeployed$ = new BehaviorSubject<boolean>(false);
    public vestingDeploymentFailed$ = new BehaviorSubject<string>(null);

    constructor(http: HttpClient, configService: ConfigService, authClient: AuthService) {
        super(http, configService, authClient);
    }

    getHubName(): string {
        return 'companyCreation';
    }

    public addListener(): void {
        this.hubConnection.on('successfullyCreated', (data: CompanyCreated) => {
            this.created$.next(data);
        });
        this.hubConnection.on('failedToCreate', (data: string) => {
            this.failed$.next(data);
        });
    }

    public addProposalListeners(): void {
        this.hubConnection.on('proposalCreated', (data: string) => {
            this.proposalCreated$.next(data);
        });
        this.hubConnection.on('proposalCreationFailed', (data: string) => {
            this.proposalCreationFailed$.next(data);
        });
    }

    public addVoteListeners(): void {
        this.hubConnection.on('voteSubmitted', (data: string) => {
            this.voteSubmitted$.next(data);
        });
        this.hubConnection.on('voteFailed', (data: string) => {
            this.voteFailed$.next(data);
        });
    }

    public addVaultListeners(): void {
        this.hubConnection.on('vaultDeployed', () => {
            this.vaultDeployed$.next(true);
        });
        this.hubConnection.on('vaultDeploymentFailed', (data: string) => {
            this.vaultDeploymentFailed$.next(data);
        });
    }

    public addDepositListeners(): void {
        this.hubConnection.on('depositCreated', () => {
            this.depositCreated$.next(true);
        });
        this.hubConnection.on('depositCreationFailed', (data: string) => {
            this.depositCreationFailed$.next(data);
        });
    }

    public addVaultSendListeners(): void {
        this.hubConnection.on('vaultSent', () => {
            this.vaultSent$.next(true);
        });
        this.hubConnection.on('vaultSendFailed', (data: string) => {
            this.vaultSendFailed$.next(data);
        });
    }

    public addVestingListeners(): void {
        this.hubConnection.on('vestingDeployed', () => {
            this.vestingDeployed$.next(true);
        });
        this.hubConnection.on('vestingDeploymentFailed', (data: string) => {
            this.vestingDeploymentFailed$.next(data);
        });
    }

    public resetObjects(): void {
        this.created$ = new BehaviorSubject<CompanyCreated>(null);
        this.failed$ = new BehaviorSubject<string>(null);
        this.proposalCreated$ = new BehaviorSubject<string>(null);
        this.proposalCreationFailed$ = new BehaviorSubject<string>(null);
        this.voteSubmitted$ = new BehaviorSubject<string>(null);
        this.voteFailed$ = new BehaviorSubject<string>(null);
        this.vaultDeployed$ = new BehaviorSubject<boolean>(false);
        this.vaultDeploymentFailed$ = new BehaviorSubject<string>(null);
        this.depositCreated$ = new BehaviorSubject<boolean>(false);
        this.depositCreationFailed$ = new BehaviorSubject<string>(null);
        this.vaultSent$ = new BehaviorSubject<boolean>(false);
        this.vaultSendFailed$ = new BehaviorSubject<string>(null);
        this.vestingDeployed$ = new BehaviorSubject<boolean>(false);
        this.vestingDeploymentFailed$ = new BehaviorSubject<string>(null);
    }
}