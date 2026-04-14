import { Component, OnInit, ViewChild } from '@angular/core';
import { TimesheetPageBase } from '../interface/timesheetPageBase';
import { TimeSheetProfile } from '../../../models/pages/timesheet/timesheetProfile';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { TimesheetVacationPastComponent } from '../timesheetVacationPast/timesheetVacationPast.component';
import { CONFIG } from 'app/configuration';
import { TimesheetVacationScheduleComponent } from 'app/pages/timesheet/timesheetVacationSchedule/timesheetVacationSchedule.component';
// tslint:disable-next-line:max-line-length
import { TimesheetVacationWithdrawalComponent } from 'app/pages/timesheet/timesheetVacationWithdrawal/timesheetVacationWithdrawal.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetVacation',
  templateUrl: './timesheetVacation.component.html',
  styleUrls: ['./timesheetVacation.component.css']
})
export class TimesheetVacationComponent extends TimesheetPageBase implements OnInit {

  public Profile: TimeSheetProfile;
  public onsubmitting = false;

  public showWithdrawal = false;
  @ViewChild(TimesheetVacationPastComponent)
  past: TimesheetVacationPastComponent;
  @ViewChild(TimesheetVacationScheduleComponent)
  schedule: TimesheetVacationScheduleComponent;

  @ViewChild(TimesheetVacationWithdrawalComponent)
  withdrawl: TimesheetVacationWithdrawalComponent;

  constructor(
    protected tss: TimesheetService,
    protected store: Store<fromRoot.State>,

  ) {
    super(tss, store);
  }

  ngOnInit() {
    super.Init();
    this.tabLabels = ['Schedule', 'Withdrawal', 'Past Requests'];
    this.tabChanged({ index: 0 });
    this.tss.getBoolean(CONFIG.apiURL.page.timesheet.vacation.paytypeId)
      .subscribe(
      (res: boolean) => {
        this.showWithdrawal = res;
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }

  afterProfileLoad(): void {

  }

  VacationChanged() {
    if (this.past) {
      this.past.loadHistory();
    }
    if (this.schedule) {
      this.schedule.getHoursDollars();
    }
    if (this.withdrawl) {
      this.withdrawl.getHoursDollars();
    }
  }

}
