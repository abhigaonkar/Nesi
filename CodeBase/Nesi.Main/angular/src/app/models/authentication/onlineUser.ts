export interface OnlineUser {
  id: number;
  name: string;
  fullName: string;
  companyName: string;
  businessUnitId: number;
  businessUnitName: string;
  issueTime: Date;
  activeTime: string;
  expires: Date;
  isExpired: boolean;
  expireSecond: number;
  gender: string;
  photo: string;
  forceChangePassword: boolean;
  fvrPassed: boolean;
  logo: string;
  icon: string;
  hasFvr: boolean;
  tax_entity_id: number;
  isLdapUser: boolean;
  force_beta: boolean;
  save_global_layout: boolean;
  show_daily_approval:boolean;
  // visibleBusinessUnitList: VisibleBusinessUnitDropDown[];
}
