import {AbstractControl, ValidationErrors, ValidatorFn} from '@angular/forms';
import {PhoneNumberUtil} from 'google-libphonenumber';

export function phoneNumberValidator(): ValidatorFn {

  const validationError = {wrongNumber: true};

  const phoneNumberUtil = PhoneNumberUtil.getInstance();
  return (control: AbstractControl) : ValidationErrors | null => {
    try {
      const phoneNumber = phoneNumberUtil.parse(control.value);
      const isValidPhoneNumber = phoneNumberUtil.isValidNumber(phoneNumber);
      return isValidPhoneNumber ? null : validationError;
    } catch {
      return validationError;
    }
  }
}
