import { Injectable } from '@angular/core';

import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';

import { Observable } from 'rxjs/Observable';

import { JobCost, JobTypeInfo, JobTypeRecordQueryParameter, IJobTypeService } from 'app/models/pages/timesheet/jobType';

@Injectable({
    providedIn: 'root'
  })
  export class JobTypeService implements IJobTypeService {
    GetJobTypeInfo(parameter: JobTypeRecordQueryParameter): Observable<JobTypeInfo> {
        let url = JobCost.JobTypeInfo;
        let params = "?business_uint_id=" + parameter.business_uint_id + "&member_id=" + parameter.member_id + "&customer_id=0";
        let urlWithParams = url + params;
        const data = this.cs.getObject<JobTypeInfo>(urlWithParams);
        return data
    }
    GetJobTypeInfowithCust(parameter: JobTypeRecordQueryParameter): Observable<JobTypeInfo> {
        let url = JobCost.JobTypeInfo;
        let params = "?business_uint_id=" + parameter.business_uint_id + "&member_id=" + parameter.member_id + "&customer_id=" + parameter.customer_id;
        let urlWithParams = url + params;
        const data = this.cs.getObject<JobTypeInfo>(urlWithParams);
        return data
    }

    constructor(
      private ts: TokenService,
      private cs: CoreService) {
      }
  }