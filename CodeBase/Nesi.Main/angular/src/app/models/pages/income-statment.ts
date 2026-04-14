import { Observable } from 'rxjs/Observable';

export class IncomeStatementInputParameter {
    taxId: number;
    businessUnitList: string;
    fiscalPeriod: string;
    query: string;
    valid: boolean;
    errMsg: string;

    taxEntity: string;
    fiscalPeriodText: string;
    buList: Array<string>;
}

export class IncomeStatementRecord {
    acct_no: string;
    ytd: number;
    account: string;
    mtd: number;
    bmtd: number;
    bytd: number;
    bytddiff: number;
    bmtddiff: number;
    gl_group_alias: string;
    drcr: string;
    type: string;
    pos: string;
    date_modified: Date;
    date_modified_string: string;
    ytd_sales: number;
    mtd_sales: number;
    note: string;
}

export class IncomeStatementData {
    random: string;
    records: Array<IncomeStatementRecord>;
}

export class NetSuiteIncomeStatementRecord {
    accountNSID: string;
    accountName: string;
    type: string;
    pos: string;
    amount: number;
    currency: number;
    strCurrency: number;
    description: string;
    accountNameFirstPart: string;
    accountNameSecondPart: string;
    percentage: number;
    profile: number;
}

export class NetSuiteIncomeStatementData {
    success: boolean;
    accounts: Array<NetSuiteIncomeStatementRecord>;

    name: string;
    message: string;
    notifyOff: boolean;

    url: string;
}

export class TaxEntityRecord {
    id: number;
    name: string;
}

export class BusinessUnitRecord {
    id: number;
    text: string;
}

export class BusinessUnitData {
    waitingRollover: boolean;
    records: Array<BusinessUnitRecord>;
}

export class FiscalPeriodRecord
{
    value: string;
    text: string;
}

export class InitialData {
    taxEntities: Array<TaxEntityRecord>;
    defaultTaxEntity: number;
    businessUnits: Array<BusinessUnitRecord>;
    defaultSelectedBusinessUnits: Array<number>;
    fiscalPeriods: Array<FiscalPeriodRecord>;
    defaultFiscalPeriod: string;
}

export interface IncomeStatementOperation {
    get(parameter: IncomeStatementInputParameter): Observable<IncomeStatementData>;
    getTaxEntities(): Observable<Array<TaxEntityRecord>>;
    getBusinessUnits(taxId: number): Observable<BusinessUnitData>;
    getFiscalPeriods(taxId: number): Observable<Array<FiscalPeriodRecord>>;
    getNetSuite(parameter: IncomeStatementInputParameter): Observable<NetSuiteIncomeStatementData>;
    getInitialData(): Observable<InitialData>;
}

export const ApiConstants = {
    reportUrl : 'api/Page/IncomeStatement/Search',
    taxEntitytUrl : 'api/Page/IncomeStatement/TaxEntity',
    businessUnitUrl: 'api/Page/IncomeStatement/BusinessUnit',
    fiscalPeriodsUrl: "api/Page/IncomeStatement/Fiscal",
    initialDataUrl: "api/Page/IncomeStatement/initial"
}
