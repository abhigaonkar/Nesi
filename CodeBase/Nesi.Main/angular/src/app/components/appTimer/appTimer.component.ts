import { Component, OnInit } from '@angular/core';
import { TokenService } from '../../services/authentication/tokenService';
import { Router } from '@angular/router';
import { CoreService } from '../../services/shared/core.service';
import { CONFIG } from '../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import * as fromProfile from '../../actions/layout/layoutPorfile';

import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { AuthorizeService } from '../../services/authentication/authorize.Service';
import { SignalRService } from '../../services/authentication/signalR.service';
import { Subscription } from 'rxjs';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-appTimer',
  templateUrl: './appTimer.component.html',
  styleUrls: ['./appTimer.component.css']
})
export class AppTimerComponent implements OnInit {
  display = false;
 public countdown;
  interval: any;
  intervalGlobal: any;
  // refreshTime: number;
  // refreshTime$: Observable<number>;
  // subrefreshTime: Subscription;
  signOutTime$: Observable<number>;
  subSignOutTime: Subscription;
  signOutTime: number;

  constructor(
    private ts: TokenService,
    private router: Router,
    public cs: CoreService,
    private store: Store<fromRoot.State>,
    private as: AuthorizeService,
    private ss: SignalRService,
  ) {
    this.signOutTime$ = store.select(fromRoot.getLayoutProfiles.SignOutTime);
    this.subSignOutTime = this.signOutTime$.subscribe(
      (res: number) => {
        this.signOutTime = res;
      }
    )
    // this.subrefreshTime = this.refreshTime$.subscribe(
    //   (res: number) => {
    //     if (this.isHomeOrOpen && res > 0) {
    //       CONFIG.LOG(res, 'refreshtime in subrefreshtime keep sigin fired');
    //       this.stop();
    //     }
    //   }
    // );
  }

  ngOnInit() {
    this.init();
    this.intervalGlobal = window.setInterval(
      () => {
        if (!this.isHomeOrOpen) { return; }
        CONFIG.LOG(this.cs.secondsSinceLastActive, 'second since last active');
        CONFIG.LOG(this.signOutTime, 'second sign out time');
        if (!this.display && this.cs.secondsSinceLastActive >= this.signOutTime) {
          this.start();
        } else {
          this.ping();
        }
      }, CONFIG.timer.checkActiveTime * 1000);
  }

  init() {
    //  this.store.dispatch(new fromCurrentUser.SetRefreshTime(0));
    this.cs.secondsSinceLastActive = 0;
    this.cs.secondsSinceLastPing = 0;
    // localStorage.setItem(CONFIG.authentication.popupTime, '0');
  }

  get isHomeOrOpen() {
    return this.router.url.startsWith('/home') || this.router.url.startsWith('/opens');
  }

  ping() {
    if (this.cs.secondsSinceLastPing < CONFIG.timer.checkActiveTime) { return; }
    this.cs.s_postData<number>(CONFIG.apiURL.currentUser.ping, null)
      .subscribe(
      (res: number) => {
        if (res === 1) {
          this.cs.secondsSinceLastPing = 0;
        }
      }
      );
  }

  start() {
    if (!this.isHomeOrOpen) {
      this.display = false;
      return;
    }
    // const popupTime = localStorage.getItem(CONFIG.authentication.popupTime);
    // CONFIG.LOG(popupTime, 'popup time in start apptimer');
    // let popupSeconds = 0;
    // if (popupTime && popupTime !== '0') {
    //   popupSeconds = Math.ceil(((new Date()).getTime() - new Date(popupTime).getTime()) / 1000);
    //   CONFIG.LOG(popupSeconds, 'popupSeconds in start apptimer');
    // } else {
    //   localStorage.setItem(CONFIG.authentication.popupTime, new Date().toUTCString());
    // }
    this.countdown = CONFIG.timer.countDownTimeWhenAutoSignOut - (this.cs.secondsSinceLastActive - this.signOutTime);
    this.display = true;
    this.interval = window.setInterval(
      () => {
        if (this.cs.secondsSinceLastActive < this.signOutTime) {
          this.hide();
          return;
        }
        this.countdown--;
        if (this.countdown === 0) {
          // if (this.interval) {
          //   window.clearInterval(this.interval);
          // }
          // this.display = false;
          this.hide();
          // this.store.dispatch(new fromCurrentUser.SetRefreshTime(0));
          this.router.navigate(['/signout']);
        }
      }, 1000);
  }

  hide() {
    CONFIG.LOG('hide', 'apptimer hide display');
    if (this.interval) {
      window.clearInterval(this.interval);
    }
    this.display = false;
    // this.ss.FireKeepSiginAll();
  }

  stop() {
    // if (this.interval) {
    //   window.clearInterval(this.interval);
    // }
    // this.display = false;
    this.hide();
    this.as.validateToken()
      .subscribe(
      (res2) => {
        CONFIG.LOG(res2, 'reinit global interval for check active in apptimer');
        this.init();
      },
      (err) => {
        this.hide();
        this.router.navigate(['/signout']);
      }
      );
  }
}
