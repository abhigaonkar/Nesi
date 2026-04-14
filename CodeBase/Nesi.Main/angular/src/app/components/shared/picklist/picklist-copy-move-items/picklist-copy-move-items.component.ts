import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { CONFIG } from 'app/configuration';
import { CoreService } from 'app/services/shared/core.service';
import { MessageBase } from 'app/core/messageBaseComponent';
import * as fromMessage from '../../../../actions/layout/growlMessage';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-copy-move-items',
  templateUrl: './picklist-copy-move-items.component.html',
  styleUrls: ['./picklist-copy-move-items.component.css']
})
export class PicklistCopyMoveItemsComponent extends MessageBase implements OnInit {
  @Input() sections: LabelValueInt[];
  @Input() selectedItems: any[];
  @Input() type: string;
  @Output() Successed = new EventEmitter();



  destinationSection: LabelValueInt;


  constructor(
    private cs: CoreService,
    protected store: Store<fromRoot.State>,
  ) {
    super(store);
  }

  ngOnInit() {
  }

  get icon(): string {
    return this.type === 'Copy' ? 'fa-copy' : 'fa-arrow-right';
  }

  done() {
    this.cs.postDataExtra(CONFIG.apiURL.page.shared.pickList.copyMoveLines, this.postData)
      .subscribe(
        (res) => {
          if (this.PushResponseMessage(res.data)) {
            this.Successed.emit(res.extra);
          }
        }
      );
  }

  get postData(): any {
    return {
      quote_id: this.selectedItems[0].quote_id,
      revision: this.selectedItems[0].revision,
      items: this.selectedItems,
      destination: this.type === 'Copy' ? this.destinationSection : [this.destinationSection],
      type: this.type,
    }
  }

}
