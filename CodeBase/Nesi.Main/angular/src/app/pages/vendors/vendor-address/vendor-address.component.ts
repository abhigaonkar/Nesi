import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { VendorFormBase } from '../_base/vendorFormBase';

@Component({
  selector: 'nesi-vendor-address',
  templateUrl: './vendor-address.component.html',
  styleUrls: ['./vendor-address.component.css']
})
export class VendorAddressComponent extends VendorFormBase implements OnInit {
  vendor_phone_exist_list: any[];
  vendor_phone_exist = false;

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
  ) {
    super(store, cs);
    super.InitVendor(
      CONFIG.apiURL.page.vendors.address,
    );
  }

  ngOnInit() {
  }


  createForm() {
    this.userform = this.fb.group({
      'vendor_id': '',
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
      'email': '',
      'web': '',
    });
  }

  checkVendorPhone() {
    // const v = String(this.userform.get('phonearea').value)
    //   + String(this.userform.get('phonefirst').value)
    //   + String(this.userform.get('phonelast').value);
    // if (v && v.length === 10) {
    //   this.cs.postList(CONFIG.apiURL.page.vendors.startNew.checkPhone, { data: v })
    //     .subscribe(res => {
    //       this.vendor_phone_exist_list = res;
    //       this.vendor_phone_exist = res && res.length > 0;
    //     });
    // } else {
    //   this.vendor_phone_exist = false;
    //   this.vendor_phone_exist_list = null;
    // }
  }

  openVendor(event) {
    // this.onChange.emit({ result: event });
  }

}
