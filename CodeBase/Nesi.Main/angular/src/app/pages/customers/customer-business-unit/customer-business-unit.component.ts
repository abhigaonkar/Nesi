import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DatatableComponent } from 'app/components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-customer-business-unit',
  templateUrl: './customer-business-unit.component.html',
  styleUrls: ['./customer-business-unit.component.css']
})
export class CustomerBusinessUnitComponent implements OnInit {
  @Input() customer_id: number;
  @Input() customer_base_profile: any;
  @Input() disabled: boolean;
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
      this.dt.showReport('CustomerBusinessUnitGrid');
    }
  }
}