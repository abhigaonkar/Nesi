import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { Store } from '@ngrx/store';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import { ConfirmationService } from 'primeng/primeng';
import * as fromRoot from '../../../reducers';
import { CONFIG } from '../../../configuration';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-edit-kill',
  templateUrl: './quote-edit-kill.component.html',
  styleUrls: ['./quote-edit-kill.component.css']
})
export class QuoteEditKillComponent extends QuoteEditFormBase implements OnInit {
  @Output()
  cancel = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    private route: ActivatedRoute,
    private cf: ConfirmationService,
  ) {
    super(winRef, store, cs, ts);
    super.Init(CONFIG.apiURL.page.quotes.updateStatus);
  }

  ngOnInit() {
  }

  createForm() {
    this.userform = this.fb.group({
      'quote_id': '',
      'revision': '',
      'why_lose': ['', Validators.required],
      'who_competitor': '',
      'what_price': '',
      'status_id': '',
    });

    this.initFormvalue = {
      'quote_id': this._q && this._q.quote_id,
      'revision': this._q && this._q.revision,
      'why_lose': '',
      'who_competitor': '',
      'what_price': '',
      'status_id': '',
    }

  }

  formValidateBefore() {
    this.userform.get('quote_id').setValue(this._q.quote_id);
    this.userform.get('revision').setValue(this._q.revision);
    this.killQuote();
  }


  killQuote() {
    if (!this._q) { return; }
    let statustosend;
    if (this._q.status_id === '6' || this._q.status_id === '9') {
      if (this._q.prev_status_id === 'NULL') {
        statustosend = '2';
      } else {
        statustosend = this.q.prev_status_id;
      }
    } else {
      statustosend = '6';
    }
    this.userform.get('status_id').setValue(statustosend);
  }
}
