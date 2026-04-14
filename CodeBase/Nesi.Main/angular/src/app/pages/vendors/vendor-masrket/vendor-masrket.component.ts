
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { DataExtra } from 'app/models/core/dataExtra';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { VendorFormBase } from '../_base/vendorFormBase';

@Component({
  selector: 'nesi-vendor-masrket',
  templateUrl: './vendor-masrket.component.html',
  styleUrls: ['./vendor-masrket.component.css']
})
export class VendorMasrketComponent extends VendorFormBase implements OnInit {

  public linecardList: any[];
  public competitorList: any[];


  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private cf: ConfirmationService,

  ) {
    super(store, cs);
    super.InitVendor(
      CONFIG.apiURL.page.vendors.market
    );
  }


  AfterProfileLoaded() {
    this.linecardList = this.profile.linecardList;
    this.competitorList = this.profile.competitorList;
  }
}
