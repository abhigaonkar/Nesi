import { InsertTimeSheetBase } from './InsertTimesheetBase';
export interface InsertTimeSheetShop extends InsertTimeSheetBase {
  selectedShopTimeTypeId: number;
  selectedShopTimeTypeName: string;
  internal_project_id : number;
}
