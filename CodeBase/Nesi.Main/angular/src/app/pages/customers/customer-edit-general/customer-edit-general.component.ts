
import { Component, OnInit } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';
import { LabelValue } from 'app/models/component/filterBuilder/filterBuilder';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { WindowRef } from '../../../services/shared/windowRef';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-edit-general',
  templateUrl: './customer-edit-general.component.html',
  styleUrls: ['./customer-edit-general.component.css']
})
export class CustomerEditGeneralComponent extends CustomerFormBase implements OnInit {

  selected_addresses: any[];
  all_addresses: any[];
  all_addresses_list: any[];
  addresses: any[];

  activeIndex = -1;
  activeIndex_addr = -1;
  activeIndex_contact_list = -1;
  activeIndex_sales = -1;
  activeIndex_assets = -1;
  activeIndex_phonenumbers = -1;
  activeIndex_workorders = -1;
  activeIndex_quotes = -1;
  activeIndex_activity = -1;
  activeIndex_phone_list = -1;
  activeIndex_customer_business_unit = -1;
  display_why_hold = false;
  why_hold: string;
  disabled:boolean=false;
   
  

  status = '';

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private cf: ConfirmationService,
    protected winRef: WindowRef
  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.edit.save,
      CONFIG.apiURL.page.customers.edit.profile
    );
  }
  
  public changeQC(type: number) {
    if (type > 2) {
      this.cf.confirm({
        message: `Are you sure you want to remove QC${type - 2} on this customer?`,
        accept: () => this.change_QC(type),
      });
    } else {
      this.change_QC(type);
    }
  }

  createForm() {
    this.userform = this.fb.group({
      'business_unit': [this.ts.currentUser.businessUnitId, [Validators.required]],
      'project_manager_id': ['', [Validators.required, Validators.min(1)]],
      'account_manager_id': ['', [Validators.required, Validators.min(1)]],
      'reg_account_manager_id': ['', [Validators.required, Validators.min(1)]],
      'isr': ['', [Validators.required, Validators.min(1)]],
      'osr': ['', [Validators.required, Validators.min(1)]],
      'status_id': [{value: '', disabled: true}, [Validators.required, Validators.min(1)]],
      'customer_id': ['', [Validators.required, Validators.min(1)]],
      'is_partner': {value: 'false', disabled: true},
      'is_on_hold': {value: 'false'},
      'why_hold': '',
      'customer_name': [{ value: '', disabled: true }, [Validators.required, Validators.maxLength(60)]],
    });
  }

  AfterProfileLoaded() {
    if (!this.profile) {
      return;
    }
    this.customer_base_profile = this.profile;
    if (this.profile.customer_id <= 0) {
      this.initFormvalue = this.profile.profile;
    } else {
      this.initFormvalue = null;
      this.userform.patchValue(this.profile.profile);
      this.all_addresses = this.profile.profile.addresses;
      this.getAddresses();
    }
    if (!this.profile.is_edit_allowed) {
      this.edit_disabled = true;
      this.userform.disable();
    }
    if (!this.profile.profile.account_manager_id) {
      this.userform.get('account_manager_id').setValue(999999);
    }
    if (!this.profile.is_edit_accountmgr_allowed) {
      this.userform.get('account_manager_id').disable();
    }
    if (!this.profile.is_admin_authenticated) {
      this.userform.get('account_manager_id').disable();
      this.userform.get('customer_name').disable();
      this.userform.get('project_manager_id').disable();
      this.userform.get('is_on_hold').disable();
    }

    if (this.userform.get('status_id').value === 4) {
      this.userform.get('status_id').disable();
    } else {
      this.profile.statusList = this.profile.statusList.filter(x => x.value !== 4);
    }

    if (this.userform.get('status_id').value === 5|| this.userform.get('status_id').value === 0) {
      this.disabled=true;
      //CONFIG.LOG('event.index', event.index);
    }
    CONFIG.LOG('status_id', this.userform.get('status_id').value);
    const s = this.profile.statusList.filter(x => x.value ===  this.profile.profile.status_id);
    if (s && s[0] && s[0].label) {
      this.status = s[0].label;
    }

  }



  getAddresses() {
    this.all_addresses_list = this.all_addresses.map(x => {
      return {
        value: x.address_id,
        label: '[' + x.addr_type + '] ' + x.addr,
      }
    });
    this.selected_addresses = this.all_addresses_list.map(x => x.value);
    this.addresses = this.all_addresses_list.filter(x => this.selected_addresses.includes(x.value));
  }

  addressUpdated(event) {
    this.all_addresses = event.result;
    this.getAddresses();
    this.activeIndex = -1;
    setTimeout(() => {
      this.activeIndex = 1;
    }, 500);
    // CONFIG.LOG(event.result.length, 'address changed in edit general');
  }

  public onAccordionOpen(event) {
    this.activeIndex = event.index;
    CONFIG.LOG('event.index', event.index);
    if (this.addresses) {
      switch (event.index) {
        case 0: // accounting settings
          break;
        case 1: // address
            this.onAccordionOpen_addr({ index: 0 }, false);
          break;
        case 2: // contacts
          this.onAccordionOpen_contact_list({ index: 0 }, false);
          break;
        case 3: // phone numbers
          this.onAccordionOpen_phone_list({ index: 0 }, false);
          break;
        case 4: // sales settings
          this.onAccordionOpen_sales({ index: 0 }, false);
          break;
        case 5: // work orders
          this.onAccordionOpen_workorders({ index: 0 }, false);
          break;
        case 6: // quotes
          this.onAccordionOpen_quotes({ index: 0 }, false);
          break;
        case 7: // notes
          this.onAccordionOpen_activity({ index: 0 }, false);
          break;
        case 8: // phone numbers
          this.onAccordionOpen_assets({ index: 0 }, false);
          break;
         case 9: // customer business unit
          this.onAccordionCustomer_business_unit({ index: 0 }, false);
          break;
      }
    }
  }

  public onAccordionOpen_addr(event, force = true) {
    if (force) {
      this.activeIndex_addr = event.index;
    }
    this.activeIndex_addr = -1;
    setTimeout(() => {
      this.activeIndex_addr = event.index;
    }, 500);
  }
  public onAccordionOpen_contact_list(event, force = true) {
    if (force) {
      this.activeIndex_contact_list = event.index;
    }
    this.activeIndex_contact_list = -1;
    setTimeout(() => {
      this.activeIndex_contact_list = event.index;
    }, 500);
  }
  public onAccordionOpen_sales(event, force = true) {
    if (force) {
      this.activeIndex_sales = event.index;
    }
    this.activeIndex_sales = -1;
    setTimeout(() => {
      this.activeIndex_sales = event.index;
    }, 500);
  }

  public onAccordionOpen_assets(event, force = true) {
    if (force) {
      this.activeIndex_assets = event.index;
    }
    this.activeIndex_assets = -1;
    setTimeout(() => {
      this.activeIndex_assets = event.index;
    }, 500);
  }

  public onAccordionOpen_phonenumbers(event, force = true) {
    if (force) {
      this.activeIndex_phonenumbers = event.index;
    }
    this.activeIndex_phonenumbers = -1;
    setTimeout(() => {
      this.activeIndex_phonenumbers = event.index;
    }, 500);
  }
  public onAccordionOpen_workorders(event, force = true) {
    if (force) {
      this.activeIndex_workorders = event.index;
    }
    this.activeIndex_workorders = -1;
    setTimeout(() => {
      this.activeIndex_workorders = event.index;
    }, 500);
  }
 public onAccordionCustomer_business_unit(event, force = true) {
    if (force) {
      this.activeIndex_customer_business_unit = event.index;
    }
    this.activeIndex_customer_business_unit = -1;
    setTimeout(() => {
      this.activeIndex_customer_business_unit = event.index;
    }, 500);
  }
  public onAccordionOpen_quotes(event, force = true) {
    if (force) {
      this.activeIndex_quotes = event.index;
    }
    this.activeIndex_quotes = -1;
    setTimeout(() => {
      this.activeIndex_quotes = event.index;
    }, 500);
  }
  public onAccordionOpen_activity(event, force = true) {
    if (force) {
      this.activeIndex_activity = event.index;
    }
    this.activeIndex_activity = -1;
    setTimeout(() => {
      this.activeIndex_activity = event.index;
    }, 500);
  }
  public onAccordionOpen_phone_list(event, force = true) {
    if (force) {
      this.activeIndex_phone_list = event.index;
    }
    this.activeIndex_phone_list = -1;
    setTimeout(() => {
      this.activeIndex_phone_list = event.index;
    }, 500);
  }


  public change_QC(type: number) {
    this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.customers.edit.changeQC), {
      data: type
    }).subscribe(
      (res) => {
        if (this.CheckResponseMessage(res.data)) {
          switch (type) {
            case 1:
            case 3:
              this.profile.profile.qc_dt1_text = res.extra;
              break;
            case 2:
            case 4:
              this.profile.profile.qc_dt2_text = res.extra;
              break;
          }
        }
      }
    );
  }

  submitValidate() {
    if (this.userform.get('is_on_hold') && this.userform.get('is_on_hold').value && !this.userform.get('why_hold').value) {
      return false;
    }
    if (!(this.userform.get('is_on_hold') && this.userform.get('is_on_hold').value)) {
      this.userform.get('why_hold').setValue('');
    }
    return true;
  }

  submitSuccess() {
    this.display_why_hold = false;
    this.profile.profile.is_on_hold = this.extraData.is_on_hold;
    this.profile.profile.why_hold = this.extraData.why_hold;
  }
  public handle_customer_request(){
    const url = CONFIG.Nesi1URL.requestCustOrVendor.replace('@type', '1').replace('@customer_id',this._customer_id.toString());
    this.winRef.boingNesi1(url ,'customerRequest'+Math.random(),"500,700");
  }

}
