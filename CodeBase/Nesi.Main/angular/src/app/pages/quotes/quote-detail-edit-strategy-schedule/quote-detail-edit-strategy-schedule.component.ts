import { Component, OnInit, Input } from '@angular/core';
import { CONFIG } from 'app/configuration';
import { CoreService } from 'app/services/shared/core.service';
import { Store } from '@ngrx/store';
import * as fromMessage from 'actions/layout/growlMessage';
import * as fromRoot from '../../../reducers';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-detail-edit-strategy-schedule',
  templateUrl: './quote-detail-edit-strategy-schedule.component.html',
  styleUrls: ['./quote-detail-edit-strategy-schedule.component.css']
})
export class QuoteDetailEditStrategyScheduleComponent implements OnInit {
  qs: any;
  @Input()
  set QS(value: any) {
    if (!this.qs) {
      this.qs = value;
    }
  };
  protected store: Store<fromRoot.State>
  get buId(): string {
    if (this.qs) {
      return this.qs.buId;
    } else {
      return null;
    }
  }

  get quote_id(): string {
    if (this.qs) {
      return this.qs.quote_id;
    } else {
      return null;
    }
  }

  blocked = false;
  constructor(
    private cs: CoreService,
  ) { }

  ngOnInit() {
  }

  itemButtonClick(event) {
    // const item = event.data;
    // CONFIG.LOG(item, 'itembutton click event in ')
    // switch (item.name) {
    //   case '':
    //     break;
    //   default:
    //     break;
    // }
    this.reloadQs();
  }

  assigntoChanged(event) {
    // this.reloadQs();
  }

  reloadQs() {
    this.blocked = true;
    this.cs.getObject<any>(CONFIG.apiURL.page.quotes.initStrategy + this.qs.quote_id)
      .subscribe(
      (res) => {
        this.blocked = false;
        this.qs = res;
      },
      (err:any)=>{
        this.store.dispatch(new fromMessage.PushWarnMessage(err));
      }
    
      );
  }

  dialogSubmited(event) {
    CONFIG.LOG(event, 'event on dialog submitted');
    this.reloadQs();
  }
}
