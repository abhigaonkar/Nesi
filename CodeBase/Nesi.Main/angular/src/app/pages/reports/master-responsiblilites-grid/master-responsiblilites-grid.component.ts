import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'app-master-responsiblilites-grid',
  templateUrl: './master-responsiblilites-grid.component.html',
  styleUrls: ['./master-responsiblilites-grid.component.css']
})
export class MasterResponsiblilitesGridComponent implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail(refreshbutton = false) {
    this.dt.loadReport();
  }
}
