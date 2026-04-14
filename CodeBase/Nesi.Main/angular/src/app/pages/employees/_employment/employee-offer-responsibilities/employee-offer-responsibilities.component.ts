import { Component, OnInit, Input } from '@angular/core';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { EmployeeService } from '../../_base/employeeService';
import { EmployeeOfferFormBase } from '../employeeOfferBase';

@Component({
  selector: 'nesi-employee-offer-responsibilities',
  templateUrl: './employee-offer-responsibilities.component.html',
  styleUrls: ['./employee-offer-responsibilities.component.css']
})
export class EmployeeOfferResponsibilitiesComponent extends EmployeeOfferFormBase implements OnInit {
  select_all = false;
  copy_from = false;
  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,

  ) {
    super(store, cs, es);
    this.InitEmployee(CONFIG.apiURL.page.employee.offer.responsibilities);
  }

  chk_select_all(e) {
    this.copy_from = false;
    this.post_data(e ? 'select_all' : 'unselect_all');
  }

  post_data(o: string, id = 0) {
    this.cs.postDataExtra(this.getUrl(this.postUrl), {
      operation: o,
      value: id
    }).subscribe((res) => {
      if (this.CheckResponseMessage(res.data)) {
        this.profile.list = res.extra;
      }
    },
    (err:any)=>
    {
      this.PushErrorMessage(err);
    });
  }

  copy_from_previous(e) {
    this.select_all = false;
    this.post_data(e ? 'select_copy_from_prev' : 'unselect_copy_from_prev');
  }

  chk_select_one(e, row) {
    this.post_data(e ? 'checked_one' : 'unchecked_one', row.id);
  }
}
