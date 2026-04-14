import { Component, OnInit } from '@angular/core';
import { PickListSearchbase } from '../_base/picklist-search-base';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CONFIG } from 'app/configuration';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../../reducers';
import { FormBuilder } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'nesi-picklist-search-blank-line',
  templateUrl: './picklist-search-blank-line.component.html',
  styleUrls: ['./picklist-search-blank-line.component.css']
})
export class PicklistSearchBlankLineComponent extends PickListSearchbase implements OnInit {

  placeholder = 'Input description';
  item: any;
  qty: number;
  cost: number;

  constructor(
    protected fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,

  ) {
    super(fb, store, cs);
    this.isDropdown = false;
    super.Init(CONFIG.apiURL.page.shared.pickList.addQuoteMaterial + '$quote_id/$revision');
  }

  createForm() {
    this.userform = this.fb.group({
      'master_id': '',
      'section_id': '',
      'description': '',
      'discount': 0,
      'qty': '',
      'cost': '',
      'sell': '',
      'extd': '',
      'extd2': ''
    });

    this.initFormvalue = {
      'master_id': '',
      'section_id': '',
      'description': '',
      'discount': 0,
      'qty': '',
      'cost': '',
      'sell': '',
      'extd': '',
      'extd2': ''
    }

    // this.userform.get('qty').valueChanges.debounceTime(300)
    //   .subscribe(
    //   (value) => {
    //     if (value !== this.qty) {
    //       this.qty = value;
    //       this.fillCost();
    //     }
    //   }
    //   );
    // this.userform.get('cost').valueChanges.debounceTime(300)
    //   .subscribe(
    //   (value) => {
    //     if (value !== this.cost) {
    //       this.cost = value;
    //       this.fillCost();
    //     }
    //   }
    //   );
  }



  getCostURL(qty: number, cost: number) {
    return CONFIG.apiURL.page.shared.pickList.costHandler
      + this.quote_id + '/' + this.revision + '/' + qty.toString().replace('.', '_') + '/' + cost.toString().replace('.', '_');
  }

  fillCost() {
    let qty = this.userform.get('qty').value;
    const cost = this.userform.get('cost').value;
    if (!cost || isNaN(cost) || isNaN(qty)) {
      return
    }
    if (!qty) {
      qty = 1;
      this.userform.get('qty').setValue(1);
    }
    this.cs.getObject<any>(this.getCostURL(qty, cost))
      .subscribe(
      (res) => {
        this.item = res;
        CONFIG.LOG(this.item, 'selected obj in material');
        this.userform.patchValue(this.item);
        this.userform.get('section_id').setValue(this.section_id);
      }
      );
  }


  submitValidate(): boolean {
    if (isNaN(this.userform.get('cost').value)) {
      this.PushWarnMessage('Cost is required.');
      return false;
    }
    if (this.userform.get('qty').value <= 0) {
      this.PushWarnMessage('Qty must be greater than 0.');
      return false;
    }
    if (!this.userform.get('description').value) {
      this.PushWarnMessage('Description is required.');
      return false;
    }
    if (!this.userform.get('extd2').value) {
      this.PushWarnMessage('~Ext\'d is required.');
      return false;
    }
    return true;
  }
}
