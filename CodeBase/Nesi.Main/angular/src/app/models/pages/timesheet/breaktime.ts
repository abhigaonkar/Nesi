import { Observable } from 'rxjs/Observable';

export class BranchBreakTimeRequirementInputParameter
{
    member_id: number;
    business_unit_id: number;
}

export class BreakTimeQueryParameter
{
    member_id: number;
    business_unit_id: number;
    date: Date;
}

export class BreakTimeRecordRequirementRecord
{
    monitor_breaktime: boolean;
    def_start_time: string;
    def_morning_start: string;
    def_morning_dur: number;
    def_lunch_start: string;
    def_lunch_dur: number;
    def_afternoon_start: string;
    def_afternoon_dur: number;
}

export class BreakTimeRecord
{
    id: number;
    date: Date;
    business_unit_id: number;
    member_id: number;
    start_time: string;
    morning_break_start: string;
    morning_break_duration: number;
    lunch_break_start: string;
    lunch_break_duration: number;
    afternoon_break_start: string;
    afternoon_break_duration: number;
    entered_by_member_id: number;
    locked: boolean;
}

export enum OperationType {
    Add = 0,
    Edit = 1
}

export class BreakTimeRecordResult extends BreakTimeRecord 
{
    success: boolean;
    reason: string;
    operationType: number;
}

export class BreakTimeRecordOnGivenDate {
    showAddingOverlay: boolean;
    showEditingButton: boolean;
    memberId: number;
    date: string;
    recordId: number;
    breaktimeRecord: BreakTimeRecord;
}

export interface IBreakTimeService
{
    // To see whether the given branch requires breaktime records.
    GetBranchBreakTimeRequirement(inputParameter: BranchBreakTimeRequirementInputParameter) : Observable<BreakTimeRecordRequirementRecord>;
    AddBreakTimeRecord(recordParameter: BreakTimeRecord): Observable<BreakTimeRecordResult>;
    UpdateBreakTimeRecord(recordParameter: BreakTimeRecord): Observable<BreakTimeRecordResult>;
    GetBreakTimeRecord(inputParameter: BreakTimeQueryParameter): Observable<Array<BreakTimeRecord>>;

    // Check given employee's breaktime record requirement on given date.
    GetBreakTimeRequirementOnSpecificDate(inputParmeter: BreakTimeQueryParameter): Observable<BreakTimeRecordOnGivenDate>;
}

export const BreakTimeAPI = {
    // GET /api/Page/Timesheet/Breaktime/BreakTimeRequirement
    // GET /api/Page/Timesheet/Breaktime/BreakTimeRecordRequired
    // POST /api/Page/Timesheet/Breaktime/AddBreakTimeRecord
    // POST /api/Page/Timesheet/Breaktime/UpdateBreakTimeRecord
    // GET /api/Page/Timesheet/Breaktime/BreakTimeRecord

    BreakTimeRequirement: 'api/Page/Timesheet/Breaktime/BreakTimeRequirement',
    BreakTimeRecordRequired: 'api/Page/Timesheet/Breaktime/BreakTimeRecordRequired',
    AddBreakTimeRecord: 'api/Page/Timesheet/Breaktime/AddBreakTimeRecord',
    UpdateBreakTimeRecord: 'api/Page/Timesheet/Breaktime/UpdateBreakTimeRecord',
    BreakTimeRecord:'api/Page/Timesheet/Breaktime/BreakTimeRecord'
}
