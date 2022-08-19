import * as signalR from "@microsoft/signalr";
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './config.service';
import { AuthService } from "./auth.service";

export abstract class HubServiceBase {
    protected hubConnection: signalR.HubConnection;
    protected baseUrl = '';
    protected connectionType: string;

    constructor(protected http: HttpClient, configService: ConfigService, private authService: AuthService) {
        this.baseUrl = configService.getConfig().gatewayUrl;
        this.connectionType = configService.getConfig().signalrConnectionType;
    }

    public startConnection(): void {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${this.baseUrl}/core/hubs/${this.getHubName()}`, {
                accessTokenFactory: () => this.authService.getAccessToken(),
                transport: this.connectionType === 'webSockets' ? signalR.HttpTransportType.WebSockets : signalR.HttpTransportType.LongPolling,
                withCredentials: true
            })
            .build();
        this.hubConnection
            .start()
            .then(() => console.log('Connection started'))
            .catch(err => console.log('Error while starting connection: ' + err));
    }

    public async startConnectionAsync(): Promise<void> {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${this.baseUrl}/core/hubs/${this.getHubName()}`, {
                accessTokenFactory: () => this.authService.getAccessToken(),
                transport: this.connectionType === 'webSockets' ? signalR.HttpTransportType.WebSockets : signalR.HttpTransportType.LongPolling,
                withCredentials: true
            })
            .build();
        try{
            await this.hubConnection.start();
            console.log('Connection started');
        }
        catch(err) {
            console.log('Error while starting connection: ' + err);
        }
    }

    abstract getHubName(): string;

    public removeListener(): void {
        if(this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
            this.hubConnection.stop();
        }
    }

    public async removeListenerAsync(): Promise<void> {
        if(this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
            await this.hubConnection.stop();
        }
    }
}