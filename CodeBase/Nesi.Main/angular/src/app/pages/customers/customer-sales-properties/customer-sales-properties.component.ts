
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';
import { WindowRef } from 'app/services/shared/windowRef';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-sales-properties',
  templateUrl: './customer-sales-properties.component.html',
  styleUrls: ['./customer-sales-properties.component.css']
})
export class CustomerSalesPropertiesComponent extends CustomerFormBase implements OnInit {
  @Input() lists: any;
  @Input() disabled:boolean;
  @Input() set data(value: any) {
    if (value) {
      this._data = value;
      if (String(value.next_followup_date) === '0001-01-01') {
        value.next_followup_date = null;
      }
      this.userform.patchValue(value);

      this.userform.get('status_id').disable();
      this.userform.get('confirmed_tax_exempt').disable();
    }
  }
  _data: any;
  ratesheet_bu: number;
  @Output() cancel = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private win: WindowRef,

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.sales.base,
      'N/A'
    );
  }

  createForm() {
    this.userform = this.fb.group({
      'customer_id': this.customer_id,
      'address_id': [this.address_id, [Validators.required]],
      'status_id': {value: '', disabled: true},
      'member_id': 0,
      'call_cycle': 0,
      'year_end': '',
      'project_mgr_member_id': 0,
      'controls_mgr_member_id': 0,
      'discount_pct': 0,
      'decision_maker': 0,
      'account_code': '',
      'next_followup_date': null,
      'next_followup_notes': '',
      'job_budget_threshold': 0.0,
      'po_required': false,
      'confirmed_po_required': false,
      'confirmed_tax_exempt': {value: false, disabled: true},
      'industry': 0,
      'origin': 0,
      'sector': '',
      'naics_code': '',
      'employee_size': 0,
      'do_at_location': '',
      'affiliated_companies': '',
      'known_suppliers': '',
      'known_competitors': '',
      'why_choose': '',
      'isr_member_id': 0,
      'osr_member_id': 0,
      'ram_member_id': 0,
      'mam_member_id': 0,
      'cisr_member_id': 0,
      'am_member_id': 0,
      'notes_public': '',
      'notes_sales': '',
      'last_invoice': '',
      'last_fiscal': 0,
      'current_fiscal': 0,
      'ytd': 0,
      'lytd': 0,
    });
  }

  submitSuccess() {
    this.data = this.extraData;
  }


  showRateSheet(event = null) {
    if (event) {
      this.ratesheet_bu = event.value;
    }
    if (this.ratesheet_bu && this.ratesheet_bu > 0) {
      this.win.boingNesi1(`/sections/reports/rates_sheet/index.aspx?business_unit_id=${this.ratesheet_bu}&customerid=${this.customer_id}`
        , 'sheet_rate_' + this.ratesheet_bu.toString());
    }
  }
}
