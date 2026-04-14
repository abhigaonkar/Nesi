import { Component, OnInit } from '@angular/core';
import { TokenService } from '../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import { LayoutProfileService } from '../services/layout/layoutProfile.services';
import * as fromRoot from '../reducers';
import * as fromLayoutProfiles from '../actions/layout/layoutPorfile';
import { MessageBase } from '../core/messageBaseComponent';
import { MenuService } from '../services/layout/menuService';
import { CONFIG } from '../configuration';
import { Router } from '@angular/router';
import * as fromCurrentUser from '../actions/layout/currentUser';
import { CoreService } from '../services/shared/core.service';
import { SignalRService } from '../services/authentication/signalR.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-opens',
  template: `
  <router-outlet></router-outlet>`,
})
export class OpensComponent extends MessageBase implements OnInit {
  favIcon: string

  constructor(
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    private ms: MenuService,
    private router: Router,
    public cs: CoreService,
    private ss: SignalRService,
  ) {
    super(store);
  }

  ngOnInit() {
    //  this.ss.connectSignalR();
    if (this.ts.currentUser) {
      this.changeIcon(this.ts.currentUser.icon);
      this.store.dispatch(new fromLayoutProfiles.LoadLayoutProfiles());
      this.ms.getMenus().subscribe((res) => {
        this.ms.menus = res;
      });
      this.ms.getMobileMenus().subscribe((res) => {
        this.ms.mobilemenus = res;
      });
    }
    // this.getRebootTime();
  }

  changeIcon(favIcon) {
    if (favIcon) {
      this.favIcon = favIcon;
      const favIconLink: HTMLLinkElement = <HTMLLinkElement>document.getElementById('fav-icon');
      favIconLink.href = 'assets/images/Logos/' + favIcon;
    }
  }

  //  rebootEnd() {
  //   CONFIG.LOG('time end', 'rebootEnd in opens');
  //   this.router.navigate(['/signout']);
  // }




  // getRebootTime() {
  //   const host = window.location.hostname.replace(/\./g, '');
  //   this.cs.getData<number>(CONFIG.apiURL.page.releaseSystem.rebootTime + '/' + host)
  //     .subscribe(
  //     (res: number) => {
  //       if (!res) { res = 0; }
  //       CONFIG.LOG(res, 'reboot time in getreboottime in banner');
  //       if (res < 0) {
  //         this.router.navigate(['/signout']);
  //         // this.rebootTime = 0;
  //       } else if (res > 0) {
  //        // this.rebootTime = res;
  //       //  this.store.dispatch(new fromCurrentUser.SetRebootTime(res));
  //       }
  //     }
  //     );
  // }

}
