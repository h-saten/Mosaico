import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DeploymentEstimate } from '../models/deployment-estimate';
import { HttpClient } from '@angular/common/http';
import { AuthService, ConfigService, HubServiceBase } from 'mosaico-base';

@Injectable({
    providedIn: 'root'
})
export class DeploymentEstimateHubService extends HubServiceBase{
    
    getHubName(): string {
        return 'estimates';
    }

    public estimates$ = new BehaviorSubject<DeploymentEstimate[]>(null);

    constructor(http: HttpClient, configService: ConfigService, authClient: AuthService) {
       super(http, configService, authClient);
    }

    public addListener(): void {
        this.hubConnection.on('updateGasEstimate', (data) => {
            this.estimates$.next(data);
        });
    }

    public resetObjects(): void {
        this.estimates$ = new BehaviorSubject<DeploymentEstimate[]>(null);
    }
}