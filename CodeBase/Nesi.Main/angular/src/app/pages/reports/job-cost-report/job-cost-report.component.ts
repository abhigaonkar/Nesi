import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { MessageBase } from 'app/core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  selector: 'nesi-job-cost-report',
  templateUrl: './job-cost-report.component.html',
  styleUrls: ['./job-cost-report.component.css']
})
export class JobCostReportComponent extends MessageBase implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;
  selected_business_unit: number;

  constructor(
    protected store: Store<fromRoot.State>,
    private ts: TokenService,
    private cs: CoreService) {
    super(store);
  }

  ngOnInit() {
  }

  loadDetail() {
    this.dt.after_onRefresh();
  }

  public jobCostReportCallback(rowData: any, rowIndex: number, columnName) {

    const value = rowData[columnName];
    const test = Number(value);
    if (isNaN(test)) {
      return;
    }

    const percentage = value * 100;

    if (value === 0) {
      return 'jobCosZero';
    }

    if (value < 0) {
      return 'jobCostLessZero';
    }

    if (columnName === 'actual_labor_hours') {
      const quoted_labor_hours = rowData['quoted_labor_hours'];
      if (value > quoted_labor_hours) {
        return 'jobCostRed';
      }
    }

    if (columnName === 'material_actual_price') {
      const material_quote_price = rowData['material_quote_price'];
      if (value > material_quote_price) {
        return 'jobCostRed';
      }
    }

    if (columnName === 'quoted_amount_used' || columnName === 'labor_complete' || columnName === 'material_complete') {
      if (percentage > 100) {
        return 'jobCostRed';
      }

      if (percentage > 75) {
        return 'jobCostOrange';
      }

      if (value === 100) {
        return 'jobCostBold';
      }
    }

  }
}
