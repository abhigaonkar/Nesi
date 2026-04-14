import { DefaultValueFromScheduler } from './defaultValueFromScheduler';
import { CurrentPayPeriod } from '../../Shared/currentPayPeriod';
import { PayPeriod } from '../../Shared/payPeriod';
export interface TimeSheetProfile {
  userId: number;
  allowUnlinkedTimesheet: boolean;
  canSwitchBusinessUnits: boolean;
  canPickAnyDate: boolean;
  isPayrollAdmin: boolean;
  canDoShopTime: boolean;
  vacationVisible: boolean;
  bankVisible: boolean;
  minDate: string;
  maxDate: string;

  isCurrentPayPeriod: boolean;
  payPeriod: PayPeriod;
  currentPayPeriod: CurrentPayPeriod;
  defaultValueFromScheduler: DefaultValueFromScheduler;
  pastDays: string[];
}
