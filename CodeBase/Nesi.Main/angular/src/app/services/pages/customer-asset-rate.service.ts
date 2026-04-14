import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Observable';

import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';
import {  CustomerAssetRateAPI, AssetCustomerRateQueryParameter, AssetCustomerRateRecord, ICustomerAssetRateService,
VisiableBUParameter, BusinessUnitRecord, AssetCustomerRateResult, OperationResult, AssetCustomerRateDeleteParameter }
from 'app/models/pages/customer/customer-asset-rate';

@Injectable({
    providedIn: 'root'
  })
  export class CustomerAssetRateService implements ICustomerAssetRateService {
    constructor(
      private ts: TokenService,
      private cs: CoreService) {
      }

      GetCustomerAssetList(inputParameter: AssetCustomerRateQueryParameter): Promise<AssetCustomerRateResult> {
        const promise = new Promise<AssetCustomerRateResult>(
            (resolve, reject) => {this._GetCustomerAssetList(resolve, reject, inputParameter);}
            );

      return promise;
    }

    GetBUList(parameter: VisiableBUParameter): Observable<BusinessUnitRecord[]> {
      let url = CustomerAssetRateAPI.BusinessUnits;
      let params = "?userId=" + parameter.userid;
      let urlWithParams = url + params;
      return this.cs.getObject<BusinessUnitRecord[]>(urlWithParams);
    }

    UpdateCustomerAssetRate(record: AssetCustomerRateRecord): Observable<OperationResult> {
      let url = CustomerAssetRateAPI.Update;
      return this.cs.postObject<OperationResult>(url, record);
    }

    DeleteCustomerAssetRate(record: AssetCustomerRateDeleteParameter): Observable<OperationResult> {
     let url = CustomerAssetRateAPI.Delete;
     return this.cs.postObject<OperationResult>(url, record);
    }

    private _GetCustomerAssetList(resolve: any, reject: any, parameter: AssetCustomerRateQueryParameter) {

        let url = CustomerAssetRateAPI.List;
        let params = "?business_unit_id=" + parameter.business_unit_id + "&customer_id=" + parameter.customer_id;
        params += "&pageSize=" + parameter.pageSize + "&skipPage=" + parameter.skipPage;
        if(parameter.searchingstring != null && parameter.searchingstring != null) {
            params += "&searchingstring=" + parameter.searchingstring;
        }

        const emptyArrary = new AssetCustomerRateResult();
        emptyArrary.totalCount = 0;
        emptyArrary.data = [];

        let urlWithParams = url + params;
        const promise = this.cs.getObject<AssetCustomerRateResult>(urlWithParams).toPromise<AssetCustomerRateResult>();
        promise.then(
          res => {resolve(res);} )
          .catch(e => {resolve(emptyArrary)})
    }
  }