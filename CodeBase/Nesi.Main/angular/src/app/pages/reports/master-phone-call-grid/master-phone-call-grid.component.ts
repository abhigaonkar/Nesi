import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-master-phone-call-grid',
  templateUrl: './master-phone-call-grid.component.html',
  styleUrls: ['./master-phone-call-grid.component.css']
})
export class MasterPhoneCallGridComponent implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail(refreshbutton = false) {
    this.dt.loadReport();
  }
}
