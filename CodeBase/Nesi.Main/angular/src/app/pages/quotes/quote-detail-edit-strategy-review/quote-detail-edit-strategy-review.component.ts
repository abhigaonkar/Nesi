import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import * as fromRoot from '../../../reducers';
import { CONFIG } from 'app/configuration';
import { MessageBase } from 'app/core/messageBaseComponent';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-quote-detail-edit-strategy-review',
  templateUrl: './quote-detail-edit-strategy-review.component.html',
  styleUrls: ['./quote-detail-edit-strategy-review.component.css']
})
export class QuoteDetailEditStrategyReviewComponent extends MessageBase implements OnInit {

  @Input()
  enabled: boolean;
  @Input()
  name: string;
  @Input()
  value: string;
  @Input()
  quote_id: string;
  @Output()
  done = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store);
  }

  get url() {
    return CONFIG.apiURL.page.quotes.strategyUpdateItem + this.quote_id.toString();
  }


  save(type: string) {
    const obj = {
      name: this.name + '_' + type,
      field: this.name,
      value: this.value,
      index: 0
    };
    this.postValueChange(obj, type);
  }

  postValueChange(obj, type) {
    this.cs.postString(this.url, obj)
      .subscribe(
      (res: string) => {
        if (this.PushShortResponseMessage(res)) {
          if (type === 'done') {
            this.done.emit();
          }
        }
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }


  ngOnInit() {
  }

}
