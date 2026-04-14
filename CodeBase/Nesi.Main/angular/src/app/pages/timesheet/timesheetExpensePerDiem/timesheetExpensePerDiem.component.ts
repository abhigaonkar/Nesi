import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl
} from '@angular/forms';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { TokenService } from '../../../services/authentication/tokenService';
import { CONFIG } from '../../../configuration';
import * as DATE from '../../../services/helper/datetime';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromMessage from '../../../actions/layout/growlMessage';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { CoreService } from '../../../services/shared/core.service';
import { TimesheetExpenseBase } from '../interface/timesheetExpenseBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetExpensePerDiem',
  templateUrl: './timesheetExpensePerDiem.component.html',
  styleUrls: ['./timesheetExpensePerDiem.component.css']
})
export class TimesheetExpensePerDiemComponent extends TimesheetExpenseBase
  implements OnInit {
  public userform: FormGroup;
  public submitted: boolean;

  public currentProfile: any;
  public buProfile: any;
  public CalendedrMinDate: Date;
  public allow_unlinked_timesheet: boolean;

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService
  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.timesheet.expense.perDiemSave);
  }

  createForm() {
    this.userform = this.fb.group(
      {
        id_member: [[], Validators.required],
        buId: '',
        date_start: '',
        date_end: '',
        rate: '',
        wo_number: '',
        isShop: '',
        amount: ''
      },
      {
        validator: this.checkWorkOrderAndShop()
      }
    );

    this.initFormvalue = {
      id_member: [this.ts.currentUser.id],
      buId: this.ts.currentUser.businessUnitId,
      date_start: DATE.Today(),
      date_end: DATE.Today(),
      rate: '',
      wo_number: '',
      isShop: false,
      amount: 0
    };
  }

  ngOnInit() {
    this.cs
      .getObject(CONFIG.apiURL.page.timesheet.expense.perDiemProfile)
      .subscribe((res: any) => {
        CONFIG.LOG(res, 'get perdiem profile');
        this.currentProfile = res;
        this.allow_unlinked_timesheet = res.allow_unlinked_timesheet;
        this.CalendedrMinDate = new Date(this.currentProfile.minDate);
        this.loadSelectedBuProfile();
      },
      (err:any)=>
      {
        super.PushErrorMessage(err);
      });
  }

  get woLineDescription(): string {
    if (!(this.buProfile && this.buProfile.forEmployees)) {
      return '';
    }

    let empList = '';
    const alluser: any[] = this.buProfile.forEmployees;

    const selectUser: any[] = this.userform.get('id_member').value;

    if (selectUser.length === 0) {
      return '';
    }
    alluser.forEach(u => {
      selectUser.forEach(s => {
        if (s === u.value) {
          empList += u.label + ',';
        }
      });
    });
    empList = empList.substring(0, empList.length - 1);
    const d1 = DATE.ToyyyyMMdd(this.userform.get('date_start').value);
    const d2 = DATE.ToyyyyMMdd(this.userform.get('date_end').value);
    return `${empList} - Per Diem ${d1} to ${d2}`;
  }

  get amount() {
    const val =
      this.userform.get('rate').value *
      (1 +
        DATE.DayDiff_UTC(
          this.userform.get('date_end').value,
          this.userform.get('date_start').value
        ));
    return val > 0 ? Math.round(val * 100) / 100 : 0;
  }
  loadSelectedBuProfile() {
    this.cs
      .getObject(
        CONFIG.apiURL.page.timesheet.expense.perDiemBuProfile +
        this.userform.get('buId').value
      )
      .subscribe((res: any) => {
        this.buProfile = res;
        if (this.userform.get('buId').value === this.ts.currentUser.businessUnitId) {
          this.userform.get('id_member').setValue([this.ts.currentUser.id]);
        } else {
          this.userform.get('id_member').setValue([]);
        }
        this.setRateValue();
      },
      (err:any)=>
      {
        super.PushErrorMessage(err);
      });
  }

  setRateValue() {
    this.userform.get('rate').setValue(this.buProfile.perdiemRate);
  }
  shopChanged(event) {
    if (this.userform.get('isShop').value) {
      this.userform.get('wo_number').setValue('');
      this.userform.get('wo_number').disable();
    } else {
      this.userform.get('wo_number').enable();
    }
  }

  formValidateBefore() {
    this.userform.get('amount').setValue(this.amount);
  }

  submitSuccess() {
    this.setRateValue();
  }
}
