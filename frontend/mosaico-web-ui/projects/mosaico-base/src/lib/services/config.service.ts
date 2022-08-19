import { Inject, Injectable } from '@angular/core';
import { EnvironmentConfig } from '../';

@Injectable({
    providedIn: 'root'
})
export class ConfigService {

    constructor(@Inject("APP_CONFIGURATION") private appConfig: EnvironmentConfig) {

    }

    public getConfig(): EnvironmentConfig {
        return this.appConfig;
    }
}
