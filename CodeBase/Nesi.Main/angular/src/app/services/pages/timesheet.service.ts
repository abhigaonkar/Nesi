import { Injectable, OnInit } from '@angular/core';
import { Response } from '@angular/http';
import { CONFIG } from '../../configuration';
import { HttpService } from '../../core/http.service';
import { ServiceBase } from '../shared/serviceBase';
import { VisibleBusinessUnitDropDown } from '../../models/Shared/visibleBusinessUnit';
import { Observable } from 'rxjs/Observable';
import { TimeSheetUser } from '../../models/pages/timesheet/timesheetUser';
import { TimesheetList } from '../../models/pages/timesheet/timesheet-list';
import { TimeSheetProfile } from '../../models/pages/timesheet/timesheetProfile';
import { TimesheetWorkOrderCustomer } from '../../models/pages/timesheet/timesheetWorkOrderCustomer';
import { TimesheetWorkOrderWo } from '../../models/pages/timesheet/timesheetWorkOrderWo';
import { UpdateTimesheetWorkOrder } from '../../models/pages/timesheet/updateTimesheetWorkOrder';
import { UpdateTimesheetShop } from '../../models/pages/timesheet/updateTimesheetShop';
import { UpdateTimesheetQuote } from '../../models/pages/timesheet/updateTimesheetQuote';
import { UpdateTimesheetTelem } from '../../models/pages/timesheet/updateTimesheetTelem';
import { InsertTimeSheetWorkOrder } from '../../models/pages/timesheet/InsertTimesheetWorkOrder';
import { InsertTimeSheetQuote } from '../../models/pages/timesheet/InsertTimesheetQuote';
import { InsertTimeSheetTelem } from '../../models/pages/timesheet/InsertTimesheetTelem';
import { InsertTimeSheetShop } from '../../models/pages/timesheet/InsertTimesheetShop';
import { DefaultValueFromScheduler } from '../../models/pages/timesheet/defaultValueFromScheduler';
import { TokenService } from '../authentication/tokenService';
import { ExpenseCurrentProfile } from '../../models/pages/timesheet/expenseCurrentUserProfile';
import { ExpenseSelectedUserProfile } from '../../models/pages/timesheet/expenseSelectedUserProfile';
import { LabelValueString } from '../../models/Shared/labelValueInt';
import { Province } from 'models/pages/timesheet/provinces';
import { ShopTimeTypeRecord, TimesheetTransfer, TimesheetTransferResult, TransferAPI } from '../../models/pages/timesheet/transfer';

@Injectable()
export class TimesheetService extends ServiceBase {
  private profile: TimeSheetProfile;
  private vbu: VisibleBusinessUnitDropDown[];
  public set Profile(value: TimeSheetProfile) {
    this.profile = value;
  }

  public get Profile(): TimeSheetProfile {
    if (this.profile && this.ts.currentUser.id === this.profile.userId) {
      return this.profile;
    } else {
      return null;
    }
  }

  public set visibleBusinessUnitList(value: VisibleBusinessUnitDropDown[]) {
    this.vbu = value;
  }
  public get visibleBusinessUnitList(): VisibleBusinessUnitDropDown[] {
    if (this.profile && this.ts.currentUser.id === this.profile.userId) {
      return this.vbu;
    } else {
      return null;
    }
  }


  constructor(
    protected http: HttpService,
    protected ts: TokenService,
  ) {
    super(http);
  }

  public GetExpenseCurrentUserProfile(): Observable<ExpenseCurrentProfile> {
    return this.getObject<ExpenseCurrentProfile>(CONFIG.apiURL.page.timesheet.expense.profile);
  }
  public GetExpenseSelectedUserProfile(userId: number): Observable<ExpenseSelectedUserProfile> {
    return this.getObject<ExpenseSelectedUserProfile>(CONFIG.apiURL.page.timesheet.expense.profile + '/' + userId.toString());
  }
  
  public GetExpenseSeller(search: string): Observable<string[]> {
    return this.postList<string>(CONFIG.apiURL.page.timesheet.expense.seller, { data: search });
  }


  public InsertWorkOrder(model: InsertTimeSheetWorkOrder): Observable<string> {
    return this.postString(CONFIG.apiURL.page.timesheet.WorkOrder, model);
  }
  public InsertQuote(model: InsertTimeSheetQuote): Observable<string> {
    return this.postString(CONFIG.apiURL.page.timesheet.Quote, model);
  }
  public InsertTelem(model: InsertTimeSheetTelem): Observable<string> {
    return this.postString(CONFIG.apiURL.page.timesheet.Telem, model);
  }
  public InsertShop(model: InsertTimeSheetShop): Observable<string> {
    return this.postString(CONFIG.apiURL.page.timesheet.Shop, model);
  }

  public UpdateTelem(model: UpdateTimesheetTelem): Observable<string> {
    return this.patchString(CONFIG.apiURL.page.timesheet.Telem, model);
  }

  public UpdateQuote(model: UpdateTimesheetQuote): Observable<string> {
    return this.patchString(CONFIG.apiURL.page.timesheet.Quote, model);
  }

  public UpdateShop(model: UpdateTimesheetShop): Observable<string> {
    return this.patchString(CONFIG.apiURL.page.timesheet.Shop, model);
  }

  public UpdateWorkOrder(model: UpdateTimesheetWorkOrder): Observable<string> {
    return this.patchString(CONFIG.apiURL.page.timesheet.WorkOrder, model);
  }

  
  public GetReviewPDf(WOID:number,IsDailySignoff:boolean): Observable<Response>{
    return  this.http.get(CONFIG.apiURL.page.timesheet.PreviewPdf+WOID.toString()+"/"+IsDailySignoff);
  }

  private getWorkOrderCustomer(buId: number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.WorkOrderCustomer + buId.toString());
  }

  public WorkOrderWos(buId: number, userId: number, customerId: number): Observable<TimesheetWorkOrderWo[]> {
    return this.ConvertList<TimesheetWorkOrderWo>(this.getWorkOrderWo(buId, userId, customerId));
  }

  public GetValueFromScheduler(userId: number, date: Date): Observable<DefaultValueFromScheduler> {
    return this.getObject<DefaultValueFromScheduler>(
      CONFIG.apiURL.page.timesheet.ValueFromScheduler + userId.toString() + '/' + date.toDateString()
    );
  }
  private getStartDate(userId: number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.startDate + userId.toString());
  }

  public GetCutPO(woId: number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.cutPO + woId.toString());
  }

  private getPastDays(userId: number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.pastDays + userId.toString());
  }
  private getCountry(BuId:number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.Country + BuId.toString());
  }

  public getProvinceList(userId:number): Observable<Response>{
    return this.http.get(CONFIG.apiURL.page.timesheet.provinceList + userId.toString());
  }

  public getDefaultProvince(Buid:number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.defaultProvince + Buid.toString());
  }
  public GetPastDays(userId: number): Observable<string[]> {
    return this.ConvertData<string[]>(this.getPastDays(userId));
  }

  public GetStartDate(userId: number): Observable<string[]> {
    return this.ConvertData<string[]>(this.getStartDate(userId));
  }

  public GetProvinceList(userId:number): Observable<Province[]> {
    return this.ConvertData<Province[]>(this.getProvinceList(userId));
  }
  public GetDefaultProvince(Buid:number): Observable<string> {
    return this.ConvertData<string>(this.getDefaultProvince(Buid));
  }
  public GetCountry(BuId:number):Observable<string>{
    return this.ConvertData<string>(this.getCountry(BuId));
  }
  
  public WorkOrderWoById(woId: number): Observable<TimesheetWorkOrderWo> {
    return this.ConvertObject<TimesheetWorkOrderWo>(this.getWorkOrderWoById(woId));
  }

  private getWorkOrderWoById(woId: number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.WorkOrderWos + woId.toString());
  }

  public GetWorkOrderLabour(buId: number, userId: number, payTypeId: number): Observable<Number> {
    return this.ConvertData<Number>(this.getWorkOrderLabour(buId, userId, payTypeId));
  }

  private getWorkOrderLabour(buId: number, userId: number, payTypeId: number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.WorkOrderLabour
      + buId.toString() + '/' + userId.toString() + '/' + payTypeId.toString()
    );
  }
  public GetQuoteQuoteNameById(quoteId: number): Observable<string> {
    return this.ConvertData<string>(this.getQuoteQuoteNameById(quoteId));
  }

  private getQuoteQuoteNameById(quoteId: number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.QuoteQuoteName + quoteId.toString());
  }
  public GetQuoteCustomerNameById(custId: string): Observable<string> {
    return this.ConvertData<string>(this.getQuoteCustomerNameById(custId));
  }

  private getQuoteCustomerNameById(custId: string): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.QuoteCustomerName + custId);
  }
  private getWorkOrderWo(buId: number, userId: number, customerId: number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.WorkOrderWos
      + buId.toString() + '/'
      + userId.toString() + '/'
      + customerId.toString());
  }

  public WorkOrderCustomer(buId: number): Observable<TimesheetWorkOrderCustomer[]> {
    return this.ConvertList<TimesheetWorkOrderCustomer>(this.getWorkOrderCustomer(buId));
  }

  private getTimesheetList(userId: number, date: Date): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.list + userId.toString() + '/' + date.toDateString());
  }
  private getTimesheeProfile(): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.profile);
  }

  public TimeSheetProfile(): Observable<TimeSheetProfile> {
    return this.ConvertObject<TimeSheetProfile>(this.getTimesheeProfile());
  }
  public TimesheetListByUserIdDate(userId: number, date: Date): Observable<TimesheetList[]> {
    return this.ConvertList<TimesheetList>(this.getTimesheetList(userId, date));
  }
  private getTimeSheetVisibleBusinessUnitDropDownList(): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.visibleBusinessUnitDropDownList);
  }

  public TimesheetVisibleBusinessUnitDropDownList(): Observable<VisibleBusinessUnitDropDown[]> {
    return this.ConvertList<VisibleBusinessUnitDropDown>(this.getTimeSheetVisibleBusinessUnitDropDownList());
  }

  private getTimeSheetUserListByBusinessUnitId(buId: number): Observable<Response> {
    return this.http.get(CONFIG.apiURL.page.timesheet.userListByBusinessUnitID + buId.toString());
  }

  public TimeSheetUserListByBusinessUnitId(buId: number): Observable<TimeSheetUser[]> {
    return this.ConvertList<TimeSheetUser>(this.getTimeSheetUserListByBusinessUnitId(buId));
  }

  public ShopTimeTypeList(): Observable<ShopTimeTypeRecord[]> {
    return this.ConvertList<ShopTimeTypeRecord>(this.getShopTimeTypes());
  }

  private getShopTimeTypes(): Observable<Response> {
    return this.http.get( CONFIG.apiURL.page.timesheet.ShopShopType);
  }

  public TransferTimesheet(data: TimesheetTransfer): Observable<TimesheetTransferResult> {
    return this._TransferTimesheet(data);
  }

  private _TransferTimesheet(data: TimesheetTransfer): Observable<TimesheetTransferResult> {
    let url = TransferAPI.Transfer;
    return this.postObject<TimesheetTransferResult>(url, data);
  }

  public GetPdfPreview(WoId:string,IsDailySignoff:boolean,SelectedDate:Date): Observable<any> {
    return this.getObject<any>(CONFIG.apiURL.page.timesheet.PreviewPdf+WoId+"/"+IsDailySignoff+"/"+SelectedDate.toDateString());
  }

  public SendingEmails(WoId: number,SelectedDate:Date, contacts: string): Observable<any> {
    let url = CONFIG.apiURL.page.timesheet.SendingEmail;
    let data = {
      woid: WoId,
      date: SelectedDate,
      contacts: contacts
    };
    return this.postObject<TimesheetTransferResult>(url, data);
  }

}
 

