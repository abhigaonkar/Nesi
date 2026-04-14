import { Component, OnInit } from '@angular/core';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { QuoteBase } from '../_base/quoteBase';
import { CoreService } from 'app/services/shared/core.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromMessage from '../../../actions/layout/growlMessage';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quoteMenu',
  templateUrl: './quoteMenu.component.html',
  styleUrls: ['./quoteMenu.component.css']
})
export class QuoteMenuComponent extends QuoteBase implements OnInit {
  quoteId: string;
  url: string;

  constructor(
    protected winRef: WindowRef,
    private route: ActivatedRoute,
    private cs: CoreService,
    protected store: Store<fromRoot.State>
  ) {
    super(winRef, store);
  }

  ngOnInit() {
    this.route.url.subscribe(
      (url) => {
        CONFIG.LOG(url, 'quotes menubar init');
        this.url = url.toString();
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
    )
  }


  goQuote() {
    if (!this.quoteId || !this.quoteId.trim() ) {
      this.store.dispatch(new fromMessage.PushWarnMessage(`Please input quote ID.`));
      return;
    }

    const len = this.quoteId.trim().length;
    let converted = parseInt(this.quoteId.trim(), 10);
    const len2 = converted.toString().length;
    if ( len != len2 ) {
      this.store.dispatch(new fromMessage.PushWarnMessage(`Please input a valid quote ID.`));
      return;
    }

    this.cs.getData<boolean>(CONFIG.apiURL.page.quotes.exist + this.quoteId.toString())
      .subscribe(
        (res: boolean) => {
          if(!res) {
            this.store.dispatch(new fromMessage.PushInfoMessage(`Quote '${this.quoteId.toString()}' doesn't exist.`));
          } else {
            this.getQuote();
          }
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
    );
  }

  private getQuote() {
    this.cs.getString(CONFIG.apiURL.page.quotes.getActiveRevision + this.quoteId.toString())
      .subscribe(
        (res) => {
          this.openQuote(this.quoteId.toString(), res);
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        }
    );
  }

}
