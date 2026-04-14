import { Injectable } from '@angular/core';
import { TokenService } from './tokenService';
import { HttpService } from '../../core/http.service';
import { AuthorizeService } from './authorize.Service';
import { CONFIG } from '../../configuration';
import { ActivatedRoute } from '@angular/router';
import {
  CanActivate,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from '@angular/router';

import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import * as fromMessage from '../../actions/layout/growlMessage';
import { MenuService } from '../layout/menuService';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AuthGuard implements CanActivate {
  pageId: number;
  url: string;

  get originPath() {
    const v = sessionStorage.getItem(CONFIG.authentication.originPath);
    if (v) {
      return v;
    } else {
      return null;
    }
  }
  constructor(private ts: TokenService,
    private http: HttpService,
    private as: AuthorizeService,
    private router: Router,
    private route: ActivatedRoute,
    protected store: Store<fromRoot.State>,
    private ms: MenuService,
  ) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> {
    if (state && state.url) {
      CONFIG.LOG(state.url, 'url in can active');
      this.url = state.url.toString().toLowerCase();
      if ((this.url.startsWith('/home') || this.url.startsWith('/opens'))) {
        if (this.url.indexOf('.aspx') > -1) {
          this.url = this.url.replace(/\&/g, '%26');
        }
        this.ts.CurrentURL = this.url;
      }
      const urls = this.url.substring(1).split('/');
      if (urls.length >= 2) {
        if (urls[1] === '1') {
          this.pageId = Number(urls[2]);
        } else if (this.url.startsWith('/home') && !isNaN(Number(urls[3]))) {
          this.pageId = Number(urls[3]);
        } else {
          this.pageId = Number(urls[1]);
        }
      } else {
        this.pageId = 0;
      }
      this.store.dispatch(new fromCurrentUser.SetCurrentPageId(this.pageId));
    }

    if (this.checkAuthorization()) {
      return true;
    } else {
      this.ts.deniedUrl = this.url;
      return false;
    }
  }

  private checkMenu(): boolean {

    CONFIG.LOG(this.pageId, 'check menu this pageid');

    if (this.pageId === 0 || isNaN(this.pageId)) { return true; }
    const p = this.ms.checkMenuPrivilege(this.pageId)
    return p;
  }

  private checkAuthorization(): boolean {
    CONFIG.LOG(this.ts.checkAuthentication(), 'isauthentication ? check guard');
    if (this.ts.checkAuthentication()) {
      CONFIG.LOG(this.ts.isFvrPassed, 'fvr passed? check guard');
      CONFIG.LOG((this.ts.isFocedChangePassword), 'is Foced Change Password? check gaurd');

      if (this.ts.isFocedChangePassword || !this.ts.isFvrPassed) {
        CONFIG.LOG('redirec to signin due to fvr passed? check guard');
        this.router.navigate(['/signin']);
        return false;
      } else {
        const p = this.checkMenu()
        CONFIG.LOG(p, 'check menu ?');
        return p;
      }
    } else {
      this.ts.clearAuthData();
      this.router.navigate(['/signin']);
      return false;
    }
  }

  // private checkRefreshToken(): Observable<boolean> {
  //   // console.log('refresh token');
  //   CONFIG.LOG(this.ts.currentAuthData.expireSecond, 'start refresh token');
  //   return this.as.refreshToken(this.ts.currentAuthData.refresh_token)
  //     .catch((err: any, caught: Observable<any>) => {
  //       const r = new Observable<boolean>((o) => {
  //         o.next(false);
  //         o.complete();
  //         CONFIG.LOG('redirec to signin due to refreshtoken failed', 'checkrefreshtoken');
  //         this.store.dispatch(new fromMessage.PushErrorMessage('[501] Refresh token failed, please signin again.'));
  //         this.ts.clearAuthData();
  //         this.router.navigate(['/signin']);
  //       });
  //       return r;
  //     })
  //     .map<any, boolean>(data => {
  //       if (!data) { return false; }
  //       CONFIG.LOG(data.json(), 'refresh token success.');
  //       this.ts.handleSignIn(data.json());
  //       return true;
  //     });
  // }
}
