import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DatatableComponent } from 'app/components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-customer-email',
  templateUrl: './customer-email.component.html',
  styleUrls: ['./customer-email.component.css']
})
export class CustomerEmailComponent implements OnInit {
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
      this.dt.showReport('CustomerEmailsGrid');
    }
  }

}
