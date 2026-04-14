
import { Component, OnInit, Output, Input,EventEmitter } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-address',
  templateUrl: './customer-address.component.html',
  styleUrls: ['./customer-address.component.css']
})
export class CustomerAddressComponent extends CustomerFormBase implements OnInit {

  @Output() address_updated = new EventEmitter();
  @Input() disabled:boolean;
  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
  }

  addressChanged(event) {
    CONFIG.LOG(event.result.length, 'address changed');
    this.address_updated.emit(event);
  }

  get isdev() {
    return CONFIG.ISDEV();
  }
}
