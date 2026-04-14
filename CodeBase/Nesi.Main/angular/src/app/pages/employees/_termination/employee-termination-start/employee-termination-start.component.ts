import { Component, OnInit, Input } from '@angular/core';
import { CONFIG } from '../../../../configuration';
import { CoreService } from '../../../../services/shared/core.service';
import { MessageBase } from '../../../../core/messageBaseComponent';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { EmployeeOfferFormBase } from '../../_employment/employeeOfferBase';
import { Validators, FormBuilder } from '@angular/forms';
import { EmployeeService } from '../../_base/employeeService';

@Component({
  selector: 'nesi-employee-termination-start',
  templateUrl: './employee-termination-start.component.html',
  styleUrls: ['./employee-termination-start.component.css']
})
export class EmployeeTerminationStartComponent extends EmployeeOfferFormBase implements OnInit {

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    private fb: FormBuilder,

  ) {
    super(store, cs, es);
    super.InitEmployee(
      CONFIG.apiURL.page.employee.termination.start
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'member_id': ['',Validators.required],
      'roe': ['',Validators.required],
      'act': ['',Validators.required],
      'ldw': ['',Validators.required],
      
    });
  }

  ngOnInit() {
  }

}
