
import { Observable } from 'rxjs/Observable';

export class GetNewLaborRateListParameter {
    customerId: number;
    businessUnitId: number;
}

export class GenericLaborRateRecord {
    customer_id: number;
    business_unit: number;
    paytype_id: number;
    paytype: string;
    chargeout: number;
    from: Date;
    to: Date;
    visible: boolean;
    valid: boolean;
}

export class UpdateParameter extends GenericLaborRateRecord {
    action: number; // 1
}

export class DeleteParameter extends GenericLaborRateRecord {
    action: number; // 2
}

export class UpdateResult {
    okay: boolean;
    error: string;
}

export const TravelMileageApi = {
    List: 'api/Page/mileage/List',
    Save: 'api/Page/mileage/Save',
    Delete: 'api/Page/mileage/Delete'
}