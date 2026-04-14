import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { VendorFormBase } from '../_base/vendorFormBase';

@Component({
  selector: 'nesi-vendor-contacts-bv',
  templateUrl: './vendor-contacts-bv.component.html',
  styleUrls: ['./vendor-contacts-bv.component.css']
})
export class VendorContactsBvComponent extends VendorFormBase implements OnInit {
  selectedbvContact = 1;
  bvcontacts = [
    { label: 'Contact 1', value: 1, icon: 'fa fa-fw fa-user' },
    { label: 'Contact 2', value: 2, icon: 'fa fa-fw fa-user' },
    { label: 'Contact 3', value: 3, icon: 'fa fa-fw fa-user' },
  ];
  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private cf: ConfirmationService,
  ) {
    super(store, cs);
  
  }



  ngOnInit() {
  }

}
