import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';


@Component({
  selector: 'nesi-fvr-grid',
  templateUrl: './fvr-grid.component.html',
  styleUrls: ['./fvr-grid.component.css']
})
export class FvrGridComponent implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail() {
    this.dt.after_onRefresh();
  }
}
