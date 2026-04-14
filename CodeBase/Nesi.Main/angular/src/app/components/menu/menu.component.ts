import { Component, Input, OnInit, EventEmitter, ViewChild, Inject, forwardRef, OnDestroy } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { HomeComponent } from '../../pages/home/home.component';
import { AppSubMenuComponent } from './appsubmenu.component';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { MenuService } from '../../services/layout/menuService';
import { NesiMenuItem, NesiMenuType } from '../../models/layout/nesiMenuItem';
import { TokenService } from '../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromNesiMenu from '../../actions/layout/nesiMenu';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import * as fromLayoutProfiles from '../../actions/layout/layoutPorfile';
import { Profile } from '../../models/layout/profile';
import { LayoutProfileService } from '../../services/layout/layoutProfile.services';
import { ChangeToDesktop } from '../../actions/layout/currentUser';
import { DeviceService } from '../../services/authentication/device';
import { CONFIG } from '../../configuration';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-mainmenu',
  templateUrl: './Menu.component.html',
  styleUrls: ['./Menu.component.css']
})
export class MainMenuComponent implements OnInit, OnDestroy {

  @Input()
  public showAll = false;

  public nesiMobileMenuItems$: Observable<Array<NesiMenuItem>>;
  public nesiMainMenuItems$: Observable<Array<NesiMenuItem>>;
  public isMobile$: Observable<boolean>;
  public isMobile: boolean;
  public isDeviceMobile: boolean;
  public isContact: boolean;

  private subscriptionMainMenu: Subscription;
  private subscriptionMobileMenu: Subscription;
  private subscriptionIsMobile: Subscription;

  private subscriptionToggle: Subscription;
  private subscriptionMenuMode: Subscription;
  private subscriptionMenuStyle: Subscription;
  menuMode: string;
  menuStyle: string;
  private menuMode$: Observable<string>;
  private menuStyle$: Observable<string>;

  redirect = '';
  isdev = false;
  expandAll$: Observable<boolean>;
  expandAll: boolean;

  constructor(
    @Inject(forwardRef(() => HomeComponent)) public app: HomeComponent,
    private router: Router,
    private store: Store<fromRoot.State>,
    private lps: LayoutProfileService,
    private ts: TokenService,
    private ms: MenuService,
    private device: DeviceService
  ) {
    this.nesiMainMenuItems$ = store.select(fromRoot.getMainMenus);
    this.nesiMobileMenuItems$ = store.select(fromRoot.getMobileMenus);
    this.expandAll$ = store.select(fromRoot.getMainMenuExpandAll);
    this.menuMode$ = store.select(fromRoot.getLayoutProfiles.MenuMode);
    this.menuStyle$ = store.select(fromRoot.getLayoutProfiles.MenuStyle);
    this.isMobile$ = store.select(fromRoot.getCurrentuser.getIsMobile);
    this.isDeviceMobile = device.isMobile;
    this.isContact = this.ts.isContact;
    this.isdev = CONFIG.ISDEV();
  }


  gotoDevPage() {
    this.router.navigate([CONFIG.DEV_HOMEPAGE()]);
  }

  doSearch(searchText: string) {
    this.searchMenu(searchText);
  }

  searchMenu(searchText: string) {
    this.store.dispatch(new fromNesiMenu.SearchMenu(searchText));
  }

  changeToDesktop() {

    this.store.dispatch(new fromCurrentUser.ChangeToDesktop());
    this.redirect = '/home/1/1';

  }
  changeToMobile() {
    this.store.dispatch(new fromCurrentUser.ChangeToMobile());
    this.redirect = '/home/1/1?mobile';
  }

  ngOnInit() {
    this.subscriptionIsMobile = this.isMobile$
      .subscribe(res => {
        this.isMobile = res;
        if (this.redirect) {
          this.router.navigate([this.redirect]);
          this.redirect = '';
        }
      }
      );

    this.subscriptionMainMenu = this.nesiMainMenuItems$
      .subscribe(menus => {
        this.onGetMenuSuccess(menus);
      }, error => {
        this.onGetMenuError(error);
      });

    this.subscriptionMobileMenu = this.nesiMobileMenuItems$
      .subscribe(menus => {
        this.onGetMenuSuccess(menus);
      }, error => {
        this.onGetMenuError(error);
      });

    this.subscriptionToggle = this.expandAll$
      .subscribe(item => {
        this.expandAll = item;
      });

    this.subscriptionMenuMode = this.menuMode$
      .subscribe((value: string) =>
        this.menuMode = value
      );

    this.subscriptionMenuStyle = this.menuStyle$
      .subscribe((value: string) => this.menuStyle = value);

    if (this.ts.isContact) {
      this.store.dispatch(new fromNesiMenu.LoadMenu());
    }
    this.store.dispatch(new fromNesiMenu.SearchMenu(''));

  }


  ngOnDestroy() {
    this.subscriptionMainMenu.unsubscribe();
    this.subscriptionMobileMenu.unsubscribe();
    this.subscriptionIsMobile.unsubscribe();
    this.subscriptionToggle.unsubscribe();
    this.subscriptionMenuMode.unsubscribe();
    this.subscriptionMenuStyle.unsubscribe();
  }


  onGetMenuSuccess(items: NesiMenuItem[]): void {
    this.ms.menus = items;
  }

  onGetMenuError(err: any): void {
    this.router.navigate(['signout']);
  }


  toggleExpandAll() {
    this.expandAll = !this.expandAll;
    if (this.expandAll) {
      this.store.dispatch(new fromNesiMenu.OpenMenuExpandAll());
    } else {
      this.store.dispatch(new fromNesiMenu.CloseMenuExpandAll());
    }
  }
}
