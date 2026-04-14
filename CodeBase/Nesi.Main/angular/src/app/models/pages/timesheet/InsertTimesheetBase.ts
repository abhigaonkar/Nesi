export interface InsertTimeSheetBase {
  date: Date;
  selectedUserId: number;
  selectedBusinessUnitId: number;

  numberOfHours: number;
  rating: number;
  percentComplete: number;
  memberTime_WoComment_ID: number;
  memberTime_WoComment: string;
  payTypeId: number;


}
