import { Component, OnInit, Input } from '@angular/core';
import { QuoteEditFormBase } from '../_base/quoteEditFormBase';
import { FormBuilder } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { Store } from '@ngrx/store';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import * as fromRoot from '../../../reducers';
import { CONFIG } from '../../../configuration';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-detail-edit-strategy',
  templateUrl: './quote-detail-edit-strategy.component.html',
  styleUrls: ['./quote-detail-edit-strategy.component.css']
})
export class QuoteDetailEditStrategyComponent implements OnInit {

  qs: any;
  @Input()
  quote_id: number;

  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    private route: ActivatedRoute,
  ) {
    // super(winRef, store, cs, ts);
    // super.Init(CONFIG.apiURL.page.quotes.new);
  }
  // createForm() {
  //   this.userform = this.fb.group({
  //   });

  //   this.initiFormvalue = {
  //   }
  // }

  ngOnInit() {
    this.cs.getObject<any>(CONFIG.apiURL.page.quotes.initStrategy + this.quote_id)
      .subscribe(
      (res) => {
        this.qs = res;
      }
      );
  }

}
