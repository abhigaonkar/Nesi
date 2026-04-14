import { DatatableComponent } from '../../../../components/nesi-datatable/components/datatable/datatable.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { EmployeeFormBase } from '../../_base/employeeFormBase';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { TokenService } from '../../../../services/authentication/tokenService';
import { CONFIG } from '../../../../configuration';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { WindowRef } from '../../../../services/shared/windowRef';
import { EmployeeService } from '../../_base/employeeService';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-days-off-vacation-trascations',
  templateUrl: './employee-days-off-vacation-trascations.component.html',
  styleUrls: ['./employee-days-off-vacation-trascations.component.css']
})
export class EmployeeDaysOffVacationTrascationsComponent extends EmployeeFormBase implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;

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
      CONFIG.apiURL.page.employee.daysOff.Override
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'member_id': ['', Validators.required],
      'amount': ['', [Validators.required, Validators.min(0.01)]],
      'memo': ['', Validators.required],
      'auto_vac_payout': ['', Validators.required]
    });

    // this.initFormvalue = {
    //   memberid: this.memberid,
    //   current: 0,
    //   memo: '',
    // }
  }


  public loadGrid(memberid: number) {
    this.dt.reportQueryParam = [{ coulumnname: 'member_id', value: memberid }];
    this.dt.refreshCache = true;
    this.dt.showReport('EmployeeDaysOffVacationTransactionGrid');
  }

  submitSuccess() {
    this.userform.patchValue(this.extraData);
    // this.loadGrid(this.memberid);
    this.loadDetail();
  }
  loadDetail(refresh = false) {
    this.dt.after_onRefresh();
  }

}
