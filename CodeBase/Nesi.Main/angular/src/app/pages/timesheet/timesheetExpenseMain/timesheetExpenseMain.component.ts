import { Component, OnInit, ViewChild } from '@angular/core';
import { TimesheetPageBase } from '../interface/timesheetPageBase';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { TimesheetExpenseHitoryComponent } from '../timesheetExpenseHitory/timesheetExpenseHitory.component';
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetExpenseMain',
  templateUrl: './timesheetExpenseMain.component.html',
  styleUrls: ['./timesheetExpenseMain.component.css']
})
export class TimesheetExpenseMainComponent extends TimesheetPageBase implements OnInit {

  public submitting = false;

  @ViewChild(TimesheetExpenseHitoryComponent)
  history: TimesheetExpenseHitoryComponent;

  constructor(
    protected tss: TimesheetService,
    protected store: Store<fromRoot.State>,

  ) {
    super(tss, store);
  }

  ngOnInit() {
    super.Init();
    this.tabLabels = ['Expense', 'Per Diem', 'History'];
    this.tabChanged({ index: 0 });
  }

  afterProfileLoad(): void {

  }

  reloadHistory() {
    this.history.load();
  }
}
