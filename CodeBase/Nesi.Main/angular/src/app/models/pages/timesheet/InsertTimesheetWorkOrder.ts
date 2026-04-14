import { InsertTimeSheetBase } from './InsertTimesheetBase';
import { TimesheetHourTypeRecord } from './timesheetHourType';

export interface InsertTimeSheetWorkOrder extends InsertTimeSheetBase {
  selectedCustomerId: number;
  selectedCustomerName: string;
  selectedWorkOrderId: number;
  selectedWorkOrderName: string;
  selectedTsLiteType: number;

  selectedJobType: number;
  allow_jobtype_selection: boolean;
  entry_type: string;
  timesheetHourTypeList: Array<TimesheetHourTypeRecord>;
  scope_id:number;
  mileage_value:number;
  mileage_unit:string;
  is_prevailing_wage: boolean;
}
