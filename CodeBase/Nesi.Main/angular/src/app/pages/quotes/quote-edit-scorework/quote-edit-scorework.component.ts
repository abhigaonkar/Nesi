import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { ConfirmationService } from 'primeng/primeng';
import { QuoteEditRowBase } from '../_base/quoteEditRowsBase';
import { DataExtra } from 'app/models/core/dataExtra';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-edit-scorework',
  templateUrl: './quote-edit-scorework.component.html',
  styleUrls: ['./quote-edit-scorework.component.css']
})
export class QuoteEditScoreworkComponent extends QuoteEditRowBase implements OnInit {

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
  }

 

}
