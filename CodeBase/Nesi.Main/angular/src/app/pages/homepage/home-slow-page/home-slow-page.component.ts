import { Component, OnInit } from '@angular/core';
import { WindowRef } from 'app/services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CONFIG } from 'app/configuration';
import { HomePageBase } from 'app/pages/homepage/home-base';

@Component({
  selector: 'nesi-home-slow-page',
  templateUrl: './home-slow-page.component.html',
  styleUrls: ['./home-slow-page.component.css']
})
export class HomeSlowPageComponent extends HomePageBase implements OnInit {


  constructor(
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
  ) {
    super(cs, store);
  }

  get Url(): string {
    return CONFIG.apiURL.page.homepage.slowPage;
  }

}

