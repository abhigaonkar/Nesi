import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { EmployeeFormBase } from '../_base/employeeFormBase';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { TokenService } from '../../../services/authentication/tokenService';
import { CONFIG } from '../../../configuration';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { WindowRef } from '../../../services/shared/windowRef';
import { EmployeeService } from '../_base/employeeService';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { EmployeeDaysOffListComponent } from '../_day-off/employee-days-off-list/employee-days-off-list.component';
// tslint:disable-next-line:max-line-length
import { EmployeeDaysOffVacationRequestComponent } from '../_day-off/employee-days-off-vacation-request/employee-days-off-vacation-request.component';
// tslint:disable-next-line:max-line-length
import { EmployeeDaysOffVacationTrascationsComponent } from '../_day-off/employee-days-off-vacation-trascations/employee-days-off-vacation-trascations.component';

@Component({
  selector: 'nesi-employee-day-off',
  templateUrl: './employee-day-off.component.html',
  styleUrls: ['./employee-day-off.component.css']
})
export class EmployeeDayOffComponent extends EmployeeFormBase implements OnInit {
  @ViewChild(EmployeeDaysOffListComponent) days_off_list: EmployeeDaysOffListComponent;
  @ViewChild(EmployeeDaysOffVacationRequestComponent) days_off_vacation_request: EmployeeDaysOffVacationRequestComponent;
  @ViewChild(EmployeeDaysOffVacationTrascationsComponent) days_off_vacation_transaction: EmployeeDaysOffVacationTrascationsComponent;

  @Input() vacation_visible: boolean;

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
      null, null
    );
  }

  loadData() {
    switch (this.activeTab) {
      case 0:
        this.days_off_list.loadGrid(this.memberid);
        break;
      case 1:
        this.days_off_vacation_request.loadGrid(this.memberid);
        break;
      case 2:
        this.days_off_vacation_transaction.loadGrid(this.memberid);
        this.days_off_vacation_transaction.loadData()
        break;
    }
  }

  tabChanged(event) {
    super.tabChanged(event);
    this.loadData();
  }

}
