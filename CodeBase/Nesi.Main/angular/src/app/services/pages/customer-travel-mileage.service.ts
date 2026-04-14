import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';

import { GetNewLaborRateListParameter, GenericLaborRateRecord, TravelMileageApi, UpdateParameter, UpdateResult, DeleteParameter }
    from 'models/pages/customer/customer-travel-mileage';

@Injectable({
    providedIn: 'root'
})
export class CustomerTravelMileageRateService {
    constructor(
        private ts: TokenService,
        private cs: CoreService) {
    }

    GetTravelMileageRates(parameter: GetNewLaborRateListParameter): Observable<Array<GenericLaborRateRecord>> {
        let url = TravelMileageApi.List;
        let params = "?customerId=" + parameter.customerId + "&businessUnitId=" + parameter.businessUnitId;
        let urlWithParams = url + params;
        const data = this.cs.getObject<Array<GenericLaborRateRecord>>(urlWithParams);
        return data
    }

    UpdateTraveMileageRates(parameter: UpdateParameter): Observable<UpdateResult> {
        let url = TravelMileageApi.Save;
        return this.cs.postObject<UpdateResult>(url, parameter);
    }

    DeleteTraveMileageRates(parameter: DeleteParameter): Observable<UpdateResult> {
        let url = TravelMileageApi.Delete;
        return this.cs.postObject<UpdateResult>(url, parameter);
    }
}