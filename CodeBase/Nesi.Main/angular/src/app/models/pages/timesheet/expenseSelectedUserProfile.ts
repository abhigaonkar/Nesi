import { LabelValueString } from '../../Shared/labelValueInt';
import { ExpenseWorkorder } from './expenseWorkorder';
export interface ExpenseSelectedUserProfile {
  userId: number;
  currency: number;
  workOrders: ExpenseWorkorder[];
  categories: LabelValueString[];
  creditCardCategories: LabelValueString[];
}
