import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from '../../../core/messageBaseComponent';
import * as DATE from '../../../services/helper/datetime';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetVacationPast',
  templateUrl: './timesheetVacationPast.component.html',
  styleUrls: ['./timesheetVacationPast.component.css']
})
export class TimesheetVacationPastComponent extends MessageBase implements OnInit {

  histories: any[];
  notes: Map<string, any>;
  @Output() deleted = new EventEmitter();

  constructor(
    private tss: TimesheetService,
    protected store: Store<fromRoot.State>,

  ) {
    super(store);
    this.notes = new Map();
  }

  ngOnInit() {
    this.loadHistory();
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
  OnRowExpand(event: any) {
    CONFIG.LOG(event.data, 'rowexpand on vacation history');
    const item = event.data;
    if (this.deleteDisabled(item)) {
      return;
    }
    if (this.notes.get(item.vacation_id)) {
      return;
    }
    this.loadNote(item.vacation_id);
  }

  loadNote(vacationId) {
    this.tss.getList<any>(CONFIG.apiURL.page.timesheet.vacation.notes + vacationId)
      .subscribe(
      res => this.notes.set(vacationId, res)
      ),
      (err:any)=>{
        this.PushErrorMessage(err);
      };
  }

  deleteDisabled(item): boolean {
    return (item.status === 'DENIED' || item.status === 'PAIDOUT')
      || (item.status === 'APPROVED' && DATE.DateLessToday(item.date_start));
  }

  deleteVacation(item) {
    CONFIG.LOG(item, 'delete vacation');
    this.tss.deleteString(CONFIG.apiURL.page.timesheet.vacation.delete + item.vacation_id)
      .subscribe(
      (res: string) => {
        super.PushResponseMessage(res);
        this.loadHistory();
        this.deleted.emit();
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }
  addNote(event: any, vacationId: number) {
    let text = '';
    try {
      text = event.target.children[0].value;
    } catch (e) {

    }
    CONFIG.LOG(text, 'text wehn addnote button click');

    CONFIG.LOG(vacationId, 'vacationId addnote button click');
    if (!text || text.length < 2) {
      super.PushWarnMessage('Please enter at least 2 characters in note.');
    }
    this.tss.postString(CONFIG.apiURL.page.timesheet.vacation.notes + vacationId, { data: text })
      .subscribe(
      (res: string) => {
        super.PushResponseMessage(res);
        this.loadNote(vacationId);
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      });

  }
}
