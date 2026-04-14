import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { CONFIG } from 'app/configuration';
import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';
import { LabelValueInt } from 'app/models/Shared/labelValueString';

@Component({
  selector: 'nesi-master-purchases-grid',
  templateUrl: './master-purchases-grid.component.html',
  styleUrls: ['./master-purchases-grid.component.css']
})
export class MasterPurchasesGridComponent implements OnInit {

  public apStatusList = [];
  public displayEdit = false;
  public entity: any;
  public linkForEdit = CONFIG.apiURL.page.reports.masterPurchasesGrid.edit;

  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor(
    public ts: TokenService,
    public cs: CoreService,
  ) { }

  ngOnInit() {
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.reports.masterPurchasesGrid.apStatus)
      .subscribe(
        (res) => {
          this.apStatusList = res;
        }
      );
  }

  loadDetail(refreshbutton = false) {
    this.dt.after_onRefresh();
  }

  public masterPurchasesGridCallback(rowData: any, rowIndex: number, columnName) {

    if(columnName != 'poprog_hasproblem_notes') {
      return;
    }

    const value = rowData[columnName];
    if (value && value != '') {
      return 'masterQuoteNotes'; //same color, so use it.
    }
  }

  dropDownForStatusReason(event) {
    this.cs.postDataExtra(CONFIG.apiURL.page.reports.masterPurchasesGrid.updateApStatus, event.data).subscribe(
      (res) => {
      }
    );
  }

  // Open editor
  openEdit(event) {
    this.entity = event;
    this.displayEdit = true;
  }

  // Close editor
  exitFromEditor() {
    this.entity = null;
    this.displayEdit = false;
  }
  

}
