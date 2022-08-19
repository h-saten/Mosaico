import { AbstractControl, FormArray, FormGroup, ValidatorFn } from '@angular/forms';

export function validateForm(form: FormGroup): boolean {
    Object.keys(form.controls).forEach(field => {
        const control = form.get(field);
        if (control) {
            if (control instanceof FormGroup) {
                validateForm(control);
            } else {
                control.markAsDirty({ onlySelf: true });
            }
        }
    });
    return form.valid;
}

export function validateFormArray(formArray: FormArray): boolean {
    formArray.controls.forEach((contr) => {
        if (contr instanceof FormGroup) {
            validateForm(contr);
        } else {
            contr.markAsDirty({ onlySelf: true });
        }
    });
    formArray.updateValueAndValidity();
    return formArray.valid;
}

export function greaterThan(field: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const fieldToCompare = control.parent?.get(field);
        if (fieldToCompare) {
            const isLessThan = Number(fieldToCompare.value) < Number(control.value);
            return !isLessThan ? { 'greaterThan': { value: control.value } } : null;
        }
        return null;
    };
}

export function greaterThanOrEqual(field: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const fieldToCompare = control.parent?.get(field);
        if (fieldToCompare) {
            const isLessThan = Number(fieldToCompare.value) <= Number(control.value);
            return !isLessThan ? { 'greaterThanOrEqual': { value: control.value } } : null;
        }
        return null;
    };
}

export function lessThan(field: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const fieldToCompare = control.parent?.get(field);
        if (fieldToCompare) {
            const isGreaterThan = Number(fieldToCompare.value) > Number(control.value);
            return !isGreaterThan ? { 'lessThan': { value: control.value } } : null;
        }
        return null;
    };
}

export function lessThanOrEqual(field: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const fieldToCompare = control.parent?.get(field);
        if (fieldToCompare) {
            const isGreaterThan = Number(fieldToCompare.value) >= Number(control.value);
            return !isGreaterThan ? { 'lessThanOrEqual': { value: control.value } } : null;
        }
        return null;
    };
}

export function dateLessThan(field: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const fieldToCompare = control.parent?.get(field);
        if (fieldToCompare) {
            const isLessThan = new Date(fieldToCompare.value) > new Date(control.value);
            return !isLessThan ? { 'dateLessThan': { value: control.value } } : null;
        }
        return null;
    };
}

export function dateLessThanOrEqual(field: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const fieldToCompare = control.parent?.get(field);
        if (fieldToCompare) {
            const isLessThan = new Date(fieldToCompare.value) >= new Date(control.value);
            return !isLessThan ? { 'dateLessThanOrEqual': { value: control.value } } : null;
        }
        return null;
    };
}

export function dateGreaterThan(field: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const fieldToCompare = control.parent?.get(field);
        if (fieldToCompare) {
            const isGreaterThan = new Date(fieldToCompare.value) < new Date(control.value);
            return !isGreaterThan ? { 'dateGreaterThan': { value: control.value } } : null;
        }
        return null;
    };
}

export function dateGreaterThanOrEqual(field: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const fieldToCompare = control.parent?.get(field);
        if (fieldToCompare) {
            const isGreaterThan = new Date(fieldToCompare.value) <= new Date(control.value);
            return !isGreaterThan ? { 'dateGreaterThanOrEqual': { value: control.value } } : null;
        }
        return null;
    };
}

export function matchPassword(field: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const fieldToCompare = control.parent?.get(field);
        if (fieldToCompare) {
            const isEqual = control.value === fieldToCompare.value;
            return !isEqual ? { 'ConfirmPassword': { value: true } } : null;
        }
        return null;
    };
}

export function trim(payload: any): any {
    if (payload) {
        for (const k in payload) {
            if(payload[k] && typeof payload[k] === 'string' || payload[k] instanceof String){
                payload[k] = payload[k].trim();
            }
        }
    }
    return payload;
}