import { Injectable, OnInit } from '@angular/core';
import { Response } from '@angular/http';
import { CONFIG } from '../../configuration';
import { HttpService } from '../../core/http.service';
import { Observable } from 'rxjs/Observable';
import { NesiMenuItem, NesiMenuType } from '../../models/layout/nesiMenuItem';
import { ServiceBase } from '../shared/serviceBase';
import { WindowRef } from '../shared/windowRef';
import { MenuItem } from 'primeng/primeng';
import { Subject } from 'rxjs';
@Injectable()
export class MenuService extends ServiceBase implements OnInit {

  public menus: NesiMenuItem[];
  public mobilemenus: NesiMenuItem[];

  private _menus_Pageid: Number[];
  private _mobile_menus_PageId: Number[];
  _menuHide: boolean;

  public subjectMenuMode = new Subject<boolean>();

  constructor(protected http: HttpService,
    private windowRef: WindowRef,
  ) {
    super(http);

  }

  ngOnInit(): void {

  }

  public set menusPageid(value: Number[]) {
    this._menus_Pageid = value;
    localStorage.setItem(CONFIG.authentication.menuString, btoa(JSON.stringify(value)));
  }
  public set mobilemenusPageid(value: Number[]) {
    this._mobile_menus_PageId = value;
    localStorage.setItem(CONFIG.authentication.menuString + '_m', btoa(JSON.stringify(value)));
  }
  public get menusPageid(): Number[] {
    // CONFIG.LOG(this._menus_Pageid, ' menu in get menus() menu service');
    if (this._menus_Pageid) { return this._menus_Pageid; }
    const s = localStorage.getItem(CONFIG.authentication.menuString);
    if (s) {
      // CONFIG.LOG(localStorage.getItem(CONFIG.authentication.menuString), 'localstorage menu in get menus() menu service');
      this._menus_Pageid = JSON.parse(atob(s));
      return this._menus_Pageid;
    } else {
      return null;
    }
  }

  public get mobilemenusPageid(): Number[] {
    if (this._mobile_menus_PageId) { return this._mobile_menus_PageId; }
    const s = localStorage.getItem(CONFIG.authentication.menuString + '_m');
    if (s) {
      this._mobile_menus_PageId = JSON.parse(atob(s));
      return this._mobile_menus_PageId;
    } else {
      return null;
    }
  }

  public checkMenuPrivilege(pageId: number): boolean {
    if ((this.menusPageid && this.menusPageid.length > 0) || (this.mobilemenusPageid && this.mobilemenusPageid.length > 0)) {
      return this.checkmenu(pageId) || this.checkmobilemenu(pageId);
    } else {
      return false;
    }
  }

  public ConvertMenuToInt(menus: NesiMenuItem[]): Number[] {
    let result = [];
    menus.forEach(item => {
      result.push(item.id);
      if (item.items) {
        item.items.forEach(subitem => {
          result.push(subitem.id);
        });
      }
    });
    result = result.sort((a, b) => {
      if (a > b) { return 1; }
      if (a === b) { return 0; }
      if (a < b) { return -1; }
    });
    return result;
  }

  checkmenu(pageId: number): boolean {
    let result = false;
    if (this.menusPageid) {
      result = this.menusPageid.indexOf(pageId) > -1;
    }
    return result;
  }

  checkmobilemenu(pageId: number): boolean {
    let result = false;

    if (this.mobilemenusPageid) {
      result = this.mobilemenusPageid.indexOf(pageId) > -1;
    }
    return result;
  }

  getMenus(): Observable<NesiMenuItem[]> {
    return this.mapMenus(this.http.get(CONFIG.apiURL.layout.menus));

  }

  getMobileMenus(): Observable<NesiMenuItem[]> {
    return this.mapMenus(this.http.get(CONFIG.apiURL.layout.mobileMenus));

  }

  getPageUrl(id: string, isMobile: boolean): Promise<Response> {
    let url = CONFIG.apiURL.currentUser.pageURL + id;
    if (isMobile) {
      url += '?isMobile=true';
    }
    // console.log(url);
    return this.http.get(url).toPromise();
  }

  private mapMenus(response: Observable<Response>): Observable<NesiMenuItem[]> {
    return this.ConvertList<NesiMenuItem>(response)
      .map((payload: NesiMenuItem[]) => {
        payload.forEach(element => {
          this.setItem(element);
        });
        return payload;
      });
  }
  getPageIdByUrl(url: string): number {
    if (!this.menus) {
      return -1;
    }
    if (!url) {
      return -1;
    }
    const m: NesiMenuItem = this.menus
      .find(x => (x.url === url)
        || (x.routerLink === url));
    if (m) {
      return m.id;
    } else {
      return -1;
    }
  }

  getPageIdByMenuItem(item: MenuItem): number {
    return this.getPageIdByUrl(item.url || item.routerLink);
  }

  private setItem(m: NesiMenuItem): void {

    switch (m.type) {
      case NesiMenuType.Nesi1:

        m.url = null;
        m.command = null;
        m.routerLink = ['/home/1/' + String(m.id)];
        break;
      case NesiMenuType.MainMenu:
        m.url = null;
        m.command = null;
        m.routerLink = null;
        break;
      case NesiMenuType.RouterLink:
        if (m.id === 300) {
          m.routerLink = ['/signout'];
        } else {
          m.routerLink = ['/home/1/' + String(m.id)];
        }
        // m.url = null;
        m.command = null;
        break;
      case NesiMenuType.Command:
        m.routerLink = null;
        m.url = null;
        break;
      case NesiMenuType.Url:
        m.openurl = m.url;
        // CONFIG.LOG(m.url, 'm.url');
        // CONFIG.LOG(this.windowRef.generateNesi1Url(m.url), 'generateNesi1Url(m.url)');
        m.url = null;
        m.routerLink = null;
        m.command = (event) => { this.windowRef.open(m.openurl, '_blank') };
        break;
      case NesiMenuType.BlankPage:
        m.openurl = m.url;
        m.url = null;
        m.routerLink = null;
        m.command = (event) => { this.windowRef.boingNesi1(m.openurl, m.label, m.target) };
        break;
      default:
        break;
    }


    if (!CONFIG.authentication.showMenuBadge || (m.badge && m.badge === 0)) {
      m.badge = null;
    } else {
      if (m.badge > 10) {
        m.badgeStyleClass = 'blue-badge';
      } else if (m.badge > 5) {
        m.badgeStyleClass = 'purple-badge';
      } else {
        m.badgeStyleClass = 'orange-badge';
      }
    }
    // console.log(m);

    if ((CONFIG.ISBETA() || CONFIG.ISDEV()) &&
      CONFIG.betaMenus.findIndex(x => x.id === m.id && !x.hide_badge
        && (!x.dev_beta_only || (CONFIG.ISDEV() && x.dev_beta_only))
      ) > -1) {
      m.badgeStyleClass = 'orange-badge';
      m.badge = -1;
    }
    if (CONFIG.releaseMenus.findIndex(x => x.id === m.id && !x.hide_badge) > -1) {
      m.badgeStyleClass = 'orange-badge';
      m.badge = -1;
    }
    if (!m.icon) { m.icon = 'fa fa-fw fa-check-circle'; }
    if (m.items && m.items.length > 0) {
      m.items.forEach(n => this.setItem(n));
    }
  }

  get menuHide(): boolean {
    return this._menuHide;
  }

  set menuHide(value: boolean) {
    this._menuHide = value;
    this.subjectMenuMode.next(value);
  }
}
