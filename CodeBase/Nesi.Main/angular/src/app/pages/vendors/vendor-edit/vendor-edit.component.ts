import { Component, OnInit, Input } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from 'app/core/messageBaseComponent';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { CustomersService } from 'app/services/pages/customer.services';
import { TokenService } from 'app/services/authentication/tokenService';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'nesi-vendor-edit',
  templateUrl: './vendor-edit.component.html',
  styleUrls: ['./vendor-edit.component.css']
})
export class VendorEditComponent extends MessageBase implements OnInit {

  private sub: any;

  public vendor_id: number;
  hasError = false;
  errorMessage: string;

  constructor(
    protected store: Store<fromRoot.State>,
    private cs: CoreService,
    private route: ActivatedRoute,
    private ts: TokenService,
    private titleService: Title,
  ) {
    super(store);
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.vendor_id = +params['vendor_id'];
      this.titleService.setTitle('#' + this.vendor_id + ' - Vendor');

      if (!this.vendor_id || this.vendor_id === NaN || this.vendor_id === 0) {
        this.hasError = true;
        // tslint:disable-next-line:max-line-length
        this.errorMessage = ('Vendor # not supplied.');
        this.PushErrorResponseMessage(this.errorMessage);
      }
    });
  }

}
