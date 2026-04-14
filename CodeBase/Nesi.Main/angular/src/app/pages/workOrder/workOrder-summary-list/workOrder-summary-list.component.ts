import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { MessageBase } from 'app/core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { DatatableComponent } from 'app/components/nesi-datatable/components/datatable/datatable.component';
import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { WorkOrderService } from 'app/services/pages/workorder.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-workOrder-summary-list',
  templateUrl: './workOrder-summary-list.component.html',
  styleUrls: ['./workOrder-summary-list.component.css']
})
export class WorkOrderSummaryListComponent extends MessageBase implements OnInit {

  selected_business_unit: number;
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  property_name = 'WorkOrderHome_TabIndex';
  constructor(
    protected store: Store<fromRoot.State>,
    private ts: TokenService,
    protected cs: CoreService,
    public ws: WorkOrderService
  ) {
    super(store);
  }

  ngOnInit() {
    this.tabLabels = ['Summary', 'Work Order List'];
    // this.cs.getString(this.url)
    //   .subscribe(
    //     (res) => {
    //       this.tabChanged({ index: Number(res) });
    //     }
    //   );
    this.tabChanged({ index: 0 });
    this.selected_business_unit = this.ts.working_businessUnit;
    setTimeout(() => {
      this.loadDetail();
    }, 1000);
  }

  // get url() {
  //   return CONFIG.apiURL.layout.propertyValue + this.property_name;
  // }

  // tabChanged(event) {
  //   if (this.activeTab !== event.index) {
  //     this.cs.postData(this.url, { lable: this.property_name, value: event.index })
  //       .subscribe(
  //         res => { }
  //       );
  //   }
  //   super.tabChanged(event);
  // }
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
      this.dt.showReport('WorkOrderBusinessUnitDetail');
    }
  }

  openUrl(type,from) {
    CONFIG.LOG(type, 'type in open url work order summary');
    CONFIG.LOG(this.selected_business_unit,  'buId in open url work order summary');

    this.ws.openUrl(type, this.selected_business_unit , from);
  }
}
