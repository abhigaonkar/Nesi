import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { VendorFormBase } from '../_base/vendorFormBase';
import { WindowRef } from '../../../services/shared/windowRef';

@Component({
  selector: 'nesi-vendor-edit-general',
  templateUrl: './vendor-edit-general.component.html',
  styleUrls: ['./vendor-edit-general.component.css']
})
export class VendorEditGeneralComponent extends VendorFormBase implements OnInit {

  activeIndex = -1;
isShown= "false";
  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    protected winRef: WindowRef,
  ) {
    super(store, cs);
    super.InitVendor(
      CONFIG.apiURL.page.vendors.edit,
    );
  }

  ngOnInit() {
  }

  AfterProfileLoaded() {
    this.activeIndex = -1;
  }

  createForm() {
    this.userform = this.fb.group({
      'vendor_id': '',
      'business_unit_id': ['', [Validators.required, Validators.min(1)]],
      'name': ['', [Validators.required, Validators.minLength(4), Validators.maxLength(60)]],
      'vendor_hold': '',
      'is_partner': '',
    });
  }

  onAccordionOpen(event) {
    this.activeIndex = event.index;
    setTimeout(() => {
      this.activeIndex = event.index;
    }, 500); }
    public handle_vendor_request(){
      console.log("handle");
      const url = CONFIG.Nesi1URL.requestCustOrVendor.replace('@type', '2').replace('@customer_id',this._vendor_id.toString());
      this.winRef.boingNesi1(url ,'VendorRequest'+Math.random(),"500,700");
   
  }


}
