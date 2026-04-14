
import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';
import { QuoteEditRowBase } from '../_base/quoteEditRowsBase';
import { ConfirmationService } from 'primeng/primeng';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-edit-noteadddress',
  templateUrl: './quote-edit-noteadddress.component.html',
  styleUrls: ['./quote-edit-noteadddress.component.css']
})
export class QuoteEditNoteadddressComponent extends QuoteEditRowBase implements OnInit {

  constructor(
    protected fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    protected route: ActivatedRoute,
    protected cf: ConfirmationService,
  ) {
    super(fb, ts, cs, store, winRef, route, cf);
    this.type = 2;
  }

  createSection(row: any) {
    this.cs.getObject<any>(CONFIG.apiURL.page.quotes.editNewSection
      + this._q.quote_id + '/' + this._q.revision + '/' + row.get('row_id').value)
      .subscribe(
      (res) => {
        row.get('section_id').setValue(res['section_id']);
        this.rowUpdated.emit(null);
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }

}
