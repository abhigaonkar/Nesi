
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-address-accounting',
  templateUrl: './customer-address-accounting.component.html',
  styleUrls: ['./customer-address-accounting.component.css']
})
export class CustomerAddressAccountingComponent extends CustomerFormBase implements OnInit {

  table_id_list = [
    { label: 'Customer', value: 1 },
    { label: 'Worksite', value: 2 },
  ];
  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.edit.addressAccounting + '/$address_id'
    );
  }


  createForm() {
    this.userform = this.fb.group({
      'customer_id': ['', [Validators.required]],
      'address_id': ['', [Validators.required, Validators.min(1)]],
      'gl_account': ['', [Validators.required]],
      'tax_1': ['', [Validators.required]],
      'tax_2': ['', [Validators.required]],
      'tax_3': ['', [Validators.required]],
      'tax_4': ['', [Validators.required]],
      'taxex_1': '',
      'taxex_2': '',
      'taxex_3': '',
      'taxex_4': '',
    });
  }

  AfterProfileLoaded() {
    if (this.profile.accounting.address_id <= 0) {
      this.initFormvalue = this.profile.accounting;
    } else {
      this.userform.patchValue(this.profile.accounting);
    }
    if (!this.customer_base_profile.is_admin_authenticated) {
      this.userform.disable();
    }
  }
}
