import { Component, OnInit,Input } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';

@Component({
  selector: 'nesi-customer-accounting-set',
  templateUrl: './customer-accounting-set.component.html',
  styleUrls: ['./customer-accounting-set.component.css']
})
export class CustomerAccountingSetComponent extends CustomerFormBase implements OnInit {
  @Input() disabled:boolean=false;
  isshown=false;
  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.edit.accountingSetting
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'customer_id': -1,
      'sell_level': '',
      'credit_type': '',
      'tax_prompt': false,
      'credit_limit': ['', [Validators.maxLength(6)]],
      'customer_creditdays': 30,
      'statement_type': '',
      'invoice_type': '',
      'apply_finance_charges': false,
      'customer_autostatements': false,
      'customer_autostatement_address': '',
      'customer_autostatement_ccaddress': '',
      'customer_invoice_address': '',
      'customer_invoice_ccaddress': '',
      'default_invoicetype': '',
      'customer_auto_invoice': false,
      'requires_wo_copy': false,
      'overallmargin': '',
      'customer_term_id':'',
    });
  }

  AfterProfileLoaded() {
    if (!this.profile || !this.profile.setting) {
      return;
    }
    if (this.profile.setting.customer_id <= 0) {
      this.initFormvalue = this.profile.setting;
    } else {
      this.initFormvalue = null;
      this.userform.patchValue(this.profile.setting);
    }
  }

}
