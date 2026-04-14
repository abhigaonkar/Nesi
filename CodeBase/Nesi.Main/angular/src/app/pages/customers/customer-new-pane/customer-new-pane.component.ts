
import { Component, OnInit } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-new-pane',
  templateUrl: './customer-new-pane.component.html',
  styleUrls: ['./customer-new-pane.component.css']
})
export class CustomerNewPaneComponent extends FormMessageBase implements OnInit {
  profile: any;
  customer_name_exist = false;
  customer_exist_id: number;
  customer_phone_exist = false;
  customer_phone_exist_id: number;
  customer_phone_exist_name: string;

  is_admin_authenticated: boolean;

  is_edit_accountmgr_allowed: boolean;
  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.customers.new.save);

  }
  ngOnInit() {
    this.cs.getObject<any>(CONFIG.apiURL.page.customers.new.profile)
      .subscribe(
        (res) => {
          this.profile = res;
          this.submitReset();
        }
      );
  }

  submitReset() {
    super.submitReset();
    if (this.profile) {
      this.setBusinessUnitProfile(this.profile.businessUnitDefaultProfile);
      if (!this.profile.is_admin_authenticated) {
        this.userform.get('tax_1').setValue(999999);
        this.userform.get('tax_1').disable();
        this.userform.get('tax_2').setValue(999999);
        this.userform.get('tax_2').disable();
        this.userform.get('tax_3').setValue(999999);
        this.userform.get('tax_3').disable();
        this.userform.get('tax_4').setValue(999999);
        this.userform.get('tax_4').disable();
      }
      if (!this.profile.is_edit_accountmgr_allowed) {
        this.userform.get('account_manager_id').setValue(999999);
        this.userform.get('account_manager_id').disable();
      }
      if (this.profile.glReceiveList && this.profile.glReceiveList.length > 0) {
        this.userform.get('gl_receivalbes').setValue(this.profile.glReceiveList[0].value);
      }
    }
  }

  createForm() {
    this.userform = this.fb.group({
      'business_unit': [this.ts.currentUser.businessUnitId, [Validators.required]],
      'project_manager_id': ['', [Validators.required]],
      'account_manager_id': ['', [Validators.required]],
      'origin_id': ['', [Validators.required]],
      'customer_name': ['', [Validators.required, Validators.maxLength(60)]],
      'address_line_1': ['', [Validators.required, Validators.maxLength(200)]],
      'address_line_2': ['', [Validators.maxLength(200)]],
      'address_line_3': ['', [Validators.maxLength(200)]],
      'address_line_4': ['', [Validators.maxLength(200)]],
      'address_city': ['', [Validators.required, Validators.maxLength(50)]],
      'address_prov': ['', [Validators.required]],
      'address_postalcode': ['', [Validators.required, Validators.maxLength(10)]],
      'address_country': ['', [Validators.required]],
      'phone_area': ['', [Validators.required, Validators.maxLength(3), Validators.minLength(3)]],
      'phone_prefix': ['', [Validators.required, Validators.maxLength(3), Validators.minLength(3)]],
      'phone_suffix': ['', [Validators.required, Validators.maxLength(4), Validators.minLength(4)]],
      'phone_ext': ['', [Validators.maxLength(5)]],
      'fax_area': ['', [Validators.maxLength(3)]],
      'fax_prefix': ['', [Validators.maxLength(3)]],
      'fax_suffix': ['', [Validators.maxLength(4)]],
      'ap_email': ['call4APEmail@newelectric.com', [Validators.required, Validators.email, Validators.maxLength(200)]],
      'gl_receivalbes': ['', [Validators.required]],
      'tax_1': 0,
      'tax_2': 0,
      'tax_3': 0,
      'tax_4': 0,
    });

    this.initFormvalue = {
      'business_unit': this.ts.currentUser.businessUnitId,
      'project_manager_id': '',
      'account_manager_id': '',
      'origin_id': '',
      'customer_name': '',
      'address_line_1': '',
      'address_line_2': '',
      'address_line_3': '',
      'address_line_4': '',
      'address_city': '',
      'address_prov': '',
      'address_postalcode': '',
      'address_country': '',
      'phone_area': '',
      'phone_prefix': '',
      'phone_suffix': '',
      'phone_ext': '',
      'fax_area': '',
      'fax_prefix': '',
      'fax_suffix': '',
      'ap_email': 'call4APEmail@newelectric.com',
      'gl_receivalbes': '',
      'tax_1': 0,
      'tax_2': 0,
      'tax_3': 0,
      'tax_4': 0,
    }
  }

  businessUnit_Changed(event) {
    this.loadBusinessUnitProfile(event.value);
  }

  loadBusinessUnitProfile(buId: number) {
    this.cs.getObject<any>(CONFIG.apiURL.page.customers.new.businessUnitPorifle + buId.toString())
      .subscribe(
        (res) => {
          this.setBusinessUnitProfile(res);
        }
      );
  }

  setBusinessUnitProfile(res: any) {
    this.profile.projectManagerList = res.projectManagerList;
    this.profile.glReceiveList = res.glReceiveList;
    this.userform.get('address_country').setValue(res.country);
    this.userform.get('address_prov').setValue(res.provstate);
  }

  checkCustomerName() {
    this.customer_name_exist = false;
    this.customer_exist_id = 0;
    const value = this.userform.get('customer_name').value;
    if (!value) {
      return;
    }
    const postData = { data: value };
    this.cs.postDataExtra(CONFIG.apiURL.page.customers.new.checkName, postData)
      .subscribe(
        (res) => {
          if (this.CheckResponseMessage(res.data)) {
            this.customer_name_exist = res.extra.value > 0;
            this.customer_exist_id = res.extra.id;
          }
        }
      );
  }
  checkCustomerPhone() {
    this.customer_phone_exist = false;
    this.customer_phone_exist_id = 0;
    this.customer_phone_exist_name = '';
    if (!(this.userform.get('phone_area').value && this.userform.get('phone_prefix').value && this.userform.get('phone_suffix').value)) {
      return;
    }
    const value = String(this.userform.get('phone_area').value)
      + ' ' + String(this.userform.get('phone_prefix').value)
      + ' ' + String(this.userform.get('phone_suffix').value);
    if (!value.trim()) {
      return;
    }
    const postData = { data: value };
    this.cs.postDataExtra(CONFIG.apiURL.page.customers.new.checkPhone, postData)
      .subscribe(
        (res) => {
          if (this.CheckResponseMessage(res.data)) {
            this.customer_phone_exist = res.extra.value > 0;
            this.customer_phone_exist_id = res.extra.value;
            this.customer_phone_exist_name = res.extra.label;
          }
        }
      );
  }
}
