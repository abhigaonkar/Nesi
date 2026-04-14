import { Component, OnInit, EventEmitter, Output, Input, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';
import {
  OverlayPanel
} from 'primeng/primeng';

@Component({
  selector: 'masterQuote-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})
export class MasterQuoteEditorComponent extends FormMessageBase implements OnInit {
  public _masterQuote: any;
  @ViewChild(OverlayPanel) overlayPanel: OverlayPanel;

  @Input()
  set masterQuote(value: any) {
    if (!value) {
      return;
    }

    this._masterQuote = value;

    this.userform.reset({
      quote: this._masterQuote.quote,
      rev: this._masterQuote.rev,

      date_due: {value: this.checkDate(this._masterQuote.date_due), disabled: this.dueDateDisabledOrNot(this._masterQuote.status_id)}, 
      completion:  this.checkDate(this._masterQuote.completion),
      pct_chance: this._masterQuote.pct_chance,
      pct_chance_reason: this._masterQuote.pct_chance_reason,
      qO_startdate:  this.checkDate(this._masterQuote.qO_startdate),
      parallel_bid: this._masterQuote.parallel_bid,
      follow_up: this._masterQuote.follow_up,
      exp_podate:  this.checkDate(this._masterQuote.exp_podate),
      notes: this._masterQuote.notes,
      newNote: '',
      status_id: this._masterQuote.status_id,
      copy: this._masterQuote
    });

    // console.dir(this.userform);
  }

  @Input('editLink') editLink = '';
  @Input('noteLink') noteLink = '';
  @Input('reasonForChangeList') optionList = [];

  @Output() onExit = new EventEmitter();
  @Output() update = new EventEmitter<any>();

  public dueDateReadony = true;
  constructor(private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService) {
    super(store, cs);
    this.createForm();
  }

  ngOnInit() {
    this.postUrl = this.editLink;

    this.onChange.subscribe(
      this.success
    );
  }

  public createForm() {
    this.userform = this.fb.group({
      quote:'',
      rev: '',
      date_due: '',
      completion: '',
      pct_chance: '',
      pct_chance_reason: '',
      qO_startdate: '', // expected start date
      parallel_bid: '',
      follow_up: '',
      notes: '',
      exp_podate: '',
      newNote: '',
      status_id: '',
      copy: ''
    });
  }

  exit() {
    this.onExit.emit();
  }

  updateNote (data) {
    // this.update.emit({quote: this._masterQuote, note: this.userform.get('newNote').value});
    let quote = this._masterQuote;
    let note = this.userform.get('newNote').value;
    quote.note = note;
    this.cs.postDataExtra(this.noteLink, quote).subscribe(
      (res) => {
        quote.notes = res.extra;

        // this.userform.reset({
        //   quote: this._masterQuote.quote,
        //   rev: this._masterQuote.rev,
    
        //   date_due: this.checkDate(this._masterQuote.date_due),
        //   completion:  this.checkDate(this._masterQuote.completion),
        //   pct_chance: this._masterQuote.pct_chance,
        //   pct_chance_reason: this._masterQuote.pct_chance_reason,
        //   qO_startdate:  this.checkDate(this._masterQuote.qO_startdate),
        //   parallel_bid: this._masterQuote.parallel_bid,
        //   follow_up: this._masterQuote.follow_up,
        //   exp_podate:  this.checkDate(this._masterQuote.exp_podate),
        //   notes: res.extra,
        //   newNote: '',
        //   copy: this._masterQuote
        // });

        this.userform.get('notes').setValue(res.extra);
        this.userform.get('newNote').setValue('');

        this.overlayPanel.hide();
      }
    );
  }

  success(event) {
    // console.dir(event);
      event.post.copy.date_due = event.result.date_due;
      event.post.copy.completion = event.result.completion;
      event.post.copy.pct_chance = event.result.pct_chance;
      event.post.copy.pct_chance_reason = event.result.pct_chance_reason;
      event.post.copy.qO_startdate = event.result.qO_startdate;
      event.post.copy.parallel_bid = event.result.parallel_bid;
      event.post.copy.follow_up = event.result.follow_up;
      event.post.copy.exp_podate = event.result.exp_podate;
  }

  private checkDate(d: any) {
    if (d === null || d.toString() === '1901-01-01' || d.toString() === '1901-01-01 00:00:00'
      || d.toString() === '0001-01-01' || d.toString() === '0001-01-01 00:00:00') {
      return null;
    }

    return d;
  }

  private dueDateDisabledOrNot (status_id) {
    if (status_id == 9 || status_id == 8 || status_id == 7 || status_id == 6 || status_id == 5 ||
        status_id == 13 || status_id == 4) {
        return true;
    }

    return false;
  }
}
