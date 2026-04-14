import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { MessageBase } from 'app/core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  selector: 'nesi-inventory-counts',
  templateUrl: './inventory-counts.component.html',
  styleUrls: ['./inventory-counts.component.css']
})
export class InventoryCountsComponent extends MessageBase implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;
  selected_business_unit: number;
  with_location = 0;
  checked = false;

  constructor(
    protected store: Store<fromRoot.State>,
    private ts: TokenService,
    private cs: CoreService) {
    super(store);
  }

  ngOnInit() {
    this.selected_business_unit = this.ts.working_businessUnit;
    // this.with_location = 0;

    setTimeout(() => {
      this.loadDetail();
    }, 1000);
  }

  loadDetail(refresh_button = false) {
    this.with_location = this.checked ? 1 : 0;

    this.ts.working_businessUnit = this.selected_business_unit;
    this.dt.reportQueryParam = [
      { coulumnname: 'business_unit', value: this.selected_business_unit },
      { coulumnname: 'location', value: this.with_location }
    ];

    this.dt.refreshCache = true;

    if (refresh_button) {
      this.dt.loadReport();
    } else {
      this.dt.showReport('InventoryCountsGrid');
    }

  }

  ReLoadDetail() {
    this.with_location = this.checked ? 1 : 0;

    this.dt.reportQueryParam = [
      { coulumnname: 'business_unit', value: this.selected_business_unit },
      { coulumnname: 'location', value: this.with_location }
    ];

    this.dt.refreshCache = true;
    this.dt.loadReport();
  }

}
