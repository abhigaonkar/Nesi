import { Component, OnInit, Input, ViewChild } from '@angular/core';

import { AssetCustomerRateQueryParameter, AssetCustomerRateRecord, BusinessUnitRecord, VisiableBUParameter,
 DataSource, ErrorInfo, IRecord, OperationResult, AssetCustomerRateDeleteParameter
 } from '../../../../models/pages/customer/customer-asset-rate'; 
import { CustomerAssetRateService } from '../../../../services/pages/customer-asset-rate.service'

import CustomStore from 'devextreme/data/custom_store';
import { DxDataGridComponent } from 'devextreme-angular';

import { fromEvent, Observable } from 'rxjs';

import { Store } from '@ngrx/store';
import * as fromMessage from '../../../../actions/layout/growlMessage';
import * as fromRoot from '../../../../reducers';

@Component({
  selector: 'customer-asset-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  @Input() customer_id: number;
  @Input() businessUnitId: any;
  @Input() userId: any;
  @Input() disabled:boolean;

  public pageSize = 20;
  public business_unit_list: Array<BusinessUnitRecord>;
  public default_business_unit: number;

  public searchedString: string;
  public search$: Observable<any>;
  public show = false;

  public dataSource: DataSource;
  @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;

  constructor(
    public car: CustomerAssetRateService,
    public store: Store<fromRoot.State>,
  ) {
      this.searchedString = "";
      this.setupDataSource();
   }

  ngOnInit() {
    this.default_business_unit = this.businessUnitId;
    this.getBusinessUnit(this.businessUnitId, this.userId);

  }

  //
  // Get bu list
  //
  private getBusinessUnit(businessId: number, userId: number) {
    const parameter = new VisiableBUParameter();
    parameter.userid = userId;

    this.car.GetBUList(parameter).subscribe(
      data => {this.business_unit_list = data;}
    );
  }

  onValueChanged(e) {
      const busineseID = e.value;
      this.businessUnitId = busineseID;
      this.dataGrid.instance.refresh();
  }

  //
  // Search...
  //
  onFocusIn(e: any) {
    this.setupSearch();
  }

  private setupSearch() {
    if(this.search$) {
      return;
    }

    const searchBox = document.getElementById('inputSearchID');
    this.search$ = fromEvent(searchBox, 'keyup')
      .map((e: any) => e.target.value)
      .debounceTime(500)
      .distinctUntilChanged();

      this.search$.subscribe(
        stringtoBeSearched => 
        {
          this.dataGrid.instance.filter(stringtoBeSearched);
          this.searchedString = stringtoBeSearched;
        })
  }

  //
  // Set up data source
  //
  private setupDataSource() {
    this.dataSource = new DataSource();
    
    this.dataSource.store = new CustomStore({
      key: "keyId",
      load: (options) => {
        const parameter = new AssetCustomerRateQueryParameter();
        parameter.business_unit_id = this.businessUnitId;
        parameter.customer_id = this.customer_id;
        parameter.pageSize = this.pageSize;
        parameter.skipPage = options.skip;

        parameter.searchingstring = "";
        if(options.filter)
        {
          parameter.searchingstring = options.filter;
        }

        return this.car.GetCustomerAssetList(parameter);
      },

      update: (key, values) => { return this.updateRow(key, values);},

      remove: (key) => {  return this.deleteRow(key);},
    });

  }

  //
  // update
  //
  private updateRow(key, values): Promise<any> {
    const info = this.updateRow_validation(key, values);
    if(!info.okay) {
      return Promise.reject(info.error);
    }

    const currentRow = this.getCurrentRow(key);
    if(!currentRow) {
      return Promise.reject('Current Row is not available to be updated.');
    }

    // Get the current value.
    const record = values as IRecord;
    const updatedRow = new AssetCustomerRateRecord();
    updatedRow.keyId = currentRow.keyId;
    updatedRow.asset_des = currentRow.asset_des;
    updatedRow.asset_id = currentRow.asset_id;
    updatedRow.customerId = this.customer_id;
    updatedRow.business_unit_id = currentRow.business_unit_id;
    updatedRow.can_delete = currentRow.can_delete;
    updatedRow.daily = currentRow.daily;
    updatedRow.weekly = currentRow.weekly;
    updatedRow.monthly = currentRow.monthly;
    updatedRow.start = currentRow.start;
    updatedRow.end = currentRow.end;
    updatedRow.id = currentRow.id;

    // Get the new value if set...
    if(record.start) {
      updatedRow.start  = record.start;
    }

    if(record.end ) {
      updatedRow.end  = record.end;
    }

    if(record.daily) {
      updatedRow.daily  = record.daily;
    }

    if(record.weekly) {
      updatedRow.weekly  = record.weekly;
    }

    if(record.monthly) {
      updatedRow.monthly  = record.monthly;
    }

    const promise = new Promise((r,j) => {
      this.car.UpdateCustomerAssetRate(updatedRow).subscribe(data => {
        this.getResult(r,j, data);
      },
      error => {
        console.log(error);
        j(error);
      });
    });

    return promise;  
  }

  private getResult(res: any, rej: any, data: OperationResult) {
    if(data == null || <any>(data.data) == null) {
      rej("Failed to save your change.");
      return;
    }

    if(data.data.okay) {
      res(data.data);
      setTimeout(() => {
        this.store.dispatch(new fromMessage.PushSuccessMessage(data.data.message));
      }, 10);

      return;
    }

    rej(data.data.message);
  }

  private updateRow_validation(key: any, values: any): ErrorInfo {
    const info = new ErrorInfo();
    info.okay = false;
    info.error = "";

    const record = values as IRecord;

    // check ing
    let check1 = !record.start && !record.end;
    let check2 = record.start && record.end && record.start < record.end;
    if( !(check1 || check2)) {
      info.okay = false;
      info.error = "Please enter a valid date range. The From date must precede the To date."
      return info;
    }

    if(record.daily) {
      if(record.daily < 0) {
        info.okay = false;
        info.error = "Daily fee should be greater than 0."
        return info;
      }
    }

    if(record.weekly) {
      if(record.weekly < 0) {
        info.okay = false;
        info.error = "Weekly fee should be greater than 0."
        return info;
      }
    }

    if(record.monthly) {
      if(record.monthly < 0) {
        info.okay = false;
        info.error = "Monthly fee should be greater than 0."
        return info;
      }
    }

    info.okay = true;
    return info;
  }

  private getCurrentRow(key) {
    if(!key || key <= 0 ) {
      return null;
    }

    const rows = this.dataGrid.instance.getVisibleRows();
    if(rows == null) {
      return null;
    }

    let foundRow = null;
    rows.forEach(element => {
      if(element.rowType == "data" && element.data.keyId == key) {
        foundRow = element.data;
      }
    });

    return foundRow;
  }

  //
  // delete
  //
  isPopupVisible: boolean = false;
  res: any;
  e: any;
  confirmationInfo: string = "";

  public rowRemoving(e) {
    // console.log(e.data);
    this.confirmationInfo = `Do you want to reset the price to the default rate for asset '${e.data.asset_des}' ?`;
    // This is why the code is a little bit harder.
    // https://www.devexpress.com/Support/Center/Question/Details/T725319/datagrid-loadpanel-is-not-hidden-when-a-custom-confirmation-dialog-is-used
    e.component.option("loadPanel.enabled", false);
    this.e = e;

    this.isPopupVisible = true;
    const promise = new Promise(
      (res, rej) => {
        this.res = res;
      }
    );

    e.cancel = promise;
  }

  ConfirmYes() {
    this.isPopupVisible = false;
    this.cleanup(false);
  }

  ConfirmNo(){
    this.isPopupVisible = false;
    this.cleanup(true);
  }

  popup_hiding (e) {
    // handle closed by without from two YES or NO buttons.
    this.cleanup(true);
  }

  cleanup (v: boolean) {
    this.res(v); // if already resoved, call it again no hurts.
    this.e.component.option("loadPanel.enabled", true);
    this.confirmationInfo = "";
  }

  //
  // End of delete
  //

  private deleteRow(key) : Promise<any> {
    if(key == 0 ) {
      return Promise.reject('No row is selected.');
    }

    const currentRow = this.getCurrentRow(key);
    if(!currentRow) {
      return Promise.reject('Current Row is not available to be deleted.');
    }

    const rowToBeDeleted = new AssetCustomerRateDeleteParameter();
    rowToBeDeleted.id  = currentRow.id;
    rowToBeDeleted.asset_id = currentRow.asset_id;
    rowToBeDeleted.customerId = currentRow.customer_id;

    const promise = new Promise(
      (res, rej) => {
        this.deleteRowAction(res, rej, rowToBeDeleted);
      }
    );

    return promise;
  }

  private deleteRowAction(res: any, rej: any, row: AssetCustomerRateDeleteParameter) {
    this.car.DeleteCustomerAssetRate(row)
    .subscribe( 
      data => { this.getResult(res, rej, data)},
      error => {console.log(error); rej(error);});
  }

  //
  // can be deleted
  //
  public canDelete(options): boolean {
    let canBedeleted = true;

    if(options.row.rowType == "data" && options.row.data.can_delete == 0 ) {
      return false;
    }

    return canBedeleted;
  }


  Refresh() {
    this.dataGrid.instance.refresh();
  }
}
