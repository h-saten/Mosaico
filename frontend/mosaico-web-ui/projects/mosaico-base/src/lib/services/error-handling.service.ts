import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from "ngx-toastr";
import { ErrorResponse } from "../utils";

export interface Error {
    error: ErrorResponse;
}

@Injectable({
    providedIn: 'root'
})
export class ErrorHandlingService {
    constructor(private toastr: ToastrService, private router: Router, private translateService: TranslateService) {

    }

    public handleErrorWithRedirect(error: HttpErrorResponse | Error, redirectUrl = '/') {
        this.handleErrorWithToastr(error);
        if (this.router && redirectUrl && redirectUrl.length > 0) {
            this.router.navigateByUrl(redirectUrl);
        }
    }


    public handleErrorWithToastr(error: HttpErrorResponse | Error | any) {
        if (error && error.error) {
            const errorResponse = error.error as ErrorResponse;
            if (errorResponse && this.toastr) {
                if(errorResponse.code && errorResponse.message){
                    this.translateService.get(errorResponse.code).subscribe((t) => {
                      this.toastr.error(t);
                    });
                }
                else if (errorResponse.extraData && errorResponse.extraData.message) {
                    this.toastr.error(errorResponse.extraData.message, errorResponse.message);
                }
                else if(errorResponse.code && errorResponse.message){
                    const translatedCode = this.translateService.instant(errorResponse.code);
                    this.toastr.error(translatedCode, errorResponse.message);
                }
                else if(error?.error?.message) {
                    this.toastr.error(error.error.message);
                }
                else {
                    this.translateService.get('ERROR_MESSAGES.UNHANDLED_ERROR').subscribe((t) => {
                        this.toastr.error(t);
                    });
                }
            }
        }
        else if(error?.message){
            this.toastr.error(error?.message);
        }
    }
}
