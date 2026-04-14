import { WindowRef } from '../../../services/shared/windowRef';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromMessage from 'actions/layout/growlMessage';

export abstract class QuoteBase {
  shortMsgTime = 500;
  constructor(
    protected winRef: WindowRef,
    protected store: Store<fromRoot.State>
  ) { }

  public openQuote(quoteId: string, rev: string) {
    // CONFIG.LOG(line, 'open quote window');
    if (!quoteId) { return; }
    const url = CONFIG.opens.quoteEdit.replace('@id', quoteId).replace('@rev', rev);
    this.winRef.boing(url, 'quote_' + quoteId, 1600, 960);
  }

  

  public ClearMessage() {
    this.store.dispatch(new fromMessage.ClearMessage());
  }

  public PushShortInfoMessage(msg: string, time: number = this.shortMsgTime) {
    this.store.dispatch(new fromMessage.PushInfoMessage(msg));
    setTimeout(() => {
      this.ClearMessage();
    }, time);
  }
  public PushShortSuccessMessage(msg: string, time: number = this.shortMsgTime) {
    this.store.dispatch(new fromMessage.PushSuccessMessage(msg));
    setTimeout(() => {
      this.ClearMessage();
    }, time);
  }

  public PushShortErrorMessage(msg: string, time: number = this.shortMsgTime) {
    this.store.dispatch(new fromMessage.PushErrorMessage(msg));
    setTimeout(() => {
      this.ClearMessage();
    }, time);
  }

  public PushShortWarnMessage(msg: string, time: number = this.shortMsgTime) {
    this.store.dispatch(new fromMessage.PushWarnMessage(msg));
    setTimeout(() => {
      this.ClearMessage();
    }, time);
  }

  public PushWarnMessage(msg: string) {
    this.store.dispatch(new fromMessage.PushWarnMessage(msg));
  }
  public PushInfoMessage(msg: string) {
    this.store.dispatch(new fromMessage.PushInfoMessage(msg));
  }
  public PushErrorMessage(msg: string) {
    this.store.dispatch(new fromMessage.PushErrorMessage(msg));
  }
  public PushSuccessMessage(msg: string) {
    this.store.dispatch(new fromMessage.PushSuccessMessage(msg));
  }

  public LOG(msg: any, title: string): void {
    if (!msg) { return };
    CONFIG.LOG(msg, title);
  }
  public SetObjectValueFromObject(from: any, to: any) {
    for (const key in from) {
      if (from.hasOwnProperty(key) && typeof key !== 'function') {
        to[key] = from[key];
      }
    }
  }

  public removeEmptyValue(list: any[]): any[] {
    return list.filter(x => x.value && x.label);
  }

  public shortenText(text: string, length: number): string {
    if (!text) {
      return '';
    }
    return text.length < length ? text : text.substr(0, length - 1) + '...';
  }


  round(value: number, digital: number) {
    const d = Math.pow(10, digital);
    const o = Math.round(value * d) / d;
    return o;
  }

}
