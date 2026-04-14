import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { MessageBase } from 'app/core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { DataExtra } from 'app/models/core/dataExtra';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-anlysis',
  templateUrl: './picklist-anlysis.component.html',
  styleUrls: ['./picklist-anlysis.component.css']
})
export class PicklistAnlysisComponent extends MessageBase implements OnInit {

  @Input() edit_disabled = false;
  @Input() model: any;
  @Input() quote_id: string;
  @Input() revision: string;
  @Output() print = new EventEmitter();
  @Output() excel = new EventEmitter();
  @Output() priceUpdated = new EventEmitter();
  @Output() applyDiscount = new EventEmitter();
  priceEditable = false;
  is_applyDiscount = false;

  constructor(
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
  ) {
    super(store);
  }

  ngOnInit() {
  }

  apply_Discount() {
    CONFIG.LOG(this.model.quoteDiscount, 'applied discount');
    this.applyDiscount.emit({ quoteDiscount: this.model.quoteDiscount });
  }

  get materialMarginPercent() {
    return (this.model.totalMaterial) > 0 ? (this.model.totalMaterial - this.model.totalMaterialCost) / (this.model.totalMaterial) : 0;
  }

  get laborMarginPercent() {
    return (this.model.totalLabor) > 0 ? (this.model.totalLabor - this.model.totalLaborCost) / (this.model.totalLabor) : 0;
  }

  get totalMarginPercent() {
    return (this.model.totalQuote) > 0 ? (this.model.totalQuote - this.model.totalCost) / (this.model.totalQuote) : 0;
  }

  get expetedMarginPercent() {
    return (this.model.quote_price) > 0 ? (this.model.quote_price - this.model.totalCost) / (this.model.quote_price) : 0;
  }


  setFocus(el) {
    this.priceEditable = true;
    window.setTimeout(() => el.focus(), 200);
  }

  closeInplace() {
    this.priceEditable = false;

  }


  field_Changed(field: string) {
    const url = CONFIG.apiURL.page.quotes.editUpdate;
    this.cs.postObject<DataExtra>(url, {
      quoteId: this.quote_id,
      revision: this.revision,
      fieldName: field,
      fieldValue: this.model && this.model.quote_price
    }).subscribe(
      (res) => {
        if (this.PushShortResponseMessage(res.data)) {
          this.priceUpdated.emit(this.model && this.model.quote_price);
        }
      }
      );
  }
}
