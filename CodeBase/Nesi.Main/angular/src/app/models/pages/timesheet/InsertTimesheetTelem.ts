import { InsertTimeSheetBase } from './InsertTimesheetBase';
import { TelemRecord } from './telemRecord';
export interface InsertTimeSheetTelem extends InsertTimeSheetBase {
  customerBusinessUnitId: number;
  customerBusinessUnitName: string;
  record: TelemRecord;
}
