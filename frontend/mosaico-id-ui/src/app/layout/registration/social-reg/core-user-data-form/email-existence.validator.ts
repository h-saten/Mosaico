import {AbstractControl, AsyncValidatorFn} from '@angular/forms';
import {AuthService, GetEmailExistenceResponse} from "../../../../services/auth.service";
import {map} from "rxjs";
import {SuccessResponse} from "../../../../utils";

export function emailExistsValidator(authClient: AuthService):AsyncValidatorFn  {
  return (control: AbstractControl) => {
    return authClient
      .checkEmailExistence(control.value)
      .pipe(
        map((response: SuccessResponse<GetEmailExistenceResponse>) => response?.data?.exist ? {emailExists:true} : null)
      );
  }
}
