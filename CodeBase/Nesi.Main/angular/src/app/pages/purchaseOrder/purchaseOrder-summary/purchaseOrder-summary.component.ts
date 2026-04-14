
import { Component, OnInit } from '@angular/core';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { MessageBase } from 'app/core/messageBaseComponent';
import { MessagespageBase } from 'app/pages/messagesPage/_base/messagespage.base';
import { OrderSummaryBase } from 'app/components/shared/Bases/OrderSummaryBase';

@Component({
  selector: 'nesi-purchaseOrder-summary',
  templateUrl: './purchaseOrder-summary.component.html',
  styleUrls: ['./purchaseOrder-summary.component.css']
})
export class PurchaseOrderSummaryComponent extends OrderSummaryBase implements OnInit {


  constructor(
    public store: Store<fromRoot.State>,
    public cs: CoreService,
    public ts: TokenService,
  ) {
    super(store, cs, ts);
  }

  init() {
    this.urls = CONFIG.apiURL.page.purchaseorder;
  }


  get init_sum() {
    return {
      total: {
        count: 0,
        value: 0,
      },
      just_cut: {
        count: 0,
        value: 0,
      },
      waiting_approval: {
        count: 0,
        value: 0,
      },
      waiting_parts: {
        count: 0,
        value: 0,
      }, 
      be_issued: {
        count: 0,
        value: 0,
      },
      questions: {
        count: 0,
        value: 0,
      },
      ap_problems: {
        count: 0,
        value: 0,
      },
      waiting_confirmation: {
        count: 0,
        value: 0,
      },
    };
  }


  caluculateSum() {
    this.sum = this.init_sum;
    this.summaryList.forEach(event => {
      if (event.total) {
        this.sum.total.count += event.total.count;
        this.sum.total.value += event.total.value;
      }
      if (event.just_cut) {
        this.sum.just_cut.count += event.just_cut.count;
        this.sum.just_cut.value += event.just_cut.value;
      }
      if (event.waiting_approval) {
        this.sum.waiting_approval.count += event.waiting_approval.count;
        this.sum.waiting_approval.value += event.waiting_approval.value;
      }
      if (event.waiting_parts) {
        this.sum.waiting_parts.count += event.waiting_parts.count;
        this.sum.waiting_parts.value += event.waiting_parts.value;
      }
      if (event.be_issued) {
        this.sum.be_issued.count += event.be_issued.count;
        this.sum.be_issued.value += event.be_issued.value;
      }
      if (event.questions) {
        this.sum.questions.count += event.questions.count;
        this.sum.questions.value += event.questions.value;
      }
      if (event.ap_problems) {
        this.sum.ap_problems.count += event.ap_problems.count;
        this.sum.ap_problems.value += event.ap_problems.value;
      }
      if (event.waiting_confirmation) {
        this.sum.waiting_confirmation.count += event.waiting_confirmation.count;
        this.sum.waiting_confirmation.value += event.waiting_confirmation.value;
      }
    });
  }
}
