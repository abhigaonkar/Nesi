import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { TokenService } from '../../../services/authentication/tokenService';
import { MessageBase } from '../../../core/messageBaseComponent';
import { CoreService } from '../../../services/shared/core.service';
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-search',
  templateUrl: './quote-search.component.html',
  styleUrls: ['./quote-search.component.css']
})
export class QuoteSearchComponent extends MessageBase implements OnInit {

  constructor(
    private ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,

  ) {
    super(store);
   }

  ngOnInit() {
  }

}
