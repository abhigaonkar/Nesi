
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

@Component({
  selector: 'nesi-customer-sales-stats',
  templateUrl: './customer-sales-stats.component.html',
  styleUrls: ['./customer-sales-stats.component.css']
})
export class CustomerSalesStatsComponent extends CustomerFormBase implements OnInit {
  @Input() lists: any;

  @Input() set data(value: any) {
    if (value) {
      this.userform.patchValue(value);
    }
  }

  @Output() cancel = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.sales.base,
      'N/A'
    );
  }

}
