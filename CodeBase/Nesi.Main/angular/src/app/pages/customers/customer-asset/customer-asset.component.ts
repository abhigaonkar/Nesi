import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DatatableComponent } from 'app/components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-customer-asset',
  templateUrl: './customer-asset.component.html',
  styleUrls: ['./customer-asset.component.css']
})
export class CustomerAssetComponent implements OnInit {
  @Input() customer_id: number;
  @Input() address_id: number;
  @Input() customer_base_profile: any;
  @Input() disabled:boolean;
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
    this.loadDetail();
  }

  loadDetail(refresh_button = false) {
    this.dt.reportQueryParam = [
      { coulumnname: 'customer_id', value: this.customer_id },
      { coulumnname: 'address_id', value: this.address_id },
    ];
    this.dt.refreshCache = true;
    if (refresh_button) {
      this.dt.loadReport();
    } else {
      this.dt.showReport('CustomerAsset');
    }
  }

}
