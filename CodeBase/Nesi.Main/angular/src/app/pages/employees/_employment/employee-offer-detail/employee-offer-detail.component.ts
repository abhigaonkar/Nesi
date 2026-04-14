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
import { ConfirmationService } from 'primeng/primeng';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-offer-detail',
  templateUrl: './employee-offer-detail.component.html',
  styleUrls: ['./employee-offer-detail.component.css']
})
export class EmployeeOfferDetailComponent extends EmployeeOfferFormBase implements OnInit {


  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    public fb: FormBuilder,
    public cf: ConfirmationService,
  ) {
    super(store, cs, es);
    this.InitEmployee(CONFIG.apiURL.page.employee.offer.detail);
  }

  createForm() {
    this.userform = this.fb.group({
      'memberid': ['', [Validators.required]],
      'id': '',
      'applicantid': '',
      'business_unit_id': ['', [Validators.required, Validators.min(0)]],
      'membertypeid': ['', [Validators.required, Validators.min(1)]],
      'isapplicant': '',
      'fullname': '',
      'status': '',
      'startdate': ['', [Validators.required]],
      'enddate': ['', [Validators.required]],
      'enteredby': '',
      'author': '',
      'reports_to': ['', [Validators.required, Validators.min(1)]],
      'notes': '',
    });
  }

  public getBMID() {
    const bu = this.userform.get('business_unit_id').value;
    const list = this.profile.reportsToList.map(x => x.value);
    if (bu > 0) {
      this.cs.getNumber(this.getUrl(CONFIG.apiURL.page.employee.offer.bm).replace('$buid', bu))
        .subscribe(res => {
          if (list.includes(res)) {
            this.userform.get('reports_to').setValue(res);
          } else {
            this.userform.get('reports_to').setValue(null);
          }
        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
        });
    }
  }

  public get notes_vislibe() {
    const o = this.userform.get('startdate').value;
    return this.save_enabled && o && new Date(o) < new Date(this.profile.payperiod_startdate);
  }

  submitValidate() {
    if (!this.notes_vislibe) {
      this.userform.get('notes').setValue(null);
      this.submitedValue['notes'] = null;
      return true;
    }
    const o = this.isFieldMinLength('notes', 10);
    if (o) {
      this.cf.confirm({
        message: 'Are you sure you want to set a back date for start date?',
        accept: () => {
          this.formPost();
          return true;
        },
        reject: () => {
          this.disableFields();
          return false;
        }
      });
    } else {
      return false;
    }
  }

}
