import {AbstractControl, AsyncValidatorFn} from '@angular/forms';
import {UserService} from "../../../services";
import {GetPhoneNumberExistenceResponse} from "../../../models";
import {map} from "rxjs/operators";
import { SuccessResponse } from 'mosaico-base';

export function phoneNumberExistsValidator(userService: UserService):AsyncValidatorFn  {
  return (control: AbstractControl) => {
    return userService
      .checkPhoneExistence(control.value)
      .pipe(
        map((response: SuccessResponse<GetPhoneNumberExistenceResponse>) => response?.data?.exist ? {phoneNumberExists:true} : null)
      );
  }
}
