import { AbstractControl, FormGroup } from '@angular/forms';
import { CONFIG } from '../../../configuration';
import { EmployeeService } from '../_base/employeeService';
import { Observable } from 'rxjs/Observable';
import { ValidateBase } from '../../../services/shared/validateBase';

export class ValidateEmployee extends ValidateBase {
  static createValidator(es: EmployeeService, url: string) {

    return (control: AbstractControl) => {
      return Observable.timer(this.debounceTime).switchMap(() => {
        return this.ConvertValidateResult(es.checkField(url, control.value));
      });
    }
  }


}

export function isBackOffice(es: EmployeeService) {
  return (control: AbstractControl) => {
    return (!control.value && es.is_backoffice) ? { label: '', value: 0 } : null;
  }
}

export function isBenefitRequired(es: EmployeeService) {
  return (control: AbstractControl) => {
    return (!control.value && es.profile.is_backoffice
      && new Date(es.profile && es.profile.entity && es.profile.entity.benefits_startdate) <= new Date()) ?
      { label: '', value: 0 } : null;
  }
}
