import { Component, OnInit, Input } from '@angular/core';
import { MessageBase } from '../../../core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { TokenService } from '../../../services/authentication/tokenService';
import { EmployeeService } from '../_base/employeeService';
import { CONFIG } from '../../../configuration';
import { EmployeeFormBase } from '../_base/employeeFormBase';
@Component({
  selector: 'nesi-employee-contact',
  templateUrl: './employee-contact.component.html',
  styleUrls: ['./employee-contact.component.css']
})
export class EmployeeContactComponent extends EmployeeFormBase implements OnInit {

  @Input() set contact_member_id(value: number) {
    if (value && value > 0) {
      this._memberid = value;
      this.loadData();
    }
  }

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public ts: TokenService,
    public es: EmployeeService,
  ) {
    super(store, cs, es);
    super.InitEmployee(
      CONFIG.apiURL.page.employee.contact
    );
  }

}
