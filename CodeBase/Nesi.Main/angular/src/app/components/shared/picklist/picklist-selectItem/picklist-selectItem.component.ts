import { Component, OnInit, Input,  Output, EventEmitter } from '@angular/core';
import { PickListItem } from '../../../../models/picklist/picklistItem';
import { PickListGroup } from 'app/models/picklist/picklistGroup';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { cost_level_tips } from 'app/components/shared/picklist/_base/pickList-constains';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-selectItem',
  templateUrl: './picklist-selectItem.component.html',
  styleUrls: ['./picklist-selectItem.component.css'],
})
export class PicklistSelectItemComponent implements OnInit {

  @Input() item: PickListGroup;
  @Input() quote_id: string;
  @Input() revision: string;

  @Output() loaded = new EventEmitter<any>();
  @Output() startLoad = new EventEmitter<any>();

  cost: number;
  onhand: number;
  loading = true;
  cost_level: number;

  constructor(
    public cs: CoreService,
  ) {

  }

  ngOnInit() {
  //  this.startLoad.emit(null);
    this.cost=this.item.cost;
    this.onhand=this.item.onhand;
    this.cost_level=this.item.cost_level;
    // if (!this.quote_id) { return; }
    // this.cs.getObject<any>(CONFIG.apiURL.page.shared.pickList.quoteInventory
    //   + this.quote_id + '/' + this.revision + '/' + this.item.master_id.toString() + '/0/0')
    //   .subscribe(
    //   (res) => {
    //     this.loading = true;
    //     this.cost = res.cost_price || 0;
    //     this.onhand = res.onhand || 0;
    //     this.cost_level = res.cost_level || 0;
    //     const event: any = { item: res, cost: this.cost, onhand: this.onhand };
    //     this.loaded.emit(event);
    //   }
    //   );
  }

  getCost_ToolTip(level: number) {
    return cost_level_tips[level];
  }
}
