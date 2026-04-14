export interface ExpenseReimbursement {
  id_expense: number;
  id_member: number;
  approved: number;
  receipt_number: string;
  date_requested: Date;
  date_purchased: Date;
  date_start: Date;
  date_end: Date;
  id_payperiod: number;
  id_seller: number;
  cust_number: number;
  wo_number: number;
  item_text: string;
  amount: number;
  currency: number;
  woprog_id: number;
  customer_id: number;
  master_id: number;
  has_file: boolean;
  file_ext: string;
  file_mime: string;
  distance: string;
  unit_distance: number;
  attendees: string;
  type: boolean;
  approved_by: number;

}
