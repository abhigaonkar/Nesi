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
import { TimesheetPelScheduleComponent } from '../timesheet-pel-schedule/timesheet-pel-schedule.component';
import { TimesheetPelHistoryComponent } from '../timesheet-pel-history/timesheet-pel-history.component';
import { CoreService } from '../../../services/shared/core.service';

@Component({
  selector: 'nesi-timesheet-pel',
  templateUrl: './timesheet-pel.component.html',
  styleUrls: ['./timesheet-pel.component.css']
})
export class TimesheetPelComponent extends TimesheetPageBase implements OnInit {

  public Profile: TimeSheetProfile;
  public onsubmitting = false;

  public showWithdrawal = false;
  @ViewChild(TimesheetPelScheduleComponent)
  schedule: TimesheetPelScheduleComponent;
  @ViewChild(TimesheetPelHistoryComponent)
  past: TimesheetPelHistoryComponent;

  constructor(
    protected tss: TimesheetService,
    protected store: Store<fromRoot.State>,
    protected cs: CoreService,

  ) {
    super(tss, store);
  }

  ngOnInit() {
    super.Init();
    this.tabLabels = ['Schedule', 'Past Requests'];
    this.tabChanged({ index: 0 });
    this.loadData();
  }

  loadData() {
    this.onsubmitting = true;
    this.cs.getObject<any>(CONFIG.apiURL.page.timesheet.pel.profile)
      .subscribe(res => {
        this.schedule.summary = res.summary;
        this.past.histories = res.historyList;
        this.onsubmitting = false;
      },
      (err:any)=>{
        this.PushErrorMessage(err);
        this.onsubmitting = false;
      }
      );
  }


  onDelete(e) {
    this.schedule.summary = e.summary;
    this.past.histories = e.historyList;
  }

  setHistory(e) {
    this.past.histories = e.result.historyList;
  }

  afterProfileLoad(): void {

  }

  VacationChanged() {
    // if (this.past) {
    //   this.past.loadHistory();
    // }
    // if (this.schedule) {
    //   this.schedule.getHoursDollars();
    // }
  }

}
