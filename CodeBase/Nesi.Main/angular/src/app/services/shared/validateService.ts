import { Injectable } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
@Injectable()
export class ValidateService {

  constructor(
  ) {
  }

  isNotValidateRequestAndMinLength(form: FormGroup, controlName: string): boolean {
    return (form.controls[controlName].hasError('required')
      || form.controls[controlName].hasError('minlength'))
      && form.controls[controlName].touched;
  }
}
