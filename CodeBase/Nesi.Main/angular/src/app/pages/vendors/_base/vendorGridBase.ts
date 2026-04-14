import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

export class VendorGridBase {

  @Input() vendor_id: number;
  @ViewChild(DatatableComponent) dt: DatatableComponent;


  constructor() { }


  ngOnInit() {
    this.loadDetail();
  }

  loadDetail(refresh_button = false) {
    this.dt.reportQueryParam = [
      { coulumnname: 'vendor_id', value: this.vendor_id },
    ];
    this.dt.refreshCache = true;
    if (refresh_button) {
      this.dt.loadReport();
    } else {
      this.dt.showReport(this.dt.gridName);
    }
  }

}
