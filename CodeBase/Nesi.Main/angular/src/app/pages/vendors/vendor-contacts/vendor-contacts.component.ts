import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { VendorFormBase } from '../_base/vendorFormBase';

@Component({
  selector: 'nesi-vendor-contacts',
  templateUrl: './vendor-contacts.component.html',
  styleUrls: ['./vendor-contacts.component.css']
})
export class VendorContactsComponent extends VendorFormBase implements OnInit {

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
  ) {
    super(store, cs);
  }

  ngOnInit() {
  }


}
