import { Component, OnInit } from '@angular/core';
import { PickListSearchbase } from '../_base/picklist-search-base';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CONFIG } from 'app/configuration';
import { Store } from "@ngrx/store";
import { CoreService } from "app/services/shared/core.service";
import * as fromRoot from '../../../../reducers';
import { FormBuilder } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';


@Component({
  selector: 'nesi-picklist-search-workorder',
  templateUrl: './picklist-search-workorder.component.html',
  styleUrls: ['./picklist-search-workorder.component.css']
})
export class PicklistSearchWorkorderComponent extends PickListSearchbase implements OnInit {
  placeholder = 'Select Work order';

  constructor(
    protected fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(fb, store, cs);

    super(fb, store, cs);
    this.getCustomerUrl = CONFIG.apiURL.page.shared.pickList.customers;
    this.getAllUrl = CONFIG.apiURL.page.shared.pickList.customersWos + '$buId/$custId';
    this.getListUrl=CONFIG.apiURL.page.shared.pickList.woParts + '$buId/$value';
    this.searchUrl=CONFIG.apiURL.page.shared.pickList.queryWos;
    this.sourceName='Work Order';
    super.Init(CONFIG.apiURL.page.shared.pickList.addQuoteMaterials + '$quote_id/$revision');
  }

  
}
