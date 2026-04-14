import { Component, OnInit, Input } from '@angular/core';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { TokenService } from 'app/services/authentication/tokenService';
import {  CustomerTravelMileageRateService } from 'app/services/pages/customer-travel-mileage.service';
import { GetNewLaborRateListParameter, GenericLaborRateRecord, UpdateParameter, DeleteParameter, UpdateResult }
    from 'models/pages/customer/customer-travel-mileage';

import { MessageBase } from 'app/core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { Observable } from 'rxjs';
import * as fromMessage from '../../../actions/layout/growlMessage';
import * as DATE from '../../../services/helper/datetime';

@Component({
  selector: 'nesi-customer-accounting-rates',
  templateUrl: './customer-accounting-rates.component.html',
  styleUrls: ['./customer-accounting-rates.component.css']
})
export class CustomerAccountingRatesComponent extends MessageBase implements OnInit {
  @Input() address_id = 0;
  @Input() customer_id: number;
  @Input() customer_base_profile: any;
  @Input() disabled:boolean;
  is_allowed_to_switch_chargeout_bu: boolean;
  selected_business_unit_id: number;
  business_unit_list: LabelValueInt[];
  table_data: any[];
  submitting = false;

  getUrl(buId: number = 0, u = null): string {
    if (!u) {
      u = CONFIG.apiURL.page.customers.edit.accountingRates;
    } else if (u === 'delete') {
      u = CONFIG.apiURL.page.customers.edit.accountingRatesDelete;
    }
    if (this.customer_id) {
      u = u.replace('$customer_id', this.customer_id.toString())
        .replace('$address_id', this.address_id.toString());
    }
    if (buId > 0) {
      u = u + '/' + buId.toString();
    }
    return u;
  }

  constructor(
    private cs: CoreService,
    protected store: Store<fromRoot.State>,
    private ts: TokenService,
    private ctm: CustomerTravelMileageRateService
  ) {
    super(store);
  }

  ngOnInit() {
    this.loadProfile();
  }

  bussiness_unitChanged(event) {

    // When bu switched, we need to know the override setting.
    this.getOverridedSetting(this.selected_business_unit_id);
    
    this.loadTableData();

    // load the default value.
    this.loadTravelMileageRates();
  }

  loadProfile() {
    if (!this.customer_id) {
      return;
    }
    this.submitting = true;
    this.cs.getObject<any>(this.getUrl())
      .subscribe(
        (res) => {
          this.submitting = false;
          this.business_unit_list = res.business_unit_list;
          this.selected_business_unit_id = res.selected_business_unit_id;
          this.table_data = res.table_data;

          this.allow_rate_modifications = res.allow_rate_modifications;
          this.is_allowed_to_switch_chargeout_bu = res.is_allowed_to_switch_chargeout_bu;  
          // When bu switched, we need to know the override setting.
          this.getOverridedSetting(this.selected_business_unit_id);

          // load the default value.
          this.loadTravelMileageRates();
        }
      );
  }

  loadTableData() {
    if (!this.customer_id || this.selected_business_unit_id === 0) {
      return;
    }
    this.submitting = true;
    this.cs.getObject<any>(this.getUrl(this.selected_business_unit_id))
      .subscribe(
        (res) => {
          this.submitting = false;
          this.table_data = res;
        }
      );
  }

  saveRate(item) {
    this.submitting = true;
    item.address_id = this.address_id;
    item.customer_id = this.customer_id;
    item.overrideflag = false;

    //
    // We need to recheck the ui again: We can override first, then use the old button, so by this we always have a correct value.
    //
    this.regChanged(item);

    this.cs.postDataExtra(this.getUrl(this.selected_business_unit_id), item).subscribe((res) => {
      if (this.PushResponseMessage(res.data)) {
        this.submitting = false;
        item.last_updated = res.extra;
      }
    }
    );
  }

  deleteRate(item) {
    this.submitting = true;
    item.address_id = this.address_id;
    item.customer_id = this.customer_id;
    this.cs.postDataExtra(this.getUrl(this.selected_business_unit_id, 'delete'), item).subscribe((res) => {
      if (this.PushResponseMessage(res.data)) {
        this.submitting = false;
        this.table_data = res.extra;
      }
    }
    );
  }
  regChanged(item) {
    if (item.reg) {
      const reg = Number(item.reg);
      if (!reg || isNaN(reg)) {
        item.has_error = true;
        return;
      }
      item.has_error = false;
      item.ot = this.round(reg * 1.5, 2);
      item.dt = this.round(reg * 2, 2);
      item.regsp = this.round(reg * 1.1, 2);
      item.otsp = this.round(reg * 1.65, 2);
      item.dtsp = this.round(reg * 2.2, 2);
    }
  }


  //
  // ***********************************************************************
  //
  // For customer travel / mileage rate
  //
  @Input() showTravelMileageUI = false;

  public totalRecordsInfo = "";
  public GenericLaborRateRecord_forTravel: GenericLaborRateRecord;
  public GenericLaborRateRecord_forMileage: GenericLaborRateRecord;
  public showErrorInfo = false;
  public errorInfo = "";
  public blockedui = false;

  isPopupVisible: boolean = false;
  selectedRecord: GenericLaborRateRecord = null;
  operation = 0; // 1: save /2 : delete
  confirmationInfo = "";


  private loadTravelMileageRates() {

    // Reset error info.
    this.showErrorInfo = false;
    this.errorInfo = "";
    this.totalRecordsInfo = "";

    this.GenericLaborRateRecord_forMileage = new GenericLaborRateRecord();
    this.GenericLaborRateRecord_forTravel = new GenericLaborRateRecord();

    if(!this.showTravelMileageUI) {
      return;
    }

    var parameter = new GetNewLaborRateListParameter();
    parameter.customerId = this.customer_id;
    parameter.businessUnitId = this.selected_business_unit_id;

    this.blockedui = true;

    this.ctm.GetTravelMileageRates(parameter).subscribe(
        res => {this.populateRecords(res);},
        error => {console.log(error);this.blockedui = false;}
      );
  }

  private populateRecords(res: Array<GenericLaborRateRecord>) {
    this.blockedui = false;
    if (!res) {
      return;
    }

    this.totalRecordsInfo = ` ( Found ${res.length} chargeout records )`;

    if (res.length <= 0) {
      return;
    }

    if (res.length == 2) {
      if (res[0].paytype_id === 7) {
        this.GenericLaborRateRecord_forMileage = res[1];
        this.GenericLaborRateRecord_forTravel = res[0];
      } else {
        this.GenericLaborRateRecord_forMileage = res[0];
        this.GenericLaborRateRecord_forTravel = res[1];
      }
    }

    if(res.length == 1) {
      this.GenericLaborRateRecord_forTravel = res[0]; // maybe travel/mileage.
    }
  }

  public saveTrvalOrMileageRateEventEnter(record: GenericLaborRateRecord) {
    const validation = this.validation4Update(record);
    if(!validation)
    {
      return;
    }

    this.confirmationInfo = `Do you want to save the customer rate?`;
    this.selectedRecord = record;
    this.operation = 1;

    //
    // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/2063/
    // Remove confirmation popup.
    //
    // this.isPopupVisible = true;
    this.ActionForSaveOrUpdate();
    //
    // eno of removing.
    //   
  }

  public saveTrvalOrMileageRate(record: GenericLaborRateRecord) {
    //
    // validaton
    //
    const validation = this.validation4Update(record);
    if(!validation)
    {
      return;
    }

    //
    // Get all parameters
    //
    const params= new UpdateParameter();
    params.business_unit = this.selected_business_unit_id;
    params.action = 1; // set to 1
    params.chargeout = record.chargeout;
    params.customer_id = this.customer_id;
    params.from = record.from;
    params.to = record.to;
    params.paytype_id = record.paytype_id;
    params.paytype = record.paytype;

    this.ctm.UpdateTraveMileageRates(params).subscribe(
      r => {this.handleOkay(r)},
      e => {this.handleError(e);}
    );
  }

  public deleteTrvalOrMileageRateEvent(record: GenericLaborRateRecord) {
    this.confirmationInfo = `Do you want to reset the customer rate to default price?`;
    this.selectedRecord = record;
    this.operation = 2; // delete

    //
    // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/2063/
    // Remove confirmation popup.
    //
    //this.isPopupVisible = true;
    this.ActionForSaveOrUpdate();
    //
    // eno of removing.
    //
  }

  public deleteTrvalOrMileageRate(record: GenericLaborRateRecord) {
    //
    // Get all parameters
    //
    const params= new DeleteParameter();
    params.business_unit = this.selected_business_unit_id;
    params.action = 2; // set to 2
    params.chargeout = record.chargeout;
    params.customer_id = this.customer_id;
    params.from = record.from;
    params.to = record.to;
    params.paytype_id = record.paytype_id;
    params.paytype = record.paytype;

    this.ctm.DeleteTraveMileageRates(params).subscribe(
      r => {this.handleOkay(r); this.loadTravelMileageRates(); },
      e => {this.handleError(e);}
    );
  }

  private handleOkay(r: UpdateResult) {
    if(!r) {
      this.store.dispatch(new fromMessage.PushWarnMessage('No result from server side - no response.'));
      return;
    }

    const reslut = (<any>r).data ;
    if(!reslut) {
      this.store.dispatch(new fromMessage.PushWarnMessage('No result from server side - decode error.'));
      console.log(reslut);
      return;
    }

    if(reslut.okay) {
      this.handleOkay_LogicOKay(reslut);
    } else {
      this.handleOkay_LogicWrong(reslut);
    }
  }

  private handleOkay_LogicOKay(r: UpdateResult) {
    this.store.dispatch(new fromMessage.PushSuccessMessage(r.error));
  }

  private handleOkay_LogicWrong(r: UpdateResult) {
    this.showErrorInfo = true;
    this.errorInfo =  r.error;
    this.store.dispatch(new fromMessage.PushErrorMessage(r.error));
  }

  private handleError(e: any) {
    console.log(e);
  }

  private validation4Update(record: GenericLaborRateRecord): boolean {

    // Reset error info.
    this.showErrorInfo = false;
    this.errorInfo = "";

    if(!record) {
      this.showErrorInfo = true;
      this.errorInfo = "No valid inputs.";
      return false;
    }

    //
    // Check the chargeout
    // 
    if(record.chargeout <= 0) {
      this.showErrorInfo = true;
      this.errorInfo = "The chargeout cannot be zero.";
      return false;
    }

    //
    // check the from & to
    //
    //let check1 = !record.from && !record.to;
    let check2 = record.from && record.to && this.fromShouldLessThenTo(record.from, record.to);
    if( !( check2)) {
      this.showErrorInfo = true;
      this.errorInfo = " Please enter a valid date range. The From date must precede the To date.";
      return false;
    }

    return true;
  }

  //
  // We need to convert to same format before comparing.
  //
  private fromShouldLessThenTo(from: Date, to: Date) {
    let newfrom = DATE.ToyyyyMMdd(from);
    let newTo = DATE.ToyyyyMMdd(to);
    let check = newfrom < newTo;

    return check;
  }

  // Confirmation

  ConfirmYes() {
    this.isPopupVisible = false;
    setTimeout(() => {
      this.ActionForSaveOrUpdate();    
    }, 0);
  }

  private ActionForSaveOrUpdate() {
    if(this.operation == 1) {
      this.saveTrvalOrMileageRate(this.selectedRecord);
    }

    if(this.operation == 2) {
      this.deleteTrvalOrMileageRate(this.selectedRecord);
    }
  }

  ConfirmNo(){
    this.selectedRecord = null;
    this.isPopupVisible = false;
  }

  //
  // End fo travel
  //

  //
  // For new Save Rates button
  //
  isPopupVisible4Rates = false;
  selectedRateRecord = null;
  backupRecord = null;

  chargeOutError = false;
  chargeOutErrorInfo = "";

  allow_rate_modifications: false;

  saveNewRate(item) {
    this.isPopupVisible4Rates = true;
    this.selectedRateRecord = item;
    this.selectedRateRecord.overrideflag = false;

    this.backupRecord = Object.assign({}, item);
  }

  ConfirmNo4Rates(){
    this.isPopupVisible4Rates = false;

    // Recove if cancel after editing.
    (<any>this.selectedRateRecord).reg = this.backupRecord.reg;
    (<any>this.selectedRateRecord).ot = this.backupRecord.ot;
    (<any>this.selectedRateRecord).dt = this.backupRecord.dt;
    (<any>this.selectedRateRecord).regsp = this.backupRecord.regsp;
    (<any>this.selectedRateRecord).otsp = this.backupRecord.otsp;
    (<any>this.selectedRateRecord).dtsp = this.backupRecord.dtsp;
    this.selectedRateRecord.overrideflag = false;
    this.selectedRateRecord = null;

    this.chargeOutError = false;
    this.chargeOutErrorInfo = "";
  }

  ConfirmYes4Rates() {
    this.NewRateUpdate();
  }

  private NewRateUpdate() {
    //
    // Bring customer info
    //
    (<any>this.selectedRateRecord).business_unit = this.selected_business_unit_id;
    (<any>this.selectedRateRecord).customer_id = this.customer_id;
    (<any>this.selectedRateRecord).address_id  = this.address_id;

    // Validation check first.
    if(!this.NewRateUpdate_Validation()) {
      return;
    }

    // Send the request to server.
    this.submitting = true;
    this.selectedRateRecord.overrideflag = true;
    this.cs.postDataExtra(this.getUrl(this.selected_business_unit_id), this.selectedRateRecord).subscribe(
      (res) => {
        if (this.PushResponseMessage(res.data)) {
          this.submitting = false;
          this.selectedRateRecord.overrideflag = false;
          (<any>this.selectedRateRecord).last_updated = res.extra;
        }
      },

      err => {
        this.selectedRateRecord.overrideflag = false;
        console.log(err);
      }

    );

    // Hide the pop up if no errors after submitting
    this.isPopupVisible4Rates = false;
  }

  private NewRateUpdate_Validation() {

    this.chargeOutError = false;
    this.chargeOutErrorInfo = "";

    if((<any>this.selectedRateRecord).reg <= 0) {
      this.chargeOutError = true;
      this.chargeOutErrorInfo = " Reg is invalid.";
      return false;
    }

    if((<any>this.selectedRateRecord).ot <= 0) {
      this.chargeOutError = true;
      this.chargeOutErrorInfo = " OT is invalid.";
      return false;
    }

    if((<any>this.selectedRateRecord).dt <= 0) {
      this.chargeOutError = true;
      this.chargeOutErrorInfo = " DT is invalid.";
      return false;
    }

    if((<any>this.selectedRateRecord).regsp <= 0) {
      this.chargeOutError = true;
      this.chargeOutErrorInfo = " SP is invalid.";
      return false;
    }

    if((<any>this.selectedRateRecord).otsp <= 0) {
      this.chargeOutError = true;
      this.chargeOutErrorInfo = " OT SP is invalid.";
      return false;
    }

    if((<any>this.selectedRateRecord).dtsp <= 0) {
      this.chargeOutError = true;
      this.chargeOutErrorInfo = " DT SP is invalid.";
      return false;
    }

    return true;
  }

  public getOverridedSetting(business_unit_id: number) {
    this.cs.getObject<any>( CONFIG.apiURL.page.customers.edit.accountingRatesOverrideSetting + business_unit_id.toString())
    .subscribe(
      (res) => {
        this.allow_rate_modifications = res.allow_rate_modifications;
      },
      err => {
        this.allow_rate_modifications =  false;
      }
    );
  }
  //
  // End of new Save Rates button
  //
}
