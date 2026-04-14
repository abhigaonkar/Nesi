import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from '../../../core/messageBaseComponent';
import * as DATE from '../../../services/helper/datetime';

@Component({
  selector: 'nesi-timesheet-pel-history',
  templateUrl: './timesheet-pel-history.component.html',
  styleUrls: ['./timesheet-pel-history.component.css']
})
export class TimesheetPelHistoryComponent extends MessageBase implements OnInit {

  @Input() histories: any[];
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
      }
      ;
  }

  deleteDisabled(item): boolean {
    return (item.status === 'DENIED' || item.status === 'PAIDOUT')
      || (item.status === 'APPROVED' && DATE.DateLessToday(item.date_start));
  }

  deleteVacation(item) {
    this.tss.deleteDataExtra(CONFIG.apiURL.page.timesheet.pel.delete + item.vacation_id)
      .subscribe(
        (res) => {
          super.PushResponseMessage(res.data);
          this.deleted.emit(res.extra);
        },
        (err:any)=>
        {
          super.PushErrorMessage(err);
        }
      );
  }

  addNote(event: any, vacationId: number) {
    let text = '';
    try {
      text = event.target.children[0].value;
    } catch (e) {

    }
    if (!text || text.length < 2) {
      super.PushWarnMessage('Please enter at least 2 characters in note.');
    }
    this.tss.postString(CONFIG.apiURL.page.timesheet.vacation.notes + vacationId, { data: text })
      .subscribe(
        (res: string) => {
          super.PushResponseMessage(res);
          this.loadNote(vacationId);
        },
        (err:any)=>
        {
          super.PushErrorMessage(err);
        });

  }
}
