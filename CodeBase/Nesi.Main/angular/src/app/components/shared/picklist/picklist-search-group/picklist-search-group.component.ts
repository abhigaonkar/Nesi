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
  selector: 'nesi-picklist-search-group',
  templateUrl: './picklist-search-group.component.html',
  styleUrls: ['./picklist-search-group.component.css']
})
export class PicklistSearchGroupComponent extends PickListSearchbase implements OnInit {
  placeholder = 'Select Group';
  sourceName = 'Group';

  constructor(
    protected fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(fb, store, cs);
    this.getAllUrl = CONFIG.apiURL.page.shared.pickList.groups;
    this.getListUrl = this.getAllUrl + '/$buId/$value';
    super.Init(CONFIG.apiURL.page.shared.pickList.addQuoteMaterials + '$quote_id/$revision');
  }

}
