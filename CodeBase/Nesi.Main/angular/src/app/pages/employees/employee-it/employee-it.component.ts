import { Component, OnInit } from '@angular/core';
import { EmployeeFormBase } from '../_base/employeeFormBase';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { TokenService } from '../../../services/authentication/tokenService';
import { CONFIG } from '../../../configuration';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { WindowRef } from '../../../services/shared/windowRef';
import { EmployeeService } from '../_base/employeeService';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-it',
  templateUrl: './employee-it.component.html',
  styleUrls: ['./employee-it.component.css']
})
export class EmployeeItComponent extends EmployeeFormBase implements OnInit {

  newpassword: string;
  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public ts: TokenService,
    private fb: FormBuilder,
    private win: WindowRef,
    public es: EmployeeService,
  ) {
    super(store, cs, es);
    super.InitEmployee(
      CONFIG.apiURL.page.employee.it.base
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'id': '',
      'emplogon': '',
      'username': '',
      'neEmail': '',
      'ldaP_user': '',
      'phoneExtension': '',
      'business_unit_id': '',
      'cellphone_id': '',
      'cellphone_number_id': '',
      'gets_barcodescanner': '',
      'gets_phone': '',
      'gets_neemail': '',
      'gets_phoneext': '',
      'gets_laptop': '',
      'include_in_mobile_contactlist': '',
      'is_LDAP': '',
      'last_mobile_login': '',
    }, { validator: this.validation() });

  }
  resetPassword() {
    if (this.newpassword) {
      this.cs.postString(this.getUrl(CONFIG.apiURL.page.employee.it.resetPassword), { data: this.newpassword })
        .subscribe(res => {
          if (this.PushResponseMessage(res)) {
            this.newpassword = null;
          }
        },
        (err:any)=>{
          super.PushErrorMessage(err);
        });
    }
  }

  validation() {
    return (group: FormGroup) => {
      const errors = {
        phoneExtension: null,
        neEmail: null,
        ldaP_user: null,
        cellphone_id: null,
        cellphone_number_id: null,
      };

      if (group.get('gets_phone').value) {
        if (!group.get('cellphone_id').value) {
          errors.cellphone_id = true;
        }
        if (!group.get('cellphone_number_id').value) {
          errors.cellphone_number_id = true;
        }
      }
      if (group.get('gets_neemail').value) {
        if (!group.get('neEmail').value) {
          errors.neEmail = true;
        }
        if (!group.get('ldaP_user').value) {
          errors.ldaP_user = true;
        }
      }
      if (group.get('gets_phoneext').value) {
        if (!group.get('phoneExtension').value) {
          errors.phoneExtension = true;
        }
      }

      if (errors.cellphone_id || errors.phoneExtension || errors.cellphone_number_id || errors.neEmail || errors.ldaP_user) {
        return errors;
      } else {
        return null;
      }
    };
  }
  // syncITLdapPassword() {
  //   this.es.syncITLdapPassword().subscribe((response) => {
  //     this.PushSuccessMessage('' + response);
  //   });
  // }
}
