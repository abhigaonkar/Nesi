
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
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-sales-history',
  templateUrl: './customer-sales-history.component.html',
  styleUrls: ['./customer-sales-history.component.css']
})
export class CustomerSalesHistoryComponent extends CustomerFormBase implements OnInit {
  @Input() lists: any;
  @Input() disabled:boolean;
  @Input() set data(value: any) {
    this._data = value;
  }

  _data: any;

  get data() {
    return this._data;
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
      CONFIG.apiURL.page.customers.sales.history,
      'N/A'
    );
  }


  get histories(): any[] {
    if (this.lists) {
      return this.lists.historyList;
    } else {
      return null;
    }
  }

  createForm() {
    this.userform = this.fb.group({
      'customer_id': this.customer_id,
      'address_id': [this.address_id, [Validators.required]],
      'date': ['', [Validators.required]],
      'member_id': ['', [Validators.required]],
      'action_id': ['', [Validators.required]],
      'origin': ['', [Validators.required]],
      'notes': ['', [Validators.required]],
    });

    this.initFormvalue={
      'customer_id': this.customer_id,
      'address_id': this.address_id,
      'date': new Date(),
      'member_id': this.ts.currentUser.id,
      'action_id': null,
      'origin': null,
      'notes': null,
    };
  }

  submitSuccess() {
    this.lists.historyList = this.extraData;
  }
}
