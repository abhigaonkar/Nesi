import { Component, OnInit, OnDestroy, Renderer2, AfterViewInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import { Observable } from 'rxjs/Observable';
import { Fvr } from '../../models/layout/fvr';
import { Subscription } from 'rxjs/Subscription';
import { Router } from '@angular/router';
import { CONFIG } from '../../configuration';
import { TokenService } from '../../services/authentication/tokenService';
import { OnlineUser } from '../../models/authentication/onlineUser';
import { WindowRef } from '../../services/shared/windowRef';
import { BannerService } from '../../services/layout/banner.service';
import { AuthorizeService } from '../../services/authentication/authorize.Service';
import { CoreService } from 'services/shared/core.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-fvr',
  templateUrl: './fvr.component.html',
  styleUrls: ['./fvr.component.css']
})
export class FvrComponent implements OnInit, OnDestroy, AfterViewInit {


  fvrs: Fvr[] = [];
  subFvr: Subscription;
  sub: Subscription;
  col: Number = 6;
  redBackground: String;
  constructor(
    private router: Router,
    private bs: BannerService,
    public ts: TokenService,
    private as: AuthorizeService,
    private win: WindowRef,
    private renderer: Renderer2,
    private cs: CoreService
  ) {

    this.renderer.addClass(document.body, 'login-body');
  }
  get originPath() {
    const v = sessionStorage.getItem(CONFIG.authentication.originPath);
    if (v) { return (v); } else { return '' }
  }

  set originPath(value: string) {
    const v = sessionStorage.setItem(CONFIG.authentication.originPath, (value));
  }

  ngOnInit() {
    this.bs.loadFVRList().subscribe(
      (res) => {
        this.fvrs = res;
        this.tryGoHome();
      });
    if (this.ts.isFvrPassed) {
      this.col = 4;
    } else {
      this.redBackground = 'redBG';
    }

    this.sub = Observable.interval(2000).subscribe(x => {
      this.checkLocalStore();
    });

  }
  ngAfterViewInit(): void {
    this.cs.loadBootstrapCSS();
  }

  checkLocalStore() {
    const FVR_check = localStorage.getItem('FVR_check');

    if (FVR_check === '1') {
      // this.sub.unsubscribe();
      localStorage.setItem('FVR_check', '-1');
      this.refresh();
    }

  }

  goHome() {
    this.win.invokeSignIn();
    if (this.originPath) {
      CONFIG.LOG(this.originPath, 'originPath url goto homepage');
      for (let i = 0; i < 10; i++) {
        this.originPath = decodeURIComponent(this.originPath);
      }
      // tslint:disable-next-line:max-line-length
      this.router.navigate(['/home/0/' + encodeURIComponent(this.originPath + (this.originPath.indexOf('is_n1') < 0 ? '?is_n1=true' : ''))]);
    } else if (this.ts.CurrentURL) {
      CONFIG.LOG(this.ts.CurrentURL, 'current url goto homepage');
      this.router.navigate([this.ts.CurrentURL]);
    } else {
      this.router.navigate(['/home/1']);
    }
  }

  goSignOut() {
    this.router.navigate(['/signout']);
  }

  refresh() {
    location.reload();
    // this.bs.loadFVRList().subscribe(
    //   (res) => {
    //     this.fvrs = res;
    //     this.tryGoHome();
    //   });
  }

  tryGoHome() {
    CONFIG.LOG(this.fvrs.length, 'fvr length in refresh fvr button');
    if (this.fvrs.length === 0) {
      this.ts.passFvr();
      CONFIG.LOG(this.ts.hasFvr, 'has fvr in refresh fvr button');
      CONFIG.LOG(this.ts.isFvrPassed, 'fvr passed in refresh fvr button');
      this.goHome();
    }
  }

  ngOnDestroy(): void {
    if (this.subFvr) { this.subFvr.unsubscribe() };
    this.renderer.removeClass(document.body, 'login-body');
    this.cs.unloadBootstrapCSS();
  }
}
