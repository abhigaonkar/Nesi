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
  selector: 'nesi-employee-termination-checklist',
  templateUrl: './employee-termination-checklist.component.html',
  styleUrls: ['./employee-termination-checklist.component.css']
})
export class EmployeeTerminationChecklistComponent extends EmployeeOfferFormBase implements OnInit {
  type = 'All';
  selected_all = false;
  enabled = false;

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    private fb: FormBuilder,

  ) {
    super(store, cs, es);
    super.InitEmployee(
      CONFIG.apiURL.page.employee.termination.checkList
    );
  }

  createForm() {
  }


  ngOnInit() {
  }


  load_checkList(e) {
    setTimeout(() => {
      this.selected_all = false;
      this.loadData();
    }, 200);
  }

  save_item(row) {
    this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.employee.termination.checkListItem), row)
      .subscribe(
        (res) => {
          if (this.CheckResponseMessage(res.data)) {
            this.profile.data = res.extra;
          }
        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
        }
      );
  }

  save_all() {
    this.cs.postDataExtra(this.getUrl(this.postUrl), this.profile.data)
      .subscribe(
        (res) => {
          if (this.PushResponseMessage(res.data)) {
            this.profile = res.extra;
          }
        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
        }
      );
  }

  AfterProfileLoaded() {
    this.type = this.profile.checklist_type;
    this.enabled = this.profile.checklist_enabled;
  }

  getUrl(url) {
    return super.getUrl(url).replace('$type', this.type);
  }

  selectall_change() {
    this.profile.data.forEach(x => {
      x.is_checked = this.selected_all;
    });
  }
}
