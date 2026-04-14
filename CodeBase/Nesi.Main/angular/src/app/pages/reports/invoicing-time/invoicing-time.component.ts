import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-invoicing-time',
  templateUrl: './invoicing-time.component.html',
  styleUrls: ['./invoicing-time.component.css']
})
export class InvoicingTimeComponent implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail() {
    this.dt.after_onRefresh();
  }

}
