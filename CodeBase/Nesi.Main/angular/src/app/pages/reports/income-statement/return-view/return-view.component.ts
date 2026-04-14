import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { DxDataGridComponent } from 'devextreme-angular';

import { NetSuiteIncomeStatementData } from 'app/models/pages/income-statment';

@Component({
  selector: 'app-return-view',
  templateUrl: './return-view.component.html',
  styleUrls: ['./return-view.component.css']
})
export class ReturnViewComponent implements OnInit {

  @Input() returnData: NetSuiteIncomeStatementData;

  constructor() { }

  ngOnInit() {
  }
}
