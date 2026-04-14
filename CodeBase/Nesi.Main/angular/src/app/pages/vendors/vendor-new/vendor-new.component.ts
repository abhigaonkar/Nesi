import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { VendorFormBase } from '../_base/vendorFormBase';
import { TokenService } from '../../../services/authentication/tokenService';
import { PUSH_SUCCESS_MESSAGE } from '../../../actions/layout/growlMessage';

@Component({
  selector: 'nesi-vendor-new',
  templateUrl: './vendor-new.component.html',
  styleUrls: ['./vendor-new.component.css']
})
export class VendorNewComponent extends VendorFormBase implements OnInit {


  vendor_name_exist = false;
  vendor_exist_list: any[];
  vendor_phone_exist_list: any[];
  vendor_phone_exist = false;


  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private fb: FormBuilder,
    private ts: TokenService,

  ) {
    super(store, cs);
    super.InitVendor(
      CONFIG.apiURL.page.vendors.startNew.base
    )
  }
  ngOnInit() {
    this.loadData(true);
  }

  createForm() {
    this.userform = this.fb.group({
      'term_id': ['', [Validators.required]],
      'name': ['', [Validators.required, Validators.minLength(4), Validators.maxLength(60)]],
      'addr1': ['', [Validators.required, Validators.maxLength(200)]],
      'addr2': ['', [Validators.maxLength(200)]],
      'addr3': ['', [Validators.maxLength(200)]],
      'addr4': ['', [Validators.maxLength(200)]],
      'city': ['', [Validators.required, Validators.maxLength(50)]],
      'prov': ['', [Validators.required]],
      'postal': ['', [Validators.required, Validators.maxLength(10)]],
      'country': ['', [Validators.required]],
      'phonearea': ['', [Validators.required, Validators.maxLength(3), Validators.minLength(3)]],
      'phonefirst': ['', [Validators.required, Validators.maxLength(3), Validators.minLength(3)]],
      'phonelast': ['', [Validators.required, Validators.maxLength(4), Validators.minLength(4)]],
      'phoneext': ['', [Validators.maxLength(5)]],
      'faxarea': ['', [Validators.maxLength(3)]],
      'faxfirst': ['', [Validators.maxLength(3)]],
      'faxlast': ['', [Validators.maxLength(4)]],
      'email': ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
      'web': '',
    });
  }


  AfterProfileLoaded() {
    this.userform.get('prov').setValue(this.profile.current_prov);
    this.userform.get('country').setValue(this.profile.current_country);
    this.userform.get('term_id').setValue(1);
  }
  checkVendorName() {
    const v = this.userform.get('name').value;
    if (v && v.length > 3) {
      this.cs.postList(CONFIG.apiURL.page.vendors.startNew.checkName, { data: v })
        .subscribe(res => {
          this.vendor_exist_list = res;
          this.vendor_name_exist = res && res.length > 0;
        });
    } else {
      this.vendor_name_exist = false;
      this.vendor_exist_list = null;
    }
  }

  checkVendorPhone() {
    const v = String(this.userform.get('phonearea').value)
      + String(this.userform.get('phonefirst').value)
      + String(this.userform.get('phonelast').value);
    if (v && v.length === 10) {
      this.cs.postList(CONFIG.apiURL.page.vendors.startNew.checkPhone, { data: v })
        .subscribe(res => {
          this.vendor_phone_exist_list = res;
          this.vendor_phone_exist = res && res.length > 0;
        });
    } else {
      this.vendor_phone_exist = false;
      this.vendor_phone_exist_list = null;
    }
  }


  openVendor(event) {
    this.onChange.emit({ result: event });
  }

}
