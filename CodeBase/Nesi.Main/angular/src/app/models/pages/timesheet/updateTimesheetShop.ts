import { UpdateTimeSheetBase } from './UpdateTimesheetBase';
export interface UpdateTimesheetShop  extends UpdateTimeSheetBase {
  memberTime_ID: number;
  rating: number;
  numberOfHours: number;
  memberTime_WoComment_ID: number;
  memberTime_WoComment: string;
  internal_project_id : number;
}
