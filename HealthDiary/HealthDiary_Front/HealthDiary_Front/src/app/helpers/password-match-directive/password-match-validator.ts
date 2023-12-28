import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const password = control.get('registerPassword');
    const confirmPassword = control.get('registerConfirmPassword');

    if(!password || !confirmPassword) return null;

    return password.value === confirmPassword.value ? null : {passwordMismatch: true}
}