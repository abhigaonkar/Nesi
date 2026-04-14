
import { Component, OnInit, Input } from '@angular/core';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { MessageBase } from 'app/core/messageBaseComponent';
import { MessagespageBase } from 'app/pages/messagesPage/_base/messagespage.base';
import { OrderSummaryBase } from 'app/components/shared/Bases/OrderSummaryBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-workOrder-summary',
  templateUrl: './workOrder-summary.component.html',
  styleUrls: ['./workOrder-summary.component.css']
})
export class WorkOrderSummaryComponent extends OrderSummaryBase implements OnInit {
  can_view_margin: boolean;
  can_view_total_TM: boolean;
  can_cut_WO: boolean;


  constructor(
    public store: Store<fromRoot.State>,
    public cs: CoreService,
    public ts: TokenService,
  ) {
    super(store, cs, ts);
  }

  init() {
    this.urls = CONFIG.apiURL.page.workOrder;
  }

  get init_sum() {
    return {
      just_scanned: 0,
      total: {
        count: 0,
        value: 0,
      },
      open: {
        count: 0,
        value: 0,
      },
      open_pos: {
        count: 0,
        value: 0,
      },
      bm_approval: {
        count: 0,
        value: 0,
      },
      pm_approval: {
        count: 0,
        value: 0,
      },
      to_be_invoiced: {
        count: 0,
        value: 0,
      },
      questions: {
        count: 0,
        value: 0,
      },
      on_hold: {
        count: 0,
        value: 0,
      },
      rework: {
        count: 0,
        value: 0,
      },
      waiting_cut_po: {
        count: 0,
        value: 0,
      },
      init_prep: {
        count: 0,
        value: 0,
      },
      being_processed: {
        count: 0,
        value: 0,
      },
    };
  }

  load_init(res: any) {
    this.can_view_margin = res.can_view_margin;
    this.can_view_total_TM = res.can_view_total_TM;
    this.can_cut_WO = res.can_cut_WO;
  }

  caluculateSum() {
    this.sum = this.init_sum;
    this.summaryList.forEach(event => {
      if (event.just_scanned && !isNaN(Number(event.just_scanned))) {
        this.sum.just_scanned += Number(event.just_scanned);
      }
      if (event.total) {
        this.sum.total.count += event.total.count;
        this.sum.total.value += event.total.value;
      }
      if (event.open) {
        this.sum.open.count += event.open.count;
        this.sum.open.value += event.open.value;
      }
      if (event.open_pos) {
        this.sum.open_pos.count += event.open_pos.count;
        this.sum.open_pos.value += event.open_pos.value;
      }
      if (event.bm_approval) {
        this.sum.bm_approval.count += event.bm_approval.count;
        this.sum.bm_approval.value += event.bm_approval.value;
      }
      if (event.pm_approval) {
        this.sum.pm_approval.count += event.pm_approval.count;
        this.sum.pm_approval.value += event.pm_approval.value;
      }
      if (event.to_be_invoiced) {
        this.sum.to_be_invoiced.count += event.to_be_invoiced.count;
        this.sum.to_be_invoiced.value += event.to_be_invoiced.value;
      }
      if (event.questions) {
        this.sum.questions.count += event.questions.count;
        this.sum.questions.value += event.questions.value;
      }
      if (event.on_hold) {
        this.sum.on_hold.count += event.on_hold.count;
        this.sum.on_hold.value += event.on_hold.value;
      }
      if (event.rework) {
        this.sum.rework.count += event.rework.count;
        this.sum.rework.value += event.rework.value;
      }
      if (event.waiting_cut_po) {
        this.sum.waiting_cut_po.count += event.waiting_cut_po.count;
        this.sum.waiting_cut_po.value += event.waiting_cut_po.value;
      }
      if (event.init_prep) {
        this.sum.init_prep.count += event.init_prep.count;
        this.sum.init_prep.value += event.init_prep.value;
      }
      if (event.being_processed) {
        this.sum.being_processed.count += event.being_processed.count;
        this.sum.being_processed.value += event.being_processed.value;
      }
    });
  }
}
