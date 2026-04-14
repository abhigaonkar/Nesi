import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { TimesheetWorkOrderCustomer } from '../../../models/pages/timesheet/timesheetWorkOrderCustomer';
import { TimesheetWorkOrderCustomerComponent } from '../timesheetWorkOrderCustomer/timesheetWorkOrderCustomer.component';
import { TimesheetQuoteQuoteComponent } from '../timesheetQuoteQuote/timesheetQuoteQuote.component';
import { TimesheetValue } from '../../../models/pages/timesheet/timesheetValue';
import { CONFIG } from '../../../configuration';
import { TimeSheetComponentBase } from '../interface/timesheetComponentBase';
import { TimesheetList } from '../../../models/pages/timesheet/timesheet-list';
import { Store } from '@ngrx/store';
import * as fromMessage from '../../../actions/layout/growlMessage';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { UpdateTimesheetQuote } from '../../../models/pages/timesheet/updateTimesheetQuote';
import { DefaultValueFromScheduler } from '../../../models/pages/timesheet/defaultValueFromScheduler';
import { TokenService } from '../../../services/authentication/tokenService';
import { InsertTimeSheetQuote } from '../../../models/pages/timesheet/InsertTimesheetQuote';
import * as DATE from '../../../services/helper/datetime';

@Component({
  selector: 'nesi-timesheetQuote',
  templateUrl: './timesheetQuote.component.html',
  styleUrls: ['./timesheetQuote.component.css']
})
export class TimesheetQuoteComponent implements OnInit, TimeSheetComponentBase {

  @ViewChild(TimesheetWorkOrderCustomerComponent)
  QuoteCustomer: TimesheetWorkOrderCustomerComponent;
  @ViewChild(TimesheetQuoteQuoteComponent)
  QuoteQuote: TimesheetQuoteQuoteComponent;


  @Input()
  disabled = false;

  public APIURLQuoteCustomerList: string;
  public APIURLQuoteQuoteList: string;
  public customerList: any[];

  public selectedCustomer: string;
  public rating: number;
  public percentComplete: number;
  public selectedQuote: number;
  public hours: number;
  public inputValue: TimesheetValue;

  public selectedRow: TimesheetList;
  public defaultValue: DefaultValueFromScheduler;
  public minvalue:number;
  public maxvalue:number;
  public currenthourValue:number;

  constructor(
    public cs: CoreService,
    private store: Store<fromRoot.State>,
    private tss: TimesheetService,
    private ts: TokenService,
  ) { }


  ngOnInit() {
  }

  OnChangeCustomer(event: any) {
    CONFIG.LOG(event.item, 'onchangecustomer quote quote');
    CONFIG.LOG(this.selectedCustomer, 'onchangecustomer quote quote');

    if (event.item && this.selectedCustomer && event.item['bu_Id']) {
      const url = this.getQuoteQuoteURL(event.item['bu_Id'], this.selectedCustomer);
      this.QuoteQuote.getList(url);
    } else {
      this.QuoteQuote.clear();
    }
  }

  OnChangeQuote(event: any) {

  }

  public LoadInputvalue(value: TimesheetValue) {
    if (value && value.businessUnitId) {
      this.inputValue = value;
      this.APIURLQuoteCustomerList = this.getQuoteCustomerURL(this.inputValue.businessUnitId);
      this.QuoteCustomer.getList(this.APIURLQuoteCustomerList);
    } else {
      this.ClearDropDownList();
    }
  }

  private getQuoteCustomerURL(buId: number) {
    if (!this.inputValue.userId) { return; }
    return CONFIG.apiURL.page.timesheet.QuoteCustomer + buId.toString() + '/' + this.inputValue.userId.toString();
  }
  private getQuoteQuoteURL(buId: number, custId: string) {
    return CONFIG.apiURL.page.timesheet.QuoteQuote
      + buId.toString() + '/'
      + custId;
  }

  public ClearDropDownList() {
    this.QuoteCustomer.clear();
    this.QuoteQuote.clear();
  }

  AfterLoadUrl() {
    CONFIG.LOG('afterload url', 'quote timesheet');
    this.setDefaultValue();
    this.selectRow();
  }
  SelectRow(item: TimesheetList, value: TimesheetValue): void {
    this.selectedRow = item;
    if (this.APIURLQuoteCustomerList) {
      this.selectRow();
    } else {
      this.LoadInputvalue(value);
    }

  }

  private selectRow() {
    if (this.selectedRow) {
      if(this.selectedRow.hours>=0)
      { 
        this.minvalue=0;
        this.maxvalue=null;
      }
      else
      {
        this.minvalue=null;
        this.maxvalue=0;
      }
      this.hours = this.selectedRow.hours;
      this.currenthourValue=this.selectedRow.hours;
      this.percentComplete = this.selectedRow.wo_percent_complete;
      this.rating = this.selectedRow.rating;
      // this.selectedCustomer = this.selectedRow.memberTime_Cust_No;

      this.QuoteCustomer.showOne(this.selectedRow.membertime_customer_name, this.selectedRow.custNo);
      // this.tss.GetQuoteCustomerNameById(this.selectedRow.custNo).subscribe(
      //   (res: string) => {
      //     this.QuoteCustomer.showOne(res, this.selectedRow.custNo);
      //   });
      this.tss.GetQuoteQuoteNameById(Number(this.selectedRow.workorderId)).subscribe(
        (res: string) => {
          if (!this.selectedRow) { return; }
          this.QuoteQuote.showOne(res, this.selectedRow.workorderId);
        });
    }
  }
  public setDefaultValue() {
    CONFIG.LOG(this.defaultValue, 'defaultvalue setdefaultvalue')


    if (this.defaultValue && this.ts.currentUser.id === this.inputValue.userId) {
      CONFIG.LOG(this.defaultValue, 'defaultvalue setdefaultvalue')
      this.selectedCustomer = this.defaultValue.customer_Id.toString();
      //this.hours = this.defaultValue.hours;
      // if(this.defaultValue.hours>=0)
      // { 
      //   this.minvalue=0;
      //   this.maxvalue=null;
      // }
      // else
      // {
      //   this.minvalue=null;
      //   this.maxvalue=0;
      // }
     // this.currenthourValue=this.defaultValue.hours;
      this.QuoteCustomer.value = this.selectedCustomer;
    } else {
      this.selectedCustomer = null;
      this.hours = null;
      this.minvalue=0;
      this.maxvalue=null;
      this.QuoteCustomer.value = null; 
    }
  }

  CancelSelectedRow(): void {
    this.selectedCustomer = null;
    this.selectedQuote = null;
    this.selectedRow = null;
    this.percentComplete = null;
    this.hours = null;
    this.rating = null;
    this.QuoteCustomer.loadDropDown();
    this.QuoteQuote.clear();
  }
  get OutputUpdateValue(): UpdateTimesheetQuote {

    return {

      date: DATE.ToDateOnly(this.inputValue.selectedDate),
      selectedBusinessUnitId: this.inputValue.businessUnitId,
      selectedUserId: this.inputValue.userId,

      memberTime_ID: this.selectedRow.id,
      numberOfHours: this.hours,
      percentComplete: this.percentComplete,
      rating: this.rating,
    }
  }
  UpdateRow(items: TimesheetList[],
    success: (msg: string) => void,
    failed: (msg: string) => void): void {
    this.tss.UpdateQuote(this.OutputUpdateValue)
      .subscribe((res: string) => {
        const updateItem = this.OutputUpdateValue;
        const item = items.find(x => x.id === updateItem.memberTime_ID);
        CONFIG.LOG(JSON.stringify(item), 'update quote found item');
        item.hours = updateItem.numberOfHours;
        item.rating = updateItem.rating;
        item.wo_percent_complete = updateItem.percentComplete;
        this.CancelSelectedRow();
        success(res);
      },
      (err: any) => {
        failed(err); 
      });

  }
  get OutputInsertValue(): InsertTimeSheetQuote {

    return {
      date: DATE.ToDateOnly(this.inputValue.selectedDate),
      selectedBusinessUnitId: this.inputValue.businessUnitId,
      selectedUserId: this.inputValue.userId,
      payTypeId: 1,

      memberTime_WoComment_ID: 0,
      numberOfHours: this.hours,
      rating: this.rating ? this.rating : 0,
      percentComplete: this.percentComplete ? this.percentComplete : 0,
      memberTime_WoComment: 'N/A',

      selectedCustomerId: Number(this.selectedCustomer),
      selectedCustomerName: this.QuoteCustomer.getLabelNameByValue(this.selectedCustomer.toString()),
      selectedQuoteId: this.selectedQuote,
      selectedQuoteName: this.QuoteQuote.getLabelNameByValue(this.selectedQuote.toString())
    }
  }

  InsertRow(items: TimesheetList[],
    success: (msg: string) => void,
    failed: (msg: string) => void): void {

    CONFIG.LOG(this.OutputInsertValue, 'insert workorder timesheet');
    this.tss.InsertQuote(this.OutputInsertValue)
      .subscribe((res: string) => {
        this.CancelSelectedRow();
        success(res);
      },
      (err: any) => {
        failed(err); 
      })


  }
  ValidateValue(): boolean {
    if (!(this.selectedCustomer)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select a customer.'));
      return false;
    }
    if (!(this.selectedQuote)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select a quote.'));
      return false;
    }
    if (!this.hours) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input hours.'));
      return false;
    }
    return true;
  }

 

  onChange(event)
  {
      if(this.hours!=null)
      { 
        if(this.minvalue!=null)
        {
          if(this.hours>=this.minvalue)
          {
            this.currenthourValue=this.hours;
          }
          else{
            this.hours=this.currenthourValue;
          }

        }
        if(this.maxvalue!=null)
        {
          if(this.hours<=this.maxvalue)
          {
            this.currenthourValue=this.hours;
          }
          else{
            this.hours=this.currenthourValue;
          }
        }
      }

  }

}
