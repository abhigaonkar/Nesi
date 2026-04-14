import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../../reducers';
import { CONFIG } from 'app/configuration';
import { EmployeeService } from '../../_base/employeeService';
import { FormBuilder } from '@angular/forms';
import { EmployeeFormBase } from '../../_base/employeeFormBase';


@Component({
  selector: 'nesi-employee-privilege-user-switching',
  templateUrl: './employee-privilege-user-switching.component.html',
  styleUrls: ['./employee-privilege-user-switching.component.css']
})
export class EmployeePrivilegeUserSwitchingComponent extends EmployeeFormBase implements OnInit {

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    public fb: FormBuilder,
  ) {
    super(store, cs, es);
    this.InitEmployee(CONFIG.apiURL.page.employee.privilege.switchUser);
  }



  ngOnInit(): void {
  }

  createForm() {

  }

  postdata() {
    this.cs.postDataExtra(this.getUrl(), { data: this.profile.target.map(x => x.value) })
      .subscribe((res) => {
        if (this.CheckResponseMessage(res.data)) {
          this.profile = res.extra;
        }
      },
        (err:any)=>
        {
          this.PushErrorMessage(err);
        }
      )
  }
}
