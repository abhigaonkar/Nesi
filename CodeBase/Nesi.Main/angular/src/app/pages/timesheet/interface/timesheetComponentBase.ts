import { TimesheetValue } from '../../../models/pages/timesheet/timesheetValue';
import { TimesheetList } from '../../../models/pages/timesheet/timesheet-list';
import { DefaultValueFromScheduler } from '../../../models/pages/timesheet/defaultValueFromScheduler';
export interface TimeSheetComponentBase {
  defaultValue: DefaultValueFromScheduler;
  setDefaultValue();
  LoadInputvalue(value: TimesheetValue): void;
  SelectRow(item: TimesheetList, value: TimesheetValue): void;
  CancelSelectedRow(): void;
  UpdateRow(items: TimesheetList[],
    success: (msg: string) => void,
    failed: (msg: string) => void): void;
  InsertRow(items: TimesheetList[],
    success: (msg: string) => void,
    failed: (msg: string) => void): void;
  ValidateValue(): boolean;
  ClearDropDownList(): void;
  
}
