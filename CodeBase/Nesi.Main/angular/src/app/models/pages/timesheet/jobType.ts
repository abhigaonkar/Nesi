import { Observable } from 'rxjs/Observable';

export class JobTypeRecordQueryParameter
{
    business_uint_id: number;
    member_id: number;
    customer_id: number;
}

export class JobTypeInfo {
    showJobType: boolean;
    jobTypes: Array<JobTypeRecord>;
    defaultValue: JobTypeRecord;

    constructor() 
    {
        this.showJobType = false;
    }
}

export class JobTypeRecord {
    membertype_id: number;
    membertype_name: string;
    reports_to: number;
    paytypeAllowed : string[];
    extraPaytypes: string[];
}

export interface IJobTypeService
{
    GetJobTypeInfo(parameter: JobTypeRecordQueryParameter): Observable<JobTypeInfo>;
}

export const JobCost = {
    JobTypeInfo: 'api/Page/Timesheet/Jobtype/Jobtype',
}