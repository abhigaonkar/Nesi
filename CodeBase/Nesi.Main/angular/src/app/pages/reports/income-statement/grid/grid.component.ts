import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { DxDataGridComponent } from 'devextreme-angular';

import { IncomeStatementRecord } from 'app/models/pages/income-statment';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {

  @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;

  imagePath = "assets/images/icon/icon[note_blank].gif";

  popupVisible = false;
  currentRecord : IncomeStatementRecord;

  @Input() records: Observable<Array<IncomeStatementRecord>>;

  constructor() {
  }

  ngOnInit() {
  }

  getRecords() {
    const records = this.dataGrid.instance.getVisibleRows();
    return records;
  }

  calculateSelectedRow(options) {
  }

  showNote(data) {
    this.currentRecord = data as IncomeStatementRecord;
    this.popupVisible = true;
  }

  onCellPrepared(e) {
    console.log("onCellPrepared", e.rowType);
    this.onCellPrepared_BackgroundColor(e);
    this.onCellPrepared_Color(e);
    this.onCellPrepared_Group(e);
    this.onCellPrepared_GroupFooter(e);
  }

  customizeExcelCell(options) {
    const handler = {
      customizeExcelCell_Data: function(options) {
        if(options.gridCell.column.dataField == 'mtd' ||
           options.gridCell.column.dataField === 'mtd_sales' || 
           options.gridCell.column.dataField === 'bmtddiff' ||
           options.gridCell.column.dataField === 'ytd' ||
           options.gridCell.column.dataField === 'ytd_sales' ||
           options.gridCell.column.dataField === 'bytddiff' ||
           options.gridCell.column.dataField === 'bmtd' ||
           options.gridCell.column.dataField === 'bytd') {
          this.customizeExcelCell_Data_Check(options);
          return;
        }
      },
     
      customizeExcelCell_Data_Check: function(options) {
        const record = options.gridCell.data as IncomeStatementRecord;

        if(record.pos === 'Gross' && record.type === 'Expense') {
          options.value = -options.value;
        }

        if(record.pos === 'Net' && record.type === 'Expense') {
          options.value = -options.value;
        }
      }
    };

    if(options.gridCell.rowType === 'data') {
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
        const re_gl = /Gl group alias:/gi;

        let v1 = v0.replace(re_pos, '');
        let v2 = v1.replace(re_type, '');
        let v3 = v2.replace(re_gl, '');
        options.value = v3;
      }
    };

    if(options.gridCell.rowType === 'group') {
      gHandler.customizeExcelCell_Group(options);
      return;
    }
  }

  private onCellPrepared_BackgroundColor(e) {
    if(e.rowType === 'data' && (e.column.dataField === 'ytd' || e.column.dataField === 'ytd_sales') ) {
      e.cellElement.style.backgroundColor = '#99FF99';
    }

    if(e.rowType === 'data' && ( e.column.dataField === 'mtd' || e.column.dataField === 'mtd_sales') ) {
      e.cellElement.style.backgroundColor = '#CCFFCC';
    }

    if(e.rowType === 'header') {
      e.cellElement.style.backgroundColor = '#4682B4';
      e.cellElement.style.color = 'white';
    }

    if(e.rowType === 'filter') {
      e.cellElement.style.backgroundColor = '#E0E0E0';
    }
  }

  private onCellPrepared_Color(e) {
    if(e.rowType === 'data' && (e.column.dataField === 'bmtddiff' || e.column.dataField === 'bytddiff' ) ) {
      if(e.data.type === 'Revenue') {
        if(e.value > 0 ){
          e.cellElement.style.color = 'Red';
        }
      }

      if(e.data.type === 'Expense') {
        if(e.value < 0 ){
          e.cellElement.style.color = 'Red';
        }
      }
    }
  }

  private onCellPrepared_Group(e) {
    if(e.rowType === 'group' ) {
    }
  }

  private onCellPrepared_GroupFooter(e) {
    if(e.rowType !== 'groupFooter' ) {
      return;
    }

    if(e.totalItem.groupIndex === 2) {
      e.cellElement.style.backgroundColor = '#cce6ff';
    }

    if(e.totalItem.groupIndex === 1) {
      e.cellElement.style.backgroundColor = '#b3d9ff';
      e.totalItem.summaryCells[3][0].displayFormat ='';
      e.totalItem.summaryCells[3][0].value ='';
    }

    if(e.totalItem.groupIndex === 0) {

      if(e.totalItem.data.key === 'Gross') {
        e.totalItem.summaryCells[3][0].displayFormat ='Gross Margin:';
      } else {
        e.totalItem.summaryCells[3][0].displayFormat ='';
      }

      e.cellElement.style.backgroundColor = '#99ccff';
    }
    //
  }

}
