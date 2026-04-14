import { Store } from '@ngrx/store';
import * as fromRoot from '../reducers';
import * as fromMessage from '../actions/layout/growlMessage';
import { PushInfoMessage, PushMessage, PushSuccessMessage, ClearMessage } from '../actions/layout/growlMessage';
import { GrowlMessage } from '../models/layout/growlMessage';
import { CONFIG } from '../configuration';
import { LabelValueInt } from '../models/Shared/labelValueString';
import { concat } from '../../../node_modules/rxjs-compat/operator/concat';

export abstract class MessageBase {
  shortMsgTime = 500;
  tabName: string;
  tabLabels: string[] = [];
  activeTab = 0;

  constructor(
    protected store: Store<fromRoot.State>,
  ) {

  }

  tabChanged(event) {
    if (this.tabLabels && this.tabLabels.length > 0) {
      this.tabName = this.tabLabels[event.index];
    }
    this.activeTab = event.index;
    this.ClearMessage();
  }

  protected GetLabelListFromLabelValueInt(list: LabelValueInt[]): string[] {
    if (!list) {
      return;
    }
    const res: string[] = [];
    list.forEach(x => res.push(x.label));
    return res;
  }

  protected GetValueListFromLabelValueInt(list: LabelValueInt[]): number[] {
    if (!list) {
      return;
    }
    const res: number[] = [];
    list.forEach(x => res.push(x.value));
    return res;
  }

  protected GetValueByLabelFromLabelValueInt(list: LabelValueInt[], label: string): number {
    if (!list) {
      return;
    } const res = list.filter(x => x.label === label);
    return res && res[0] ? res[0].value : -1;
  }

  protected GetLabelByValueFromLabelValueInt(list: LabelValueInt[], value: number): string {
    if (!list) {
      return;
    }
    const res = list.filter(x => x.value === value);
    return res && res[0] ? res[0].label : '';
  }

  protected GetObjectByValueFromLabelValueInt(list: LabelValueInt[], value: number): any {
    if (!list) {
      return;
    } const res = list.filter(x => x.value === value);
    return res && res[0];
  }

  public CheckResponseMessage(msg: string): boolean {
    return msg && msg.toLowerCase().indexOf('success') > -1;
  }

  public PushShortResponseMessage(msg: string): boolean {
    if (this.CheckResponseMessage(msg)) {
      this.PushShortSuccessMessage(msg);
      return true;
    } else {
      this.PushWarnMessage(msg);
      CONFIG.LOG(msg, 'error message in push short respons emessage');
      return false;
    }
  }

  public PushErrorResponseMessage(msg: string): boolean {
    if (this.CheckResponseMessage(msg)) {
      // this.PushShortSuccessMessage(msg);
      return true;
    } else {
      this.PushShortWarnMessage(msg, 5000);
      return false;
    }
  }

  public PushResponseMessage(msg: string): boolean {
    if (this.CheckResponseMessage(msg)) {
      if (msg !== 'success...') {
        this.PushSuccessMessage(msg);
      }
      return true;
    } else {
      this.PushErrorMessage(msg);
      CONFIG.LOG(msg, 'error message in push response message');
      return false;
    }


  }

  public PushResponseExtraMessage(data: string,extra: string): boolean {
    if (this.CheckResponseMessage(data)) {
      if (data !== 'success...') {
        this.PushSuccessMessage(data);
      }
      return true;
    } else {
      let heading= "Error(s) have occurred: ";
      extra = heading + " <br/>" + extra;
      this.PushErrorMessage(extra);
      CONFIG.LOG(extra, 'error message in push response message');
      return false;
    }


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

  public PushMultipleMessage(msg: GrowlMessage[]) {
    this.store.dispatch(new fromMessage.PushMultipleMessage(msg));
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
