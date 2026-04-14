import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-advancepayreport',
  templateUrl: './advancepayreport.component.html',
  styleUrls: ['./advancepayreport.component.css']
})
export class AdvancepayreportComponent implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail(refreshbutton = false) {
    this.dt.after_onRefresh();
  }

}
