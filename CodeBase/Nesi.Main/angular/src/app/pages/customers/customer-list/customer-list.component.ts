import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from 'app/core/messageBaseComponent';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { CustomersService } from 'app/services/pages/customer.services';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { WindowRef } from '../../../services/shared/windowRef';

@Component({
  selector: 'nesi-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent extends MessageBase implements OnInit {
  profile: any;
  customer_id: number;
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor(
    protected store: Store<fromRoot.State>,
    private cs: CoreService,
    private customerS: CustomersService,
    protected winRef: WindowRef
  ) {
    super(store);
  }
  ngOnInit() {
    this.tabLabels = ['Search', ' Details', 'Postcode Search' ];
    this.tabName = this.tabLabels[0];
    this.cs.getObject<any>(CONFIG.apiURL.page.customers.profile)
      .subscribe(
        (res) => {
          this.profile = res;
        }
      );
  }

  open_customer(event) {
    this.customer_id = event.data.customer_id;
    setTimeout(() => {
      this.activeTab = 1;
    }, 200);
    // this.customerS.openCustomer(event.data.customer_id);
  }

  loadDetail(refresh = false) {
    this.dt.after_onRefresh();
  }
  new_customer_request(){
    const url = CONFIG.Nesi1URL.requestCustOrVendor.replace('@type', '1').replace('@customer_id','0');
    this.winRef.boingNesi1(url ,'CustomerRequest'+Math.random(),"500,700");
  }

  tabChanged(event) {
    super.tabChanged(event);

    if(this.activeTab == 0) {
      // In this way we can get rid of 'Details' page and make sure the new selected customer will be loaded correctly.
      this.customer_id = 0
      this.dt.dataTable.previousRowIndex = -1;
    }

  }
}
