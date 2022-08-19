import { FormGroup } from '@angular/forms';

export abstract class FormBase {
    public form: FormGroup;

    public hasErrors(controlName?: string, validatorName?: string, localForm?: FormGroup): boolean {
        if(!localForm){
            localForm = this.form;
        }
        if (localForm && controlName && controlName.length > 0) {
            const control = localForm.get(controlName);
            if (control) {
                if (validatorName && validatorName.length > 0) {
                    return control?.errors && control.errors[validatorName] && true;
                }
                return control.errors !== null && control.errors !== undefined && true;
            }
        }
        return false;
    }

    public isInvalid(controlName?: string, localForm?: FormGroup): boolean {
        if(!localForm){
            localForm = this.form;
        }
        if (localForm && controlName && controlName.length > 0) {
            const control = localForm.get(controlName);
            if (!control) {
                return localForm.invalid;
            } else {
                return control.invalid;
            }
        }
        return false;
    }

    public isDirty(controlName?: string, localForm?: FormGroup): boolean {
        if(!localForm) {
            localForm = this.form;
        }
        if (localForm && controlName && controlName.length > 0) {
            const control = localForm.get(controlName);
            if (!control) {
                return localForm.dirty;
            } else {
                return control.dirty;
            }
        }
        return false;
    }

    public isTouched(controlName?: string, localForm?: FormGroup): boolean {
        if(!localForm) {
            localForm = this.form;
        }
        if (localForm && controlName && controlName.length > 0) {
            const control = localForm.get(controlName);
            if (!control) {
                return localForm.touched;
            } else {
                return control.touched;
            }
        }
        return false;
    }
}
