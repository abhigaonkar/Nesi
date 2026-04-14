import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { MessageBase } from 'app/core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  selector: 'nesi-inventory-usage',
  templateUrl: './inventory-usage.component.html',
  styleUrls: ['./inventory-usage.component.css']
})
export class InventoryUsageComponent extends MessageBase implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;
  selected_business_unit: number;

  constructor(
    protected store: Store<fromRoot.State>,
    private ts: TokenService,
    private cs: CoreService) {
    super(store);
  }

  ngOnInit() {
    // this.selected_business_unit = this.ts.working_businessUnit;
    // setTimeout(() => {
    //   this.loadDetail();
    // }, 1000);
  }

  loadDetail() {
    // this.ts.working_businessUnit = this.selected_business_unit;
    // this.dt.reportQueryParam = [
    //   { coulumnname: 'business_unit', value: this.selected_business_unit },
    // ];

    this.dt.refreshCache = true;
    this.dt.showReport('InventoryUsage');
  }

  reLoadDetail() {
    // this.dt.reportQueryParam = [
    //   { coulumnname: 'business_unit', value: this.selected_business_unit },
    // ];

    this.dt.refreshCache = true;
    this.dt.loadReport();
  }
}
