import { AuthConfig } from "angular-oauth2-oidc";

export interface EnvironmentConfig {
    appVersion: string;
    production: boolean;
    moralis: MoralisConfig;
    gatewayUrl: string;
    transak: any;
    ckEditorLicenseKey: string;
    beamerProductKey: string;
    rampApiKey: string;
    signalrConnectionType: 'longPolling' | 'webSockets';
    kycProvider: 'BASISID' | 'PASSBASE';
    basisIdKey: string;
    passbaseKey: string;
    auth: AuthConfig;
    relayUrl: string;
}

export interface MoralisConfig {
    appId: string;
    serverUrl: string;
}
