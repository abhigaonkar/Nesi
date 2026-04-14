import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-cust-status-history',
  templateUrl: './cust-status-history.component.html',
  styleUrls: ['./cust-status-history.component.css']
})
export class CustStatusHistoryComponent implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail(refreshbutton = false) {
    this.dt.loadReport();
  }

}
