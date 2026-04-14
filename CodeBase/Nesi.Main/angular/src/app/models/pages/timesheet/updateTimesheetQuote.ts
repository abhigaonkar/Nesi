import { UpdateTimeSheetBase } from './UpdateTimesheetBase';
export interface UpdateTimesheetQuote extends UpdateTimeSheetBase {
  memberTime_ID: number;
  rating: number;
  percentComplete: number;
  numberOfHours: number;
}
