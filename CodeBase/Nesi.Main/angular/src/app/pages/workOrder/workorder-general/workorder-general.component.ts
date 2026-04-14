import { Component, OnInit, Input, OnChanges, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { CoreService } from '../../../services/shared/core.service';
import { WindowRef } from '../../../services/shared/windowRef';
import { ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { WorkOrderFormBase } from '../_base/workorderFormBase';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { DataExtra } from '../../../models/core/dataExtra';
import * as DATE from '../../../services/helper/datetime';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import {CheckboxModule} from 'primeng/checkbox';
import {SpinnerModule} from 'primeng/spinner';

@Component({
  selector: 'nesi-workorder-general',
  templateUrl: './workorder-general.component.html',
  styleUrls: ['./workorder-general.component.css']
})
export class WorkorderGeneralComponent extends WorkOrderFormBase implements OnInit {

  checked: boolean = false;

  constructor(
    private fb: FormBuilder,
    protected ts: TokenService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    protected winRef: WindowRef,
    private route: ActivatedRoute,
    private cf: ConfirmationService,


  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.quotes.editUpdateAll);
  }
  createForm() {
    this.userform = this.fb.group({
      'service_call':'',
      'creditcard_payment':'',
      'R_D':'',
      'visible_to_customer':'',
      'discount_applied':'',
      'warranty':'',
      'labor_only':'',
      'material_only': '',
      'erjobid':'',
      'customer':'',
      'address':'',
      'contact':'',
      'oncustomerasset':'',
      'projectmanager' :'',
      'quote':'',
      'dayscredit':'',
      'defaultinvoicetype':'',
      'autoinvoice':'',
      'currency':'',
      'requiresinspection':'',
      'inspectionlink':'',
      'sustainabilityproject':'',
      'workordertag':'',
      /* 'quote_id': '',
      'revision': '',
      'customer': '',
      'customer_id': ['', Validators.required],
      'customer_name': ['', Validators.required],
      'customer_contact': ['', Validators.required],
      'date_due': ['', Validators.required],
      'date_expected_start': ['', Validators.required],
      'exp_podate': '',
      'chance_winning': ['', Validators.required],
      'chance_winning_reason': '',
      'chance_winning_note': '',
      'completion_date': ['', Validators.required],
      'job_description': ['', [Validators.required]],
      'follow_up': '',
      'cust_spec_doc': '',
      'quoted_by': ['', Validators.required],
      'quoted_business_unit_id': ['', Validators.required],
      'pricetype_id': '',
      'quoted_price': ['', Validators.required],
      'price_to': '',
      'expected_value': ['', Validators.required],
      'us_currency': '',
      'inflation_term': '',
      'percent_down': '',
      'net_due': '',
      'customer_term': '',
      'address_id': '',
      'ts_ticks': '',
      'quoter_locked_bool': '', */
    });


  }
  ngOnInit() {
  }

}
