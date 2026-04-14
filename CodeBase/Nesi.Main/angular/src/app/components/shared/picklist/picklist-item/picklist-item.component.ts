import { Component, OnInit, Input, EventEmitter, Output, ElementRef, ViewChild } from '@angular/core';
import { PickListItem } from '../../../../models/picklist/picklistItem';
import { cost_level_tips } from '../_base/pickList-constains';
import { WindowRef } from 'app/services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { DomSanitizer } from '@angular/platform-browser';
import { MessageBase } from 'app/core/messageBaseComponent';
import * as fromRoot from '../../../../reducers';
import { Store } from '@ngrx/store';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-item',
  templateUrl: './picklist-item.component.html',
  styleUrls: ['./picklist-item.component.css']
})
export class PicklistItemComponent extends MessageBase implements OnInit {

  image: any;
  tooltipX: string;
  tooltipY: string;
  kittedParts: any[];
  discriptionEditable = false;

  @Input() item: PickListItem
  @Input() quote_id: number;
  @Input() revision: number;
  @Input() buid: number;
  @Input() edit_disabled = false;
  @Input() can_see_cost = true;

  @Output() onDelete = new EventEmitter();
  @Output() onCheckChange = new EventEmitter();

  @Output() onNoteChanged = new EventEmitter();


  constructor(
    private winRef: WindowRef,
    public cs: CoreService,
    private sanitizer: DomSanitizer,
    protected store: Store<fromRoot.State>,
  ) {
    super(store);
  }

  ngOnInit() {
    if (this.item.part_no && this.item.part_no < 990000) {
      // this.cs.getString(CONFIG.apiURL.page.shared.pickList.InventoryPicture + this.item.part_no)
      //   .subscribe(
      //   (res) => {
      //     this.image = 'data:image/png;base64,' + res;
      //   }
      //   );
    } else if (this.item.part_no && this.item.part_no >= 2000000) {
      this.getKittedList();
    }
  }

  get margin() {
    if (!this.item.extd2 || this.item.extd2 <= 0) {
      return 0;
    }
    const m = (this.item.extd2 - (this.item.cost * this.item.qty)) / this.item.extd2;
    return m;

  }
  setFocus(el) {
    this.discriptionEditable = true;
    CONFIG.LOG(el, 'setFocus inplace');
    window.setTimeout(() => el.focus(), 200);
  }

  closeInplace() {
    this.discriptionEditable = false;
    CONFIG.LOG(this.discriptionEditable, 'close inplace');
    //  this.field_Changed('description');

  }

  get cost_readonly(): boolean {
    const o = !(!this.item.part_no || ( Number(this.item.part_no) === 55556 || Number(this.item.part_no) === 55558 ) || this.item.is_exclude);
    return o;
  }

  getCostClass(item: any): string {
    if (item.part_no === null || item.part_no === '' || item.part_no === '55556' || item.part_no === '55558') {
      return 'cost_level_30';
    } else {
      return 'cost_level_' + item.cost_level;
    }
  }

  getCostURL(master_id: number, qty: number) {
    return CONFIG.apiURL.page.shared.pickList.quoteInventory
      + this.quote_id + '/' + this.revision + '/' + master_id.toString() + '/' + qty.toString() + '/0';
  }

  field_Changed(field: string) {
    if (field === 'extd2' && !this.item.extd2) {
      this.item.extd2 = 0;
    }
    CONFIG.LOG(this.item, 'item submit to edit');
    const url = CONFIG.apiURL.page.shared.pickList.quoteLineUpdate + field;
    this.cs.patchObject<any>(url, this.item)
      .subscribe(
      (res) => {
        if (this.CheckResponseMessage(res.data)) {
          if (field === 'qty') {
            CONFIG.LOG(res.extra, 'res in update qty obj in picklist item');
            this.item.cost = res.extra.cost;
            if (this.can_see_cost) {
              this.item.display_cost = Math.round(this.item.cost * 100) / 100;
              
            }
            this.item.extd = res.extra.extd;
            this.item.extd2 = res.extra.extd2;
            if (this.item.cost > this.item.extd) {
              this.PushWarnMessage('The cost price is less than sell price, please contact with admistrator!');
            }
          }
          this.onCheckChange.emit();
        }
      }
      );
  }

  fillCost2() {
    let qty = this.item.qty;

    this.item.cost = this.item.display_cost;
    const cost = this.item.display_cost;
    if (!cost) {
      return
    }
    if (!qty) {
      qty = 1;
      this.item.qty = 1;
    }
    this.cs.getObject<any>(this.getCost2URL(qty, cost))
      .subscribe(
      (res) => {
        this.item.extd = res.extd;
        this.item.extd2 = res.extd2;
        this.field_Changed('cost');
      }
      );
  }



  getCost2URL(qty: number, cost: number) {
    return CONFIG.apiURL.page.shared.pickList.costHandler
      + this.quote_id + '/' + this.revision + '/' + qty.toString().replace('.', '_') + '/' + cost.toString().replace('.', '_');
  }

  getExtd2Tip() {
    if (this.item.extd2 > this.item.extd) {
      return 'Quote Price greater than benchmark sell price';
    } else if (this.item.extd2 === this.item.extd) {
      return 'Quote Price equals benchmark sell price';
    } else {
      return 'Quote Price less than benchmark sell price';
    }
  }

  getKittedList() {
    const url = CONFIG.apiURL.page.shared.pickList.kitteds + '/' + this.buid + '/' + this.item.part_no;
    this.cs.getList<any>(url)
      .subscribe(res => this.kittedParts = res);
  }

  getCost_ToolTip(level: number) {
    return cost_level_tips[level];
  }

  delete(id: number) {
    CONFIG.LOG(id, 'id in delete picklist item');
    this.cs.deleteString(CONFIG.apiURL.page.shared.pickList.quote
      + this.quote_id.toString() + '/' + this.revision.toString() + '/' + id.toString())
      .subscribe(
      (res: string) => {
        this.PushResponseMessage(res);
        this.onDelete.emit();
      }
      )

  }

  partNoClick(id: number) {
    this.winRef.openInventory(id);
  }

  moveTip(event: any) {
    this.tooltipX = (event.clientX + 20) + 'px';
    this.tooltipY = (event.clientY + 20) + 'px';
  }

  check_changed() {
    this.cs.postString(CONFIG.apiURL.page.shared.pickList.LineIsChecked + this.quote_id.toString() + '/' + this.revision,
      {
        id: this.item.id,
        value: this.item.is_checked,
      }).subscribe(
      (res) => {
        this.onCheckChange.emit()
      }
      );
  }


}
