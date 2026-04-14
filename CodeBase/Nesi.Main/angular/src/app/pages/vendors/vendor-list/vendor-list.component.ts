import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from 'app/core/messageBaseComponent';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { VendorsService } from '../../../services/pages/vendor.services';
import { WindowRef } from '../../../services/shared/windowRef';
@Component({
  selector: 'nesi-vendor-list',
  templateUrl: './vendor-list.component.html',
  styleUrls: ['./vendor-list.component.css']
})
export class VendorListComponent extends MessageBase implements OnInit {
  profile: any;
  vendor_id: number;
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor(
    protected store: Store<fromRoot.State>,
    private cs: CoreService,
    private vendorS: VendorsService,
    protected winRef: WindowRef
  ) {
    super(store);
  }
  ngOnInit() {
    this.tabLabels = ['Search',' Details'];
    this.tabName = this.tabLabels[0];
    // this.cs.getObject<any>(CONFIG.apiURL.page.vendors.profile)
    //   .subscribe(
    //     (res) => {
    //       this.profile = res;
    //     }
    //   );
  }

  open_vendor(event) {
    CONFIG.LOG(event, 'open vendor event');
    this.vendor_id = event.result || (event.data && event.data.vendor_id);
    setTimeout(() => {
      this.activeTab = 1;
    }, 200);
  }

  loadDetail(refresh = false) {
    this.dt.after_onRefresh();
  }
  new_vendor_request(){
    const url = CONFIG.Nesi1URL.requestCustOrVendor.replace('@type', '2').replace('@customer_id','0');
    this.winRef.boingNesi1(url ,'VendorRequest'+Math.random(),"500,700");
  }
}
