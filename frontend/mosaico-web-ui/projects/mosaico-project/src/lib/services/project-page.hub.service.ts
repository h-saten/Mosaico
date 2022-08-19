import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuthService, ConfigService, HubServiceBase } from 'mosaico-base';

@Injectable({
    providedIn: 'root'
})
export class ProjectPageHubService extends HubServiceBase{
    public logoUpdated$ = new BehaviorSubject<string>(null);
    public coverUpdated$ = new BehaviorSubject<string>(null);

    getHubName(): string {
        return 'tokenPage';
    }

    constructor(http: HttpClient, configService: ConfigService, authClient: AuthService) {
        super(http, configService, authClient);
    }

    public addListener(): void {
        this.hubConnection.on('updatePageCover', (data: string) => {
            this.coverUpdated$.next(data);
        });
        this.hubConnection.on('updatePageLogo', (data: string) => {
            this.logoUpdated$.next(data);
        });
    }

    public resetObjects(): void {
        this.logoUpdated$ = new BehaviorSubject<string>(null);
        this.coverUpdated$ = new BehaviorSubject<string>(null);
    }
}