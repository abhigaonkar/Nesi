import { Component, Input, OnInit, EventEmitter, ViewChild, Inject, forwardRef } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/primeng';
import { HomeComponent } from '../../pages/home/home.component';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import { MenuService } from '../../services/layout/menuService';
import { TokenService } from '../../services/authentication/tokenService';
import { WindowRef } from '../../services/shared/windowRef';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { isArray } from 'util';


@Component({
  // tslint:disable-next-line:component-selector
  selector: '[app-submenu]',
  templateUrl: './appsubmenu.component.html',
  styleUrls: ['./appsubmenu.component.css'],
  animations: [
    trigger('children', [
      state('hiddenAnimated', style({
        height: '0px'
      })),
      state('visibleAnimated', style({
        height: '*'
      })),
      state('visible', style({
        height: '*'
      })),
      state('hidden', style({
        height: '0px'
      })),
      transition('visibleAnimated => hiddenAnimated', animate('100ms cubic-bezier(0.86, 0, 0.07, 1)')),
      transition('hiddenAnimated => visibleAnimated', animate('100ms cubic-bezier(0.86, 0, 0.07, 1)'))
    ])
  ]
})
export class AppSubMenuComponent {

  @Input() item: MenuItem;

  @Input() root: boolean;

  @Input() visible: boolean;

  @Input() expandAll: boolean;
  @Input() menuStyle: string;

  _reset: boolean;

  activeIndex: number;

  hover: boolean;



  constructor(@Inject(forwardRef(() => HomeComponent)) public app: HomeComponent,
    public router: Router,
    public location: Location,
    private store: Store<fromRoot.State>,
    private ms: MenuService,
    public cs: CoreService,
    private ts: TokenService,
    private winRef: WindowRef,
  ) {

  }



  itemClick(event: MouseEvent, item: MenuItem, index: number, type: number = 0) {
    CONFIG.LOG(type, 'item click typein menu item');
    CONFIG.LOG(item, 'item click item value in menu item');
    const id = this.ms.getPageIdByMenuItem(item);
    if (id > 0) {
      this.store.dispatch(new fromCurrentUser.SetCurrentPageId(id));
    }

    // avoid processing disabled items
    if (item.disabled) {
      event.preventDefault();
      return true;
    }

    // activate current item and deactivate active sibling if any
    if (item.routerLink || item.items) {
      if (type === 3) {
        const u = item.routerLink + '/n2';
        if (event.ctrlKey || event.button === 1) {
          this.winRef.newTab('/#' + u);
          event.preventDefault();
          event.stopPropagation();
          return;
        } else if (event.shiftKey) {
          this.winRef.newWindow('/#' + u);
          event.preventDefault();
          event.stopPropagation();
          return;
        } else {
          event.preventDefault();
          event.stopPropagation();
          this.router.navigate([u]);
        }
      } else {
        if (item.items && item.routerLink) {
          const u = String(item.routerLink);
          if (event.ctrlKey || event.button === 1) {
            //  this.ts.CtrlURL = u;
            this.winRef.newTab('/#' + item.routerLink);
            event.preventDefault();
            event.stopPropagation();
            return;
          } else if (event.shiftKey) {
            // this.ts.CtrlURL = u;
            this.winRef.newWindow('/#' + item.routerLink);
            event.preventDefault();
            event.stopPropagation();
            return;
          } else if (type !== 2) {
            CONFIG.LOG(item.routerLink, 'item router link item value in menu item');
            event.preventDefault();
            if (isArray(item.routerLink)) {
              this.router.navigate(item.routerLink);
            } else {
              this.router.navigate([item.routerLink]);
            }
          }
        }
      }
      this.activeIndex = (this.activeIndex === index) ? null : index;
    }

    // execute command
    if (item.command) {
      item.command({ originalEvent: event, item: item });
    }

    // prevent hash change
    if (item.items || (!item.url && !item.routerLink) || type === 3) {
      event.preventDefault();
    }

    // hide menu
    if (!item.items || ((item.url || item.routerLink) && item.items.length === 0)) {
      if (this.app.overlayMenu || this.cs.isSm) {
        this.app.overlayMenuActive = false;
        this.app.mobileMenuActive = false;
      }

      if (!this.root && this.app.slimMenu) {
        this.app.resetSlim = true;
      }
    }
  }

  isActive(index: number): boolean {
    return this.activeIndex === index || this.expandAll;
  }

  unsubscribe(item: any) {
    if (item.eventEmitter) {
      item.eventEmitter.unsubscribe();
    }

    if (item.items) {
      for (const childItem of item.items) {
        this.unsubscribe(childItem);
      }
    }
  }

  @Input() get reset(): boolean {
    return this._reset;
  }

  set reset(val: boolean) {
    this._reset = val;

    if (this._reset && this.app.slimMenu) {
      this.activeIndex = null;
    }
  }
}
