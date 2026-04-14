import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { TimesheetWorkOrderCustomerComponent } from 'app/pages/timesheet/timesheetWorkOrderCustomer/timesheetWorkOrderCustomer.component';
import { TimesheetValue } from '../../../models/pages/timesheet/timesheetValue';
import { CONFIG } from '../../../configuration';
import { TimesheetList } from '../../../models/pages/timesheet/timesheet-list';
import { TimeSheetComponentBase } from '../interface/timesheetComponentBase';
import { UpdateTimesheetQuote } from '../../../models/pages/timesheet/updateTimesheetQuote';
import { UpdateTimesheetTelem } from '../../../models/pages/timesheet/updateTimesheetTelem';
import { Store } from '@ngrx/store';
import * as fromMessage from '../../../actions/layout/growlMessage';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { DefaultValueFromScheduler } from '../../../models/pages/timesheet/defaultValueFromScheduler';
import { TokenService } from '../../../services/authentication/tokenService';
import { InsertTimeSheetTelem } from '../../../models/pages/timesheet/InsertTimesheetTelem';
import * as DATE from '../../../services/helper/datetime';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetTelem',
  templateUrl: './timesheetTelem.component.html',
  styleUrls: ['./timesheetTelem.component.css']
})
export class TimesheetTelemComponent implements OnInit, TimeSheetComponentBase {

  @Input()
  disabled = false;
  @ViewChild(TimesheetWorkOrderCustomerComponent)
  workTelemBu: TimesheetWorkOrderCustomerComponent;
  public selectedBu: number;
  public quoteopps: number;
  public vms: number;
  public emails: number;
  public dropoffs: number;
  public calls: number;
  public hours: number;
  public rating: number;
  public meetings: number;

  public inputValue: TimesheetValue;
  public APIURLBuList: string;
  public selectedRow: TimesheetList;
  public defaultValue: DefaultValueFromScheduler;
  public minvalue:number = 0;
  public maxvalue:number;
  public currenthourValue:number;

  constructor(
    private store: Store<fromRoot.State>,
    public cs: CoreService,
    private tss: TimesheetService,
    private ts: TokenService,
  ) { }

  ngOnInit() {
  }

  OnChangeBu(event: any) {

  }

  public LoadInputvalue(value: TimesheetValue) {
    this.inputValue = value;
    this.APIURLBuList = CONFIG.apiURL.core.activeBusinessUnitDropDownList;
    this.CancelSelectedRow();
  }



  AfterLoadUrl() {
    this.selectRow();
  }
  SelectRow(item: TimesheetList, value: TimesheetValue): void {
    if (!item) { return; }
    this.selectedRow = item;
    if (this.APIURLBuList) {
      this.selectRow();
    } else {
      this.inputValue = value;
      this.APIURLBuList = CONFIG.apiURL.core.activeBusinessUnitDropDownList;
    }
  }

  public setDefaultValue() {
    CONFIG.LOG(this.defaultValue, 'defaultvalue setdefaultvalue')


    if (this.defaultValue && this.ts.currentUser.id === this.inputValue.userId) {
      CONFIG.LOG(this.defaultValue, 'defaultvalue setdefaultvalue')
      this.selectedBu = this.defaultValue.customer_Id;
     
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
      //this.hours = this.defaultValue.hours;
     // this.currenthourValue=this.defaultValue.hours;
    } else {
      this.selectedBu = null;
      this.hours = null;
      this.minvalue=null;
      this.maxvalue=null;
    }
  }
  private selectRow() {
    if (this.selectedRow) {
      this.loadValuesFromComment(this.selectedRow.comments);
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
      this.selectedBu = Number(this.selectedRow.custNo);
      this.rating = this.selectedRow.rating;
    }
  }
  loadValuesFromComment(comment: string) {
    CONFIG.LOG(comment, 'comment value loadvaluesfromcomment timesheettelem');
    // tslint:disable-next-line:max-line-length
    const results: string[] = new RegExp('CALLS:\\s+(\\d+)\\s+MEETINGS:\\s+(\\d+)\\s+DropOffs:\\s+(\\d+)\\s+EMAILS:\\s+(\\d+)\\s+VMs:\\s+(\\d+)\\s+QuoteOpps:\\s+(\\d+)')
      .exec(comment);
    CONFIG.LOG(results, 'regexp value loadvaluesfromcomment timesheettelem');
    if (results && results.length === 7) {
      this.calls = this.changeZeroToNull(Number(results[1]));
      this.meetings = this.changeZeroToNull(Number(results[2]));
      this.dropoffs = this.changeZeroToNull(Number(results[3]));
      this.emails = this.changeZeroToNull(Number(results[4]));
      this.vms = this.changeZeroToNull(Number(results[5]));
      this.quoteopps = this.changeZeroToNull(Number(results[6]));
    }
  }

  changeZeroToNull(n: number) {
    return n === 0 ? null : n;
  }

  CancelSelectedRow(): void {
    this.selectedRow = null;
    this.rating = null;
    this.selectedBu = null;
    this.hours = null;
    this.calls = null;
    this.meetings = null;
    this.dropoffs = null;
    this.emails = null;
    this.vms = null;
    this.quoteopps = null;
  }

  get comment(): string {
    // tslint:disable-next-line:max-line-length
    const c = `CALLS: ${this.calls}  MEETINGS: ${this.meetings}  DropOffs: ${this.dropoffs}  EMAILS: ${this.emails}  VMs: ${this.vms}  QuoteOpps: ${this.quoteopps}`;
    // replace null value to 0
    return c.replace('null', '0');
  }

  get OutputUpdateValue(): UpdateTimesheetTelem {

    return {
      date: DATE.ToDateOnly(this.inputValue.selectedDate),
      selectedBusinessUnitId: this.inputValue.businessUnitId,
      selectedUserId: this.inputValue.userId,

      memberTime_ID: this.selectedRow.id,
      numberOfHours: this.hours,
      rating: this.rating,
      memberTime_WoComment_ID: this.selectedRow.memberTime_WoComment_ID,
      memberTime_WoComment: this.comment,
      record: {
        bizdev_calls: this.calls,
        bizdev_faxes: this.dropoffs,
        bizdev_emails: this.emails,
        bizdev_mailers: this.vms,
        bizdev_meetings: this.meetings,
        bizdev_quoteopps: this.quoteopps,
      }
    }
  }

  UpdateRow(items: TimesheetList[],
    success: (msg: string) => void,
    failed: (msg: string) => void): void {
    this.tss.UpdateTelem(this.OutputUpdateValue)
      .subscribe((res: string) => {
        const updateItem = this.OutputUpdateValue;
        const item = items.find(x => x.id === updateItem.memberTime_ID);
        CONFIG.LOG(JSON.stringify(item), 'update shop found item');
        item.hours = updateItem.numberOfHours;
        item.rating = updateItem.rating;
        item.comments = updateItem.memberTime_WoComment;
        this.CancelSelectedRow();
        success(res);
      },
      (err: any) => {
        failed(err);
      });
  }

  get OutputInsertValue(): InsertTimeSheetTelem {

    return {
      date: DATE.ToDateOnly(this.inputValue.selectedDate),
      selectedBusinessUnitId: this.inputValue.businessUnitId,
      selectedUserId: this.inputValue.userId,
      payTypeId: 1,

      memberTime_WoComment_ID: 0,
      numberOfHours: this.hours,
      rating: this.rating ? this.rating : 0,

      percentComplete: 100,
      memberTime_WoComment: this.comment,

      customerBusinessUnitId: this.selectedBu,
      customerBusinessUnitName: this.workTelemBu.getLabelNameByValue(this.selectedBu.toString()),
      record: {
        bizdev_calls: this.calls,
        bizdev_faxes: this.dropoffs,
        bizdev_emails: this.emails,
        bizdev_mailers: this.vms,
        bizdev_meetings: this.meetings,
        bizdev_quoteopps: this.quoteopps,
      }
    }
  }

  InsertRow(items: TimesheetList[],
    success: (msg: string) => void,
    failed: (msg: string) => void): void {

    CONFIG.LOG(this.OutputInsertValue, 'insert Telem timesheet');
    this.tss.InsertTelem(this.OutputInsertValue)
      .subscribe((res: string) => {
        this.CancelSelectedRow();
        success(res);
      },
      (err: any) => {
        failed(err);
      })

  }
  ValidateValue(): boolean {
    if (!(this.selectedBu && this.selectedBu > 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select a Business unit.'));
      return false;
    }
    if (!this.hours) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input hours.'));
      return false;
    }
    /*
    if (!(Number.isInteger(this.calls) && this.calls >= 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input an interger for Call(s).'));
      return false;
    }
    if (!(Number.isInteger(this.meetings) && this.meetings >= 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input an interger for Meeting(s).'));
      return false;
    }
    if (!(Number.isInteger(this.dropoffs) && this.dropoffs >= 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input an interger for DropOff(s).'));
      return false;
    }
    if (!(Number.isInteger(this.emails) && this.emails >= 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input an interger for Email(s).'));
      return false;
    }
    if (!(Number.isInteger(this.vms) && this.vms >= 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input an interger for VM(s).'));
      return false;
    }
    if (!(Number.isInteger(this.quoteopps) && this.quoteopps >= 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input an interger for QuoteOpp(s).'));
      return false;
    }*/

    return true;

  }
  ClearDropDownList(): void {
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
