import { UpdateTimeSheetBase } from './UpdateTimesheetBase';
export interface UpdateTimesheetWorkOrder extends UpdateTimeSheetBase {
  memberTime_ID: number;
  rating: number;
  percentComplete: number;
  numberOfHours: number;
  memberTime_WoComment_ID: number;
  memberTime_WoComment: string;
  selectedWorkOrderId: number;
  scope_id: number;
  prov_id: number;
  mileage_value:number;
  mileage_unit:string;

  selectedJobType: number;
  allow_jobtype_selection: boolean;
  is_prevailing_wage: boolean;
}
