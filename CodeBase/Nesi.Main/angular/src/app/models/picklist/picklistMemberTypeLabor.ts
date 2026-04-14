import { PicklistLaborType } from './picklistLaborType';
export interface PickListMemberTypeLabor {
  membertype_id: number;
  membertype_name: string;
  labors: PicklistLaborType[];
  is_checked: boolean;
}
