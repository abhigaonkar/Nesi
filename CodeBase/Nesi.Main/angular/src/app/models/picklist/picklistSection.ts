import { PickListItem } from './picklistItem';
export interface PickListSection {
  section_id: number;
  section_name: string;
  items: PickListItem[];
}
