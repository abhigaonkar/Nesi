import { LabelValueInt } from '../../Shared/labelValueString';
import { PayPeriod } from '../../Shared/payPeriod';
export interface ExpenseCurrentProfile {
  userId: number;
  isAdmin: boolean;
  visibleUserList: LabelValueInt;
  subUsers: string;
  canSelecteUsers: boolean;
  currentOpenPayPeriod: PayPeriod;
  minDate: Date;
  maxDate: Date;
  uploadFullPath: string;
  allow_unlinked_timesheet: boolean;
  currency:number;
}
