import { Component, OnInit } from '@angular/core';
import { WindowRef } from 'app/services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CONFIG } from 'app/configuration';
import { HomePageBase } from 'app/pages/homepage/home-base';

@Component({
  selector: 'nesi-home-invoice-service',
  templateUrl: './home-invoice-service.component.html',
  styleUrls: ['./home-invoice-service.component.css']
})
export class HomeInvoiceServiceComponent extends HomePageBase implements OnInit {


  constructor(
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
  ) {
    super(cs, store);
  }

  get Url(): string {
    return CONFIG.apiURL.page.homepage.invoiceService;
  }

}

