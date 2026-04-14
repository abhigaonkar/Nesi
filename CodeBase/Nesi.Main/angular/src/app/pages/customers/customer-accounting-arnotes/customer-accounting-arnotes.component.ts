import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';

@Component({
  selector: 'nesi-customer-accounting-arnotes',
  templateUrl: './customer-accounting-arnotes.component.html',
  styleUrls: ['./customer-accounting-arnotes.component.css']
})
export class CustomerAccountingArnotesComponent  extends CustomerFormBase implements OnInit {

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.edit.accountingArNotes
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'customer_id': -1,
      'customer_arnotes':  '',
      'memo':  '',
    });
  }

  AfterProfileLoaded() {
    if (!this.profile) {
      return;
    }
    if (this.profile.customer_id <= 0) {
      this.initFormvalue = this.profile;
    } else {
      this.initFormvalue = null;
      this.userform.patchValue(this.profile);
    }
  }

}
