import { Injectable } from '@angular/core';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { Observable } from 'rxjs/Observable';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { FormGroup } from '@angular/forms';
import { HttpService } from 'app/core/http.service';
import { ServiceBase } from 'app/services/shared/serviceBase';

@Injectable()
export class EmployeeService {

    public applicant_statusList = [
        { label: 'New', value: 'New' },
        { label: 'Offered', value: 'Offered' },
        { label: 'Suspended', value: 'Suspended' },
        { label: 'Rejected', value: 'Rejected' },
        { label: 'Deleted', value: 'Deleted' },
        { label: 'Hired', value: 'Hired' }
      ];

    public memberid: number;
    public is_backoffice: boolean;
    protected _profile: any;
    public get profile() {
        return this._profile || { entity: {} };
    }

    public set profile(value: any) {
        this._profile = value;
    }

    constructor(private cs: CoreService) {

    }


    public checkField(url: string, value: string) {
        return this.cs.postObject<LabelValueInt>(
            this.getUrl(url),
            { data: value });
    }

    public getUrl(path: string): string {
        if (!this.memberid) {
            this.memberid = 0;
        }
        const o = path.replace('$member_id', this.memberid.toString());

        return o;
    }
    syncITLdapPassword(): Observable<string> {
        return this.cs.getString(CONFIG.apiURL.page.employee.it.syncLdap + this.memberid.toString());
    }
}

