import { Injectable } from '@angular/core';

import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';

import { Observable } from 'rxjs/Observable';

import { IncomeStatementInputParameter, IncomeStatementRecord, ApiConstants,
   IncomeStatementData, IncomeStatementOperation, TaxEntityRecord,
   BusinessUnitData, BusinessUnitRecord, FiscalPeriodRecord, NetSuiteIncomeStatementData,
   InitialData } from 'app/models/pages/income-statment'

@Injectable({
  providedIn: 'root'
})
export class IncomeStatementService implements IncomeStatementOperation {
  getInitialData(): Observable<InitialData> {
    let url = ApiConstants.initialDataUrl;
    return this.cs.getObject<InitialData>(url);
  }

  getNetSuite(parameter: IncomeStatementInputParameter): Observable<NetSuiteIncomeStatementData> {
    let url = ApiConstants.reportUrl;
    let params = "?taxId=" + parameter.taxId + "&fiscalPeriod=" + parameter.fiscalPeriod + "&businessUnitList=" + parameter.businessUnitList;
    let urlWithParams = url + params;
    const data = this.cs.getObject<NetSuiteIncomeStatementData>(urlWithParams);
    return data
  }

  getFiscalPeriods(taxId: number): Observable<Array<FiscalPeriodRecord>> {
    let url = ApiConstants.fiscalPeriodsUrl + "?taxid=" + taxId;
    const data = this.cs.getObject<Array<FiscalPeriodRecord>>(url);
    return data;
  }

  getBusinessUnits(taxId: number): Observable<BusinessUnitData> {
    let url = ApiConstants.businessUnitUrl + "?taxid=" + taxId;
    const data = this.cs.getObject<BusinessUnitData>(url);
    return data;
  }

  getTaxEntities(): Observable<TaxEntityRecord[]> {
    let url = ApiConstants.taxEntitytUrl;
    const data = this.cs.getObject<TaxEntityRecord[]>(url);
    return data;
  }

  get(parameter: IncomeStatementInputParameter): Observable<IncomeStatementData> {
    let url = ApiConstants.reportUrl;
    let params = "?taxId=" + parameter.taxId + "&fiscalPeriod=" + parameter.fiscalPeriod + "&businessUnitList=" + parameter.businessUnitList;
    let urlWithParams = url + params;
    const data = this.cs.getObject<IncomeStatementData>(urlWithParams);
    return data;
  }

  constructor(
    private ts: TokenService,
    private cs: CoreService) {
    }
}
