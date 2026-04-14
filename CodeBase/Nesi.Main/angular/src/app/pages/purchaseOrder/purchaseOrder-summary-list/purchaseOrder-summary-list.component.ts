import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { MessageBase } from 'app/core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { TokenService } from 'app/services/authentication/tokenService';
import { DatatableComponent } from 'app/components/nesi-datatable/components/datatable/datatable.component';
import { CONFIG } from 'app/configuration';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-purchaseOrder-summary-list',
  templateUrl: './purchaseOrder-summary-list.component.html',
  styleUrls: ['./purchaseOrder-summary-list.component.css']
})
export class PurchaseOrderSummaryListComponent extends MessageBase implements OnInit {

  selected_business_unit: number;
  @ViewChild(DatatableComponent) dt: DatatableComponent;
  apstatusList = [{ label: '0', value: 0 }];
  constructor(
    protected store: Store<fromRoot.State>,
    private ts: TokenService,
    private cs: CoreService,
  ) {
    super(store);
  }

  ngOnInit() {
    this.tabLabels = ['Summary', 'Purchase Order List', 'Details 2'];
    this.tabChanged({ index: 0 });
    this.selected_business_unit = this.ts.working_businessUnit;
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.purchaseorder.apStatus)
      .subscribe(
        (res) => {
          this.apstatusList = res;
        }
      );
    setTimeout(() => {
      this.loadDetail();
    }, 1000);
  }

  tabChanged(e) {
    super.tabChanged(e);
    if (this.activeTab === 1) {
      setTimeout(() => {
        this.dt.fixed_header_scroll = 210;
        this.dt.copyColumnWidth();
      }, 500);
    } else {
      this.dt.fixed_header_scroll = 0;
    }
  }

  loadDetail(refresh_button = false) {
    this.ts.working_businessUnit = this.selected_business_unit;
    this.dt.reportQueryParam = [{ coulumnname: 'business_unit', value: this.selected_business_unit }];
    this.dt.refreshCache = true;
    if (refresh_button) {
      this.dt.loadReport();
    } else {
      this.dt.showReport('PurchaseOrderBusinessUnitDetail');
    }
  }

  noteUpdate(event) {
    CONFIG.LOG(event.field, 'field in ap status update');
    CONFIG.LOG(event.index, 'index in note update');
    CONFIG.LOG(event.data, 'data in note update');
    if (event.field === 'apnotes') {
      this.updateAPNotes(event.data);
    }
  }

  apstatusUpdate(event) {
    CONFIG.LOG(event.field, 'field in ap status update');
    CONFIG.LOG(event.index, 'index in ap status update');
    CONFIG.LOG(event.data, 'data in ap status update');
    if (event.field === 'poprog_apstatus') {
      this.updateAPStatus(event.data);
    }
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
