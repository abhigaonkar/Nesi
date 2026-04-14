import { Component, OnInit } from '@angular/core';
import { PickListSearchbase } from '../_base/picklist-search-base';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CONFIG } from 'app/configuration';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../../reducers';
import { FormBuilder } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';
import { PickListGroup } from 'app/models/picklist/picklistGroup';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-search-quote',
  templateUrl: './picklist-search-quote.component.html',
  styleUrls: ['./picklist-search-quote.component.css']
})
export class PicklistSearchQuoteComponent extends PickListSearchbase implements OnInit {
  placeholder = 'Select Quote';



  constructor(
    protected fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(fb, store, cs);
    this.getCustomerUrl = CONFIG.apiURL.page.shared.pickList.customers;
    this.getAllUrl = CONFIG.apiURL.page.shared.pickList.customersQuotes + '$buId/$custId';
    this.getListUrl = CONFIG.apiURL.page.shared.pickList.quoteParts + '$buId/$value';
    this.searchUrl = CONFIG.apiURL.page.shared.pickList.queryQuotes;
    this.sourceName = 'Quote';
    super.Init(CONFIG.apiURL.page.shared.pickList.addQuoteMaterials + '$quote_id/$revision');
  }

}
