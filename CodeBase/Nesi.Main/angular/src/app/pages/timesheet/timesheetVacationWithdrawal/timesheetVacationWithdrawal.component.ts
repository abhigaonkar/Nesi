import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CONFIG } from '../../../configuration';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { CoreService } from '../../../services/shared/core.service';

import * as DATE from '../../../services/helper/datetime';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetVacationWithdrawal',
  templateUrl: './timesheetVacationWithdrawal.component.html',
  styleUrls: ['./timesheetVacationWithdrawal.component.css']
})
export class TimesheetVacationWithdrawalComponent extends FormMessageBase implements OnInit {

  get available_hours(): number {
    return this.old_hours - this.requestedHours;
  }
  get available_dollars(): number {
    return this.available_hours * this.rate;
  }
  old_hours: number;
  old_dollars: number;
  old_check: boolean;
  histories: any[];
  loading_hours = true;
  get rate(): number {
    if (this.old_hours !== 0) {
      return this.old_dollars / this.old_hours;
    } else {
      return 0;
    }
  }


  constructor(
    public cs: CoreService,
    private fb: FormBuilder,
    private tss: TimesheetService,
    protected store: Store<fromRoot.State>,
  ) {
    super(store, cs);
  }

  ngOnInit() {
    super.Init(CONFIG.apiURL.page.timesheet.vacation.save);
    //   this.loadHistory();
    this.getHoursDollars();
  }


  getHoursDollars() {
    this.userform.get('is_alloutstanding').disable();
    this.cs.getObject<any>(CONFIG.apiURL.page.timesheet.vacation.avaliableAtDate)
      .subscribe((res) => {
        this.old_hours = res.hours;
        this.old_dollars = res.dollars;
        this.loading_hours = false;
        if (this.available_hours <= 0) {
          this.userform.disable();
        } else {
          this.userform.get('is_alloutstanding').enable();
        }
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      });
  }
  createForm() {
    this.userform = this.fb.group({
      'is_alloutstanding': '',
      'txt_hours': ['', Validators.min(0.01)],
      'txt_money': ['', Validators.min(0.01)],
      'money': '',
      'hours': '',
      'alloutstanding': false,
      'unpaid': false,
      'o': ''
    });

    this.initFormvalue = {
      'is_alloutstanding': '',
      'txt_hours': '',
      'txt_money': '',
      'money': 0,
      'hours': 0,
      'alloutstanding': false,
      'unpaid': false,
      'o': '2'
    };
  }

  //   this.userform.get('txt_hours').valueChanges.subscribe(
  //     (value) => {
  //       if (this.old_hours !== value) {
  //         this.old_hours = value;
  //         this.userform.get('txt_money').setValue(value * this.rate);
  //       }
  //     }
  //   );

  //   this.userform.get('txt_money').valueChanges.subscribe(
  //     (value) => {
  //       if (this.old_dollars !== value) {
  //         this.old_dollars = value;
  //         if (this.rate != 0) {
  //           this.userform.get('txt_hours').setValue(value / this.rate);
  //         } else {
  //           this.userform.get('txt_hours').setValue(0);
  //         }
  //       }
  //     }
  //   );

  //   this.userform.get('is_alloutstanding').valueChanges.subscribe(
  //     (value) => {
  //       if (this.old_check !== value) {
  //         this.old_check = value;
  //         if (value) {
  //           this.userform.get('txt_money').setValue(this.available_hours);
  //           this.userform.get('txt_hours').setValue(this.available_dollars);
  //         } else {
  //           this.userform.get('txt_money').setValue(null);
  //           this.userform.get('txt_hours').setValue(null);
  //         }
  //       }
  //     }
  //   );
  // }

  hoursChanged() {
    const value = this.userform.get('txt_hours').value;
    this.userform.get('txt_money').setValue(Math.round(value * this.rate * 100) / 100);
  }

  dollarsChanged() {
    const value = this.userform.get('txt_money').value;
    if (this.rate !== 0) {

      this.userform.get('txt_hours').setValue(Math.round(value * 100 / this.rate) / 100);
    } else {
      this.userform.get('txt_hours').setValue(0);
    }
  }


  formValidateBefore() {
    const check = this.userform.get('is_alloutstanding').value;
    super.setFormValueFromTxt('money', 'txt_money', 0);
    super.setFormValueFromTxt('hours', 'txt_hours', 0);
    this.userform.get('alloutstanding').setValue(false);
  }

  submitValidate() {
    const money = this.userform.get('txt_money').value;
    const hours = this.userform.get('txt_hours').value;

    this.afterSubmit.emit();

    if (hours > this.available_hours) {
      this.PushWarnMessage('Withdrawn hours must be less than or equal to available hours');
      return false;
    }
    if (money > this.available_dollars) {
      this.PushWarnMessage('Withdraw dollars must be less than available dollars.');
      return false;
    }
    if (this.cs.checkQuarterNumber(hours)) {
      this.PushWarnMessage('Withdrawn hours must be a whole number, or in quarter hour increments.');
      return false;
    }

    if (!(money && hours)) {
      this.PushWarnMessage('You must input one of hours, dollars, all outstanding to withdrawing.');
      return false;
    } else {
      return true;
    }
  }


  isOutstandingChanged(event: any) {
    const check = this.userform.get('is_alloutstanding').value;

    this.userform.get('alloutstanding').setValue(check);
    if (check) {
      this.userform.get('txt_money').setValue(this.round(this.available_dollars, 2));
      this.userform.get('txt_hours').setValue(this.round(this.available_hours, 2));
    } else {
      this.userform.get('txt_money').setValue(null);
      this.userform.get('txt_hours').setValue(null);
    }

  }

  public loadHistory() {
    this.tss.getList<any>(CONFIG.apiURL.page.timesheet.vacation.history)
      .subscribe(
        res => this.histories = res
      ),
      (err:any)=>{
        this.PushErrorMessage(err);
      };
  }

  get requestedHours(): number {
    let sum = 0;
    if (!this.histories) {
      return 0;
    }
    this.histories.forEach(x => {
      if (!this.deleteDisabled(x)) {
        sum += x.amount;
      }
    });
    CONFIG.LOG(sum, 'total delete hours in histories.');
    return sum;

  }

  deleteDisabled(item): boolean {
    return (item.status === 'DENIED' || item.status === 'PAIDOUT')
      || (item.status === 'APPROVED'
        && (DATE.DateLessToday(item.date_start) || item.date_start === '--')
      );
  }

  submitSuccess() {
    //  this.loadHistory();
    this.getHoursDollars();
  }
}
