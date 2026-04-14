
import { Component, OnInit } from '@angular/core';
import { QuoteBase } from '../_base/quoteBase';
import { WindowRef } from '../../../services/shared/windowRef';
import { PostResult } from '../../../models/core/postResult';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-addnew',
  templateUrl: './quote-addnew.component.html',
  styleUrls: ['./quote-addnew.component.css']
})
export class QuoteAddnewComponent extends QuoteBase implements OnInit {

  constructor(
    protected winRef: WindowRef,
    private router: Router,
    protected store: Store<fromRoot.State>
  ) {
    super(winRef, store);
  }

  ngOnInit() {
  }

  addnewChanged(event: PostResult) {
    this.openQuote(event.result, '1');
    this.router.navigate(['/home/65/quotes']);
  }
}
