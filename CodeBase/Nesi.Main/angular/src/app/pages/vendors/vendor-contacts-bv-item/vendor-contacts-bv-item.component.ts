import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { VendorFormBase } from '../_base/vendorFormBase';

@Component({
  selector: 'nesi-vendor-contacts-bv-item',
  templateUrl: './vendor-contacts-bv-item.component.html',
  styleUrls: ['./vendor-contacts-bv-item.component.css']
})
export class VendorContactsBvItemComponent extends VendorFormBase implements OnInit {

  _index = 1;

  @Input() set index(value: number) {
    this._index = value;
    this.loadData();
  }

  get index() {
    return this._index;
  }

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private cf: ConfirmationService,
  ) {
    super(store, cs);
    super.InitVendor(
      CONFIG.apiURL.page.vendors.bvcontact,
    );
  }

  getUrl(path: string) {
    return super.getUrl(path).replace('$bv_index', this.index.toString());
  }



  createForm() {
    this.userform = this.fb.group({
      'vendor_id': '',
      'name': ['', [Validators.required, Validators.maxLength(50)]],
      'phone_area': '',
      'phone_first': '',
      'phone_last': '',
      'phone_ext': '',
      'fax_area': '',
      'fax_first': '',
      'fax_last': '',
      'email': '',
    });
  }

  ngOnInit() {
  }


}
