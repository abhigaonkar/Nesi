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
  selector: 'nesi-employee-wage',
  templateUrl: './employee-wage.component.html',
  styleUrls: ['./employee-wage.component.css']
})
export class EmployeeWageComponent extends EmployeeFormBase implements OnInit {

  addnewDisplay = false;
  new_employee: any;
  selected_employee: any;
  displayDialog = false;
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
      CONFIG.apiURL.page.employee.wage.base
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'id': '',
      'is_CDN_country': '',
      'vacation_amount_1': '',
      'vacation_amount_2': '',
      'vacation_amount_3': '',
      'vacation_interval_1': '',
      'vacation_interval_2': '',
      'vacation_interval_3': '',
      'timetostat': '',
      'is_receive_stat_pay': '',
    }, { validator: this.validation() });

  }

  validation() {

  }

  add_new() {
    this.new_employee = {
      id: 0,
      member_id: this.memberid,
      date: new Date(),
      date_next_raise: '',
      member_id_audit: this.ts.currentUser.id,
      member_id_added_by: this.ts.currentUser.id,
      business_unit_id: this.ts.currentUser.businessUnitId,
      bonus_amount: 0,
      membertype_id: 0,
      bonus_type: 0,
      comment: '',
      current_wage: ''
    };
    // this.addnewDisplay = !this.addnewDisplay;
    this.selected_employee = this.new_employee;
    this.displayDialog = true;
  }

  edit_wage(data) {
    this.selected_employee = data;
    this.displayDialog = true;
  }

  wage_updated(event) {
    // CONFIG.LOG(event, 'wage updated event');
    this.displayDialog = false;
    this.profile = event.result;
    this.loadProfile();
  }
}
