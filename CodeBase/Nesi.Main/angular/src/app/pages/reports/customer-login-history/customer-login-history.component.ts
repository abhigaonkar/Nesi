import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-customer-login-history',
  templateUrl: './customer-login-history.component.html',
  styleUrls: ['./customer-login-history.component.css']
})
export class CustomerLoginHistoryComponent implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail(refreshbutton = false) {
    this.dt.after_onRefresh();
  }

}
