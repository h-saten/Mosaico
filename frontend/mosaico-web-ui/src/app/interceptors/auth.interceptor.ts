import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { AuthService } from 'mosaico-base';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {
    constructor(public auth: AuthService) { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.auth.isAuthenticated()) {
            const header = request.headers.get('Auth-Type');
            if (header && header.length > 0) {
                request = request.clone({
                    setHeaders: {
                        Authorization: `Bearer ${this.auth.getIdToken()}`
                    }
                });
                request.headers.delete('Auth-Type');
            }
            else {
                request = request.clone({
                    setHeaders: {
                        Authorization: `Bearer ${this.auth.getAccessToken()}`
                    }
                });
            }
        }
        return next.handle(request);
    }
}