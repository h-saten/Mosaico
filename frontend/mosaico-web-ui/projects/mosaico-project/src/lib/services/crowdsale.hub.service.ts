import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuthService, ConfigService, HubServiceBase } from 'mosaico-base';


export interface StageDeploymentFailed {
    stageId: string;
    error: string;
}

@Injectable({
    providedIn: 'root'
})
export class CrowdsaleHubService extends HubServiceBase {
    public deployed$ = new BehaviorSubject<any>(null);
    public deploymentFailed$ = new BehaviorSubject<string>(null);
    public stageDeployed$ = new BehaviorSubject<any>(null);
    public stageDeploymentFailed$ = new BehaviorSubject<StageDeploymentFailed>(null);
    public purchaseSuccessful$ = new BehaviorSubject<string>(null);
    public purchaseFailed$ = new BehaviorSubject<string>(null);

    getHubName(): string {
        return 'crowdsale';
    }
    constructor(http: HttpClient, configService: ConfigService, authClient: AuthService) {
        super(http, configService, authClient);
    }

    public addListener(): void {
        this.hubConnection.on('crowdsaleCreated', (data: any) => {
            this.deployed$.next(data);
        });
        this.hubConnection.on('crowdsaleDeploymentFailed', (data: string) => {
            this.deploymentFailed$.next(data);
        });
    }

    public addStageListeners(): void {
        this.hubConnection.on('stageDeployed', (data: string) => {
            this.stageDeployed$.next(data);
        });
        this.hubConnection.on('stageDeploymentFailed', (data: StageDeploymentFailed) => {
            this.stageDeploymentFailed$.next(data);
        });
    }

    public addPurchaseListener(): void {
        this.hubConnection.on('purchaseSuccessful', (data: string) => {
            this.purchaseSuccessful$.next(data);
        });
        this.hubConnection.on('purchaseFailed', (data: string) => {
            this.purchaseFailed$.next(data);
        });
    }

    public resetObjects(): void {
        this.deployed$ = new BehaviorSubject<any>(null);
        this.deploymentFailed$ = new BehaviorSubject<string>(null);
        this.stageDeployed$ = new BehaviorSubject<any>(null);
        this.stageDeploymentFailed$ = new BehaviorSubject<StageDeploymentFailed>(null);
        this.purchaseSuccessful$ = new BehaviorSubject<string>(null);
        this.purchaseFailed$ = new BehaviorSubject<string>(null);
    }
}
