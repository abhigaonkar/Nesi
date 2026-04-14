import { Component, OnInit, Input } from '@angular/core';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { MessageBase } from '../../../core/messageBaseComponent';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { EmployeeOfferFormBase } from '../_employment/employeeOfferBase';
import { Validators, FormBuilder } from '@angular/forms';
import { EmployeeService } from '../_base/employeeService';

@Component({
  selector: 'nesi-employee-termination',
  templateUrl: './employee-termination.component.html',
  styleUrls: ['./employee-termination.component.css']
})
export class EmployeeTerminationComponent extends EmployeeOfferFormBase implements OnInit {

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    private fb: FormBuilder,

  ) {
    super(store, cs, es);
    super.InitEmployee(
      CONFIG.apiURL.page.employee.termination.profile
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'member_id': ['', Validators.required],
      'id': ['', Validators.required],
      'mgr_member_id': '',
      'trm_member_id': '',
      'dt_added': '',
      'dt_modified': '',
      'term_date': ['', Validators.required],
      'term_time': ['', Validators.required],
      'reason_actual': ['', Validators.required],
      'reason_roe': ['', Validators.required],
      'is_returning': '',
      'return_date': '',
      'is_vac_bank': '',
      'vac_bank_detail': '',
      'is_returned_property': '',
      'non_returned_value': '',
      'last_day_worked': ['', Validators.required],
    });
  }

  ngOnInit() {
  }



}
