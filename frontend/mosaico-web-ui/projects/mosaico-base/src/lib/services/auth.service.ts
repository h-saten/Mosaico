import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { AuthConfig, OAuthService } from 'angular-oauth2-oidc';
import { ConfigService } from './config.service';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isAuthenticated$ = new BehaviorSubject<boolean>(false);
  sessionExpired$ = new BehaviorSubject<boolean>(false);
  private interval: NodeJS.Timer;

  constructor(private oauthService: OAuthService, private config: ConfigService, private cookieStorage: CookieService) {
    this.oauthService.events.subscribe((res) => {
      if (res.type === 'token_received') {
        const t = this.getAccessToken();
        this.isAuthenticated$.next(true);
      }
      if (res.type === 'token_refreshed') {
      }
    });
  }

  public async init(): Promise<void> {
    this.oauthService.configure(this.getClientSettings());
    await this.oauthService.loadDiscoveryDocument();
    this.startListeningToSession();
  }

  public async tryLogin(): Promise<void> {
    await this.oauthService.tryLoginCodeFlow();
    if(this.oauthService.getRefreshToken()){
      await this.oauthService.refreshToken();
    }
  }

  public loginWithRedirect(url?: string): void {
    if (url && url.length > 0) {
      localStorage.setItem('LOGIN_REDIRECT_URL', url);
    }
    this.oauthService.initCodeFlow();
  }

  public logout(): void {
    this.oauthService.revokeTokenAndLogout();
    this.cookieStorage.deleteAll();
  }

  public getUserId(): string {
    const claims = this.oauthService.getIdentityClaims();
    return claims['sub'];
  }

  public getAccessToken(): string {
    return this.oauthService.getAccessToken();
  }

  public getIdToken(): string {
    return this.oauthService.getIdToken();
  }

  getClientSettings(): AuthConfig {
    const config = this.config.getConfig();
    return config.auth;
  }

  isAuthenticated(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  public getUserRoles(): string[] {
    const claims = this.oauthService.getIdentityClaims();
    return claims['role'];
  }

  private startListeningToSession(): void {
    this.interval = setInterval(() => {
      const token = this.cookieStorage.get('access_token');
      const expiresAt = this.cookieStorage.get('expires_at');
      if(token && expiresAt) {
        const now = new Date();
        const date = new Date(+expiresAt);
        if(now >= date) {
          this.sessionExpired$.next(true);
        }
      }
    }, 60000);
  }
}
