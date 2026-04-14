import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-edit-maintenance',
  templateUrl: './quote-edit-maintenance.component.html',
  styleUrls: ['./quote-edit-maintenance.component.css']
})
export class QuoteEditMaintenanceComponent extends QuoteEditFormBase implements OnInit {

  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    private route: ActivatedRoute,
  ) {
    super(winRef, store, cs, ts);
    super.Init(CONFIG.apiURL.page.quotes.secheduleFollowup);
  }
  createForm() {
    this.userform = this.fb.group({
      'quote_id': '',
      'revision': '',
      'schedule_date': ['', Validators.required],
      'follow_up_note': ['', [Validators.required, Validators.pattern(CONFIG.SQL_PATTERN)]],
    });

    this.initFormvalue = {
      'quote_id': this.q && this.q.quote_id,
      'revision': this.q && this.q.revision,
      'schedule_date': '',
      'follow_up_note': '',
    }
  }

  formValidateBefore() {
    this.userform.get('quote_id').setValue(this._q.quote_id);
    this.userform.get('revision').setValue(this._q.revision);
  }

  submitSuccess() {
    this.loadHistories();
  }

  ngOnInit() {
  }

  loadHistories() {
    this.cs.getList<any>(CONFIG.apiURL.page.quotes.getFollowupHistory + this._q.quote_id + '/' + this._q.revision)
      .subscribe(
      (res) => {
        this._q.followup_history = res;
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }

  getAction(type: string) {
    switch (type) {
      case 'f':
        return 'Follow Up';
      case 'h':
        return 'History Item';
      case 'n':
        return 'Note';
      case 's':
        return 'Status Changed';
      default:
        return '';
    }
  }
  getNote(row: any) {
    let note = row.note;
    if (row.type === 's') {
      note = note.replace(' from: \'', ' from: <b style=\'color:#f00\'>\'');
      note = note.replace(' to \'', '</b> to <b style=\'color:#f00\'>\'');
      note = note + '</b>';
    }
    return note;
  }
  OnRowExpand(event: any) {
    CONFIG.LOG(event.data, 'rowexpand on maintenance history');
    const item = event.data;
    // if (this.notes.get(item.vacation_id)) {
    //   return;
    // }
    // this.loadNote(item.vacation_id);
  }

  clickRow(table, event: any) {
    table.toggleRow(event.data);
  }


  doneFollowUp(row: any) {
    const id = row.id.substring(1);
    this.cs.postString(CONFIG.apiURL.page.quotes.doneFollowUp + id, null)
      .subscribe(
      (res) => {
        if (this.PushShortResponseMessage(res)) {
          row.is_done = 1;
        }
      }
      )
  }
}
