import { LabelValueInt } from '../../Shared/labelValueString';
export interface BankSummary {
  balance_money: number;
  balance_hours: number;
  ledger: any[];
  history: LabelValueInt[];
  availableHours: number;
  withdrawableHours: number;
  payrate: number;
  currentPayPeriod: string;
}
