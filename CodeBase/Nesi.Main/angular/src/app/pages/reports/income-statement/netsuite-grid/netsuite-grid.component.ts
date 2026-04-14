import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { DxDataGridComponent } from 'devextreme-angular';

import { NetSuiteIncomeStatementRecord, NetSuiteIncomeStatementData } from 'app/models/pages/income-statment';

@Component({
  selector: 'app-netsuite-grid',
  templateUrl: './netsuite-grid.component.html',
  styleUrls: ['./netsuite-grid.component.css']
})
export class NetsuiteGridComponent implements OnInit {

  @Input() records: Observable<Array<NetSuiteIncomeStatementRecord>>;
  @Input() topHead: string;
  @Input() bottomHead: string;
  private _value: string;

  constructor() {
  }

  ngOnInit() {
  }

  calculateSelectedRow = (options) => {
    // if (options.name === "SelectedRowsSummary") {
    //   if (options.summaryProcess === "calculate") {
    //     this._value = options.value;
    //   }
    //   if (options.summaryProcess === "finalize") {
    //     options.totalValue = "Total of " + this._value;
    //     options.value = options.totalValue;
    //   }
    // }
  }

  customizeExcelCell(options) {

    let that = options;
    console.dir(that);
    const handler = {
      customizeExcelCell_Data: function(options) {
          this.customizeExcelCell_Data_Check(options);
          return;
      },
     
      customizeExcelCell_Data_Check: function(options) {
        const record = options.gridCell.data as NetSuiteIncomeStatementRecord;
      }
    };

    if(options.gridCell.rowType === 'data') {
      handler.customizeExcelCell_Data(options);
      return;
    }

    const gfHandler = {
      customizeExcelCell_GroupFooter: function(options) {
        options.backgroundColor = '#b3d9ff';
      }
    };

    if(options.gridCell.rowType === 'groupFooter') {
      gfHandler.customizeExcelCell_GroupFooter(options);
      return;
    }

    const gHandler = {
      customizeExcelCell_Group: function(options) {
        if(!options.value) {
          return;
        }

        let v0 = options.value;
        const re_pos = /Pos: /gi;
        const re_type = /Type: /gi;
        const re_FP = /First Part: /gi;

        let v1 = v0.replace(re_pos, '');
        let v2 = v1.replace(re_type, '');
        let v3 = v2.replace(re_FP, '');
        options.value = v3;
      }
    };

    if(options.gridCell.rowType === 'group') {
      gHandler.customizeExcelCell_Group(options);
      return;
    }
  }

  onCellPrepared(e) {
    // console.log("onCellPrepared", e.rowType);
    this.onCellPrepared_BackgroundColor(e);
    this.onCellPrepared_GroupFooter(e);
  }

  private onCellPrepared_BackgroundColor(e) {
    if(e.rowType === 'header') {
      e.cellElement.style.backgroundColor = '#4682B4';
      e.cellElement.style.color = 'white';
    }

    if(e.rowType === 'filter') {
      e.cellElement.style.backgroundColor = '#E0E0E0';
    }
  }

  private onCellPrepared_GroupFooter(e) {
    if(e.rowType !== 'groupFooter' ) {
      return;
    }

    if(e.totalItem.groupIndex === 0) {
      e.totalItem.summaryCells[3][0].displayFormat = "Net " + e.totalItem.data.key;
      e.cellElement.style.backgroundColor = '#99ccff';
    }

    if(e.totalItem.groupIndex === 1) {
      e.totalItem.summaryCells[3][0].displayFormat = "Total - " + e.totalItem.data.key;
      e.cellElement.style.backgroundColor = '#b3d9ff';
    }

    if(e.totalItem.groupIndex === 2) {
      e.totalItem.summaryCells[3][0].displayFormat = "Total - " + e.totalItem.data.key;
      e.cellElement.style.backgroundColor = '#cce6ff';
    }
  }

}
