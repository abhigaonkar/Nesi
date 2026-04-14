import { Component, OnInit } from '@angular/core';

import { Subscription } from 'rxjs/Subscription';

import { IncomeStatementService } from 'app/services/pages/income-statement.service'

import { IncomeStatementInputParameter, IncomeStatementData, TaxEntityRecord, 
  NetSuiteIncomeStatementRecord, NetSuiteIncomeStatementData } from 'app/models/pages/income-statment'

import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromMessage from '../../../actions/layout/growlMessage';

@Component({
  selector: 'app-income-statement',
  templateUrl: './income-statement.component.html',
  styleUrls: ['./income-statement.component.css']
})
export class IncomeStatementComponent implements OnInit {

  incomeStatementData: NetSuiteIncomeStatementData;
  taxEntityRecords: Array<TaxEntityRecord>;
  loadingVisible: boolean = false;

  constructor( public incomeSrv: IncomeStatementService,
    private store: Store<fromRoot.State>) { }

  ngOnInit() {
  }

  fetchReport(inputParameter: IncomeStatementInputParameter) {

    if(!inputParameter.valid) {
      this.store.dispatch(new fromMessage.PushWarnMessage(inputParameter.errMsg));
      return;
    }

    this.store.dispatch(new fromMessage.ClearMessage());
    
    this.loadingVisible = true;
    const that = this;
    this.empty(this.incomeStatementData);
    this.incomeSrv.getNetSuite(inputParameter).subscribe(
      result => {
        that.incomeStatementData = result;
        that.loadingVisible = false;

        setTimeout(() => {
          this.open(that.incomeStatementData);
        }, 0.5);

      });
  }

  showLoading(show: boolean) {
    this.loadingVisible = show;
  }

  open(data: NetSuiteIncomeStatementData) {
    if(!data) {
      return;
    }

    if(!data.success) {
      return;
    }
    
    window.open(data.url, "_blank");
  }

  empty(data: NetSuiteIncomeStatementData){
    if(!data) {
      return;
    }

    data.message = "";
  }
}