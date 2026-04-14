import { Component, OnInit, Input } from '@angular/core';
import { CONFIG } from '../../../configuration';
import { WindowRef } from '../../../services/shared/windowRef';
import { QuoteBase } from '../_base/quoteBase';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-list-item',
  templateUrl: './quote-list-item.component.html',
  styleUrls: ['./quote-list-item.component.css']
})
export class QuoteListItemComponent extends QuoteBase implements OnInit {
  @Input()
  item: any[];
  tooltipX: string;
  tooltipY: string;
  @Input()
  smaller = false;
  constructor(
    protected winRef: WindowRef,
    protected store: Store<fromRoot.State>
  ) {
    super(winRef, store);
  }

  ngOnInit() {

  }


  cutCustomerName(name: string): string {
    if (!name) { return ''; }
    return name.length > 14 ? name.substring(0, 12) : name;
  }
  moveTip(event: any) {
    this.tooltipX = (event.clientX + 20) + 'px';
    this.tooltipY = (event.clientY + 20) + 'px';
  }

  myopenQuote(line: any) {
    // if (line.allowed_color) { return; }
    this.openQuote(line.quote_id, line.revision);
  }

  disabled_click(line: any) {
    return false;
    // return (line.allowed_color && (line.status_id==10 || line.status_id==11));
  }
}
