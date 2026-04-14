import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { CONFIG } from 'app/configuration';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-inventory-rowdata',
  templateUrl: './inventory-rowdata.component.html',
  styleUrls: ['./inventory-rowdata.component.css']
})
export class InventoryRowdataComponent implements OnInit {

  @Input() master_id: string;
  @Input() quote_id: string;
  @Input() revision: string;

  @Output() loaded = new EventEmitter<any>();
  @Output() startLoad = new EventEmitter<any>();

  constructor(
    public cs: CoreService,
  ) {

  }

  ngOnInit() {
    if (!this.master_id) { return; }
    const e: any = { master_id: this.master_id, data: null };
    this.startLoad.emit(e);
    this.cs.getObject<any>(CONFIG.apiURL.page.shared.pickList.quoteInventory
      + this.quote_id + '/' + this.revision + '/' + this.master_id.toString() + '/0/0')
      .subscribe(
      (res) => {
        const event: any = { master_id: this.master_id, data: res };
        this.loaded.emit(event);
      }
      );
  }

}
