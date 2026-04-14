import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DatatableComponent } from 'app/components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-customer-phonenumber',
  templateUrl: './customer-phonenumber.component.html',
  styleUrls: ['./customer-phonenumber.component.css']
})
export class CustomerPhonenumberComponent implements OnInit {
  @Input() customer_id: number;
  @Input()  customer_base_profile: any;

  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
    this.loadDetail();
  }

  loadDetail(refresh_button = false) {
    this.dt.reportQueryParam = [
      { coulumnname: 'customer_id', value: this.customer_id },
    ];
    this.dt.refreshCache = true;
    if (refresh_button) {
      this.dt.loadReport();
    } else {
      this.dt.showReport('CustomerPhoneCallsGrid');
    }
  }

}
