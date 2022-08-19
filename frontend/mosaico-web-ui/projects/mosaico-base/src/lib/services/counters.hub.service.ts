import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './config.service';
import { Counter } from "../models";
import { BehaviorSubject } from "rxjs";
import { HubServiceBase } from "./hub.base.service";
import { AuthService } from "./auth.service";

@Injectable({
    providedIn: 'root'
})
export class CountersHubService extends HubServiceBase {

    getHubName(): string {
        return 'counters';
    }

    public counter$ = new BehaviorSubject<Counter>(null);

    constructor(http: HttpClient, configService: ConfigService, authClient: AuthService) {
        super(http, configService, authClient);
    }

    public addListener() {
        this.hubConnection.on('updateCounters', (data) => {
            this.counter$.next(data);
        });
    }

    public resetObjects(): void {
        this.counter$ = new BehaviorSubject<Counter>(null);
    }
}