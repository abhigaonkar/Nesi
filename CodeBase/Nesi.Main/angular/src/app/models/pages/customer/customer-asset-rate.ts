import { Observable } from 'rxjs/Observable';

export class VisiableBUParameter {
    userid: number;
    visibleBusinessList: string;
}

export class BusinessUnitRecord {
    id: number;
    name: string;
}

export class AssetCustomerRateQueryParameter {
    customer_id: number;
    business_unit_id: number;
    searchingstring: string;
    pageSize: number;
    skipPage: number;
}

export class AssetCustomerRateRecord {
    keyId: number;
    id: number;
    business_unit_id: number;
    start: Date;
    end: Date;
    asset_id: number;
    asset_des: string;
    daily: number;
    weekly: number;
    monthly: number;
    lastupdate: Date;
    can_delete: number;
    customerId: number;
}


export interface IRecord {
    keyId?: number;
    id?: number;
    business_unit_id?: number;
    start?: Date;
    end?: Date;
    asset_id?: number;
    asset_des?: string;
    daily?: number;
    weekly?: number;
    monthly?: number;
}

export class ErrorInfo {
    okay: boolean;
    error: string;
}

export class AssetCustomerRateResult {
    data: Array<AssetCustomerRateRecord>;
    totalCount: number;
}

export class OperationResult
{
    data: innerData;
}

export class innerData {
    okay: number;
    message: string;
    errors: any;
}

export const CustomerAssetRateAPI = {
    List: 'api/Page/AssetRate/List',
    BusinessUnits: 'api/Page/AssetRate/BusinessUnit',
    Update: 'api/Page/AssetRate/Save',
    Delete: 'api/Page/AssetRate/Delete'
}

export class AssetCustomerRateDeleteParameter{
    id: number;
    asset_id: number;
    customerId: number;
}

export interface ICustomerAssetRateService
{
    GetBUList(queryParameter: VisiableBUParameter): Observable<Array<BusinessUnitRecord>>;
    GetCustomerAssetList(inputParameter: AssetCustomerRateQueryParameter) : Promise<AssetCustomerRateResult>;
    UpdateCustomerAssetRate(record: AssetCustomerRateRecord): Observable<OperationResult>;
    DeleteCustomerAssetRate(record: AssetCustomerRateDeleteParameter): Observable<OperationResult>;
}

//
// Data source
//
export class DataSource {
    store: any;
}

//
// End of Data source
//