import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { HttpService } from '../../core/http.service';
import { ServiceBase } from '../shared/serviceBase';
import { TokenService } from '../authentication/tokenService';

import {
  BranchBreakTimeRequirementInputParameter,
  BreakTimeQueryParameter,
  BreakTimeRecord,
  BreakTimeRecordRequirementRecord,
  BreakTimeRecordResult,
  OperationType,
  BreakTimeRecordOnGivenDate,
  IBreakTimeService,
  BreakTimeAPI 
} from '../../models/pages/timesheet/breaktime';

@Injectable({
  providedIn: 'root'
})
export class BreaktimeService extends ServiceBase implements IBreakTimeService {
  GetBranchBreakTimeRequirement(inputParameter: BranchBreakTimeRequirementInputParameter): Observable<BreakTimeRecordRequirementRecord> {
    let url = BreakTimeAPI.BreakTimeRequirement;
    let params="?member_id=" + inputParameter.member_id + "&business_unit_id=" + inputParameter.business_unit_id;
    let urlWithParams = url + params;
    return this.getObject<BreakTimeRecordRequirementRecord>(urlWithParams);
  }

  AddBreakTimeRecord(recordParameter: BreakTimeRecord): Observable<BreakTimeRecordResult> {
    let url = BreakTimeAPI.AddBreakTimeRecord;
    return this.postObject<BreakTimeRecordResult>(url, recordParameter);
  }

  UpdateBreakTimeRecord(recordParameter: BreakTimeRecord): Observable<BreakTimeRecordResult> {
    let url = BreakTimeAPI.UpdateBreakTimeRecord;
    return this.postObject<BreakTimeRecordResult>(url, recordParameter);
  }

  GetBreakTimeRecord(inputParameter: BreakTimeQueryParameter): Observable<Array<BreakTimeRecord>> {
    let url = BreakTimeAPI.BreakTimeRecord;
    let params="?member_id=" + inputParameter.member_id + "&business_unit_id=" + inputParameter.business_unit_id + "&date=" + inputParameter.date;
    let urlWithParams = url + params;
    return this.getObject<Array<BreakTimeRecord>>(urlWithParams);
  }

  GetBreakTimeRequirementOnSpecificDate(inputParmeter: BreakTimeQueryParameter): Observable<BreakTimeRecordOnGivenDate> {
    let url = BreakTimeAPI.BreakTimeRecordRequired;
    let params="?member_id=" + inputParmeter.member_id + "&business_unit_id=" + inputParmeter.business_unit_id + "&date=" + inputParmeter.date.toDateString();
    let urlWithParams = url + params;
    return this.getObject<BreakTimeRecordOnGivenDate>(urlWithParams);
  }

  constructor(
    protected http: HttpService,
    protected ts: TokenService
  ) {
    super(http);
  }
}
