import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-customer-asset',
  templateUrl: './customerAsset.component.html'
})

export class CustomerAssetComponent implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;
  ngOnInit() {

  }

  loadDetail() {
    this.dt.after_onRefresh();
  }
}
