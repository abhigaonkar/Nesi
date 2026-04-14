import { Component, OnInit, Input } from '@angular/core';
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
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-search-material',
  templateUrl: './picklist-search-material.component.html',
  styleUrls: ['./picklist-search-material.component.css']
})
export class PicklistSearchMaterialComponent extends PickListSearchbase implements OnInit {

  @Input() set searchText(value: string) {
    if (!value) {
      return;
    }
    this.is_newValue = (this.oldValue !== Number(value));
    this.searching_masterId = true;
    this.oldValue = Number(value);
    this.fillCost(Number(value), 0);
  }

  is_newValue = true;
  oldValue: number;
  placeholder = 'Search PartNo';
  item: any;
  cost_readonly = true;
  searching_masterId = false;
  qty: number;

  constructor(
    protected fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,

  ) {
    super(fb, store, cs);
    this.searchUrl = CONFIG.apiURL.page.shared.pickList.quotePart + '$quote_id/$revision';
    this.isDropdown = false;
    super.Init(CONFIG.apiURL.page.shared.pickList.addQuoteMaterial + '$quote_id/$revision');
  }

  createForm() {
    this.userform = this.fb.group({
      'master_id': '',
      'section_id': '',
      'description': '',
      'discount': '',
      'qty': '',
      'cost': '',
      'sell': '',
      'extd': '',
      'extd2': ''
    });
    this.userform.get('qty').valueChanges
      .debounceTime(300)
      .subscribe(
      (value) => {
        if (value !== this.qty) {
          this.qty = value;
          this.qtyChanged();
        }
      }
      );
  }

  searchValidate(query: string) {
    this.ClearMessage();
    const reg = /^\d+$/;
    if (!reg.test(query)) {
      this.PushWarnMessage('Please input number only.');
      return false;
    }
    return true;
  }


  fillCost(master_id: number, qty: number) {
    if (!master_id) {
      master_id = 0;
    }
    if (!this.is_newValue && !this.cost_readonly) {
      this.fillCost2();
      return;
    } else {
      this.cs.getObject<any>(this.getCostURL(master_id, qty))
        .subscribe(
        (res) => {
          if (res) {
            this.item = res;
            if (this.item.cost === 0 || master_id === 0 || master_id === 55556 || master_id === 55558) {
              this.item.cost = null;
              this.cost_readonly = false;
            } else {
              this.cost_readonly = true;
            }
            if (this.searching_masterId) {
              this.text = {
                label: this.item.master_id + (!!this.item.description ? ' - ' + this.item.description : ''),
                value: this.item.master_id
              };
            }
            CONFIG.LOG(this.item, 'selected obj in material');
            this.userform.patchValue(this.item);
            this.userform.get('section_id').setValue(this.section_id);
          } else {
            this.clearTextBox();
            if (this.searching_masterId) {
              this.text = { label: master_id, value: master_id };
            }
          }
        },
        (err) => {
          this.clearTextBox();
        }
        );
    }
  }

  qtyChanged() {
    const value = this.userform.get('qty').value;
    if (!value) {
      this.userform.get('extd').setValue('');
      this.userform.get('extd2').setValue('');
      return;
    }
    CONFIG.LOG(value, 'key up in qty');
    this.fillCost(this.item.master_id, value);
  }

  select(event: any) {
    CONFIG.LOG(event.value, 'selected value in material');
    this.fillCost(event.value, 0);
  }


  clearTextBox() {
    this.text = '';
    this.item = null;
    this.userform.get('qty').setValue('');
    this.userform.get('cost').setValue('');
    this.userform.get('extd').setValue('');
    this.userform.get('extd2').setValue('');
  }

  submitValidate(): boolean {
    if (this.userform.get('qty').value <= 0) {
      this.PushWarnMessage('Qty must be greater than 0.');
      return false;
    }
    if (!this.userform.get('cost').value) {
      this.PushWarnMessage('Cost is requited.');
      return false;
    }
    if (!this.userform.get('extd').value) {
      this.PushWarnMessage('Extd is requited.');
      return false;
    }
    if (!this.userform.get('extd2').value) {
      this.PushWarnMessage('Ext\'d is requited.');
      return false;
    }
    return true;


  }

  submitSuccess() {
    this.clearTextBox();
  }


  getCost2URL(qty: number, cost: number) {
    return CONFIG.apiURL.page.shared.pickList.costHandler
      + this.quote_id + '/' + this.revision + '/' + qty.toString() + '/' + cost.toString();
  }

  fillCost2() {
    let qty = this.userform.get('qty').value;
    const cost = this.userform.get('cost').value;
    if (!cost) {
      return
    }
    if (!qty) {
      qty = 1;
      this.userform.get('qty').setValue(1);
    }
    this.cs.getObject<any>(this.getCost2URL(qty, cost))
      .subscribe(
      (res) => {
        this.item = res;
        CONFIG.LOG(this.item, 'selected obj in material');
        this.userform.patchValue(this.item);
      }
      );
  }


}
