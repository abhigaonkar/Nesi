import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { EmployeeFormBase } from '../../_base/employeeFormBase';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { TokenService } from '../../../../services/authentication/tokenService';
import { CONFIG } from '../../../../configuration';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { WindowRef } from '../../../../services/shared/windowRef';
import { EmployeeService } from '../../_base/employeeService';
import { LabelValueInt } from '../../../../models/Shared/labelValueString';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-wage-wage',
  templateUrl: './employee-wage-wage.component.html',
  styleUrls: ['./employee-wage-wage.component.css']
})
export class EmployeeWageWageComponent extends EmployeeFormBase implements OnInit {
  @Input() bonusTypeList: LabelValueInt[];

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
      CONFIG.apiURL.page.employee.wage.wage,
      'N/A'
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'id': '',
      'member_id': '',
      'date': ['', [Validators.required]],
      'current_wage': ['', [Validators.required]],
      'date_next_raise': ['', [Validators.required]],
      'comment': '',
      'member_id_audit': '',
      'audit_username': '',
      'member_id_added_by': '',
      'business_unit_id': '',
      'membertype_id': '',
      'bonus_type': '',
      'bonus_type_name': '',
      'bonus_amount': '',
      'active':false
    }, { validator: this.validation() });

  }

  validation() {

  }

  loadProfile() {
    super.loadProfile();
    this.userform.get('bonus_amount').setValue(this.profile.entity.bonus_amount * 100);
  }

  formValidateBefore() {
    this.userform.get('bonus_amount').setValue(this.userform.get('bonus_amount').value / 100);
  }
}
