import { Component, AfterViewInit, ElementRef, Renderer, ViewChild, OnInit, OnDestroy, forwardRef, Inject } from '@angular/core';
import { TokenService } from '../../services/authentication/tokenService';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { Profile } from '../../models/layout/profile';
import { LayoutProfileService } from '../../services/layout/layoutProfile.services';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromLayoutProfiles from '../../actions/layout/layoutPorfile';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import * as fromMessage from '../../actions/layout/growlMessage';
import { Router, ActivatedRoute,  RouterEvent, NavigationEnd, NavigationCancel, NavigationStart, NavigationError } from '@angular/router';
import { DeviceService } from '../../services/authentication/device';
import { HubEvent } from '../../models/authentication/hubEvent';
import { CONFIG } from '../../configuration';
import { SignalRService } from '../../services/authentication/signalR.service';
import { AuthorizeService } from '../../services/authentication/authorize.Service';
import { MenuService } from '../../services/layout/menuService';
import { MessageBase } from '../../core/messageBaseComponent';
import { ClearMessage } from '../../actions/layout/growlMessage';
import { CoreService } from 'app/services/shared/core.service';
import { FlyoutComponent } from 'app/components/flyout/flyout.component';
import { WindowRef } from 'app/services/shared/windowRef';

enum MenuMode {
  STATIC,
  OVERLAY,
  SLIM
};

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-home',
  templateUrl: './Home.component.html',
  styleUrls: ['./Home.component.css']
})

export class HomeComponent extends MessageBase implements OnInit, AfterViewInit, OnDestroy {


  layoutLoaded = false;


  menu: MenuMode = MenuMode.STATIC;

  layout: String = 'default';

  favIcon: String = 'assets\images\Logos\nesi-logo-ico.png';

  darkMenu: boolean;

  documentClickListener: Function;

  staticMenuInactive: boolean;

  overlayMenuActive: boolean;

  mobileMenuActive: boolean;

  menuClick: boolean;

  menuButtonClick: boolean;

  topbarMenuButtonClick: boolean;

  topbarMenuClick: boolean;

  topbarMenuActive: boolean;

  activeTopbarItem: Element;

  resetSlim: boolean;
  showFlyOut = false;
  menuMode: string;
  menuStyle: string;
  layoutColor: string;
  theme: string;
  public allowToRunCheckURL = false;

  private subscriptionMenuMode: Subscription;
  private subscriptionMenuStyle: Subscription;
  private subscriptionLayoutColor: Subscription;
  private subscriptionTheme: Subscription;
  MenuMode$: Observable<string>;
  MenuStyle$: Observable<string>;
  LayoutColor$: Observable<string>;
  Theme$: Observable<string>;
  pageId: number;
  urlTime: any;

  constructor(
    public renderer: Renderer,
    public ts: TokenService,
    private ss: SignalRService,
    private as: AuthorizeService,
    private ms: MenuService,
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    private lps: LayoutProfileService,
    private router: Router,
    private route: ActivatedRoute,
    private device: DeviceService,
    private win: WindowRef
  ) {
    super(store);
    this.MenuMode$ = store.select(fromRoot.getLayoutProfiles.MenuMode);
    this.MenuStyle$ = store.select(fromRoot.getLayoutProfiles.MenuStyle);
    this.LayoutColor$ = store.select(fromRoot.getLayoutProfiles.LayoutColor);
    this.Theme$ = store.select(fromRoot.getLayoutProfiles.Theme);
  }


  ngOnInit(): void {
    // this.ss.connectSignalR();
    // this.ts.FireSignalREvent(CONFIG.SignalR.Events.Connected.name);
    // this.route.firstChild.url.subscribe((url) => {
    //   if (url[0] && url[1]) {
    //     const v1 = url[0].toString();
    //     const v2 = url[1].toString();
    //     if (v1 !== '1') {
    //       this.pageId = Number(v1);
    //     } else {
    //       this.pageId = Number(v2);
    //     }
    //   } else {
    //     this.pageId = 1;
    //   }
    //   this.store.dispatch(new fromCurrentUser.SetCurrentPageId(this.pageId));
    // });

    CONFIG.LOG(this.ts.isContact, 'home init')
    this.as.validateToken()
      .subscribe(res => {
        this.ts.handleOnlineUser(res);
        this.win.invokeSignIn();
      },
        err => {
          CONFIG.LOG(err, 'err in as');
          // this.router.navigate(['/signout']);
        });

    this.subscriptionMenuMode = this.MenuMode$
      .subscribe((value: string) => {
        this.menuMode = value;
        switch (this.menuMode) {
          case 'overlay':
            this.changeToOverlayMenu();
            break;
          case 'slim':
            this.changeToSlimMenu();
            break;
          case 'static':
          default:
            this.changeToStaticMenu();
            break;
        }
      });

    this.subscriptionMenuStyle = this.MenuStyle$
      .subscribe((value: string) => {
        this.menuStyle = value;
        switch (this.menuStyle) {
          case 'dark':
            this.darkMenu = true;
            break;
          case 'light':
          default:
            this.darkMenu = false;
            break;
        }
      });
    this.subscriptionLayoutColor = this.LayoutColor$
      .subscribe((value: string) => {
        this.layoutColor = value;
        this.changeLayout(value);
      });
    this.subscriptionTheme = this.Theme$
      .subscribe((value: string) => {
        this.layoutLoaded = true;
        this.theme = value;
        this.changeTheme(value);
      });
    this.store.dispatch(new fromLayoutProfiles.LoadLayoutProfiles());
    this.changeIcon(this.ts.currentUser.icon);

    this.router.events.subscribe( event => {
      if (event instanceof NavigationStart) {
        this.allowToRunCheckURL = false;
      }

      if (event instanceof NavigationEnd) {
        this.allowToRunCheckURL = true;
      }

      if (event instanceof NavigationError) {

      }

      if (event instanceof NavigationCancel) {
      }

    });

    this.urlTime = setInterval(() => {
      this.checkURL();
    }, 500);

  }


  ngAfterViewInit() {

    this.documentClickListener = this.renderer.listenGlobal('body', 'click', (event) => {
      if (!this.menuClick && !this.menuButtonClick) {
        this.mobileMenuActive = false;
        this.overlayMenuActive = false;
        this.resetSlim = true;
      }

      if (!this.topbarMenuClick && !this.topbarMenuButtonClick) {
        this.topbarMenuActive = false;
      }

      this.menuClick = false;
      this.menuButtonClick = false;
      this.topbarMenuClick = false;
      this.topbarMenuButtonClick = false;
    });

  }

  checkURL() {
    if (!this.allowToRunCheckURL) {
      return;
    }

    CONFIG.LOG(this.router.url, 'url in homepage');
    const u = this.router.url;
    if (u.startsWith('/home/1/') && !u.endsWith('default')) {
      CONFIG.LOG('redirect to new url', 'url in homepage');
      this.router.navigate([u + '/default']);
    } else {
      window.clearInterval(this.urlTime);
    }
  }

  onMenuButtonClick(event: Event) {
    this.ClearMessage();
    this.menuButtonClick = true;

    if (this.cs.isSm || this.device.isMobile) {
      this.mobileMenuActive = !this.mobileMenuActive;
    } else {
      if (this.staticMenu) {
        this.staticMenuInactive = !this.staticMenuInactive;
        this.ms.menuHide = this.staticMenuInactive;
      } else if (this.overlayMenu) {
        this.overlayMenuActive = !this.overlayMenuActive;
        this.ms.menuHide = true;
      }
    }

    event.preventDefault();
  }

  onTopbarMenuButtonClick(event: Event) {
    this.ClearMessage();
    this.topbarMenuButtonClick = true;
    this.topbarMenuActive = !this.topbarMenuActive;
    event.preventDefault();
  }

  onTopbarItemClick(event: Event, item: Element) {
    this.ClearMessage();
    if (this.activeTopbarItem === item) {
      this.activeTopbarItem = null;
    } else {
      this.activeTopbarItem = item;
    }
    event.preventDefault();
  }

  onTopbarMenuClick(event: Event) {
    this.ClearMessage();
    this.topbarMenuClick = true;
  }

  onMenuClick(event: Event) {
    this.ClearMessage();
    this.menuClick = true;
    this.resetSlim = false;
  }

  hideTopbarMenu() {
    try {
      this.activeTopbarItem = null;
      this.topbarMenuActive = false;

    }
    finally {

    }
  }

  get slimMenu(): boolean {
    return this.menu === MenuMode.SLIM;
  }

  get overlayMenu(): boolean {
    return this.menu === MenuMode.OVERLAY;
  }

  get staticMenu(): boolean {
    return this.menu === MenuMode.STATIC;
  }

  changeToSlimMenu() {
    this.menu = MenuMode.SLIM;
  }

  changeToOverlayMenu() {
    this.menu = MenuMode.OVERLAY;
  }

  changeToStaticMenu() {
    this.menu = MenuMode.STATIC;
  }


  changeTheme(theme) {
    // if (theme) {
    //   const themeLink: HTMLLinkElement = <HTMLLinkElement>document.getElementById('theme-css');
    //   themeLink.href = 'assets/theme/theme-' + theme + '.css';
    // }
  }

  changeLayout(layout) {
    // if (layout) {
    //   this.layout = layout;
    //   const layoutLink: HTMLLinkElement = <HTMLLinkElement>document.getElementById('layout-css');
    //   layoutLink.href = 'assets/layout/css/layout-' + layout + '.css';
    // }
  }
  changeIcon(favIcon) {
    if (favIcon) {
      this.favIcon = favIcon;
      const favIconLink: HTMLLinkElement = <HTMLLinkElement>document.getElementById('fav-icon');
      favIconLink.href = 'assets/images/Logos/' + favIcon;
    }
  }

  ngOnDestroy() {

    if (this.subscriptionMenuMode) {
      this.subscriptionMenuMode.unsubscribe();
    }
    if (this.subscriptionLayoutColor) {
      this.subscriptionLayoutColor.unsubscribe();
    }
    if (this.subscriptionMenuStyle) {
      this.subscriptionMenuStyle.unsubscribe();
    }
    if (this.subscriptionTheme) {
      this.subscriptionTheme.unsubscribe();
    }
    this.layoutLoaded = false;
  }

  flyOut() {
    CONFIG.LOG(this.ts.isContact, 'isContact?');
    if (!this.ts.isContact) {
      this.showFlyOut = true;
      // this.flyoutComp.loadRelatedTickets(this.pageId);
    }
  }


}

