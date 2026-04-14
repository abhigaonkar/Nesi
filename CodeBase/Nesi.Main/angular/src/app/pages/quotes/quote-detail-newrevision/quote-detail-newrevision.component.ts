import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { Store } from '@ngrx/store';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import * as fromRoot from '../../../reducers';
import { CONFIG } from '../../../configuration';
import { ConfirmationService } from 'primeng/primeng';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-detail-newrevision',
  templateUrl: './quote-detail-newrevision.component.html',
  styleUrls: ['./quote-detail-newrevision.component.css']
})
export class QuoteDetailNewrevisionComponent extends QuoteEditFormBase implements OnInit {
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
    super.Init(CONFIG.apiURL.page.quotes.editUpdate);
  }

  ngOnInit() {
  }

  createForm() {
    this.userform = this.fb.group({
      'note': ['', Validators.required],
    });
  }


  newVersionGo() {
    this.formSubmitAttempt = true;
    if (this.userform.valid) {
      this.cf.confirm({
        message: 'Please verify that you wish to revise this quote.',
        accept: () => {
          this.updateField('do_revision', this.userform.get('note').value);
          this.cancel.emit();
        },
        reject: () => {

        }
      });
    }
  }

}
