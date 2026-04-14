import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-customer-rates-grid',
  templateUrl: './customer-rates-grid.component.html',
  styleUrls: ['./customer-rates-grid.component.css']
})
export class CustomerRatesGridComponent implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail(refreshbutton = false) {
    this.dt.after_onRefresh();
  }

  public customerRatesGridCallback(rowData: any, rowIndex: number, columnName) {

    if (columnName !== 'chargeout') {
      return '';
    }

    const chargeout = rowData['chargeout'];
    const normal = rowData['normal'];
    if (chargeout < normal) {
      return 'custRatesDarkPink';
    }

    if (chargeout > normal) {
      return 'custRatesGreen';
    }

    const date1 = rowData['to_date'];
    const to_date = new Date(date1);

    const today = new Date();
    if (today > to_date) {
      return 'custRatesPink';
    }

    return '';
  }
}
