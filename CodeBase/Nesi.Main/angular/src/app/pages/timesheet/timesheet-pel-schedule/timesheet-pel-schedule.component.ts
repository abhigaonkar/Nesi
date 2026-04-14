import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
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
  selector: 'nesi-timesheet-pel-schedule',
  templateUrl: './timesheet-pel-schedule.component.html',
  styleUrls: ['./timesheet-pel-schedule.component.css']
})
export class TimesheetPelScheduleComponent extends FormMessageBase implements OnInit {

  @Output() reviewHide = new EventEmitter();
  @Output() confirmStart = new EventEmitter();

  public reviewValue: any;
  @Input() summary: any;

  public minDate = DATE.Today();
  startDate: Date = DATE.Today();

  confirmDisplay = false;
  reasonList = [
    { label: 'Sick', value: 'Sick' },
    { label: 'Other', value: 'Other' },
  ];

  constructor(
    public cs: CoreService,
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.timesheet.pel.save);
  }

  ngOnInit() {
  }

  createForm() {
    this.userform = this.fb.group({
      'date_start': ['', [Validators.required, this.checkDateGreatThanToday]],
      'date_end': ['', [Validators.required, this.checkDateGreatThanToday]],
      'hours': ['', [Validators.required]],
      'note': ['', Validators.required],
    }, {
        validator: this.lessThanDateCompare()
      });

    this.initFormvalue = {
      'date_start': this.startDate,
      'date_end': this.startDate,
      'hours': 8,
      'note': 'Sick',
    };
  }


  startDate_changed(e) {
    const end = this.userform.get('date_end');
    const start = this.userform.get('date_start');
    if (start.value && end.value) {
      end.setValue(start.value);
    }
  }

  checkHours(control: FormControl) {
    if (this.summary) {
      const max_hours = this.summary.total_hours - this.summary.used_hours;
    }
  }


  checkDateGreatThanToday(control: FormControl) {
    if (DATE.DateLessToday(control.value)) {
      return {
        checkDateGreatThanToday: true
      };
    }
  }

  submitValidate(): boolean {
    this.confirmDisplay = true;
    this.reviewValue = null;
    this.submitting = true;
    this.cs.postDataExtra(CONFIG.apiURL.page.timesheet.pel.review, this.submitedValue)
      .subscribe(
        (res) => {
          if (this.CheckResponseMessage(res.data)) {
            this.reviewValue = res.extra;
          } else {
            this.confirmDisplay = false;
            this.PushWarnMessage(res.data);
          }
          this.afterSubmit.emit();
          this.submitting = false;
        },
        (err:any)=>
        {
          super.PushErrorMessage(err);
          this.submitting=false;
        }
      );
    return false;
  }

  formValidateBefore() {
  }

  confirm() {
    this.confirmDisplay = false;
    this.confirmStart.emit();
    this.beforeSubmit.emit();
    this.formPost();
  }

  lessThanDateCompare() {
    return (group: FormGroup): { [key: string]: any } => {
      const d_start = group.controls['date_start'].value;
      const d_end = group.controls['date_end'].value;

      if (d_end < d_start) {
        return {
          EndDateGreatThanStartDate: d_end < d_start ? true : null,
        };
      }
      return null;
    }
  }

  submitSuccess() {
    this.summary = this.extraData.summary;
  }
}
