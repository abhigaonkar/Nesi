import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'app-wo-line-grid',
  templateUrl: './wo-line-grid.component.html',
  styleUrls: ['./wo-line-grid.component.css']
})
export class WoLineGridComponent implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail(refreshbutton = false) {
    this.dt.loadReport();
  }

  woLineGridReportColumnCallback(rowData: any, rowIndex: number, columnName) {
    if (columnName === 'int_onhand_qty' || columnName === 'stillNotOrdered' || columnName === 'ext_onhand_qty') {
      return 'bold';
    }

    return;
  }

}
