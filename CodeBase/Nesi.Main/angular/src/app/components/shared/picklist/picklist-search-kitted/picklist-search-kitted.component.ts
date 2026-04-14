import { Component, OnInit } from '@angular/core';
import { PickListSearchbase } from '../_base/picklist-search-base';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CONFIG } from 'app/configuration';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../../reducers';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';
import { PickListKitted } from 'app/models/picklist/picklistKitted';
import { PickListGroup } from 'app/models/picklist/picklistGroup';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-search-kitted',
  templateUrl: './picklist-search-kitted.component.html',
  styleUrls: ['./picklist-search-kitted.component.css']
})
export class PicklistSearchKittedComponent extends PickListSearchbase implements OnInit {
  placeholder = 'Select Kitted';
  cost_sum = 0;
  sourceName = 'Kitted';
  item: PickListGroup;

  constructor(
    protected fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(fb, store, cs);

    this.getAllUrl = CONFIG.apiURL.page.shared.pickList.kitteds;
    this.getListUrl = this.getAllUrl +  '/$buId/$value';
    super.Init(CONFIG.apiURL.page.shared.pickList.addQuoteMaterial + '$quote_id/$revision');

  }



  select(event: any) {
    super.select(event);
    this.fillkittedCost(event.value, 0);
  }

  fillkittedCost(master_id: number, qty: number) {
    this.cost_count++;
    this.cs.getObject<any>(this.getCostURL(master_id, qty))
      .subscribe(
      (res) => {
        this.item = res;
        CONFIG.LOG(this.item, 'get kitted obj in search kitted');
        this.userform.patchValue(this.item);
        this.userform.get('section_id').setValue(this.section_id);
        this.userform.get('description').setValue(this.label);
        this.cost_sum = res.cost;
        this.cost_count--;
      }
      );
  }

  createForm() {
    super.createForm();
    this.userform.addControl('master_id', new FormControl());
    this.userform.addControl('description', new FormControl());
    this.userform.addControl('qty', new FormControl());
    this.userform.addControl('is_checked', new FormControl());
    this.userform.addControl('section_id', new FormControl());
    this.userform.addControl('cost', new FormControl());
    this.userform.addControl('sell', new FormControl());
    this.userform.addControl('extd', new FormControl());
    this.userform.addControl('extd2', new FormControl());
    this.userform.addControl('discount', new FormControl());
  }


  submitBefore() {
    super.submitBefore();
    this.submitedValue = this.userform.value;
    this.submitedValue.rows = null;
  }

  submitValidate() {
    const amount = Number(this.userform.get('qty').value);
    if (amount && amount > 0) {
      return true;
    } else {
      this.PushWarnMessage('The number must be greater than 0.');
      return false;
    }
  }


  costLoaded(event: any, i: number) {
    super.costLoaded(event, i);
    const qty = Number(this.rows.controls[i].get('qty').value);
    // this.cost_sum = this.cost_sum + Number(event.cost) * qty;
  }
}
