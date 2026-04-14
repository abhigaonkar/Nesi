
import { Component, OnInit,Input } from '@angular/core';
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
  selector: 'nesi-customer-sales',
  templateUrl: './customer-sales.component.html',
  styleUrls: ['./customer-sales.component.css']
})
export class CustomerSalesComponent extends CustomerFormBase implements OnInit {

  @Input() disabled:boolean;
  lists: any;
  data: any;

  yearendList = [
    {
      label: 'Unknown',
      value: 0
    },
    {
      label: 'January',
      value: 1
    },
    {
      label: 'February',
      value: 2
    },
    {
      label: 'March',
      value: 3
    },
    {
      label: 'April',
      value: 4
    },
    {
      label: 'May',
      value: 5
    },
    {
      label: 'June',
      value: 6
    },
    {
      label: 'July',
      value: 7
    },
    {
      label: 'August',
      value: 8
    },
    {
      label: 'September',
      value: 9
    },
    {
      label: 'October',
      value: 10
    },
    {
      label: 'November',
      value: 11
    },
    {
      label: 'December',
      value: 12
    },
  ];

  accountCodeList = [
    {
      label: ' ',
      value: ''
    },
    {
      label: 'A - They can bring us other A,B or C type customers',
      value: 'A'
    },
    {
      label: 'B - They have mulitple facilities and would use more than 1 branch as a result',
      value: 'B'
    },
    {
      label: 'C - They have more than 50 employees in either manufacturing or logistics',
      value: 'C'
    },
    {
      label: 'D - Everyone else',
      value: 'D'
    },
    {
      label: 'TN - Target New',
      value: 'TN'
    },
    {
      label: 'TE - Target Expand',
      value: 'TE'
    },
    {
      label: 'P - Prospect',
      value: 'P'
    },
  ];

  industryList = [
    {
      label: 'Commercial',
      value: 1
    },
    {
      label: 'Industrial',
      value: 2
    },
    {
      label: 'Institutional',
      value: 3
    },
  ];

  employeeSizeList = [
    {
      label: '1 - 4',
      value: 1
    },
    {
      label: '5 - 9',
      value: 2
    },
    {
      label: '10 - 19',
      value: 3
    },
    {
      label: '20 - 49',
      value: 4
    },
    {
      label: '50 - 99',
      value: 5
    },
    {
      label: '100 - 199',
      value: 6
    },
    {
      label: '200 - 499',
      value: 7
    },
    {
      label: '500 - 999',
      value: 8
    },
    {
      label: '1000 - 9999',
      value: 9
    },
    {
      label: '10000+',
      value: 10
    },
  ];

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.sales.profile
    );
  }

  AfterProfileLoaded() {
    this.lists = this.profile.lists;
    this.lists.employeeSizeList = this.employeeSizeList;
    this.lists.industryList = this.industryList;
    this.lists.accountCodeList = this.accountCodeList;
    this.lists.yearendList = this.yearendList;
    this.data = this.profile.data;
  }
}
