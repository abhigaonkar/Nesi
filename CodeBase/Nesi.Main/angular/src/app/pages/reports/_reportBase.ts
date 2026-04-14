import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../components/nesi-datatable/components/datatable/datatable.component';


// tslint:disable-next-line:component-class-suffix
export class ReportBase implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;
  ngOnInit() {

  }

  loadDetail() {
    this.dt.after_onRefresh();
  }
}

