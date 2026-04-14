import { Calendar, TreeNode, ConfirmationService, TabView } from 'primeng/primeng';
import { Component, OnInit, ViewChild,EventEmitter,Output } from '@angular/core';
import { TimesheetList } from '../../../models/pages/timesheet/timesheet-list';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { TokenService } from '../../../services/authentication/tokenService';
import { WindowRef } from '../../../services/shared/windowRef';
import { CONFIG } from '../../../configuration';
import { TimeSheetUser } from '../../../models/pages/timesheet/timesheetUser';
import { TimeSheetProfile } from '../../../models/pages/timesheet/timesheetProfile';
import { TimesheetValue } from '../../../models/pages/timesheet/timesheetValue';
import { TimesheetWorkOrderComponent } from '../timesheetWorkOrder/timesheetWorkOrder.component';
import { TimesheetWorkOrderWo } from '../../../models/pages/timesheet/timesheetWorkOrderWo';
import { UpdateTimesheetWorkOrder } from '../../../models/pages/timesheet/updateTimesheetWorkOrder';
import * as MessageHelper from '../../../services/helper/MessageHelper';
import { TimesheetShopComponent } from '../timesheetShop/timesheetShop.component';
import { TimesheetTelemComponent } from '../timesheetTelem/timesheetTelem.component';
import { TimesheetQuoteComponent } from '../timesheetQuote/timesheetQuote.component';
import { TimeSheetComponentBase } from '../interface/timesheetComponentBase';
import * as DATE from '../../../services/helper/datetime';
import { Profile } from '../../../models/layout/profile';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import * as fromMessage from '../../../actions/layout/growlMessage';
import { TimeSheetVisibleBusinessUnitDropDownComponent } from '../visibleBusinessUnitDropDown/visibleBusinessUnitDropDown.component';
import { TimesheetUserDropDownComponent } from '../timesheetUserDropDown/timesheetUserDropDown.component';
import { DefaultValueFromScheduler } from '../../../models/pages/timesheet/defaultValueFromScheduler';
import { DirectoryInfo } from '../../../models/component/fileManager/directory';
import { TimesheetPageBase } from '../interface/timesheetPageBase';
import { element } from 'protractor';
import { CoreService } from 'app/services/shared/core.service';
import { BreaktimeService } from '../../../services/pages/breaktime.service';
import { PostResult } from '../../../models/core/postResult';
import {   
  BranchBreakTimeRequirementInputParameter,
  BreakTimeQueryParameter,
  BreakTimeRecord,
  BreakTimeRecordRequirementRecord,
  BreakTimeRecordResult,
  OperationType,
  BreakTimeRecordOnGivenDate } from 'models/pages/timesheet/breaktime';
  
import { forkJoin } from 'rxjs';
import { Timesheet_Shop_ProjectsComponent } from 'pages/timesheet/timesheet_Shop_Projects/timesheet_Shop_Projects.component';

import { JobTypeService } from '../../../services/pages/job-type.service';
import { JobTypeInfo, JobTypeRecordQueryParameter } from 'models/pages/timesheet/jobType';

import {
  TimesheetTransfer
} from 'models/pages/timesheet/transfer';

@Component({
  selector: 'nesi-timesheetlist',
  templateUrl: './timesheet-list.component.html',
  styleUrls: ['./timesheet-list.component.css']
})

export class TimesheetListComponent extends TimesheetPageBase implements OnInit {

  timesheetsignoffDisplay=false;
  @ViewChild(Calendar)
  calendar: Calendar;

  @ViewChild(TimesheetWorkOrderComponent)
  workOrder: TimesheetWorkOrderComponent;
  @ViewChild(TimesheetShopComponent)
  shop: TimesheetShopComponent;
  @ViewChild(TimesheetTelemComponent)
  telem: TimesheetTelemComponent;
  @ViewChild(TimesheetQuoteComponent)
  quote: TimesheetQuoteComponent;
  @ViewChild(Timesheet_Shop_ProjectsComponent)
  shopProject: Timesheet_Shop_ProjectsComponent;

  public items: TimesheetList[];
  public totalRecords: number;
  public totalPages: number;
  public selectBusinessUnitId: number;
  public selectedUserid: number;
  public IsSignoff=false;
  public selectedDate: any;
  public minDate: Date;
  public maxDate: Date;
  public pastDays: string[];
  public UserOptions: any[];

  public Profile: TimeSheetProfile;
  public periodStartDate: Date;
  public periodEndDate: Date;
  public IsselectedDate=false;

  // is a row of timesheet table selected?
  public timesheetRowSelected = false;
  public timesheetSelectedRow: TimesheetList;
  public activeTableView = 0;

  public showShop = true;
  public show_daily_approval:boolean;

  public disabledSaveButton = false;
  public disabledCancelButton = false;
  public disabledProjectButton = true;
  public displayLabouMasterError = false;
  public selectedWorkOrderId = 0;
  public selectedCustomer:number;
  public showProjectFilesTab = false;
  public ProjectFileDirectory: TreeNode[];
  public loadingTable = false;
  public saving = false;
  public editPermission = true;
  public allow_unlinked_timesheet = false;
  public showTotalHours = true;
  public showTotalHours1 = true;
  public showTotalHours2 = true;
  public hourLabels = ['RT', 'OT', 'DT', 'RTsp', 'OTsp', 'DTsp'];
  hoursInit = {
    total: 0,
    total1: 0,
    total2: 0,
    ototal: 0,
    ototal1: 0,
    ototal2: 0,
    dt_total: 0,
    dt_total1: 0,
    dt_total2: 0,
    rtsp_total: 0,
    rtsp_total1: 0,
    rtsp_total2: 0,
    otsp_total: 0,
    otsp_total1: 0,
    otsp_total2: 0,
    dtsp_total: 0,
    dtsp_total1: 0,
    dtsp_total2: 0
  };
  public hours = Object.assign({}, this.hoursInit);

  children: TimeSheetComponentBase[];
  totalHours = 0;
  totalHoursRT = 0;
  totalHoursOT = 0;
  totalHoursDT = 0;
  totalHoursRTsp = 0;
  totalHoursOTsp = 0;
  totalHoursDTsp = 0;

  ////////////////
  // The selected branch requires break time records. 
  public branchRequireBreakTimeRecord: boolean = false;

  // Below are meaningful only when branchRequireBreakTimeRecord is true;
  // true:  Need to submit the break time record on selected date.
  public showBreakTimeOverlaySection: boolean = false;

  // true: users can edit the existing record on given date.
  public showExistingBreakTimeRecord: boolean = false;

  public branchSetting: BreakTimeRecordRequirementRecord;
  public settingOnGivenDate: BreakTimeRecordOnGivenDate;
  public currentBreaktimeRecord: BreakTimeRecord;
  public showup: { val: boolean, record: any } = {val: false, record: null};
  ////////////////

  ////////////////
  // This is the JobTypeInfo for current selected users
  public JobTypeInfo: JobTypeInfo = new JobTypeInfo();
  ////////////////
  @Output()
  change = new EventEmitter();

  constructor(
    private winRef: WindowRef,
    protected tss: TimesheetService,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    private cfs: ConfirmationService,
    public cs: CoreService,
    public bts: BreaktimeService,
    public jt: JobTypeService
  ) {
    super(tss, store);
  }

  getDateStyleClass(date: any): string {
    const d = new Date(date.year, date.month, date.day);

    const dstring = DATE.ToyyyyMMdd(d);

    let classname = '';
    if (DATE.IsWeekend(d)) {
      classname = 'weekend';
    }

    if (this.pastDays && this.pastDays.indexOf(dstring) > -1) {
      classname += ' workday';
    }

    if (d >= this.periodStartDate && d <= this.periodEndDate) {
      classname += ' periodday';
    }
    return classname;
  }

  get ActiveChild(): TimeSheetComponentBase {
    return this.children[this.activeTableView];
  }

  public get inputValue(): TimesheetValue {
    return {
      businessUnitId: this.selectBusinessUnitId,
      userId: this.selectedUserid ? this.selectedUserid : this.ts.currentUser.id,
      selectedDate: this.selectedDate,
      allowUnlinkedTimesheet: this.Profile.allowUnlinkedTimesheet,
      allow_jobtype_selection: false //set a default value, may not be correct.
    };
  }

  afterProfileLoad(): void {
    //  CONFIG.LOG(JSON.stringify(this.Profile), 'timesheet list init');
    //  CONFIG.LOG(new Date(this.Profile.minDate).getFullYear(), 'timesheet list init');
    const fMinDate = this.Profile.minDate;
    this.minDate = new Date(fMinDate.replace(/-/g, '/'));
    this.maxDate = new Date();
    this.pastDays = this.Profile.pastDays;
    this.periodStartDate = new Date(this.Profile.payPeriod.startDate);
    this.periodEndDate = new Date(this.Profile.payPeriod.endDate);
    this.showShop = this.Profile.canDoShopTime;
    this.children = [this.workOrder, this.quote, this.telem, this.shop];
    this.allow_unlinked_timesheet = this.Profile.allowUnlinkedTimesheet;
    if (this.allow_unlinked_timesheet) {
      if (this.tabLabels) {
        this.tabLabels[0] = 'JOB';
        if (this.activeTableView === 0) {
          this.tabName = this.tabLabels[0];
        }
      }
      this.minDate = null;
    }
    this.reloadUser();
    this.loadTable();
    this.checkLabourMaster();
    this.getValueFromScheduler();
  }

  ngOnInit() {
    super.Init();
    this.tabLabels = ['Work Order', 'Esimating', 'Business Development', 'Shop Time'];
    this.tabChanged({ index: 0 });

  }
  onTimesheetsignoffClick()
  {
     this.timesheetsignoffDisplay=true;
  }
  getValueFromScheduler() {
    if (this.selectedUserid && this.selectedDate) {
      this.tss.GetValueFromScheduler(this.selectedUserid, this.selectedDate)
        .subscribe(
          (res: DefaultValueFromScheduler) => {
            this.setDefaultValue(res);
          },
          (err:any)=>{
            this.PushErrorMessage(err);
          }
        );
    }
  }

  setDefaultValue(value: DefaultValueFromScheduler) {
    // if (!value) {
    //   return;
    // }
    // CONFIG.LOG(value, 'defaultvalue in workorder when profile loaded');
    if (value) {
      const tv = this.children[value.entryType]
      tv.defaultValue = value;
      tv.defaultValue.callFromScheduler = true;
      tv.defaultValue.callFromSchedulerForJobType = true;
      tv.setDefaultValue();
      tv.defaultValue.callFromSchedulerForJobType = false;
    }
  }


  checkLabourMaster() {
    this.workOrder.CheckLabourMaster(
      () => this.hideLabourMasterError(),
      () => this.showLabourMasterError()
    );
  }

  hideLabourMasterError() {
    this.disabledSaveButton = false;
  }

  afterUserLoad() {
    if (this.selectBusinessUnitId === this.ts.currentUser.businessUnitId) {
      this.selectedUserid = this.ts.currentUser.id;
      this.pastDays = this.Profile.pastDays;
      this.checkLabourMaster();
      this.loadPermission();
    } else {
      this.selectedUserid = null;
      this.editPermission = true;
    }
  }

  showLabourMasterError() {
    // this.disabledSaveButton = true;
    this.store.dispatch(
      new fromMessage.PushWarnMessage(
        `Your member type is not set up with a charge out rate in the selected business unit. <br/>
    Until this is corrected, you will not be able to enter time on work orders in this business unit.`
      ));
  }

  // reload user when init component and cancel edit.
  reloadUser() {
    this.selectedDate = DATE.Today();
    this.selectBusinessUnitId = this.ts.currentUser.businessUnitId;
    this.selectedUserid = this.ts.currentUser.id;
    this.show_daily_approval=this.ts.currentUser.show_daily_approval;
    CONFIG.LOG(this.show_daily_approval, 'show_daily_approval');
    this.loadBreakTimeRecordInfor(this.selectedUserid, this.selectBusinessUnitId, this.selectedDate);
    this.loadUser();
    this.workOrder.LoadInputvalue(this.inputValue);
   this.getJobTypeSetting(this.selectedUserid, this.selectBusinessUnitId,this.selectedCustomer,this.jt);
  }


  // bu id changed event
  businessUnitChanged(event: any) {
    CONFIG.LOG('business unit change', 'timesheet list');
    this.pastDays = [];

    if (this.selectBusinessUnitId === this.ts.currentUser.businessUnitId) {
      this.selectedUserid = this.ts.currentUser.id;
      this.loadBreakTimeRecordInfor(this.ts.currentUser.id, this.selectBusinessUnitId, this.selectedDate);
    } else {
      this.selectedUserid = 0;
      this.loadBreakTimeRecordInfor(0, this.selectBusinessUnitId, this.selectedDate);
    }

    //
    // Load the hours type when switching the branch to another one.
    //
    if(this.workOrder && this.workOrder.workOrderHourType && this.inputValue && this.inputValue.businessUnitId > 0) {
      //
      // The regular way is to pass the new list to the 'workOrderHourType' component.
      // But here is kind of wired of doing so by recaculating the url which will trigger the reload the options for this control.
      //
      this.workOrder.workOrderHourType.URL = CONFIG.apiURL.core.paytypeHoursList + this.inputValue.businessUnitId;

      //
      // Fixed the re-enter problem from non-chargeout branch to a chargeout branch. (for example: Tiltrn Power Service -> Oakville)
      // So the hour type control will be selectable for users.
      //
      this.workOrder.workOrderHourType.disabled = false;
    }

    this.loadUser();
    this.loadTabview();
    this.loadTable();

   this.getJobTypeSetting(this.selectedUserid, this.selectBusinessUnitId,this.selectedCustomer, this.jt);

     this.resetSelectedWorkOrderIdWhenSwitching();
  }

  onWorkOrderSelectedChange(event: any) {
    CONFIG.LOG(event, 'onWorkOrderSelectedChange');
    this.selectedWorkOrderId = event.selectedWorkOrderId;
    this.tss.WorkOrderWoById(event.selectedWorkOrderId)
    .subscribe(
      (res: TimesheetWorkOrderWo) => {
        if (res) {
         if(res.status=='Open')
         {
          this.IsSignoff=false;
         }
         else{
          this.IsSignoff=true;
         }
        }
      });
    if (this.selectedWorkOrderId) {
      this.tss.TimesheetListByUserIdDate(this.inputValue.userId, this.inputValue.selectedDate)
      .subscribe(
        (res: TimesheetList[]) => {
              var workorderid= this.pad(this.selectedWorkOrderId,10);
              if(res.filter(x => x.workorderId === workorderid).length>0)
              {
                    this.IsselectedDate=true;
              }
              else
              {
                    this.IsselectedDate=false;        
              }
        });

      this.disabledProjectButton = !(this.selectedWorkOrderId && this.selectedWorkOrderId > 0);
      this.tss.getList<TreeNode>(CONFIG.apiURL.core.fileManager.workOrder + this.selectedWorkOrderId.toString())
        .subscribe(
          (res: TreeNode[]) => {
            this.ProjectFileDirectory = res;
          },
          (err:any)=>{
            this.PushErrorMessage(err);
          }
        );
    }
  }
  pad(num:number, size:number): string {
    let s = num+"";
    while (s.length < size) s = "0" + s;
    return s;
}
  // invoke webapi to load user with buid
  private loadUser() {

    if (this.Profile.canSwitchBusinessUnits) {
      this.tss.TimeSheetUserListByBusinessUnitId(this.selectBusinessUnitId)
        .subscribe((res: TimeSheetUser[]) => {
          this.UserOptions = this.converToUserListDropDown(res);
          this.afterUserLoad();
          this.items = [];
        },
        (err:any)=>{
          this.PushErrorMessage(err);
        });
    }
  }

  // convert user data to dropdown list array
  private converToUserListDropDown(userList: TimeSheetUser[]): any[] {
    const options = [];
    userList.forEach(x => {
      options.push({ label: x.fullName, value: x.id });
    });
    return options;
  }

  // user dropdownlist changed event
  userChanged(event: any) {
    this.loadBreakTimeRecordInfor(this.selectedUserid, this.selectBusinessUnitId, this.selectedDate);
    this.getJobTypeSetting(this.selectedUserid, this.selectBusinessUnitId,this.selectedCustomer, this.jt);
    this.loadPermission();
    this.loadTable();
    this.loadTabview();
    this.loadCalender();
    //this.loadStartDate();
    this.checkLabourMaster();
    this.getValueFromScheduler();
    this.resetSelectedWorkOrderIdWhenSwitching();
  }

  loadCalender() {
    if (this.selectedUserid && this.selectedUserid > 0) {
      this.tss.GetPastDays(this.selectedUserid)
        .subscribe(
          (res: string[]) => {
            CONFIG.LOG(res, 'loadcalender in timesheet');
            this.pastDays = res;
            // if( this.pastDays.length>0 )
            // {
            //     this.minDate=new Date(this.pastDays[0]);
            // }
            if (this.selectedUserid === this.tss.Profile.userId) {
              this.tss.Profile.pastDays = this.pastDays;
            }
            // this.calendar.showOverlay();
            // this.calendar.
          },
          (err:any)=>{
            this.PushErrorMessage(err);
          }
        );
    }
  }
  // load timesheet table with use selected date and buid, userid
  loadTable() {
    this.items = [];
    this.loadingTable = true;
    if (this.Profile.canSwitchBusinessUnits) {
      if (this.selectedDate && this.selectedUserid && this.selectedUserid > 0) {
        this.getTable(this.selectedUserid, this.selectedDate);
      } else {
        this.loadingTable = false;
      }
    } else {
      this.getTable(this.ts.currentUser.id, this.selectedDate);
    }
  }

  // Get Timesheet table by userid and selected Date
  getTable(userId: number, selectedDate: Date) {
    this.tss.TimesheetListByUserIdDate(userId, selectedDate)
      .subscribe(
        (res: TimesheetList[]) => {
          this.items = res;
          this.loadingTable = false;
          this.totalHours = 0;
          this.totalHoursRT = 0;
          this.totalHoursOT = 0;
          this.totalHoursDT = 0;
          this.totalHoursDTsp = 0;
          this.totalHoursOTsp = 0;
          this.totalHoursRTsp = 0;

          this.items.forEach(e => {
            this.totalHours += e.hours;
            if (e.hourTypeId === 1) {
              this.totalHoursRT += e.hours;
            }
            if (e.hourTypeId === 2) {
              this.totalHoursOT += e.hours;
            }
            if (e.hourTypeId === 3) {
              this.totalHoursDT += e.hours;
            }
            if (e.hourTypeId === 4) {
              this.totalHoursRTsp += e.hours;
            }
            if (e.hourTypeId === 5) {
              this.totalHoursOTsp += e.hours;
            }
            if (e.hourTypeId === 6) {
              this.totalHoursDTsp += e.hours;
            }
           
          });
        },
        (err: any) => {
          this.PushErrorMessage(err);
          this.loadingTable = false;
        });
    this.getPayperiodHour(userId);
  }


  getPayperiodHour(userId: number) {
    if (this.selectedDate >= this.periodStartDate) {
      this.tss.getObject(CONFIG.apiURL.page.timesheet.payperiodTotalHours + userId.toString())
        .subscribe(
          (res: any) => {
            if (res) {
              this.hours = res;

              if (this.hours.total > 0) {
                this.showTotalHours = true;
              } else {
                this.showTotalHours = false;
              }
              if (this.hours.total1 > 0) {
                this.showTotalHours1 = true;
              } else {
                this.showTotalHours1 = false;
              }
              if (this.hours.total2 > 0) {
                this.showTotalHours2 = true;
              } else {
                this.showTotalHours2 = false;
              }

            } else {
              this.showTotalHours = false;
              this.hours = Object.assign({}, this.hoursInit);
            }
          },
          (err:any)=>{
            this.PushErrorMessage(err);
          }
        );
    } else {
      this.showTotalHours = false;
      this.hours = Object.assign({}, this.hoursInit);
    }
  }

  loadPermission() {
    if (!this.selectedUserid) { return; }
    this.tss.getBoolean(CONFIG.apiURL.page.timesheet.editPermission
      + this.selectedUserid.toString() + '/' + this.selectedDate.toDateString())
      .subscribe(
        (res: boolean) => {
          this.editPermission = res;
        },
          (err:any)=>{
            this.PushErrorMessage(err);
          }
      );
  }

  // loadStartDate(){
  //   if (!this.selectedUserid) { return; }
  //   if (this.selectedUserid && this.selectedUserid > 0) {
  //     this.tss.GetStartDate(this.selectedUserid)
  //       .subscribe(
  //         (res: string[]) => {
  //           CONFIG.LOG(res, 'loadStartDate in timesheet');
  //           if(res.length>0)
  //           this.minDate = new Date(res[0]);
  //         },
  //         (err:any)=>{
  //           this.PushErrorMessage(err);
  //         }
  //       );
  //   }
    
  // }

    dateChanged(event: any) {
    // super.PushInfoMessage('get date ' + this.selectedDate.getDate());
    this.loadBreakTimeRecordInfor(this.selectedUserid, this.selectBusinessUnitId, this.selectedDate);
    this.loadPermission();
    this.loadTable();
    this.loadTabview();
    this.getValueFromScheduler();
    this.resetSelectedWorkOrderIdWhenSwitching();
  }

  // click edit button in timesheet table
  selectTimeSheet(item: TimesheetList) {
    if (this.timesheetRowSelected) {
      this.ActiveChild.CancelSelectedRow();
    } else {
      //
      // 1883: Edit Time Record in Open period - Comments are disabled.
      // See the attached videos to see more. https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1883/
      //
      this.ActiveChild.CancelSelectedRow(); // By default the active child is wo workorder.
    }
    CONFIG.LOG(JSON.stringify(item), 'timesheet list select item button');
    // this.selectBusinessUnitId = item.businessUnitId;
    // this.selectedUserid = item.memberId;
    this.selectedDate = DATE.ToDateOnlyS(item.date);
    //  super.PushInfoMessage(this.selectedDate);
    this.timesheetRowSelected = true;
    this.timesheetSelectedRow = item;
    this.activeTableView = item.woTypeId;
    this.ActiveChild.SelectRow(item, this.inputValue);

  }

  private resetSelectedWorkOrderIdWhenSwitching() {
    if(this.workOrder) {
      this.workOrder.selectedWorkorderIdFromWON = 0;
    }
  }

  // click delete button in timesheet table
  deleteTimeSheet(item: TimesheetList) {

    this.cfs.confirm({
      message: `Are you sure you want to delete this record?`,
      accept: () => {
        // CONFIG.LOG(JSON.stringify(item), 'timesheet list delete item button');
        this.loadingTable = true;
        this.tss.deleteString(CONFIG.apiURL.page.timesheet.delete + this.selectedUserid + '/' + item.id)
          .subscribe(
            (res: string) => {
              if (super.PushResponseMessage(res)) {
                this.loadTable();
                this.loadCalender();
              } else {
                this.loadingTable = false;
              }
            },
            (err: any) => {
              this.PushErrorMessage(err);
              this.loadingTable = false;
            }
          );
      }
    });
  }
  onProjectFileClick() {
    this.showProjectFilesTab = !this.showProjectFilesTab;
  }
  onSaveClick() {
    if (!(this.selectedUserid && this.selectedUserid > 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select a member.'));
      return;
    }
    if (!this.ActiveChild.ValidateValue()) { return };
    // if selectrow = true, then save, else add new

    this.saving = true;
    if (this.timesheetSelectedRow) {
      this.ActiveChild.UpdateRow(this.items,
        (msg: string) => {
          this.loadTable();
          this.loadCalender();
          this.clearSelected();
          MessageHelper.pushResponseMessage(this.store, msg);
        },
        (err: string) => {
          this.saving = false;
          this.loadTable();
          this.loadCalender();
          MessageHelper.pushResponseMessage(this.store, err);
        });
    } else {
      this.ActiveChild.InsertRow(this.items,
        (msg: string) => {
          this.clearSelected();
          this.loadTable();
          this.loadCalender();
          MessageHelper.pushResponseMessage(this.store, msg);
        },
        (err: string) => {
          this.saving = false;
          this.loadTable();
          this.loadCalender();
          MessageHelper.pushResponseMessage(this.store, err);
        });
    }

  }

  getWorkOrderData(data: TimesheetList): string {
    // CONFIG.LOG(data, 'getworkorderdata');
    if (!data || !data.workorderId) {
      return '0';
    }
    if (data.childWo && data.childWo.length > 0) {
      return `
          (CUST WO) ${data.workorderId} (INTERCO WO) ${data.childWo}
      `
    } else {
      return data.workorderId.toString();
    }
  }

  clearSelected() {
    this.saving = false;
    this.timesheetRowSelected = false;
    this.timesheetSelectedRow = null;
    this.disabledProjectButton = true;
    this.selectedWorkOrderId = 0;
    this.disabledSaveButton = false;
    window.scroll(0, 0);
  }


  // validate before save.

  onCancelClick() {
    // clear slected flag

    this.showProjectFilesTab = false;
    this.ActiveChild.CancelSelectedRow();
    this.clearSelected();
    // set to default tabview
    // this.WorkOrderTypeIndex = 0;
    // reload user customer name and work order name
    // this.reloadUser();
    // scroll window to top
  }

  onClickBank() {

  }

  // lazay load componet
  tabviewOnChange(event: any) {
    super.ClearMessage();
    super.tabChanged(event);
    this.activeTableView = event.index;
    this.loadTabview();
  }

  loadTabview() {
    CONFIG.LOG(this.activeTableView, 'timesheet acitve tableview');
    if (this.children && this.children[this.activeTableView]) {
      this.children[this.activeTableView].LoadInputvalue(this.inputValue);
    }
  }

  // BreakTime Record
  loadBreakTimeRecordInfor(member_id: number, business_id: number, date: Date) {
    CONFIG.LOG(`Input info ${member_id} on branch ${business_id} on date ${date}.`);

    if(business_id === 0) {
      return;
    }

    if(!date) {
      return;
    }

    if(member_id === 0) {
      // When switching from one branch to another branch, the member is not selected.
      this.showBreakTimeOverlaySection = false;
      this.showExistingBreakTimeRecord = false;
      return;
    }

    const input1 = new BranchBreakTimeRequirementInputParameter();
    input1.business_unit_id = business_id;
    input1.member_id = member_id;
    const breaktimeConfigOnBranch$ = this.bts.GetBranchBreakTimeRequirement(input1);

    const input2 = new BreakTimeQueryParameter();
    input2.business_unit_id = business_id;
    input2.member_id = member_id;
    input2.date = date;
    const breaktimeConfigOnSelectedDate$ = this.bts.GetBreakTimeRequirementOnSpecificDate(input2);

    forkJoin(breaktimeConfigOnBranch$, breaktimeConfigOnSelectedDate$).subscribe(
      (res: [BreakTimeRecordRequirementRecord, BreakTimeRecordOnGivenDate] )=> {
        this.branchRequireBreakTimeRecord = res[0].monitor_breaktime; 
        this.showBreakTimeOverlaySection = res[0].monitor_breaktime && res[1].showAddingOverlay;
        this.showExistingBreakTimeRecord = res[0].monitor_breaktime && res[1].showEditingButton;
        this.branchSetting = res[0];
        this.settingOnGivenDate = res[1];
        this.currentBreaktimeRecord =  res[1].breaktimeRecord;
      },
      err => {
        this.PushErrorMessage(err);
        CONFIG.LOG(err);
      }
    );
  }

  addBreaktimeRecord(input: BreakTimeRecord) {
    this.bts.AddBreakTimeRecord(input).subscribe(
      (res: BreakTimeRecordResult) => {
        if(res.success) {
          this.showBreakTimeOverlaySection = false;
          this.showExistingBreakTimeRecord = true;
          this.settingOnGivenDate.breaktimeRecord = input;
          this.settingOnGivenDate.breaktimeRecord.id = res.id;
          this.currentBreaktimeRecord = this.settingOnGivenDate.breaktimeRecord;
          CONFIG.LOG(this.settingOnGivenDate);
          this.store.dispatch(new fromMessage.PushSuccessMessage(res.reason));
        } else {
          // Error on inputs.
          this.store.dispatch(new fromMessage.PushWarnMessage(res.reason));
        }
      },
      err => {
        this.PushErrorMessage(err);
        CONFIG.LOG(err);
      }
    );
  }

  updateBreaktimeRecord(input: BreakTimeRecord) {
    this.bts.UpdateBreakTimeRecord(input).subscribe(
      (res: BreakTimeRecordResult) => {
        if(res.success) {
          this.settingOnGivenDate.breaktimeRecord = res;
          this.currentBreaktimeRecord = res;
          this.store.dispatch(new fromMessage.PushSuccessMessage(res.reason));
        } else {
          this.store.dispatch(new fromMessage.PushWarnMessage(res.reason));
        }

        // make the popup disappear.
        this.showup = {val: false, record: this.currentBreaktimeRecord}
      },
      err => {
        this.PushErrorMessage(err);
        CONFIG.LOG(err);
      }
    );
  }

  showBreakTimeDialog() {
    this.showup = {val: true, record: this.currentBreaktimeRecord}
  }

  //
  // Below is the area for the code of JobType
  //
  private getJobTypeSetting(memberid: number, businessUnitID: number,customer_id:number, jobService: JobTypeService ) {
    let parameter: JobTypeRecordQueryParameter = new JobTypeRecordQueryParameter();
    parameter.business_uint_id = businessUnitID;
    parameter.member_id = memberid;
    parameter.customer_id=customer_id>0? customer_id:0;
    jobService.GetJobTypeInfo(parameter).subscribe(
       result => {
         this.JobTypeInfo = result;
        },
       error => { 
        console.log(error);
        }
    );
  }

  //
  // End of JobType
  //

  public SignoffCancel() {
    this.timesheetsignoffDisplay = false;
  }

  public SignoffSubmit(event: PostResult)
  {
    super.LOG(event, 'SignoffSubmit');
    this.selectedCustomer=this.workOrder.selectedCustomer;
    super.LOG( this.selectedCustomer, ' this.selectedCustomer');
    if(event.post)
    {
     // 
     this.workOrder.selectedCustomer=this.selectedCustomer;
     this.workOrder.selectedWorkOrder=this.selectedWorkOrderId;
     this.workOrder.workOrderreLoad(this.selectedWorkOrderId,this.selectedCustomer);
     this.workOrder.noReloadpage=true;
     //this.timesheetsignoffDisplay = false;
    }  
  }

  //
  // Timesheet records Transfer part in here.
  //
  public transferUIShowup: {v: boolean } = {v: false};
  public toBeTransferedRecords : Array<TimesheetList> = [];
  public timesheetValue: TimesheetValue = null;
  
  pickupTimeSheetRecord(item: TimesheetList) {
    //
    // Get all timesheet records.  Currently it supports 1, but have it is better to extend to support multilple records.
    //
    const selectToTransferRecords : Array<TimesheetList> = [];
    selectToTransferRecords.push(item);

    this.toBeTransferedRecords = selectToTransferRecords;

    // The input value that passed to this component.
    this.timesheetValue = <TimesheetValue>{};
    this.timesheetValue = this.inputValue;

    // Trigger the UI
    this.transferTimesheetRecords(selectToTransferRecords);
  }

  private transferTimesheetRecords(list: Array<TimesheetList>) {
    this.transferUIShowup = {v: true};
  }

  // Main entry function.
  transferTimesheetEvent(parameter: TimesheetTransfer) {
    this.transferUIShowup = {v: false};
    this.transferTimesheet_ListUpdate();
  }

  private transferTimesheet_ListUpdate() {
    // Flow the exactly same logic from: onSaveClick.
    this.clearSelected();
    this.loadTable();
    this.loadCalender();
  }

  transferTimesheetExit(parameter: any) {
    this.transferUIShowup = {v: false};
  }

  //
  // End of timesheet transfer part.
  //

}