import { Component, OnInit } from '@angular/core';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { CONFIG } from '../../../configuration';
import { MessageBase } from '../../../core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetExpenseHitory',
  templateUrl: './timesheetExpenseHitory.component.html',
  styleUrls: ['./timesheetExpenseHitory.component.css']
})
export class TimesheetExpenseHitoryComponent extends MessageBase implements OnInit {

  public histories: any[];
  public minDate: Date;

  constructor(
    public tss: TimesheetService,
    protected store: Store<fromRoot.State>
  ) {
    super(store);
  }

  ngOnInit() {
    this.load();
  }

  public load() {
    this.tss.getObject<any>(CONFIG.apiURL.page.timesheet.expense.history)
      .subscribe(
      res => {
        this.histories = res.histories;
        this.minDate = res.minDate;
      },
      (err:any)=>
      {
        super.PushErrorMessage(err);
      }
      );
  }

  delete(item: any) {
    this.tss.deleteString(CONFIG.apiURL.page.timesheet.expense.delete + item.id_expense)
      .subscribe(
      (res: string) => {
        if (super.PushResponseMessage(res)) {
          this.load();
        }
      },
      (err:any)=>
      {
        super.PushErrorMessage(err);
      }
      );
  }

}
