import { Component, OnInit, Input } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import * as DATE from '../../../services/helper/datetime';
import { DataTableBase } from 'app/components/shared/Bases/DataTableBase';
import { CustomersService } from 'app/services/pages/customer.services';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-search-results',
  templateUrl: './customer-search-results.component.html',
  styleUrls: ['./customer-search-results.component.css']
})
export class CustomerSearchResultsComponent extends DataTableBase implements OnInit {

  constructor(
    protected store: Store<fromRoot.State>,
    protected cs: CoreService,
    private custs: CustomersService,
  ) {
    super(store, cs);

  }

  init() {
    this.sortField = 'customer_name';

  }

  get url(): string {
    return CONFIG.apiURL.page.customers.search;
  }

  get postData(): any {
    return {
      criteria: this.text, page: this.page, pageSize: this.pageSize,
      sortField: this.sortField, sortOrder: this.sortOrder,
    };
  }

  applyRowStyle(row: any) {
    if (DATE.DayDiff(new Date(), new Date(row.dateadded)) < 30) {
      return 'last_month';
    }
    if (!row.qc1) {
      return 'not_QC1';
    }
    if (row.hold === 'T') {
      return 'on_hold';
    }
  }

  openCustomer(event) {
    const id = event.data.customer_id;
    this.custs.openCustomer(id);
  }
}
