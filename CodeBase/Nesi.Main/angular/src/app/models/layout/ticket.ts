export interface Ticket {
  id: number;
  ordered_Id: string;
  issue: string;
  mod_Date: Date;
  ticketheader_Id: number;
  raisedIssue: string;
  ticketheader_modified_date: Date;
  ticketheader_private: boolean;
  dateCreated: Date;
  pageName: string;
  release_Id: number;
  status: string;
  typeOfTicket: string;
  asignedTo: string;
  raisedBy: string;
  priority: string;
  priority_Id: number;
  groupname: string;
  exp_fin: Date;
  g_admin: number;
  status_id: number;
  type: number;
  v: number;
}
