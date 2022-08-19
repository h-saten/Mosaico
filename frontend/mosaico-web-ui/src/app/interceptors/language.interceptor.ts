import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";
import { TranslateService } from '@ngx-translate/core';
import { Injectable } from "@angular/core";

@Injectable()
export class LanguageInterceptor implements HttpInterceptor {
    constructor(private translateService: TranslateService) {

    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let applanguage = this.translateService.currentLang;
        applanguage = applanguage && applanguage.length > 0 ? applanguage : 'en';
        req = req.clone({setHeaders: { applanguage }});
        return next.handle(req);
    }
}
