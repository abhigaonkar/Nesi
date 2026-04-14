
import { Component, OnInit,Input } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-address-files',
  templateUrl: './customer-address-files.component.html',
  styleUrls: ['./customer-address-files.component.css']
})
export class CustomerAddressFilesComponent extends CustomerFormBase implements OnInit {
  fileDirectory: any;
  @Input() disabled:boolean;
  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.core.fileManager.customerFiles
    );
  }

  AfterProfileLoaded() {
    this.fileDirectory=this.profile;
  }
}
