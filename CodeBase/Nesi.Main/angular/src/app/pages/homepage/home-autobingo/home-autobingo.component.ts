import { Component, OnInit } from '@angular/core';
import { WindowRef } from 'app/services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CONFIG } from 'app/configuration';
import { HomePageBase } from 'app/pages/homepage/home-base';

@Component({
  selector: 'nesi-home-autobingo',
  templateUrl: './home-autobingo.component.html',
  styleUrls: ['./home-autobingo.component.css']
})
export class HomeAutobingoComponent extends HomePageBase implements OnInit {


  constructor(
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
  ) {
    super(cs, store);
  }

  get Url(): string {
    return CONFIG.apiURL.page.homepage.autoBingo;
  }


  getColor(x, data, doBackgroundColor) {
    // CONFIG.LOG(String(data.watch).toLowerCase().indexOf('line count'), 'line count index');
    if (x && String(data.watch).toLowerCase().indexOf('line count') > -1) {
      if (x.parentNode && x.parentNode.parentNode && data.count < 100) {
        if (doBackgroundColor){
        x.parentNode.parentNode.style.background = '#fcc';
        }
        x.parentNode.parentNode.style['font-weight'] = 'bold';
      } else {
        if (x.parentNode && x.parentNode.parentNode) {
          if (doBackgroundColor){
          x.parentNode.parentNode.style.background = '#cfc';
          }
        }
      }
    } else {
      if (x.parentNode && x.parentNode.parentNode && data.count > 0) {
        if (doBackgroundColor){
            x.parentNode.parentNode.style.background = '#fcc';
            }
        x.parentNode.parentNode.style['font-weight'] = 'bold';
      } else if (x.parentNode && x.parentNode.parentNode) {
        if (doBackgroundColor){
            x.parentNode.parentNode.style.background = '#cfc';
          }
        x.style['opacity'] = 0.5;
      }
    }
    if (x) {
      x.style.color = 'black';
    }
  }
}

