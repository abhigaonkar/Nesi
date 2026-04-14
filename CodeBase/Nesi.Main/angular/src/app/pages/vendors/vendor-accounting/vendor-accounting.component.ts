import { Component, OnInit } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { LabelValue } from 'app/models/component/filterBuilder/filterBuilder';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { VendorFormBase } from '../_base/vendorFormBase';

@Component({
  selector: 'nesi-vendor-accounting',
  templateUrl: './vendor-accounting.component.html',
  styleUrls: ['./vendor-accounting.component.css']
})
export class VendorAccountingComponent extends VendorFormBase implements OnInit {

  isShown=false;
  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private cf: ConfirmationService,
  ) {
    super(store, cs);
    super.InitVendor(
      CONFIG.apiURL.page.vendors.accounting,
    );
  }

  ngOnInit() {
  }


  createForm() {
    this.userform = this.fb.group({
      'business_segment':'',
      'subsidiary':'',
      'branch':'',
      'vendor_id': '',
      'business_unit': ['', [Validators.required, Validators.min(1)]],
      'revenue_line':'',
      'vendor_term_id': '',
      'po_exempt': false,
      'cprs': false,
      'vendor_idtype': '',
      'vendor_idnumber': '',
      'vendor_credit_type': '',
      'vendor_credit_limit': 0,
      'vendor_account': '',
      'vendor_buyer': '',
      'tax1': '',
      'tax2': '',
      'tax3': '',
      'tax4': '',
      
    });
  }
}
