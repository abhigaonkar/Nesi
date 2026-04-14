
import { Component, OnInit, Input } from '@angular/core';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { MessageBase } from 'app/core/messageBaseComponent';
import { MessagespageBase } from 'app/pages/messagesPage/_base/messagespage.base';
import { SelectItem } from 'primeng/primeng';
import { OrderSummaryDetailBase } from 'app/components/shared/Bases/OrderSummaryDetailBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-workOrder-summary-detail',
  templateUrl: './workOrder-summary-detail.component.html',
  styleUrls: ['./workOrder-summary-detail.component.css']
})
export class WorkOrderSummaryDetailComponent extends OrderSummaryDetailBase implements OnInit {
  constructor(
    public store: Store<fromRoot.State>,
    public cs: CoreService,
    public ts: TokenService,
  ) {
    super(store, cs, ts);
  }

  init() {
    this.url=CONFIG.apiURL.page.workOrder.summaryDetail;
    this.columnOptions = [
      { label: 'Wo No', value: 'Wo No' },
      { label: 'Customer Name', value: 'Customer Name' },
      { label: 'Cust PO', value: 'Cust PO' },
      { label: 'Status', value: 'Status' },
      { label: 'Last Modified', value: 'Last Modified' },
      { label: 'Last Modified By', value: 'Last Modified By' },
      { label: 'Still to be Billed', value: 'Still to be Billed' },
      { label: 'Cut Date', value: 'Cut Date' },
      { label: 'Quote', value: 'Quote' },
      { label: 'PM', value: 'PM' },
      { label: 'Invoice #', value: 'Invoce #' },
      { label: 'Contact', value: 'Contact' },
      { label: 'Next Date', value: 'Next Date' },
      { label: 'On Hold', value: 'On Hold' },
      { label: 'Margin', value: 'Margin' },
    ];
  }
}
