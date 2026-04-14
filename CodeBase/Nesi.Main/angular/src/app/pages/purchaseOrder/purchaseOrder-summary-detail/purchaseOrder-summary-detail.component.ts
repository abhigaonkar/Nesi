
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
import { LabelValueInt } from 'app/models/Shared/labelValueString';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-purchaseOrder-summary-detail',
  templateUrl: './purchaseOrder-summary-detail.component.html',
  styleUrls: ['./purchaseOrder-summary-detail.component.css']
})
export class PurchaseOrderSummaryDetailComponent extends OrderSummaryDetailBase implements OnInit {
  apStatus: LabelValueInt[];

  constructor(
    public store: Store<fromRoot.State>,
    public cs: CoreService,
    public ts: TokenService,
  ) {
    super(store, cs, ts);
  }

  init() {
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.purchaseorder.apStatus)
      .subscribe(
      (res) => {
        this.apStatus = res;
      }
      );

    this.url = CONFIG.apiURL.page.purchaseorder.summaryDetail;
    this.columnOptions = [
      { label: 'PO', value: 'PO' },
      { label: 'Vendor', value: 'Vendor' },
      { label: 'Cut', value: 'Cut' },
      { label: 'Issued', value: 'Issued' },
      { label: 'Required', value: 'Required' },
      { label: 'Description', value: 'Description' },
      { label: 'Status', value: 'Status' },
      { label: 'Shipping', value: 'Shipping' },
      { label: 'Cost', value: 'Cost' },
      { label: 'Rec Cost', value: 'Rec Cost' },
      { label: 'Purchaser', value: 'Purchaser' },
      { label: 'AP Status', value: 'AP Status' },
      { label: 'AP Notes', value: 'AP Notes' },
    ];
  }


  detailLoaded() {
    this.items.forEach(x => x.has_notes = !!x.apnotes);
  }

  updateAPNotes(data) {
    this.cs.postString(CONFIG.apiURL.page.purchaseorder.updateAPNotes, { id: data.poprog_id, value: data.apnotes })
      .subscribe(
      (res) => {
        data.has_notes = !!data.apnotes;
      }
      );
  }

  updateAPStatus(data) {
    this.cs.postString(CONFIG.apiURL.page.purchaseorder.updateStatus, { id: data.poprog_id, value: data.poprog_apstatus })
      .subscribe(
      (res) => {

      }
      );
  }
}
