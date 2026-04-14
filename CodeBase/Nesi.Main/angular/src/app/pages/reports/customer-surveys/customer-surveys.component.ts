import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-customer-surveys',
  templateUrl: './customer-surveys.component.html',
  styleUrls: ['./customer-surveys.component.css']
})
export class CustomerSurveysComponent implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail(refreshbutton = false) {
    this.dt.after_onRefresh();
  }
}
