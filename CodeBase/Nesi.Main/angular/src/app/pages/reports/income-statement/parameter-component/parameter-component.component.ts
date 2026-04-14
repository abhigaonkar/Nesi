import { Component, OnInit, Input, ChangeDetectionStrategy, ChangeDetectorRef, Output, EventEmitter, ViewChild } from '@angular/core';

import { DxListComponent, DxSelectBoxComponent } from "devextreme-angular";
import DataSource from 'devextreme/data/data_source';
import ArrayStore from 'devextreme/data/array_store';

import { BusinessUnitData, BusinessUnitRecord, TaxEntityRecord, FiscalPeriodRecord, IncomeStatementInputParameter } from 'app/models/pages/income-statment';
import { IncomeStatementService } from 'app/services/pages/income-statement.service'
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-parameter-component',
  templateUrl: './parameter-component.component.html',
  styleUrls: ['./parameter-component.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ParameterComponentComponent implements OnInit {

  selectAllModeVlaue: string = "page";
  selectionModeValue: string = "single";
  showup: Boolean = false;

  fiscalRecords: Array<FiscalPeriodRecord> = [];
  businessRecords: DataSource;

  @Input() taxEntityRecords: Array<TaxEntityRecord>;
  defaultTaxEntity: number;
  defaultSelectedBusinessUnits: Array<number>;
  defaultFiscalPeriod: string;

  @Output() generated = new EventEmitter<IncomeStatementInputParameter>();
  @Output() loadingInfo = new EventEmitter<boolean>();

  @ViewChild('taxEntity') taxEntity: DxSelectBoxComponent;
  @ViewChild(DxListComponent) businessUnits: DxListComponent;
  @ViewChild('fiscal') fiscalPeriod: DxSelectBoxComponent;

  private initialiazed = false;

  constructor(public incomeSrv: IncomeStatementService, private cd: ChangeDetectorRef) { }

  ngOnInit() {
    this.initialLoad();
  }

  onValueChanged(e) {
    if (this.initialiazed) {
      const taxid = e.value;
      this.getValuesP(taxid);
    } else {
      this.initialiazed = true;
    }
  }

  goReport() {
    const inputData = new IncomeStatementInputParameter();
    inputData.valid = false;

    // Tax Entity
    const taxRecord = this.taxEntity.selectedItem as TaxEntityRecord;
    if (!taxRecord) {
      inputData.errMsg = "Please select the tax entity.";
      this.generated.emit(inputData);
      return;
    }
    inputData.taxId = taxRecord.id;
    inputData.taxEntity = taxRecord.name;

    // Selected Business List
    const buList = this.businessUnits.selectedItems as Array<BusinessUnitRecord>;
    if (!buList || buList.length === 0) {
      inputData.errMsg = "Please select one business unit.";
      this.generated.emit(inputData);
      return;
    }

    let ids = "";
    inputData.buList = new Array<string>();
    buList.forEach(item => {
      if (ids === "") {
        ids = item.id.toString();
      } else {
        ids += "," + item.id.toString();
      }

      inputData.buList.push(item.text);
    });
    inputData.businessUnitList = ids;

    // Fiscal Period
    const fsRecord = this.fiscalPeriod.selectedItem as FiscalPeriodRecord;
    if (!fsRecord) {
      inputData.errMsg = "Please select the fiscal period.";
      this.generated.emit(inputData);
      return;
    }
    inputData.fiscalPeriod = fsRecord.value;
    inputData.fiscalPeriodText = fsRecord.text;
    inputData.valid = true;

    this.generated.emit(inputData);
  }

  private getValuesP(taxId: number) {
    const getBU$ = this.incomeSrv.getBusinessUnits(taxId);
    const getFP$ = this.incomeSrv.getFiscalPeriods(taxId);

    // Reset the UI
    this.businessRecords = new DataSource({
      store: new ArrayStore({ key: "id", data: []})
    });
    this.defaultSelectedBusinessUnits = [];

    this.fiscalRecords = [];
    this.fiscalPeriod.value = "";
    this.defaultFiscalPeriod = "";
    this.cd.markForCheck();

    forkJoin(getBU$, getFP$).subscribe(
      ( res: [BusinessUnitData, Array<FiscalPeriodRecord>]) => {
        this.businessRecords = new DataSource({
          store: new ArrayStore({ key: "id",  data: res[0].records })
        });

        this.showup = res[0].waitingRollover;
        this.fiscalRecords = res[1];

        if(this.fiscalRecords.length > 0) {
          let defaultvalue = this.getCurrentRecord(this.fiscalRecords);
          if(defaultvalue != "") {
            this.defaultFiscalPeriod = defaultvalue;
          }
        }

        this.cd.markForCheck();
      },

      err => {
        console.dir(err);
      }
    );
  }

  private initialLoad() {
    this.loadingInfo.emit(true);

    this.incomeSrv.getInitialData().subscribe(
      result => {
        this.businessRecords = new DataSource({
          store: new ArrayStore({ key: "id", data: result.businessUnits })
        });

        this.defaultSelectedBusinessUnits = result.defaultSelectedBusinessUnits;

        this.fiscalRecords = result.fiscalPeriods;

        if(this.fiscalRecords.length > 0) {
          let defaultvalue = this.getCurrentRecord(this.fiscalRecords);
          if(defaultvalue != "") {
            this.defaultFiscalPeriod = defaultvalue;
          }
        }

        this.taxEntityRecords = result.taxEntities;
        this.defaultTaxEntity = result.defaultTaxEntity;

        this.loadingInfo.emit(false);

        // This [this.defaultTaxEntity = result.defaultTaxEntity] will trigger the update to the tax entity control after initialLoad.
        // But the getReport requires everything in there, so we run it later.
        setTimeout(() => {
          // this.goReport();
        }, 0);

      }
    );
  }

  private getCurrentRecord(list: Array<FiscalPeriodRecord>): string {
    // Current year-month info
    let current = new Date();
    let year = current.getFullYear();
    let month = current.getMonth() + 1;
    let key = year.toString() + "_" + month.toString();
    
    let found = false;
    list.forEach(element => {
      if(element.value === key) {
        found = true;
      }
    });

    if(found) {
      return key;
    }

    return "";
  }
}