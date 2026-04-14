import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-master-contact',
  templateUrl: './masterContact.component.html'
})

export class MasterContactComponent implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;
  ngOnInit() {

  }

  loadDetail() {
    this.dt.after_onRefresh();
  }
}
