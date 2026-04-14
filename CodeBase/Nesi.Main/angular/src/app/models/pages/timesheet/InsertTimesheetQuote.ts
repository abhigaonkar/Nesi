import { InsertTimeSheetBase } from './InsertTimesheetBase';
export interface InsertTimeSheetQuote extends InsertTimeSheetBase {
  selectedCustomerId: number;
  selectedCustomerName: string;
  selectedQuoteId: number;
  selectedQuoteName: string;

}
