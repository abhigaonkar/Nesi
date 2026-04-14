import { Component, OnInit, Input } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { CONFIG } from 'app/configuration';
import { BannerService } from 'app/services/layout/banner.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { MessageBase } from 'app/core/messageBaseComponent';
import { Observable } from 'rxjs/Observable';
// tslint:disable-next-line:import-blacklist
import { Subscription } from 'rxjs';
import { TokenService } from '../../../../services/authentication/tokenService';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-breadcurmb',
  templateUrl: './breadcurmb.component.html',
  styleUrls: ['./breadcurmb.component.css']
})
export class BreadcurmbComponent extends MessageBase implements OnInit {
  @Input() items: MenuItem[];
  @Input() isHome = false;
  @Input() no_default = false;

  home: MenuItem;
  pageId = -1;
  PageId$: Observable<number>;
  private subscriptionPageId: Subscription;
  defaultPageId = -1;
  lastItemLabel: string;

  constructor(
    private bs: BannerService,
    protected store: Store<fromRoot.State>,
    private ts: TokenService,
  ) {
    super(store);
    this.PageId$ = store.select(fromRoot.getCurrentuser.getCurrentPageId);
  }

  ngOnInit() {
    this.no_default = this.no_default || window.location.href.includes('/#/opens/');
    this.home = { icon: 'fa fa-home', routerLink: this.isHome ? '' : '/home/1/1/homepage/default', routerLinkActiveOptions: false }
    this.lastItemLabel = this.items[this.items.length - 1].label;
    this.subscriptionPageId = this.PageId$
      .subscribe((value: number) => this.pageId = value);
    if (!this.ts.isContact) {
      this.bs.getData<number>(CONFIG.apiURL.layout.defaultPage)
        .subscribe(
          (res) => {
            this.defaultPageId = res;
            if (!this.no_default) {
              this.addSetDefaultPageLink();
            }
          }
        );
    }
  }

  addSetDefaultPageLink() {
    if (!(this.defaultPageId && this.defaultPageId === this.pageId)) {
      this.items[this.items.length - 1].label = this.lastItemLabel + ' (Click to set as default page)';
      this.items[this.items.length - 1].command = () => this.setDefault();
    } else {
      this.items[this.items.length - 1].label = this.lastItemLabel;
      this.items[this.items.length - 1].command = null;
    }
  }
  setDefault(): void {
    if (this.defaultPageId && this.defaultPageId === this.pageId) {
      return;
    }
    CONFIG.LOG(this.pageId, 'setting default page to');

    if (this.pageId) {
      this.bs.saveDefaultPage(this.pageId).subscribe((response) => {
        this.defaultPageId = this.pageId;
        this.addSetDefaultPageLink();
        this.PushSuccessMessage('Default page set to ' + response);
      });
    }
  }

  get text() {
    if (this.defaultPageId < 0 || this.pageId < 0) {
      return '';
    }
    return this.defaultPageId === this.pageId ? ' ' : '(Make this my default page)'
  }
}
