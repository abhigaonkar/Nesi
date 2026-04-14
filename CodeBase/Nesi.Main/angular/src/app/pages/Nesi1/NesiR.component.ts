import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CONFIG } from '../../configuration';
import { TokenService } from '../../services/authentication/tokenService';
import { LoaderService } from 'app/core/loader/loader.service';
import * as fromRoot from '../../reducers';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
// tslint:disable-next-line:import-blacklist
import { Subscription } from 'rxjs';
import { RouterExtService } from '../../services/layout/routerExtService';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-NesiR',
  template: ``,
})
export class NesiRComponent implements OnInit, AfterViewInit {
  private isMobile: boolean;
  private isMobile$: Observable<boolean>;
  private subIsMobile: Subscription;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private ts: TokenService,
    private loadingService: LoaderService,
    private store: Store<fromRoot.State>,
  ) {
    this.isMobile$ = this.store.select(fromRoot.getCurrentuser.getIsMobile);

  }

  ngOnInit() {
    this.subIsMobile = this.isMobile$.subscribe(res => this.isMobile = res);
    this.loadingService.hide();
    this.route.params.subscribe(
      (u) => {
        CONFIG.LOG(u, 'url in nesiR');
        this.ts.nesi1IframeUrl = '';
        this.ts.nesi1IframeUrlParent = '';
        const id = u['id'];
        // const prevUrl = this.routerExt.getPreviousUrl();
        // if (prevUrl && prevUrl.indexOf('/' + id + '/') > -1 && (prevUrl.indexOf('/home/1/') === -1 || prevUrl.indexOf('/n2') > -1) {
        //   this.router.navigate([prevUrl]);
        //   return;
        // }
        if (this.isReleaseMenu(id) || this.isBetaMenu(id)) {
          return;
        } else {
          this.router.navigate(['home', '1', id, 'default']);
        }
      }
    )
  }

  isBetaMenu(id: number): boolean {
    if (!CONFIG.ISALPHA() && (CONFIG.ISBETA() || CONFIG.ISDEBUG() || CONFIG.ISDEV())) {
      const item = CONFIG.betaMenus.find(x => x.id === Number(id)
        && ((CONFIG.ISBETA() && !x.dev_beta_only) || !CONFIG.ISBETA()));
      if (item && (item.hide_badge ||  (CONFIG.ISBETA() && this.ts.currentUser.force_beta))) {
        this.router.navigate([item.routerLink]);
        return true;
      } else {
        return false;
      }
    }
  }

  isReleaseMenu(id: number): boolean {
    const item = CONFIG.releaseMenus.find(x => x.id === Number(id));
    if (item && (item.hide_badge || (CONFIG.ISBETA() && this.ts.currentUser.force_beta)) && !(this.isMobile && item.mobileN1Link)) {
      this.router.navigate([item.routerLink]);
      return true;
    } else {
      return false;
    }
  }

  ngAfterViewInit(): void {
    // this.router.navigate([this.router.url + '/default']);
  }
}
