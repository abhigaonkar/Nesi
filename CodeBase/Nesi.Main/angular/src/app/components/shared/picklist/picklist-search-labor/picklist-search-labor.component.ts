import { Component, OnInit } from '@angular/core';
import { PickListSearchbase } from '../_base/picklist-search-base';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CONFIG } from 'app/configuration';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../../reducers';
import { FormBuilder } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';
import { PicklistLaborType } from 'app/models/picklist/picklistLaborType';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-search-labor',
  templateUrl: './picklist-search-labor.component.html',
  styleUrls: ['./picklist-search-labor.component.css']
})
export class PicklistSearchLaborComponent extends PickListSearchbase implements OnInit {
  placeholder = 'Select Labour';
  sourceName = 'Labour';

  constructor(
    protected fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(fb, store, cs);
    super.Init(CONFIG.apiURL.page.shared.pickList.addQuoteLabor + '$quote_id/$revision');
    this.getAllUrl = CONFIG.apiURL.page.shared.pickList.memberTypes + '$buId';
  }

  select(event: any) {
    if (!event.value) {
      return;
    }
    super.select(event);
    this.cs.getList<PicklistLaborType>(CONFIG.apiURL.page.shared.pickList.chargeOut + this.buId
      + '/' + event.value + '/' + this.quote_id + '/' + this.revision)
      .subscribe(
      (res: PicklistLaborType[]) => {
        this.dialogDisplay = true;
        this.setRows(res);
      }
      );
  }





}

