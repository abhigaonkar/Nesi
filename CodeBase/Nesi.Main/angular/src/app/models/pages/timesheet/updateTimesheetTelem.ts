import { UpdateTimeSheetBase } from './UpdateTimesheetBase';
import { TelemRecord } from './telemRecord';
export interface UpdateTimesheetTelem  extends UpdateTimeSheetBase {
  memberTime_ID: number;
  rating: number;
  numberOfHours: number;
  memberTime_WoComment_ID: number;
  memberTime_WoComment: string;
  record: TelemRecord;
}
