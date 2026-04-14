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
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-edit',
  templateUrl: './customer-edit.component.html',
  styleUrls: ['./customer-edit.component.css']
})
export class CustomerEditComponent extends MessageBase implements OnInit {

  private sub: any;

  public customer_id: number;
  hasError = false;
  errorMessage: string;

  constructor(
    protected store: Store<fromRoot.State>,
    private cs: CoreService,
    private customerS: CustomersService,
    private route: ActivatedRoute,
    private ts: TokenService,
    private titleService: Title,
  ) {
    super(store);
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.customer_id = +params['customer_id'];
      this.titleService.setTitle('#' + this.customer_id + ' - Customer');

      if (!this.customer_id || this.customer_id === NaN || this.customer_id === 0) {
        this.hasError = true;
        // tslint:disable-next-line:max-line-length
        this.errorMessage = ('Customer # not supplied.');
        this.PushErrorResponseMessage(this.errorMessage);
      }
    });
  }

}
