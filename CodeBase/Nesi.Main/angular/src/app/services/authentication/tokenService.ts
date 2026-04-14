import { Injectable } from '@angular/core';
import { CONFIG, HostContains } from '../../configuration';
import { AuthData } from 'models/authentication/authorData';
import { OnlineUser } from 'models/authentication/onlineUser';
import { CookieService } from 'ng2-cookies';


@Injectable()
export class TokenService {


  private atCurrentUser: OnlineUser;
  private atCurrentAuthData: AuthData;
  private _visible_business_unit_list: any[];

  get visible_business_unit_list(): any[] {
    return this._visible_business_unit_list || [];
  }

  set visible_business_unit_list(value: any[]) {
    if (value) {
      this._visible_business_unit_list = value.map(x => {
        return { label: x.label, value: { field: x.value, display: true } };
      });
    }
  }

  get working_businessUnit(): number {
    const o = localStorage.getItem('working_businessUnit_' + this.currentUser.id);
    if (o && !isNaN(Number(o))) {
      return Number(o);
    } else {
      return this.currentUser.businessUnitId;
    }
  }

  set working_businessUnit(value: number) {
    localStorage.setItem('working_businessUnit_' + this.currentUser.id, value.toString());
  }

  get currentUser(): OnlineUser {
    this.getCurrentUserDataFromStorage();
    if (this.atCurrentUser != null) {
      return this.atCurrentUser;
    } else {
      // this.getCurrentUserDataFromStorage();
      // if (this.atCurrentUser != null) {
      //   return this.atCurrentUser;
      // }
      return null;
    }
  }

  public set CtrlURL(url: string) {
    if (url) {
      CONFIG.LOG(url, 'set ctrl url in token service');
      localStorage.setItem(CONFIG.authentication.ctrlURLString, btoa(url));
    } else {
      localStorage.setItem(CONFIG.authentication.ctrlURLString, '');
    }
  }

  public get CtrlURL(): string {
    const u = localStorage.getItem(CONFIG.authentication.ctrlURLString);
    if (u) {
      return atob(u);
    } else {
      return null;
    }
  }

  public set CurrentURL(url: string) {
    if (url && this.currentAuthData && this.currentAuthData.userId) {
      CONFIG.LOG(url, 'set current url in token service');
      sessionStorage.setItem(CONFIG.authentication.currentURL_UserId, this.currentUser.id.toString());
      sessionStorage.setItem(CONFIG.authentication.currentURLString, url);
    } else {
      sessionStorage.setItem(CONFIG.authentication.currentURLString, '');
    }
  }

  public get CurrentURL(): string {
    let u = sessionStorage.getItem(CONFIG.authentication.currentURL_UserId);
    if (!u || u !== this.currentUser.id.toString()) {
      return null;
    }
    u = sessionStorage.getItem(CONFIG.authentication.currentURLString);
    if (u && this.deniedUrl && (u === this.deniedUrl || this.deniedUrl.includes(u))) {
      this.CurrentURL = null;
      return null;
    }
    if (u) {
      return u;
    } else {
      this.CurrentURL = null;
      return null;
    }
  }

  // calculate if need to refresh token before a http request.
  public needRefreshToken(): boolean {
    //  CONFIG.LOG(Date.now() / 1000, 'need refreshtoken now ticket');
    //   CONFIG.LOG(this.atCurrentAuthData, 'vlaue of need refreshtoken');
    const u = this.currentAuthData;
    return (u && Date.now() / 1000
      >= u.expireSecond - 60 * CONFIG.authentication.refreshTokenMinutes);
  }

  public get isDeveloper(): boolean {
    const u = this.currentAuthData;
    return u && u.isDeveloper === 'true';
  }

  public checkAuthentication(): boolean {
    const u = this.currentAuthData;
    const c = this.CookieGuid;

    if (u && c) {
      const o = Date.now() / 1000 <= u.expireSecond && c === u.guid;
      if (CONFIG.ISDEVELOPER_ONLY()) {
        return o && this.isDeveloper;
      } else {
        return o;
      }
    } else {
      return false;
    }
    // CONFIG.LOG(u, 'checkAuthentication u: ');
  }


  get hasFvr(): boolean {
    const u = this.currentUser;
    return (u && u.hasFvr);
  }
  get isAuthentication(): boolean {
    const u = this.currentUser;
    return (u && !u.isExpired);
  }

  get isSwitchedUser(): boolean {
    const u = this.currentUser;
    const a = this.currentAuthData;

    return (u && a
      // tslint:disable-next-line:triple-equals
      && u.id.toString() != a.userId)
      && !this.isContact;
  }

  passFocedChangePassword() {
    this.currentUser.forceChangePassword = false;
    this.setCurrentUserData();
  }
  get isFocedChangePassword(): boolean {
    const u = this.currentUser;
    return !!(!this.isSwitchedUser && u && u.forceChangePassword);
  }

  passFvr() {
    this.atCurrentUser.fvrPassed = true;
    this.atCurrentUser.hasFvr = false;
    this.setCurrentUserData();
  }

  get isFvrPassed(): boolean {
    const u = this.currentUser;
    return !!(this.isContact || (!this.isContact && u && u.fvrPassed));
  }

  get currentAuthData(): AuthData {
    this.tryLoadAuthData();
    return this.atCurrentAuthData;
  }

  get isContact(): boolean {
    const a = this.currentAuthData;
    return a && a.isContact === 'true';
  }

  constructor(

    private cookie: CookieService,

  ) {

    this.tryLoadAuthData();
  }



  handleSignIn(data: any): void {
    if (data != null) {
      this.atCurrentAuthData = data;
      this.atCurrentAuthData.issued = data['.issued'];
      this.atCurrentAuthData.expires = data['.expires'];
    } else {
      this.tryLoadAuthData();
    }
    // console.log(this.atCurrentAuthData);
    // console.log(this.checkAuthentication);
    if (this.checkAuthentication) {
      //  console.log(this.atCurrentAuthData);
      this.setAuthData();
    } else {
      this.atCurrentAuthData = null;
    }
  }


  public get swtiched(): boolean {
    // tslint:disable-next-line:triple-equals
    return this.currentAuthData.userId != this.currentUser.id.toString();
  }
  // Try to load auth data
  public tryLoadAuthData(): void {
    this.getAuthDataFromStorage();
  }


  public handleOnlineUser(data: any): void {
    const user = data.user;
    this.visible_business_unit_list = data.visible_business_unit_list;
    if (user != null && !user.isExpired) {
      this.atCurrentUser = user;
      this.setCurrentUserData();
    } else {
      this.clearAuthData();
    }
  }


  // Try to get auth data from storage.
  private getAuthDataFromStorage(): void {
    const s = localStorage.getItem(CONFIG.authentication.authDataString);
    if (s) {
      this.atCurrentAuthData = JSON.parse(atob(s))
    } else {
      this.atCurrentAuthData = null;
    }
  }
  private getCurrentUserDataFromStorage(): void {
    const s = localStorage.getItem(CONFIG.authentication.currentUserDataString)
    if (s) {
      this.atCurrentUser = JSON.parse(atob(s));
    } else {
      this.atCurrentUser = null;
    }
  }

  /**
   *
   * Set Auth Data
   *
   */

  // Write auth data to storage
  private setAuthData(): void {
    localStorage.setItem(CONFIG.authentication.authDataString, btoa(JSON.stringify(this.atCurrentAuthData)));
    if (this.atCurrentAuthData) {
      this.cookie.set(CONFIG.authentication.cookieName, this.atCurrentAuthData.guid);
    }
  }

  private get CookieGuid(): string {
    return this.cookie.get(CONFIG.authentication.cookieName);
  }


  public isCookieAuthrized(): boolean {
    return this.CookieGuid && this.currentAuthData && this.CookieGuid === this.currentAuthData.guid;
  }

  private setCurrentUserData(): void {

    localStorage.setItem(CONFIG.authentication.currentUserDataString, btoa(JSON.stringify(this.atCurrentUser)));
  }
  // Write auth data to storage
  public clearAuthData(clearSessionStorage = false): void {
    this.atCurrentAuthData = null;
    this.atCurrentUser = null;
    localStorage.clear();
    this.cookie.deleteAll();
    if (clearSessionStorage) {
      sessionStorage.clear();
    }
  }

  public clearSessionStorage() {
    sessionStorage.clear();
  }

  public set deniedUrl(value: string) {
    CONFIG.LOG(value, 'set Iframe src url');
    sessionStorage.setItem(CONFIG.authentication.deniedUrl, value);
  }

  public get deniedUrl(): string {
    const r = sessionStorage.getItem(CONFIG.authentication.deniedUrl);
    CONFIG.LOG(r, 'get deniedUrl');
    return r;
  }

  public set nesi1IframeUrl(value: string) {
    CONFIG.LOG(value, 'set Iframe src url');
    sessionStorage.setItem(CONFIG.authentication.nesi1IframeUrl, value);
    sessionStorage.setItem(CONFIG.authentication.nesi1Iframe_UserId, this.currentUser.id.toString());

  }



  public set nesi1IframeUrlParent(value: string) {
    CONFIG.LOG(value, 'set Iframe parent src url');

    sessionStorage.setItem(CONFIG.authentication.nesi1IframeUrl_parent, value);
  }
  public get nesi1IframeUrlParent(): string {
    const r = sessionStorage.getItem(CONFIG.authentication.nesi1IframeUrl_parent);
    CONFIG.LOG(r, 'get Iframe parent src url');
    return r;
  }
  public get nesi1IframeUrl(): string {
    const u = sessionStorage.getItem(CONFIG.authentication.nesi1Iframe_UserId);
    if (!u || u !== this.currentUser.id.toString()) {
      return null;
    }
    let r = sessionStorage.getItem(CONFIG.authentication.nesi1IframeUrl);
    CONFIG.LOG(r, 'get Iframe src url from session storage.');

    if (!r || r === 'about:blank') { return null; }
    if (r.indexOf('https://') > -1) {
      r = r.replace('https://', '');
    }
    if (r.indexOf('http://') > -1) {
      r = r.replace('http://', '');
    }
    const i = r.indexOf('/');
    if (r.startsWith('//')) {
      r = r.substr(i + 1);
    } else {
      r = r.substr(i);
    }
    if (r.indexOf('?') < 0) {
      r += '?is_n1=true';
    }
    CONFIG.LOG(r, 'get Iframe src url');
    return r;
  }
}

