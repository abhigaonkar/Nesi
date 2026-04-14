import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { VendorFormBase } from '../_base/vendorFormBase';

@Component({
  selector: 'nesi-vendor-notes',
  templateUrl: './vendor-notes.component.html',
  styleUrls: ['./vendor-notes.component.css']
})
export class VendorNotesComponent extends VendorFormBase implements OnInit {

  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
  ) {
    super(store, cs);
    super.InitVendor(
      CONFIG.apiURL.page.vendors.notes,
    );
  }

  ngOnInit() {
  }


  createForm() {
    this.userform = this.fb.group({
      'vendor_id': '',
      'data': '',
      'data2': '',
    });
  }

  submitSuccess() {
    this.profile = this.extraData;
    this.loadProfile();
  }

}
