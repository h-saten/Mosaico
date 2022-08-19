import {HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {TranslateService} from '@ngx-translate/core';

@Injectable()
export class DefaultInterceptor implements HttpInterceptor {

    constructor(private translate: TranslateService) {
      this.translate.currentLang = 'pl';
    }

    public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      const lang = this.translate.currentLang;
      const headers: HttpHeaders = req.headers.set('AppLanguage', lang);
      req = req.clone({ headers });
      return next.handle(req);
    }
}
