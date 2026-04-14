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
  selector: 'nesi-timesheetVacationSchedule',
  templateUrl: './timesheetVacationSchedule.component.html',
  styleUrls: ['./timesheetVacationSchedule.component.css']
})
export class TimesheetVacationScheduleComponent extends FormMessageBase implements OnInit {

  @Output() reviewHide = new EventEmitter();
  @Output() confirmStart = new EventEmitter();

  reviewValue: any;

  public minDate = DATE.Today();
  startDate: Date = DATE.Today();

  confirmDisplay = false;
  currentAvaliable: any;
  
  loading_hours = true;


  constructor(
    public cs: CoreService,
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.timesheet.vacation.save);
  }

  ngOnInit() {
    this.getHoursDollars();
  }


  getHoursDollars(date: Date = null) {
    this.loading_hours = true;
    this.cs.getObject(CONFIG.apiURL.page.timesheet.vacation.avaliableAtDate + (!!date ? DATE.ToyyyyMMdd(date) : ''))
      .subscribe(
      (res) => {
           this.loading_hours = false;
          this.currentAvaliable = res;
     },
      (err:any)=>{
        this.PushErrorMessage(err);
        this.loading_hours=false;
      }
      );
  }
  createForm() {
    this.userform = this.fb.group({
      'date_return': ['', [Validators.required, this.checkDateGreatThanToday]],
      'date_start': ['', [Validators.required, this.checkDateGreatThanToday]],
      'date_end': ['', [Validators.required, this.checkDateGreatThanToday]],
      'payment': ['', Validators.required],
      'alloutstanding': false,
      'unpaid': false,
      'note': '',
      'o': ''
    }, {
        validator: this.lessThanDateCompare()
      });

    this.initFormvalue = {
      'date_return': '',
      'date_start': '',
      'date_end': '',
      'payment': '',
      'alloutstanding': false,
      'unpaid': false,
      'note': '',
      'o': '1'
    };

    this.userform.get('date_start').valueChanges
      .subscribe(
      (res) => {
        if (res) {
          this.startDate = res;
          CONFIG.LOG(this.startDate, 'startDate changed');
          this.getHoursDollars(res);
        }
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }

  checkDateGreatThanToday(control: FormControl) {
    CONFIG.LOG(new Date(control.value), 'checkDateGreatThanToday');

    if (DATE.DateLessToday(control.value)) {
      return {
        checkDateGreatThanToday: true
      };
    }
  }

  submitValidate(): boolean {
    this.confirmDisplay = true;
    CONFIG.LOG(this.submitedValue, 'submit value for review');
    this.reviewValue = null;
    this.cs.postObject<any>(CONFIG.apiURL.page.timesheet.vacation.review, this.submitedValue)
      .subscribe(
      (res: any) => {
        this.reviewValue = res;
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
    return false;
  }

  formValidateBefore() {
    const payment = this.userform.get('payment').value;
    if (!payment) { return; }
    const alloutstanding = this.userform.get('alloutstanding');
    const unpaid = this.userform.get('unpaid');
    alloutstanding.setValue(false);
    unpaid.setValue(false);
    switch (payment) {
      case '1':
        break;
      case '2':
        alloutstanding.setValue(true);
        break;
      case '3':
        unpaid.setValue(true);
        break;
      default:
        break;
    }

  }

  confirm() {
    this.confirmDisplay = false;
    this.confirmStart.emit();
    this.formPost();
  }

  lessThanDateCompare() {
    return (group: FormGroup): { [key: string]: any } => {
      const d_start = group.controls['date_start'].value;
      const d_end = group.controls['date_end'].value;
      const d_return = group.controls['date_return'].value;

      if (d_end < d_start && d_return < d_end) {
        return {
          EndDateGreatThanStartDate: d_end < d_start ? true : null,
          ReturnDateGreatThanStartDate: d_return < d_end ? true : null,
        };
      }
      if (d_end < d_start) {
        return {
          EndDateGreatThanStartDate: d_end < d_start ? true : null,
        };
      }
      if (d_return < d_end) {
        return {
          ReturnDateGreatThanStartDate: d_return < d_end ? true : null,
        };
      }
      return null;
    }
  }

}
