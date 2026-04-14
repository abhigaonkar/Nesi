
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { VendorFormBase } from '../_base/vendorFormBase';

@Component({
  selector: 'nesi-vendor-files',
  templateUrl: './vendor-files.component.html',
  styleUrls: ['./vendor-files.component.css']
})
export class VendorFilesComponent extends VendorFormBase implements OnInit {
  fileDirectory: any;
  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.InitVendor(
      CONFIG.apiURL.core.fileManager.vendorFiles
    );
  }

  AfterProfileLoaded() {
    this.fileDirectory = this.profile;
  }
}
