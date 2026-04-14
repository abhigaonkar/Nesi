import { Component, OnInit, ChangeDetectionStrategy, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { DxSelectBoxComponent } from "devextreme-angular";

import { TimesheetService } from '../../../services/pages/timesheet.service';
import { TokenService } from '../../../services/authentication/tokenService';

import {
  TimesheetTransfer,
  TimesheetTransferList,
  ShopTimeTypeRecord,
  TransferTypeRecord,
  TimesheetTransferResult
} from 'models/pages/timesheet/transfer';

import { TimesheetList } from '../../../models/pages/timesheet/timesheet-list';
import { TimesheetWorkOrderCustomer } from '../../../models/pages/timesheet/TimesheetWorkOrderCustomer';
import { TimesheetWorkOrderWo } from '../../../models/pages/timesheet/TimesheetWorkOrderWo';
import { TimesheetValue } from '../../../models/pages/timesheet/timesheetValue';
import { JobTypeInfo, JobTypeRecord, JobTypeRecordQueryParameter } from 'models/pages/timesheet/jobType';
import { JobTypeService } from '../../../services/pages/job-type.service';

import DataSource from 'devextreme/data/data_source';
import ArrayStore from 'devextreme/data/array_store';
import { Province } from 'models/pages/timesheet/provinces';

@Component({
  selector: 'app-timesheet-transfer',
  templateUrl: './timesheet-transfer.component.html',
  styleUrls: ['./timesheet-transfer.component.css'],
})
export class TimesheetTransferComponent implements OnInit {
  @Input()
  records: Array<TimesheetList>;

  @Input()
  timesheetValue: TimesheetValue;

  @Input()
  public jobTypeInfo: JobTypeInfo;
  @Input()
  public showJobType: boolean;

  public _record: TimesheetList = null;

  @Output()
  transferTimesheetEvent = new EventEmitter<TimesheetTransfer>();

  @Output()
  transferTimesheetCloseEvent = new EventEmitter<boolean>();

  @ViewChild('customerNickname') customernickname: DxSelectBoxComponent;
  @ViewChild('workorderNickname') workordernickname: DxSelectBoxComponent;
  @ViewChild('shoptimeNickname') shoptimenickname: DxSelectBoxComponent;
  @ViewChild('jobTypeNickName') jobTypeNickName: DxSelectBoxComponent;
  
  public userform: FormGroup;
  public blockedPanel: boolean = false;

  public customers: Array<TimesheetWorkOrderCustomer>;
  public workorders: Array<TimesheetWorkOrderWo>;
  public shoptimeTypes: Array<ShopTimeTypeRecord>;
  public transferTypes: Array<TransferTypeRecord>;
  public showWhichPart = false; //true: timesheet record / false: Shop
  public timesheetTransferResult: TimesheetTransferResult;
  public show: boolean = false;

  private WoType_Timesheet = 'WO';
  private woType_Shop = 'Shop';

  //
  // For provinces
  //
  provinces: DataSource;
  public provinceArray: Province[];
  //
  // end of provinces
  //

  constructor(
    private fb: FormBuilder,
    private tss: TimesheetService,
    private ts: TokenService,
    private js: JobTypeService
  ) {
    this.provinceArray = [];
    this.customers = [];
    this.workorders = [];
    this.transferTypes = [];
    this.transferTypes.push({ label: 'WO', value: 0 });
    this.transferTypes.push({ label: 'Shop', value: 1 });
    this.createForm();
  }

  ngOnInit() {
    // Validaton checks.
    if (!this.records || this.records.length === 0 || !this.timesheetValue) {
      return;
    }

    this._record = this.records[0];

    if (this._record.woType === this.woType_Shop) {
      this.showWhichPart = false

      this.userform.reset({
        customerId: 0,
        workOrderId: 0,
        jobTypeId: this._record.membertype_id,
        transferType: 1,
        shopTimetypeId: this._record.membertime_shop_type_id,
        comment: this._record.comments
      });
    } else {
      this.showWhichPart = true;

      this.userform.reset({
        customerId: 0,
        workOrderId: 0,
        jobTypeId: this._record.membertype_id,
        transferType: 0,
        comment: this._record.comments,
        province: this._record.prov_id
      });
    }

    if (this._record.woType === this.woType_Shop) {
      //
      // Loading shop time type
      //
      this.getShoptimetypes();
    }
    else {
      //
      // Loading the customers baded on passed in workorder's businessunit info.
      //
      this.getCustomersBasedonSelectedBusinessUnit(this._record.businessUnitId);
      this.getShoptimetypes();
      this.getProvinceList();

      //
      // First time load for job type list if required.
      //
      let customer = this._record.memberTime_Customer_ID;
      this.getJobTypes(customer);
    }
  }

  private getShoptimetypes() {
    this.blockedPanel = true;
    this.tss.ShopTimeTypeList().subscribe(
      data => { this.shoptimeTypes = data; this.blockedPanel = false; },
      error => { this.blockedPanel = false; }
    );
  }

  private getCustomersBasedonSelectedBusinessUnit(businessUnitId: number) {
    this.blockedPanel = true;
    this.tss.WorkOrderCustomer(businessUnitId).subscribe(
      data => { this.customers = data; this.blockedPanel = false; },
      error => { this.blockedPanel = false; }
    );
  }

  private getJobTypes(customerId: number) {
    if (this.showJobType) {
      let bu = this._record.businessUnitId;
      let employee = this._record.membertime_memberid;

      let parameter: JobTypeRecordQueryParameter = new JobTypeRecordQueryParameter();
      parameter.business_uint_id = bu;
      parameter.member_id = employee;
      parameter.customer_id = customerId;

      // Reset list and value.
      this.jobTypeInfo.jobTypes = [];
      if( this.jobTypeNickName) {
        this.jobTypeNickName.value = 0;
      }

      this.js.GetJobTypeInfowithCust(parameter).subscribe(
        (result: any) => {
          if (result) {
            this.jobTypeInfo.jobTypes = result.jobTypes;
            this.jobTypeInfo.showJobType = result.showJobType;

            if(this.jobTypeInfo.jobTypes && this.jobTypeInfo.jobTypes.length > 0) {
              // Setup default value.
              this.jobTypeInfo.jobTypes.forEach(element => {
                if(element.membertype_id == this._record.membertype_id) {
                  this.jobTypeNickName.value = this._record.membertype_id;
                }
              });
            }

          }
        },
        error => {
          console.log(error);
        }
      );
    }
  }

  //
  // For provnice, copy code from workorder
  //
  private getProvinceList() {
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

  public mapProvinces(res: any) {
    let items = res;
    items.forEach(element => {
      if (element.label == "Not Applicable" || element.label == "") { 
        return;
      }
      var province = new Province();
      province.label = element.label;
      province.value = element.value;
      province.country_code = element.country_code;
      this.provinceArray.push(province);
    });
  }

  //
  // end of copy code for province
  //

  private createForm() {
    this.userform = this.fb.group({
      customerId: ['', Validators.required],
      workOrderId: ['', Validators.required],
      transferType: [0, Validators.required],
      jobTypeId: ['', Validators.required],
      shopTimetypeId: ['', Validators.required],

      province: ['', Validators.required],
      comment: ['']
    });
  }

  onValueChanged_transferType(e: any) {
    if (!e) {
      return;
    }

    // Remove all errors
    this.resetErrors();

    const trnsId = e.value;
    if (trnsId === 0) {
      this.showWhichPart = true;

      this.userform.reset({
        customerId: 0,
        workOrderId: 0,
        jobTypeId: this._record.membertype_id,
        transferType: 0,
        comment: this._record.comments,
        province: this._record.prov_id
      });
    } else {
      this.showWhichPart = false

      this.userform.reset({
        customerId: 0,
        workOrderId: 0,
        jobTypeId: this._record.membertype_id,
        transferType: 1,
        shopTimetypeId: 10, // default one
        comment: this._record.comments
      });
    }

  }

  onValueChanged_customer(e: any) {
    if (!e) {
      return;
    }

    const customerId = e.value;
    if (customerId) {
      this.resetErrors();
    }

    this.getWorkordersBasedonSelectedCustomer(customerId);

    //
    // When customer changes, we need to update the job types.
    //
    this.getJobTypes(customerId);
  }

  onValueChanged_workorder(e: any) {
    if (!e) {
      return;
    }

    const woid = e.value;
    if (woid) {
      this.resetErrors();
    }
  }

  onValueChanged_shoptimetype(e: any) {
    if (!e) {
      return;
    }

    const shoptimeTypeId = e.value;
    if (shoptimeTypeId) {
      this.resetErrors();
    }
  }

  private getWorkordersBasedonSelectedCustomer(customerId: number) {
    if (!customerId) {
      // if u remove the customer, then the workorer list also needs to be emptied.
      this.workorders = [];
      return;
    }

    this.blockedPanel = true;

    this.tss.WorkOrderWos(this._record.businessUnitId, this.timesheetValue.userId, customerId).subscribe(
      data => { this.workorders = data; this.blockedPanel = false; },
      error => { this.blockedPanel = false; }
    );
  }

  //
  // When close this page, it will notify outside component.
  //
  closeTimesheetTransferPage() {
    this.transferTimesheetCloseEvent.emit();
  }

  closeTimesheetTransferPageAndRefresh() {
    const t = new TimesheetTransfer();
    this.transferTimesheetEvent.emit(t)
  }

  //
  // let the outside component to do the actural action.
  //
  onSubmit() {
    //
    // Validation first
    //
    const valid = this.validation();
    if (!valid) {
      return;
    }

    //
    // Reset errors
    //
    this.resetErrors();

    //
    // Get all required data
    //
    const data = this.getTransferData();

    //
    // Start transfer
    //
    this.transfer(data);
  }

  private transfer(data: TimesheetTransfer) {
    if (!data) {
      return;
    }

    // Remove the previous result.
    this.timesheetTransferResult = null;
    this.show = true;

    // Block the ui so no more clicks.
    this.blockedPanel = true;

    this.updatePanelStatus('beforeupdate');
    this.tss.TransferTimesheet(data).subscribe(
      data => { this.transfer_okay(data); },
      error => { this.transfer_error(error); }
    );
  }

  private transfer_okay(result: TimesheetTransferResult) {
    console.log(result);
    this.blockedPanel = false;
    if (result.okay) {
      this.transfer_okay_business_Okay(result);
    } else {
      this.transfer_okay_business_Failed(result);
    }
  }

  private transfer_okay_business_Okay(result: TimesheetTransferResult) {
    this.updatePanelStatus('okay');
    this.timesheetTransferResult = result;
    this.show = false;
  }

  private transfer_okay_business_Failed(result: TimesheetTransferResult) {
    this.updatePanelStatus('failed');
    this.timesheetTransferResult = result;
    this.show = false;
  }

  private transfer_error(result: any) {
    console.log(result);
    this.blockedPanel = false;
    this.updatePanelStatus('failed');
    this.show = false;
  }

  private validation() {
    this.resetErrors();
    if (!this.records) {
      return false;
    }

    if (this.records.length == 0) {
      return false;
    }

    let error = true;
    if (this.showWhichPart) {
      // Validation for timesheet records
      if (!this.customerId.value || this.customerId.value == 0) {
        this.customerIdError = true;
        this.customerErrorInfo = "Customer is required."
        error = false;
        return error;
      }

      if (!this.workOrderId.value || this.workOrderId.value == 0) {
        this.workorderError = true;
        this.workorerErrorInfo = "Work order is required."
        error = false;
        return error;
      }

      if(!this.provinceInfo.value || this.provinceInfo.value == 0) {
        this.provinceError = true;
        this.provinceErrorInfo = "Province is required.";
        error = false;
        return error;
      }

    } else {
      // Validation for shop time.
      if (!this.shopTimetypeId.value || this.shopTimetypeId.value == 0) {
        this.shopTimeTypeError = true;
        this.shopTimeTypeErrorInfo = "Shop Time Type is required."
        error = false;
        return error;
      }
    }

    if (!this.comments.value || this.comments.value.length <= 10) {
      this.commentsError = true;
      this.CommentsErrorInfo = "Please enter a detailed comment greater than 10 characters.";
      error = false;
      return error;
    }

    if (this.jobTypeInfo.showJobType) {
      if( this.transferTypeId.value == 0) {
        // for wo-> wo transfer
        if(!this.jobTypeId.value) {
          this.jobTypeError = true;
          this.jobTypeErrorInfo = "No job types available for selected customer.";
          error = false;
          return error;
        }
      }
    }
    return error;
  }

  private getTransferData(): TimesheetTransfer {
    const data = new TimesheetTransfer();

    // Origional info alwyas is from outside.
    data.originalTimesheetInfo = <TimesheetTransferList>this.records[0];
    data.timesheetValue = this.timesheetValue;
    data.timesheetValue.allow_jobtype_selection = this.jobTypeInfo.showJobType;

    // target info from the UI.

    // Timesheet record part:
    data.targetTimesheetInfo = new TimesheetTransferList();
    data.targetTimesheetInfo.custId = this.customerId.value;
    data.targetTimesheetInfo.workorderId = this.workOrderId.value;
    data.targetTimesheetInfo.membertime_workorder_id = data.targetTimesheetInfo.workorderId;
    data.targetTimesheetInfo.membertype_id = this.jobTypeId.value;
    data.targetTimesheetInfo.woTypeId = this.transferTypeId.value;
    data.targetTimesheetInfo.comments = this.comments.value;

    if (data.targetTimesheetInfo.woTypeId == 0) {
      data.targetTimesheetInfo.transfer_type = 'WO';
      data.targetTimesheetInfo.membertime_customer_name = this.customernickname.text;
      data.targetTimesheetInfo.scope_name = this.workordernickname.text; // borrow this field to pass the work order name.
      data.targetTimesheetInfo.prov_id = this.provinceInfo.value;
    } else {
      data.targetTimesheetInfo.transfer_type = 'Shop';
      data.targetTimesheetInfo.membertime_shop_type_id = this.shopTimetypeId.value;
      data.targetTimesheetInfo.scope_name = this.shoptimenickname.text; // borrow this field to pass the work order name.
    }

    // Shop time part:
    data.targetTimesheetInfo.membertime_shop_type_id = this.shopTimetypeId.value;

    return data;
  }

  //
  // Get helper
  //
  get customerId() { return this.userform.get('customerId'); }
  get workOrderId() { return this.userform.get('workOrderId'); }
  get jobTypeId() { return this.userform.get('jobTypeId'); }
  get shopTimetypeId() { return this.userform.get('shopTimetypeId'); }
  get transferTypeId() { return this.userform.get('transferType'); }
  get comments() { return this.userform.get('comment'); }
  get provinceInfo() { return this.userform.get('province'); }

  private resetErrors() {
    this.customerIdError = false;
    this.customerErrorInfo = "";

    this.workorderError = false;
    this.workorerErrorInfo = "";

    this.shopTimeTypeError = false;
    this.shopTimeTypeErrorInfo = "";

    this.commentsError = false;
    this.CommentsErrorInfo = "";

    this.jobTypeError = false;
    this.jobTypeErrorInfo = "";

    this.provinceError = false;
    this.provinceErrorInfo = "";
  }

  public customerIdError = false;
  public customerErrorInfo = "";

  public workorderError = false;
  public workorerErrorInfo = "";

  public shopTimeTypeError = false;
  public shopTimeTypeErrorInfo = "";

  public jobTypeError = false;
  public jobTypeErrorInfo = "";

  public commentsError = false;
  public CommentsErrorInfo = "";

  public provinceError = false;
  public provinceErrorInfo = "";

  //
  // Panel controls
  //

  // Start point.
  public collapse4Original = false; //show
  public collapse4Target = false; // show
  public collapse4Result = true; // hide
  public disabledForButtons = false;

  private updatePanelStatus(s: string) {
    if ('okay' == s) {
      this.collapse4Original = true; //hide
      this.collapse4Target = false; // show
      this.collapse4Result = false; // show
      this.disabledForButtons = true;

      this.userform.disable(); // disabled, so nobody can edit this.
    } else if ('failed' == s) {
      this.collapse4Original = true; //hide
      this.collapse4Target = false; //show
      this.collapse4Result = false; // show

    } else if ('beforeupdate' == s) {
      this.collapse4Original = true; //hide
      this.collapse4Target = false; // show
      this.collapse4Result = false; // show
    }
  }

}
