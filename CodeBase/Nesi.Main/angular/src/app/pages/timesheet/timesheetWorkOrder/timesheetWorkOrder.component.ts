import { Component, OnInit, Input, ViewChild, EventEmitter, Output } from '@angular/core';
import { CoreService } from '../../../services/shared/core.service';
import { PayTypeHour } from '../../../models/Shared/paytypeHour';
import { SelectItem } from 'primeng/primeng';
import { CONFIG } from '../../../configuration';
import { TimesheetValue } from '../../../models/pages/timesheet/timesheetValue';
import { TimesheetWorkOrderCustomer } from '../../../models/pages/timesheet/timesheetWorkOrderCustomer';
import { TimesheetWorkOrderCustomerComponent } from '../timesheetWorkOrderCustomer/timesheetWorkOrderCustomer.component';
import { TimesheetWorkOrderWoComponent } from '../timesheetWorkOrderWo/timesheetWorkOrderWo.component';
import { TimesheetWorkOrderCommentComponent } from '../timesheetWorkOrderComment/timesheetWorkOrderComment.component';
import { TimesheetList } from '../../../models/pages/timesheet/timesheet-list';
import { TimesheetWorkOrderHourTypeComponent } from '../timesheetWorkOrderHourType/timesheetWorkOrderHourType.component';
import { Store } from '@ngrx/store';
import * as fromMessage from '../../../actions/layout/growlMessage';
import * as fromRoot from '../../../reducers';
import { UpdateTimesheetWorkOrder } from '../../../models/pages/timesheet/updateTimesheetWorkOrder';
import { TimeSheetComponentBase } from '../interface/timesheetComponentBase';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { TimesheetWorkOrderWo } from '../../../models/pages/timesheet/timesheetWorkOrderWo';
import { InsertTimeSheetWorkOrder } from '../../../models/pages/timesheet/InsertTimesheetWorkorder';
import { DefaultValueFromScheduler } from '../../../models/pages/timesheet/defaultValueFromScheduler';
import { TokenService } from '../../../services/authentication/tokenService';
import * as DATE from '../../../services/helper/datetime';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { WindowRef } from 'app/services/shared/windowRef';
import { TimesheetHourTypeRecord, TimesheetTransactionRecord, BatchInsertionResult, ParseResult } from '../../../models/pages/timesheet/timesheetHourType';
import {ToggleButtonModule} from 'primeng/togglebutton';
import DataSource from 'devextreme/data/data_source';
import ArrayStore from 'devextreme/data/array_store';
import {InputSwitchModule} from 'primeng/inputswitch';
//
// Use a dumb way to pass the data in. This is the best pratice.
//
import { JobTypeInfo, JobTypeRecord,JobTypeRecordQueryParameter } from 'models/pages/timesheet/jobType';
import { DxSelectBoxComponent } from "devextreme-angular";
import {Scope, Task} from 'models/pages/timesheet/scopeTasks';
import { TimeSheetVisibleBusinessUnitDropDownComponent } from '../visibleBusinessUnitDropDown/visibleBusinessUnitDropDown.component';
import { LabelValueString } from 'models/Shared/labelValueInt';
import { Province } from 'models/pages/timesheet/provinces';
import { JobTypeService } from '../../../services/pages/job-type.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetWorkOrder',
  templateUrl: './timesheetWorkOrder.component.html',
  styleUrls: ['./timesheetWorkOrder.component.css']
})
export class TimesheetWorkOrderComponent implements OnInit, TimeSheetComponentBase {

  @ViewChild(TimesheetWorkOrderCustomerComponent)
  workOrderCustomer: TimesheetWorkOrderCustomerComponent;

  @ViewChild(TimesheetWorkOrderWoComponent)
  workOrderWo: TimesheetWorkOrderWoComponent;
  @ViewChild(TimesheetWorkOrderCommentComponent)
  workOrderComment: TimesheetWorkOrderCommentComponent;
  @ViewChild(TimesheetWorkOrderHourTypeComponent)
  workOrderHourType: TimesheetWorkOrderHourTypeComponent;
  selectedWorkOrderLabel: string;

  @Input()
  disabled = false;
  @Input()
  customerWoEditable: boolean;

  @Output()
  OnWorkOrderSelectedChange = new EventEmitter();
  selectedWorkorderIdFromWON = 0;

  public inputValue: TimesheetValue;

  public APIURLCustomerList: string;
  public APIURLWorkOrderWosList: string;

  public selectedCustomer: any;

  public comment: string;
  public selectedWorkOrderComment: number;
  public selectedHourType =1;
  public WorkOrders: any[];
  public selectedWorkOrder: any;
  public selectedWO:number;
  public selectedScope: any;
  public IsHour=true;
  public isMileageOrTavel= false;
  public mileageChk = false;
  public showMileage = false;
  public travelChk = false;
  public showTravel = false;
  public isPrevailingWage = false;
  public isPrevailingWageOldState: boolean = undefined;
  public prevailingWageVisible = false;
  public prevailingWageVisibleOldState: boolean = undefined;
  public rating: number;

  public percentComplete: number;
  public hours: number;
  public paytypeHourAPIURL;
  public WorkOrderCommentsDisabled = false;
  public disableMileage= false;
  public disableTravel = false;
  // store selected row data, used for update value
  public selectedRow: TimesheetList;
  public customerWoFilterable = true;
  public hourTypeFilter = false;
  public defaultValue: DefaultValueFromScheduler;
  public tsLiteTypes: LabelValueInt[];
  public selectedTsLiteType: number;
  public minvalue:number;
  public maxvalue:number;
  public currenthourValue:number;
  public currentdistanceValue:number;
  public noReloadpage=false;
  
  provinces: DataSource;
  public provinceArray: Province[];
  taskList: Array<Task>;
  checked: boolean=false;
  Unit:any="Miles";
  public minDistance:number=0.00;
  public maxDistance:number=99999.99;
  public distance=0;
  public newhourtype:any[];
  
  public defaultProvinceBasedonBusinessUnit = '';

  public noshowMultiple = false;

  //
  // Below for job types
  //
  public jobTypesBackupList: Array<JobTypeRecord>;
  public JobTypeInfo: JobTypeInfo = new JobTypeInfo();

  @Input()
  public jobTypeInfo: JobTypeInfo;
 

  @Input()
  set jobTypeInfoBackup(jti: JobTypeInfo){
    this.jobTypesBackupList = new Array<JobTypeRecord>();

    if(!jti || !jti.showJobType) {
      return;
    }

    if(!jti || !jti.jobTypes || jti.jobTypes.length == 0) {
      return;
    }

    jti.jobTypes.forEach(element => {
      let record = new JobTypeRecord();
      record.membertype_id = element.membertype_id;
      record.membertype_name = element.membertype_name;
      record.paytypeAllowed=(element.paytypeAllowed != undefined && element.paytypeAllowed.length > 0)? element.paytypeAllowed : [];
      record.extraPaytypes=element.extraPaytypes.length < 0 ? []:element.extraPaytypes;
      this.jobTypesBackupList.push(record);
    });
  }

  @Input()
  public showJobType: boolean;

  public showTravelOnEdit: boolean = false;
  public showMileagelOnEdit: boolean = false;
  public noshowSingle: boolean = false;

  @ViewChild('jobtypecontrol') jobtypecontrol: DxSelectBoxComponent;
  
  @ViewChild('scopes') scopecontrol: DxSelectBoxComponent;
  @ViewChild('provinceControl') provincecontrol: DxSelectBoxComponent;

  constructor(
    public cs: CoreService,
    private store: Store<fromRoot.State>,
    private tss: TimesheetService,
    private ts: TokenService,
    private winRef: WindowRef,
    private jti: JobTypeService
  ) {
    this.paytypeHourAPIURL = CONFIG.apiURL.core.paytypeHoursList + this.ts.currentUser.businessUnitId;
    this.jobTypeInfo = new JobTypeInfo();
    this.showJobType = false;
    this.timesheetHourTypeList = new Array<TimesheetHourTypeRecord>();
    this.taskList = [];
    this.provinceArray = [];
    
    this.tss.GetProvinceList(this.ts.currentUser.id).subscribe(res => {
      
      this.mapProvinces(res);
      this.provinces = new DataSource({
        store: new ArrayStore({
          data: this.provinceArray,
          key: "value"
        }),
        group: "country_code"
      });

    })
  
  }


  //
  // Entry Type Definition
  //
  @ViewChild('hourTypesListControl') hourTypesListControl: DxSelectBoxComponent;

  // Entry Type, default value is 'single'.
  entryType = 'single';

  // UI control for single part & multiple part: only for Adding; always be hidden when editing.
  entryType_SinglePart_Showingup = 'block';
  entryType_MultiplePart_Showingup = 'none';
  // UI control to make Hour type invisible if Mileage or Travel is entered
  hide_hourjobtype = 'none';
  // UI Control for 'single/multiple' radio boxes: When editing an existing record, we need to hide this.
  entryType_radio_boxes = 'block';

  // hourType list
  hourTypesList: any[];
  hourTypesListBackup: any[];

  // Row Editing
  hourRow = 0;
  defaultHourType = 1;

  timesheetHourTypeList: Array<TimesheetHourTypeRecord>;

  lastAddedOne: TimesheetHourTypeRecord = null;

  isWarningOnHour = false;

  //
  // End of Entry type
  //
  public getWorkOrderURL(customerId: number) {
    if (this.inputValue && this.inputValue.businessUnitId) {
      return CONFIG.apiURL.page.timesheet.WorkOrderWos
        + this.inputValue.businessUnitId.toString()
        + '/' + this.inputValue.userId + '/' + customerId.toString();
    }
  }

  ngOnInit() {

  }


  public CheckLabourMaster(success: () => void,
    failed: (msg: string) => void) {
    if (this.inputValue.businessUnitId && this.inputValue.userId && this.inputValue.userId > 0) {
      // CONFIG.LOG(success, 'checklabourmaster in workorder ');
      this.tss.GetWorkOrderLabour(this.inputValue.businessUnitId, this.inputValue.userId, this.selectedHourType ? this.selectedHourType : 1)
        .subscribe(
          (res: Number) => {
            CONFIG.LOG(res, 'resposne checklabourmaster in workorder');
            if (res > 0 || this.customerWoEditable) {
              success();
            } else {
              failed('');
            }
          },
          (err: any) => {
            failed(err);
          }
        );
    }
  }

  public LoadInputvalue(value: TimesheetValue) {
    CONFIG.LOG(JSON.stringify(value), 'timesheet workorder inputvalue');
    
    //
    // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1899/
    // Bug: 1899: corner case from [mileage setup] => [switch to shop tab] =>[switch to wo Tab] ===> comments is disabled.
    // Tab changes will trigger this load funtion called again, so here we will reset the it to enabled like first time loading.
    this.WorkOrderCommentsDisabled = false;
    // end of bug 1899

    //
    // Handle tab switching.
    //
    this.JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingTabs_FromShopOrTelemOrQuote_To_WO_Tab();

    if (value && value.businessUnitId) {
      this.inputValue = value;
      this.tss.GetCountry(this.inputValue.businessUnitId).subscribe(res=>{

        if(res=="USA")
        {
          this.checked=false;  
          this.Unit="Miles";
        }
        else{
          this.checked=true;
          this.Unit="Kilometers";
        }

        CONFIG.LOG(res, 'GetCountry');
    })
    
      this.tss.getBoolean(CONFIG.apiURL.page.timesheet.WorkOrderUnLinkTimeSheet + this.inputValue.businessUnitId.toString())
        .subscribe(
          (res: boolean) => {
            // make customer/wos can be edit and do not filter
            this.customerWoEditable = res;
            this.customerWoFilterable = !this.customerWoEditable;

            // load tslite types when allow unlinked timesheet = true
            if (this.customerWoEditable) {
              this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.timesheet.TsLitePaytypes + this.inputValue.businessUnitId.toString())
                .subscribe(
                  (res2) => {
                    this.tsLiteTypes = res2;
                  },
                  (err:any)=>{
                    this.store.dispatch(new fromMessage.PushErrorMessage(err));
                  }
                  );
            }
          });
      // added by Luke Dec 9,2017
      // allowunlinked timesheet don't initilize dropdown list.
      if (!this.customerWoEditable) {
        //   CONFIG.LOG(this.inputValue.businessUnitId, 'timesheet workorder inputvalue');
        this.APIURLCustomerList = CONFIG.apiURL.page.timesheet.WorkOrderCustomer + value.businessUnitId.toString();
        //  this.workOrderCustomer.getList(this.APIURLCustomerList);
        if (value.userId && value.userId > 0) {
          this.APIURLWorkOrderWosList = this.getWorkOrderURL(0);
          if (this.workOrderWo) {
            this.workOrderWo.getList(this.APIURLWorkOrderWosList);
          }
        } else {
          this.workOrderWo.clear();
        }
      }
      this.workOrderComment.clear();
      this.comment = '';
    } else {
      this.ClearDropDownList();
    }
    this.setDefaultValue();
  }

  afterCustomerLoad() {
    if(!this.noReloadpage)
    {
         this.setDefaultValue();
    }
  }

  afterWosLoad() {
    CONFIG.LOG(this.selectedCustomer, 'afterWosLoad selectedCustomer');
    this.filterCustomer();
    if(!this.noReloadpage)
    {
      //
      // Don't load when loading an existing timeshee record.
      //
      if(this.selectedRow && this.selectedRow.memberTime_ID && this.selectedRow.memberTime_ID > 0) {
        this.workOrderWo.showOne(this.selectedRow.workorderDescription, this.selectedRow.workorderId);
        console.log("Skip filterCustomer afterwosload");
        return;
      }
    
      //
      // The long-run loading Wos will trigger this event. It is working oaky if this can be done before the customer's change.
      // Otherwise will reset the customer to null value.
      //
      let alreadySelectedACustomer = false;
      let selectedValue = 0;
      if(this.selectedCustomer && this.workOrderCustomer.value && this.selectedCustomer == this.workOrderCustomer.value) {
        alreadySelectedACustomer = true;
        selectedValue = this.workOrderCustomer.value;
      }

      console.log(this.workOrderCustomer.value);
      this.setDefaultValue();
      console.log(this.workOrderCustomer.value);

      if(alreadySelectedACustomer) {
        this.workOrderCustomer.value = selectedValue;
      }
      console.log(this.workOrderCustomer.value);

      if(this.selectedWorkorderIdFromWON ) {
        //
        // Sometime the selected work order is gone.
        //
        console.log("before.....check: " + this.workOrderWo.value);
        this.workOrderWo.value =  this.selectedWorkorderIdFromWON ;
      }

      //
      // Eno of fixing 
      //

    }
    else{
      this.setwovalue();
      CONFIG.LOG(this.selectedWorkOrder, 'this.selectedWorkOrder;----');
    }
  }

  public setwovalue()
  {
      this.workOrderCustomer.value = this.selectedCustomer;
      this.workOrderWo.value=this.selectedWO;
      this.selectedWorkOrder=this.selectedWO;
      this.selectedWorkOrderLabel=this.workOrderWo.getLabelNameByValue(this.selectedWorkOrder.toString());
      this.OnWorkOrderSelectedChange.emit({ selectedWorkOrderId: this.selectedWorkOrder,selectedCustomerId: this.selectedCustomer,label:this.selectedWorkOrderLabel,value:this.selectedWO});
  }

  public setDefaultValue() {
    CONFIG.LOG(this.defaultValue, 'defaultvalue setdefaultvalue')

    this.hours = null;
    this.IsHour=true;
    this.isMileageOrTavel=false;
    this.mileageChk =false;
    this.travelChk =false;
    this.disableMileage = true;
    this.disableTravel = true;

    //
    // Because it is set to 0 when it (distance) is being defined, so here change null to 0. (Yan)
    //
    this.distance=0;
    this.minvalue= 0; // Constrain the ability to enter negative time via the timesheet & scheduler UI
    this.maxvalue=null;
    this.selectedScope = null;
    this.comment = '';
    this.percentComplete = null;
    this.rating = null;
    this.selectedRow = null;
    if (!this.customerWoEditable) {
      this.workOrderCustomer.loadDropDown();
      this.workOrderWo.loadDropDown();
      this.disableMileage = false;
      this.disableTravel = false;
    }
  
    if(!this.isMileageOrTavel){
    this.workOrderHourType.loadDropDown();
      
   }

    //
    // Get a copy of hour type for 'Entry Type'
    //
    this.fetch_HourTypes(this.workOrderHourType.options, false);

    if (this.defaultValue && this.inputValue.userId === this.defaultValue.userId) {
      CONFIG.LOG(this.defaultValue, 'defaultvalue setdefaultvalue')
      this.selectedCustomer = this.defaultValue.customer_Id;
      this.selectedWorkOrder = this.defaultValue.id;

      if(this.defaultValue.callFromScheduler) {
        // Call from scheduler requirement.
        if(this.workOrderWo.items) {
          this.getScopes(this.workOrderWo.items, this.defaultValue.id);
        }
      }

      if(this.defaultValue.callFromSchedulerForJobType) {
        //
        // Bug fix for https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/2069/
        // Job type not selectable on timesheet when default filled in by scheduler
        //
        console.log('loading job type..... for scheduler....', this.inputValue);
        this.getJobTypeSetting(this.inputValue.userId,this.inputValue.businessUnitId,this.selectedCustomer,this.jti);
      }

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
      // this.hours = this.defaultValue.hours;
      // this.currenthourValue=this.defaultValue.hours;
      this.selectedHourType = this.defaultValue.payType;
      this.comment = this.defaultValue.comment;
      if (!this.customerWoEditable) {
        this.workOrderCustomer.value = this.selectedCustomer;
        this.workOrderWo.value = this.selectedWorkOrder;
        this.OnWorkOrderSelectedChange.emit({ selectedWorkOrderId: this.selectedWorkOrder });
      }
      this.selectedTsLiteType = 0;
    }


     this.tss.GetDefaultProvince(this.inputValue.businessUnitId).subscribe(res => {
       if (this.provincecontrol.value == null) {
         this.provincecontrol.value = res;

         // Get a copy of the default province info.
         this.defaultProvinceBasedonBusinessUnit = res;
       }
     });

  }

  onDistanceChange(event: any)
  {
    this.hours=0;
    this.caculationForMileage(event);
  }


  //
  // Bug: https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1891/
  // Inconsistent decimal places on mileage records
  //
  // This function will make sure only two decimals In Max will be included before sending to API side.
  //
  constrainToHave2DecimalsInMax(e: any) {
    //
    // Validation checks.
    //
    if(!this.distance) {
      // If no valid value, do nothing.
      return;
    }

    //
    // From Matt: For mileage, it should be exactly like the other time entries... round to two digits.
    //

    //
    // To have 2 decimals in max only from angular side.
    //
    this.distance = parseFloat(this.distance.toFixed(2));

    //
    // Recalculate the comments.
    //
    this.caculationForMileage(event);
  }

  caculationForMileage(event: any)
  {
    //
    // populate this function from 'onDistanceChange', so it can be called from both of events.
    //
    if(this.distance>0 && this.distance<=this.maxDistance)
    {
      this.currentdistanceValue=this.distance;
      this.comment =this.distance+' '+this.Unit+" traveled.";
      this.WorkOrderCommentsDisabled=true;
    }
    else
    {
      this.distance=this.currentdistanceValue;
      this.comment='';
      this.WorkOrderCommentsDisabled=false;
    }
  }

  public ClearDropDownList() {
    if (!this.customerWoEditable) {
      this.workOrderCustomer.clear();
      this.workOrderComment.clear();
      this.workOrderWo.clear();
    }
  }
  public workOrderreLoad(selectWo:number,selectCustomer_id:number)
  {
    this.APIURLWorkOrderWosList = this.getWorkOrderURL(0);
    
    if (this.workOrderWo) {
      this.selectedCustomer=selectCustomer_id; 
      this.workOrderWo.value=selectWo;
      this.selectedWO=selectWo;
      this.workOrderWo.getList_WO(this.APIURLWorkOrderWosList);
    }
  }
  
  OnChangeCustomer(event: any) {
    this.noshowSingle = false;
    this.taskList = [];
    this.workOrderWo.clear();
    if (this.customerWoFilterable && this.selectedCustomer && this.selectedCustomer > 0) {
      // 
      // Move the logic from filterCustomer to this event.
      // filterCustomer will be called in other events but we only need to reload the job types when customer is changed.
      //
      this.getJobTypeSetting(this.inputValue.userId,this.inputValue.businessUnitId,this.selectedCustomer,this.jti);
    }

    this.filterCustomer();   
    this.refreshJobtypeAndPaytype();
    
    // filtercustoemr inside call getjobtypesetting.
    // this.getJobTypeSetting(this.inputValue.userId,this.inputValue.businessUnitId,event.value,this.jti);
    if(this.showMileage){
      this.IsHour=false;
      this.distance=0;
      this.hours=0;
      this.comment ="";
      this.WorkOrderCommentsDisabled=false; 
      this.selectedHourType=8; 
      this.isMileageOrTavel=true;
      this.hide_hourjobtype='none';
      this.JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingHourType(8);
     
      }
      else
      {
        this.hide_hourjobtype='block';
        this.IsHour=true;
        this.comment="";
        this.WorkOrderCommentsDisabled=false;
        this.isMileageOrTavel=false;
        this.disableTravel=false;
      }  
      if(this.showTravel){
        
        this.travelChk=false;
        this.hours=0;
      }
      else{
        this.disableMileage=false;
        this.hide_hourjobtype='block';
      }
     
    
  }
 private getJobTypeSetting(memberid: number, businessUnitID: number,customer_id: number, jobService: JobTypeService ) {
    let parameter: JobTypeRecordQueryParameter = new JobTypeRecordQueryParameter();
    parameter.business_uint_id = businessUnitID;
    parameter.member_id = memberid;
    parameter.customer_id = customer_id;
    jobService.GetJobTypeInfowithCust(parameter).subscribe((result:any) => {
          if(result){
         this.jobTypeInfo.jobTypes = result.jobTypes;
         this.showJobType= result.showJobType;
         this.setExtrapayments();              
        }
       error => { 
        console.log(error);
        }
    });
  }

  private filterCustomer() {
    if (this.customerWoFilterable && this.selectedCustomer && this.selectedCustomer > 0) {

      //
      // Don't load when loading an existing timeshee redcord.
      //
      if(this.selectedRow && this.selectedRow.memberTime_ID && this.selectedRow.memberTime_ID > 0) {
        console.log("Skip filterCustomer");
        return;
      }

      CONFIG.LOG(this.selectedCustomer, 'OnChangeCustomer this.selectedWorkOrder;----');
      // filter work order byase bu selected customer
      this.workOrderWo.filterCustomer(this.selectedCustomer);

      // this.getJobTypeSetting(this.inputValue.userId,this.inputValue.businessUnitId,this.selectedCustomer,this.jti);
    }
  }

  OnChangeWOs(event: any) {
    //event.selectedCustomerId=this.selectedCustomer;
    CONFIG.LOG(event, 'OnChangeWos timesheetwork order1');
    if (this.customerWoFilterable && event.selectedCustomerId && event.selectedCustomerId > 0) {
      CONFIG.LOG(event.selectedCustomerId, 'OnChangeWos timesheetwork order2');
      this.selectedCustomer = event.selectedCustomerId;
      this.loadComments();
      event.selectedWorkOrderId = this.selectedWorkOrder;
      CONFIG.LOG(this.selectedWorkOrder, 'OnChangeWos timesheetwork order3');
      this.selectedWorkOrderLabel = event.label;
      this.taskList = [];
      this.scopecontrol.value = 0;
      this.getScopes(event.woList, event.selectedWorkOrderId);
      this.getJobTypeSetting(this.inputValue.userId,this.inputValue.businessUnitId,event.selectedCustomerId,this.jti);
      this.IsPrevailingWageEnabledForWo(event.selectedWorkOrderId);
      if(this.showMileage){
        this.IsHour=false;
        this.distance=0;
        this.hours=0;
        this.comment ="";
        this.WorkOrderCommentsDisabled=false; 
        this.selectedHourType=8; 
        this.isMileageOrTavel=true;
        this.hide_hourjobtype='none';
        this.JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingHourType(8);
        this.disableTravel=true; 
        }
        else
        {
          this.hide_hourjobtype='block';
          this.IsHour=true;
          this.comment="";
          this.WorkOrderCommentsDisabled=false;
          this.isMileageOrTavel=false;
          this.disableTravel=false;
        }  
        if(this.showTravel){       
          
          this.hours=0;
          this.travelChk= false;
          
        }
        else{
          this.disableMileage=false;
          this.hide_hourjobtype='block';
        }
      
      this.selectedWorkorderIdFromWON = this.selectedWorkOrder;  
      this.OnWorkOrderSelectedChange.emit(event);     
    }
    
  }

  getScopes(woList:Array<any>,woId:number){

    //
    // When reload the list, first step is to empty the list.
    //
    this.taskList = [];

    //
    // Have a validation on the passing list.
    // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/2059/
    //
    if(!woList) {
      return;
    }

    woList.forEach(wo => {
       if(wo.value == woId){
         wo.scopes.forEach(scope => {
           var task = new Task();
           task.id = scope.id;
           task.name = scope.name;

           let found = false;
           for(let i = 0; i < this.taskList.length; i++) {
              if(this.taskList[i].id == task.id) {
                found = true;
                break;
              }
           }

           if(!found) {
            this.taskList.push(task);
           }

         });
          
       }
     });
   
    
  }


  loadComments() {
    // CONFIG.LOG(this.selectedWorkOrder, 'loadcomments selected workorder');
    // CONFIG.LOG(JSON.stringify(this.inputValue), 'loadcomments inputvalue');
    if (this.inputValue && this.inputValue.businessUnitId
      && this.inputValue.userId && this.selectedWorkOrder) {
      const url = CONFIG.apiURL.page.timesheet.WorkOrderComments +
        this.inputValue.businessUnitId.toString() + '/' +
        this.inputValue.userId.toString() + '/' +
        this.selectedWorkOrder.toString();

      CONFIG.LOG(url, 'loadcomments url');

      this.workOrderComment.getList(url);
      this.cs.getBoolean(CONFIG.apiURL.page.timesheet.WorkOrderCommentsDisabled
        + this.inputValue.businessUnitId.toString() + '/' +
        this.inputValue.userId.toString() + '/' +
        this.selectedWorkOrder.toString())
        .subscribe(res => {
          this.WorkOrderCommentsDisabled = res;
          if (this.WorkOrderCommentsDisabled) {
            this.comment = '';
          }
        },
        (err:any)=>{
          this.store.dispatch(new fromMessage.PushErrorMessage(err));
        });
    }

  }

  // fill workorder components content
  public SelectRow(item: TimesheetList, value: TimesheetValue) {
    if (!item) { return; }
    this.selectedRow = item;
    this.comment = item.comments;
    
    if(item.hours>=0)
    { 
      this.minvalue=0;
      this.maxvalue=null;
    }
    else
    {
      this.minvalue=null;
      this.maxvalue=0;
    }
    this.hours = item.hours;
    if(item.payTypeId==8)
    {
        this.distance=item.mileage_value? item.mileage_value:0;
        this.Unit=item.mileage_unit;
        this.checked=item.mileage_unit=="Miles"?false:true;
        this.IsHour=false;
    }

   
    this.currenthourValue=item.hours;
    this.currentdistanceValue=item.mileage_value? item.mileage_value:0;

    this.noshowSingle = true;
    let run = false;
    if (item.payTypeId == 7 || item.payTypeId == 8) {
      this.workOrderHourType.disabled = true;

      //
      // When loading for existing records, set showMileage & showTravel  to false.
      // Then use showTravelOnEdit/ showMileagelOnEdit to control show or hide.
      //
      this.showMileage = false;
      this.showTravel = false;

      this.showTravelOnEdit = false;
      this.showMileagelOnEdit = false;

      //
      // Later the function setExtrapayments will be called if we selecting once added a new one.
      // Hard to control, so disable them.
      //
      if(item.payTypeId == 7) {
        this.showTravelOnEdit = true;
        this.travelChk = true;
        this.disableTravel = true;
        this.mileageChk = false;
        this.disableMileage = true;
      }
  
      if(item.payTypeId == 8) {
        this.showMileagelOnEdit = true;
        this.mileageChk = true;
        this.disableMileage = true;
        this.travelChk = false;
        this.disableTravel = true;
      }

    } else {
      run = true;
    }

    this.percentComplete = item.wo_percent_complete;
    this.rating = item.rating;
    this.selectedTsLiteType = item.tsLitePaytypeId;
    const woid = Number(item.workorderId);
    
    if(item.scope_id){
      this.scopecontrol.value = item.scope_id;
    } else {
      //
      // There are 3 cases (in terms of the scope for timesheet records):
      //
      // (1) One timesheet record with a WO which does not have any scopes records.
      // (2) One timesheet record with a WO which does HAVE any scopes records, but for this timesheet record, it does not have one selected when it was created.
      // (3) One timesheet record with a WO which does HAVE any scopes records, but for this timesheet record, it does HAVE one selected when it was created.

      //
      // Below code will handle a record of type '(2)' when switching from previous record of type '(3)', so no default value will be carried on.
      //
      this.scopecontrol.value = 0;
    }
    
    this.getScopes(this.workOrderWo.items,woid)

    this.provincecontrol.value = item.prov_id;

    if (!this.customerWoEditable && woid > 0) {
      this.tss.WorkOrderWoById(woid).subscribe(
        (res: TimesheetWorkOrderWo) => {
          if (res) {
            item.workorderDescription = res.label;  
          }
          this.workOrderWo.showOne(item.workorderDescription, item.workorderId);
          this.workOrderCustomer.showOne(item.custName, item.custId);
          this.selectedWorkOrder = item.workorderId;
          this.workOrderComment.showOne('', item.woCommentId);

          if(run) {
            this.showTravelOnEdit = false;
            this.showMileagelOnEdit = false;
            this.mileageChk = false;
            this.disableMileage = true;
            this.travelChk = false;
            this.disableTravel = true;
            this.workOrderHourType.showOne(item.hourType, item.hourTypeId);
          }

        },
        (err:any)=>{
          this.store.dispatch(new fromMessage.PushErrorMessage(err));
        });
    } else {
      this.selectedRow.woCommentId = 0;
      //      this.workOrderWo.showOne(item.workorderId, item.workorderId);
      this.selectedWorkOrder = item.workorderId;
      this.selectedCustomer = item.custName;
      //    this.workOrderCustomer.showOne(item.custName, item.custName);
    }

    if(this.showJobType) {
      let jtr = new JobTypeRecord();
      jtr.membertype_id = item.membertype_id;
      jtr.membertype_name = item.membertype_name;
      //In future we may need to add extra paytypes and paytype id here
      // 
      // Check and add if required.
      //
      if(this.jobTypeInfo.jobTypes != null && this.jobTypeInfo.jobTypes.length == 0) {
        this.jobTypeInfo.jobTypes.push(jtr)
      }

      if(this.jobTypeInfo.jobTypes != null) {
        let found = false;
        this.jobTypeInfo.jobTypes.forEach(item => {
          if(item.membertype_id === jtr.membertype_id) {
            found = true;
          }
        });

        if(!found) {
          this.jobTypeInfo.jobTypes.push(jtr)
        }
      }

      this.jobtypecontrol.value = jtr.membertype_id;
      this.jobtypecontrol.disabled = true;
    }
    if(this.selectedHourType !== 7 && this.selectedHourType !== 8){
      this.getIsPrevailingWage(woid, item.id);
    }
    
    //
    // When select a row to edit, hide this thing.
    //
    this.ResetEntryTypeStuffWhenEditing();

    //
    // Hide the multiple entry type when editing
    //
    this.noshowMultiple = true;

    //
    // end of 'No customer selected, no hours.'
    //
  }

  public CancelSelectedRow() {
    this.defaultValue = null;
    this.scopecontrol.disabled = false;

    if(this.showJobType) {
      this.jobtypecontrol.disabled = false;
        
      //
      // Reset the binding in here.
      // This will be called when cancelling or updating or inserting...
      //
      this.resetJobTypeList();
      
      this.refreshJobtypeAndPaytype();
      

    }

    this.setDefaultValue();

    //
    // This will be called when cancelling or updating or inserting...
    // Reset to default mode for entry type.
    //
    this.ResetEntryTypeStuffAfterInsertingOrEditingOrCancelling(true);

    //
    // 1883: Edit Time Record in Open period - Comments are disabled.
    // See the attached videos to see more. https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1883/
    //
    this.WorkOrderCommentsDisabled = false;

    this.taskList = [];
    this.scopecontrol.value = 0;

    this.noshowMultiple = false;
    this.workOrderHourType.disabled = false;
    this.showTravelOnEdit = false;
    this.showMileagelOnEdit = false;
    this.noshowSingle = false;
    this.prevailingWageVisible = false;
    this.isPrevailingWage = false;
    this.prevailingWageVisibleOldState = undefined;
    this.isPrevailingWageOldState = undefined;

    this.selectedWorkorderIdFromWON = 0;
  }

  get OutputUpdateValue(): UpdateTimesheetWorkOrder {

    return {
      date: DATE.ToDateOnly(this.inputValue.selectedDate),
      selectedBusinessUnitId: this.inputValue.businessUnitId,
      selectedUserId: this.inputValue.userId,

      memberTime_ID: this.selectedRow.id,
      memberTime_WoComment_ID: this.selectedWorkOrderComment || 0,
      numberOfHours: this.hours? this.hours:0,
      rating: this.rating,
      percentComplete: this.percentComplete,
      memberTime_WoComment: this.comment,
      selectedWorkOrderId: this.selectedWorkOrder,
      scope_id: this.scopecontrol.value? this.scopecontrol.value :  null,
      prov_id: this.provincecontrol.value? this.provincecontrol.value : 1,
      mileage_value:this.distance,
      mileage_unit:this.Unit,
      allow_jobtype_selection: this.jobTypeInfo.showJobType,
      selectedJobType: this.jobTypeInfo.showJobType? this.jobtypecontrol.value: 0,
      is_prevailing_wage: this.isPrevailingWage
    }
  }

  get OutputInsertValue(): InsertTimeSheetWorkOrder {

    let intertTimesheetRecord = {
      date: DATE.ToDateOnly(this.inputValue.selectedDate),
      selectedBusinessUnitId: this.inputValue.businessUnitId,
      selectedUserId: this.inputValue.userId,
      payTypeId: this.selectedHourType ? this.selectedHourType : this.travelChk ? 7 : 1,
      scope_id: this.scopecontrol.value ? this.scopecontrol.value : null,
      prov_id: this.provincecontrol.value? this.provincecontrol.value : 1,

      memberTime_WoComment_ID: 0,
      numberOfHours: this.hours? this.hours:0,
      rating: this.rating ? this.rating : 0,
      mileage_value:this.distance? this.distance:0,
      mileage_unit:this.Unit,

      percentComplete: this.percentComplete ? this.percentComplete : 0,
      memberTime_WoComment: this.comment,

      selectedCustomerId: this.customerWoEditable ? 0 : this.selectedCustomer,
      selectedCustomerName: this.customerWoEditable ?
        this.selectedCustomer.toString() : this.workOrderCustomer.getLabelNameByValue(this.selectedCustomer.toString()),
      selectedWorkOrderId: this.customerWoEditable ? 0 : this.selectedWorkOrder,
      selectedWorkOrderName: this.customerWoEditable ?
        this.selectedWorkOrder.toString() : this.workOrderWo.getLabelNameByValue(this.selectedWorkOrder.toString()),
      selectedTsLiteType: this.selectedTsLiteType || 0,

      allow_jobtype_selection: this.jobTypeInfo.showJobType,
      selectedJobType: this.jobTypeInfo.showJobType? this.jobtypecontrol.value: 0,
      entry_type: this.entryType,
      timesheetHourTypeList: this.timesheetHourTypeList,
      is_prevailing_wage: this.isPrevailingWage
    };

    if(intertTimesheetRecord.entry_type == "multiple") {
      //
      // The server side will crash if not have a valid number.
      //
      intertTimesheetRecord.numberOfHours = 0;
    }
    // console.log(intertTimesheetRecord);
    return intertTimesheetRecord;
  }
  public ValidateValue(): boolean {
    CONFIG.LOG(this.selectedTsLiteType, 'selected selectedTsLiteType in workorder');
    if (this.customerWoEditable && !(this.selectedTsLiteType && this.selectedTsLiteType > 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select a lite type.'));
      return false;
    }
    if (!this.customerWoEditable && !(this.selectedCustomer && this.selectedCustomer > 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select a Customer.'));
      return false;
    }
    if (this.customerWoEditable && !(this.selectedCustomer && this.selectedCustomer.toString().length >= 2)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input a valid Customer Name.'));
      return false;
    }
    if (this.customerWoEditable && !(this.selectedWorkOrder && this.selectedWorkOrder.toString().length >= 3)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input a valid Work order number.'));
      return false;
    }
    if (!this.customerWoEditable && !(this.selectedWorkOrder && this.selectedWorkOrder > 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select a Work order.'));
      return false;
    }

    if(this.provincecontrol.value == null){
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select a province/state.'));
        return false;
    }

    if(this.entryType != 'multiple') {
      if(this.IsHour)
      {
        if (!this.hours)
         {
             this.store.dispatch(new fromMessage.PushInfoMessage('Please input hours.'));
             return false;
         }
         else{
          if(this.hours==0)
          {
           this.store.dispatch(new fromMessage.PushInfoMessage('Please input hours greater than zero.'));
           return false;
          }
       }
     }
     else
     {
       if(this.selectedHourType===8 && !this.distance)
       {
            this.store.dispatch(new fromMessage.PushInfoMessage('Please input distance.'));
            return false;
       }
     }
    }
    else
    {
      if( (this.timesheetHourTypeList) && this.timesheetHourTypeList.length == 0)
       {
            this.store.dispatch(new fromMessage.PushInfoMessage('Please add one or more hours.'));
            return false;
       }
    }

    if (!((this.comment && this.comment.length >= 10) || (this.selectedWorkOrderComment && this.selectedWorkOrderComment > 0)))
     {
         this.store.dispatch(new fromMessage.PushInfoMessage('Please select a comment, or enter a detailed comment greater than 10 characters.)'));
         return false;
     }

    if (this.showJobType) {
      if(this.jobtypecontrol.value  === 0) {
        this.store.dispatch(new fromMessage.PushInfoMessage
          ('Please select a job type.'));
        return false;
      }
    }

    if (this.showJobType) {
      let valid = this.validation_on_hours_for_jobTypeEnabledBranch_WhenAdding();
      if (!valid) {
        return false;
      }
    } 

    return true;
  }

  HourTypeChange(event: any) {

    if(event.value === 7 || event.value === 8){
      this.toggleIsPrevailingWageVisibility(false);
    }else{
      this.toggleIsPrevailingWageVisibility(true);
    }

    if(event.value===8)
    {
       this.IsHour=false;
       this.distance=0;
       this.hours=0;
       this.comment ="";
       this.WorkOrderCommentsDisabled=false;  
    }
    else
    {
      this.IsHour=true;
      this.comment="";
      this.WorkOrderCommentsDisabled=false;
    }

    //
    // Call this function due to bug 1893 about 'Job Type should be disabled on new Mileage entry record'.
    //
    this.JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingHourType(event.value);

    CONFIG.LOG(event.value, 'HourTypeChange');
  }

  onMileageChecked(event:any){
    if(event){
    this.IsHour=false;
    this.distance=0;
    this.hours=0;
    this.comment ="";
    this.WorkOrderCommentsDisabled=false; 
    this.selectedHourType=8; 
    this.isMileageOrTavel=true;
    this.hide_hourjobtype='none';
    this.JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingHourType(8);
    this.disableTravel=true; 
    this.toggleIsPrevailingWageVisibility(false);
    }
    else
    {
      this.hide_hourjobtype='block';
      this.IsHour=true;
      this.comment="";
      this.WorkOrderCommentsDisabled=false;
      this.isMileageOrTavel=false;
      this.disableTravel=false;
      this.selectedHourType=1; // what happen if no regular in the list? will not select anything.
      this.toggleIsPrevailingWageVisibility(true);
    }  
  }
  onTravelChecked(event:any){
    if(event){
      this.selectedHourType=7;
      this.disableMileage=true;
      this.hide_hourjobtype='none';
      this.toggleIsPrevailingWageVisibility(false);
    }
    else{
      this.selectedHourType=1; // what happen if no regular in the list? will not select anything.
      this.disableMileage=false;
      this.hide_hourjobtype='block';
      this.toggleIsPrevailingWageVisibility(true);
    }
  }
  //
  // The code section for https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1893/
  // Job Type should be disabled on new Mileage entry record
  //

  //
  // ************************************************************************
  // User scenorioes: 
  // (1) Values switching; 
  // (2) Tab Switching; 
  // (3) Cancel button; 
  // (4) Select existing records; 
  // (5) Delete records;
  // ************************************************************************
  // 

  private JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingHourType( hourTypeId: number) {
    if (!this.showJobType) {
      // The user can't see the jobtype input field, so do nothing.
      return;
    }

    if(!this.jobTypeInfo.defaultValue.membertype_id) {
      // no default value
      return;
    }

    //
    // Now the user can see the jobtype input field and also have a valid default value.
    //
    if(hourTypeId == 8) {
      // Now the hour Type change to mileage.
      this.JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingHourType_FromOther_To_MileageType();
    } else {
      // Now the hour Type change back to mileage.
      this.JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingHourType_FromMileage_To_OtherType();
    }
    
  }

  private JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingHourType_FromOther_To_MileageType() {
    // The things should be done when switching to mileage.
    // (1) Set the job type to default value if changeds.
    // (2) Disabled job type type control.
    // Above two will guarantee job type is disabled and also set to default value.
    //

    this.jobtypecontrol.value = this.jobTypeInfo.defaultValue.membertype_id;
    this.jobtypecontrol.disabled = true;
  }

  private JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingHourType_FromMileage_To_OtherType() {
    // The things should be done when swithcing back from mileage.
    // (1) Enable job type type control.
    // Above two will guarantee job type is disabled and also set to default value.
    //
    this.jobtypecontrol.disabled = false;
  }

  private JobTypeShouldBeDisabledOnNewMileageEntryRecord_SwitchingTabs_FromShopOrTelemOrQuote_To_WO_Tab() {
    // The things should be done when switching back from other tab, for example, from shop to Wo.
    // (1) Enable job type type control.
    // Above two will guarantee job type is disabled and also set to default value.
    //
    if (!this.showJobType) {
      // The user can't see the jobtype input field, so do nothing.
      return;
    }

    this.jobtypecontrol.disabled = false;
  } 

  //
  // End of section for 'Job Type should be disabled on new Mileage entry record'
  //

  handleChange(event: any) {

      if(event.checked)
      {
         this.Unit="Kilometers";
      }
      else
      {
        this.Unit="Miles";
      }
     if(this.distance>0)
     {
       this.comment =this.distance+' '+this.Unit +" traveled.";
       this.WorkOrderCommentsDisabled=true;
     }
     else
     {
       this.comment="";
       this.WorkOrderCommentsDisabled=false;  
     }
  }

  UpdateRow(items: TimesheetList[],
    success: (msg: string) => void,
    failed: (msg: string) => void): void {


    this.tss.UpdateWorkOrder(this.OutputUpdateValue)
      .subscribe((res: string) => {
        // update table list value.
        const updateItem = this.OutputUpdateValue;
        CONFIG.LOG(JSON.stringify(items), 'update work order items');
        const item = items.find(x => x.id === updateItem.memberTime_ID);
        CONFIG.LOG(JSON.stringify(item), 'update work order found item');
        item.hours = updateItem.numberOfHours;
        item.rating = updateItem.rating;
        item.comments = updateItem.memberTime_WoComment;
        item.wo_percent_complete = updateItem.percentComplete;
        this.CancelSelectedRow();
        success(res);
      },
        (err: any) => {
          failed(err);
        });
  }

  private validation_on_hours_for_jobTypeEnabledBranch_WhenAdding() {
    //
    // Say Invalid if matching
    // (1) If branch is haveing job type enabled feature, and
    // (2) the current item is not travel or mileage, and
    // (3) the hour type is not given
    //

    if(this.selectedRow && this.selectedRow.memberTime_ID &&this.selectedRow.memberTime_ID > 0) {
      // existing one.
      return true;
    }

     if( !this.showJobType ) {
        return true;
     }

     if(this.selectedHourType && this.selectedHourType >= 7 ) {
       // trave (7) or mileage (8)
       return true;
     }

     // by default the value is 1, you must select to have a type other than 1.
     if(this.selectedHourType != 1) {
       return true;
     }

     // No the value is one
     if(this.workOrderHourType.options && this.workOrderHourType.options.length > 0) {
      //
      // check in future.
      //
      return true;
     } else {
       // No types in the list, so not allowed to insert regular timesheet.
       this.store.dispatch(new fromMessage.PushWarnMessage("Please select a hour type."));
       return false;
     }

  }

  InsertRow(items: TimesheetList[],
    success: (msg: string) => void,
    failed: (msg: string) => void): void {
     CONFIG.LOG(this.OutputInsertValue, 'insert workorder timesheet');

    this.tss.InsertWorkOrder(this.OutputInsertValue)
      .subscribe((res: string) => {
        this.CancelSelectedRow();
        this.handleCallBack_Data();
        let result = this.handleCallBack(res);
        if(result.continue) {
          success(result.message);
        } else {
          // Passs null to pass the messge part.
          //success(null);
          this.store.dispatch(new fromMessage.PushWarnMessage(result.message));
          return;
          
        }
      },
        (err: any) => {
          failed(err);
        })


  }

  OnChangeComments(event: any) {
    // this.comment = this.selectedWorkOrderComment;
    if (event.item) {
      this.comment = event.item.label;
    }

  }


  signOff() {
    this.winRef.boingNesi1('/mobile/index.aspx?a=signoff&woprog_id=' + this.selectedWorkOrder + '&from=scheduler', 'signOff');
  }

 
  onChange(event)
  {
      if(this.hours!=null)
      { 
        if(Math.abs(this.hours)<0.01 && (Math.round(this.hours*100)/100)<0.01)
        {
          this.hours=0;
        }
        else{
          this.hours=Math.round(this.hours*100)/100;
        }
        
        if(this.minvalue!=null)
        {
          if(this.hours>=this.minvalue)
          {
            this.currenthourValue=this.hours;
          }
          else
          {
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

  private resetJobTypeList() {
    if(!this.showJobType) {      
      return;
    }

    if(!this.jobTypesBackupList) {
      return;
    }

    let list = Array<JobTypeRecord>();

    this.jobTypesBackupList.forEach(element => {
      let record = new JobTypeRecord();
      record.membertype_id = element.membertype_id;
      record.membertype_name = element.membertype_name;
      record.paytypeAllowed =(element.paytypeAllowed != undefined && element.paytypeAllowed.length > 0)? element.paytypeAllowed : [];
      record.extraPaytypes=(element.extraPaytypes != undefined && element.extraPaytypes.length > 0)? element.extraPaytypes : [];
      list.push(record);
    });

    // Reset list * default value.
    this.jobTypeInfo.jobTypes = list;
    this.jobtypecontrol.value = this.jobTypeInfo.defaultValue.membertype_id;
  // if(this.jobTypeInfo.jobTypes!= undefined && this.jobTypeInfo.jobTypes.length>0){
   // this.jobtypecontrol.value = this.jobTypeInfo.jobTypes[0].membertype_id;}
  }

  //
  // Entry Type code in here
  //
  public entyType_Single_Click() {
    this.fetch_HourTypes(this.workOrderHourType.options, false);
    this.setHourTypeList();
    this.entryType_SinglePart_Showingup = 'block';
    this.entryType_MultiplePart_Showingup = 'none';
    this.disableMileage=false;
    this.disableTravel=false;
    if(this.showJobType){
      this.setExtrapayments();
    }
  }

  public entyType_Multiple_Click() {
    
    this.fetch_HourTypes(this.workOrderHourType.options, false);

    const newOptions: any[] = [];
    this.hourTypesList.forEach(element => {
      if(element.value != 8 ){
        newOptions.push(element);
      }
    });

    this.fetch_HourTypes(newOptions, false);

    this.setHourTypeList();

    this.entryType_SinglePart_Showingup = 'none';
    this.entryType_MultiplePart_Showingup = 'block';
    this.hide_hourjobtype = 'none';
    this.mileageChk=false;
    this.travelChk=false;
    this.disableMileage=true;  
    this.disableTravel=true;
  }

  public ResetEntryTypeStuffWhenEditing() {
    // UI
    this.entryType_radio_boxes = 'none';
    this.entryType_SinglePart_Showingup = 'block';
    this.entryType_MultiplePart_Showingup = 'none';
    this.hide_hourjobtype = 'block'; 

    // data
    this.entryType = "single";
  }

  public ResetEntryTypeStuffAfterInsertingOrEditingOrCancelling(forcedReset: boolean) {
    this.entryType_radio_boxes = 'block';
    this.entryType_SinglePart_Showingup = 'block';
    this.entryType_MultiplePart_Showingup = 'none';
    this.entryType = 'single';    
    this.hide_hourjobtype = 'block'; 
    this.timesheetHourTypeList = [];

    this.isWarningOnHour = false;

    if(this.defaultProvinceBasedonBusinessUnit && forcedReset) {
      this.provincecontrol.value = this.defaultProvinceBasedonBusinessUnit;
    }
  }

  public AddToList() {  
    let hourTypeId = this.hourTypesListControl.value;
    let hourType = this.hourTypesListControl.text;
    let hour = this.hourRow;

    //
    // Validation
    if(!hourTypeId) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Hour Type is required.'));
      return;
    }

    if(hour == 0) {
      this.isWarningOnHour = true;
      this.store.dispatch(new fromMessage.PushInfoMessage('Hour(s) cannot be zero.'));
      return;
    } else {
      this.isWarningOnHour = false;
    }

    const record = new TimesheetHourTypeRecord();
    record.hourTypeId = hourTypeId;
    record.hourType = hourType;
    record.hours = hour;
    this.timesheetHourTypeList.push(record);

    //
    // Remove it from the hourTypesListControl once added...
    //
    const newOptions: any[] = [];
    this.hourTypesList.forEach(element => {
      if(element.value != hourTypeId ){
        newOptions.push(element);
      }
    });

    this.hourTypesList = newOptions;
    this.setHourTypeList();
  }

  public RemoveFromList(record: TimesheetHourTypeRecord) {
    if(!this.hourTypesList) {
      return;    
    }

    if(!record) {
      return;
    }

    //
    // Bring back to List
    let option = {
      label: record.hourType,
      value: record.hourTypeId
    };

    this.hourTypesList.push(option);
    this.hourTypesList.sort( (a, b) => { return (a.value - b.value)} );
    this.setHourTypeList();

    //
    // remove for the selection
    const newList = new Array<TimesheetHourTypeRecord>();
    this.timesheetHourTypeList.forEach(element => {
      if(element.hourTypeId != record.hourTypeId) {
        newList.push(element);
      }
    });

    this.timesheetHourTypeList = newList;
  }

  private setHourTypeList() {
   
    if( (!this.hourTypesList) ||
      (this.hourTypesList.length == 0) ) {
      this.hourTypesListControl.value = null;
      this.hourRow = 0;
      return;
    }

    //
    // set first one as default one.
    this.hourTypesListControl.value = this.hourTypesList[0].value;
    this.hourRow = 0;
  }

  private fetch_HourTypes(options: any[], forcedReset: boolean) {
    if(!options) {
      return;
    }

    this.hourTypesListBackup = options;
    this.hourTypesList = options;
    
    if(!this.hourTypesList) {
      this.hourTypesList = [];
      this.hourTypesListBackup = [];
    }

    // Also call this when switching btw persons and dates.
    this.ResetEntryTypeStuffAfterInsertingOrEditingOrCancelling(forcedReset);
  }

  public mapProvinces(res: any){
    let items = res;
    items.forEach(element => {
      if(element.label == "Not Applicable" || element.label == ""){return}
       var province = new Province();
       province.label = element.label;
       province.value = element.value;
       province.country_code = element.country_code;
       this.provinceArray.push(province);
  }
    )
}

  handleCallBack(res: any): ParseResult {
    
    // Handle message
    const object = new ParseResult();
    object.continue = true;
    object.message = this.handleCallBack_Message(res, object);
    return object;
  }

  handleCallBack_Data() {
    // Empty added list
    this.timesheetHourTypeList = [];
    this.hourTypesList = this.hourTypesListBackup;
    this.hourRow = 0;
  }

  handleCallBack_Message(res: any, object: ParseResult): string {
     // Validation checks first.
    if(!res) {
      return res;
    }

    if(! (res.hasOwnProperty('okay') && res.hasOwnProperty('result') && res.hasOwnProperty('list')) ) {
      return res;
    }

    const batchType = res as BatchInsertionResult;
    if(batchType.okay == 1) {
      // okay, no errors
      return batchType.result;
    }

    // 
    // Need to have a new result due to partially successful.
    //
    let s = batchType.result;
    batchType.list.forEach(element => {
      s += `<br/><p>When creating a timesheet record for <b> ${element.hours} ${element.hourType} Hour(s) </b>, we got an error: <b> ${element.error} </b></p>`;
    });

    object.continue = false;
    object.message = s;
    return s;
  }

  //
  // End of Entry Type ...
  //
  
  public onNetsuiteHrSelect(event: any) 
  {
    var s = this.tsLiteTypes.find(x => x.value === this.selectedTsLiteType);
    var nsHourType = new RegExp('RT|DT|OT').exec(s.label);
    if (nsHourType && nsHourType.length) {
      if (nsHourType.includes('RT')) {        
        this.selectedHourType = 1;
      }
      else if (nsHourType.includes('OT')) {
        this.selectedHourType = 2;
      }
      else if (nsHourType.includes('DT')) {
        this.selectedHourType = 3;
      }
    }
    else {
      this.selectedHourType = 1;     
    }
    this.workOrderHourType.disabled = true;
  }

  private jobTypeSelected(event: any) {
    if (this.jobtypecontrol.value) {
      var selectedJob = this.jobTypeInfo.jobTypes.find(x => x.membertype_id === this.jobtypecontrol.value);
      if (selectedJob != undefined && selectedJob.paytypeAllowed != undefined && selectedJob.paytypeAllowed.length > 0) {
        var paytypearray = JSON.parse("[" + selectedJob.paytypeAllowed + "]");

        this.hourTypeFilter = true;
        if (this.workOrderHourType) {
          this.workOrderHourType.filterHourType(paytypearray, this.hourTypeFilter);
        }
        if (this.showJobType) {
          this.setExtrapayments();
        }
      } else {
        // Case no regualr hours setup but the extra hours (travel/mileage) may set up.
        if (this.workOrderHourType) {
          this.workOrderHourType.filterHourType([], true);
        }

        if (this.showJobType) {
          this.setExtrapayments();
        }
      }
    }

    this.fetch_HourTypes(this.workOrderHourType.options, false);
    this.setHourTypeList();
    this.toggleIsPrevailingWageVisibility(true);
  }

  private setExtrapayments() {
    if (this.showJobType) {
      if (this.jobTypeInfo.jobTypes != undefined && this.jobTypeInfo.jobTypes.length > 0) {
        if (this.jobtypecontrol.value) {

          var selectedJob = this.jobTypeInfo.jobTypes.find(x => x.membertype_id === this.jobtypecontrol.value);
          if (selectedJob != undefined && selectedJob.extraPaytypes != undefined ) {
            var extraPaytypes = JSON.parse("[" + selectedJob.extraPaytypes + "]");
            this.workOrderHourType.getExtrapaytype(extraPaytypes);
          }
          this.showMileage = this.workOrderHourType.canApplyMileage;
          this.showTravel = this.workOrderHourType.canApplyTravel;
        }
      }
    }
  }

  private refreshJobtypeAndPaytype(){
    if(this.showJobType){
      this.showMileage=false;
      this.showTravel=false;
     // this.getJobTypeSetting(this.inputValue.userId,this.inputValue.businessUnitId,this.selectedCustomer,this.jti);
     // this.setExtrapayments();
     // this.mileageChk=false;
    //  this.travelChk=false;           
    }
  }

  public isJobTypeSelected() {
    if(!this.showJobType) {
      // User cannot see job type list, return true.
      return true;
    }

    if(!this.jobtypecontrol) {
      return true;
    }

    if ( this.jobtypecontrol.value > 0) {
      return true;
    }

    return false;
  }

  public onIsPrevailingWageChecked(isPrevailingWage){
    this.isPrevailingWage = isPrevailingWage;
  }

  private IsPrevailingWageEnabledForWo(woId: number){
    this.cs.getBoolean(`${CONFIG.apiURL.page.timesheet.IsPrevailingWageEnabledForWo}/${this.inputValue.userId.toString()}/${woId}`)
    .subscribe(res => {
        this.prevailingWageVisible = res;
        this.isPrevailingWage = res;
    });
  }

  private getIsPrevailingWage(woId: number, timesheetId: number){
    this.cs.getBoolean(`${CONFIG.apiURL.page.timesheet.IsPrevailingWage}/${this.inputValue.userId.toString()}/${timesheetId}`)
    .subscribe(res => {
      this.isPrevailingWage = res; 
    
      if(!this.isPrevailingWage){
        this.cs.getBoolean(`${CONFIG.apiURL.page.timesheet.IsPrevailingWageEnabledForWo}/${this.inputValue.userId.toString()}/${woId}`)
        .subscribe(res => {
            this.prevailingWageVisible = res;
        });
      } else {
        this.prevailingWageVisible = true;
      }    
    });
  }

  private toggleIsPrevailingWageVisibility(show: boolean){
    if(!show && this.prevailingWageVisible){

      this.isPrevailingWageOldState = this.isPrevailingWage;
      this.prevailingWageVisibleOldState = this.prevailingWageVisible;
      this.isPrevailingWage = false;
      this.prevailingWageVisible = false;

    }else if(show && !this.prevailingWageVisible){

      if(this.isPrevailingWageOldState !== undefined && this.prevailingWageVisibleOldState !== undefined){
        this.isPrevailingWage = this.isPrevailingWageOldState;
        this.prevailingWageVisible = this.prevailingWageVisibleOldState;
      }
    }
  }
}
