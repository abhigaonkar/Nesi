import { AbstractControl } from '@angular/forms';

/**
 * Do not allow just spaces in a required control
 * @param  {AbstractControl} control
 */
export function validateSpaces(control: AbstractControl) {
    return String(control.value).trim() === '' ? { validatespace: true } : null;
}

/**
 * Restrict characters given in restrictedCharacters array and allow in case they are present in allowedCharacters array
 * @param  {string[]} allowedCharacters
 */
export const validateCharacters = (allowedCharacters: string[]) => {
    return (control: AbstractControl) => {
        const value = String(control.value);
        const restrictedCharacters = [';', '\'', '"'];
        let validation = false;
        for (let i = 0; i < restrictedCharacters.length; i++) {
            if (value.indexOf(restrictedCharacters[i]) > -1 && allowedCharacters.indexOf(restrictedCharacters[i]) < 0) {
                validation = true;
                break;
            }
        }
        return validation ? { validatecharacters: true } : null;
    }
}
