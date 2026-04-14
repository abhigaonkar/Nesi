import { ServiceBase } from '../shared/serviceBase';
import { Injectable } from '@angular/core';
import { HttpService } from '../../core/http.service';
import { Observable } from 'rxjs/Observable';
import { CONFIG } from '../../configuration';
import { VisibleBusinessUnitDropDown } from '../../models/Shared/visibleBusinessUnit';
import { Response, ResponseContentType, RequestOptions } from '@angular/http';
import { OnlineUser } from '../../models/authentication/onlineUser';
import { PayTypeHour } from '../../models/Shared/paytypeHour';
import { PhoneLog } from '../../models/Shared/phoneLog';
import { FileInfo } from '../../models/component/fileManager/file';
// tslint:disable-next-line:import-blacklist
import { Subject } from 'rxjs';

@Injectable()
export class CoreService extends ServiceBase {
  public visibleBusinessUnitList: VisibleBusinessUnitDropDown[];

  public iframeUrl = new Subject<string>();


  public set_iframeUrl(url: string) {
    this.iframeUrl.next(url);
  }

  public showLoading() {
    this.http.showLoading();
  }

  public hideLoading() {
    this.http.hideLoading();
  }

  public superGet(url: string) {
    return this.http.superGet(url);
  }

  public get isXXLg() {
    return window.innerWidth >= 1600;
  }

  public get isXLg() {
    return window.innerWidth > 1280;

  }
  public get isLg() {
    return window.innerWidth > 1024;
  }

  public get isMd() {
    return window.innerWidth > 640 && window.innerWidth <= 1024;
  }

  public get isSm() {
    return window.innerWidth <= 640;
  }

  public get dialogWidth() {
    return this.isXLg ? '1280' : this.isLg ? '1024' : this.isMd ? '640' : '0';
  }

  public get secondsSinceLastActive(): number {
    return Math.ceil(((new Date()).getTime() - this.http.lastActiveTime.getTime()) / 1000);
  }

  public set secondsSinceLastActive(value: number) {
    if (value === 0) {
      this.http.lastActiveTime = new Date();
    }
  }


  public get secondsSinceLastPing(): number {
    return Math.ceil(((new Date()).getTime() - this.http.lastPingTime.getTime()) / 1000);
  }

  public set secondsSinceLastPing(value: number) {
    if (value === 0) {
      this.http.lastPingTime = new Date();
    }
  }

  constructor(protected http: HttpService,
  ) {
    super(http);
  }

  public uploadFile(uploadURL: string, name: string, type: string, file: any): Observable<string> {
    return this.http.post(uploadURL, file, this.http.fileOptions(type));
  }


  public downloadFile(file: FileInfo): Observable<Blob> {

    const options = new RequestOptions(
      {
        responseType: ResponseContentType.Blob
      });
    return this.http.post(CONFIG.apiURL.core.fileManager.downloadFile, file, options)
      .map(res => res.blob());
  }

  private getPhoneLog(userId: number, date: Date): Observable<Response> {
    return this.http.get(CONFIG.apiURL.core.phoneLog + userId.toString() + '/' + date.toDateString());
  }

  public GetPhoneLog(userId: number, date: Date): Observable<PhoneLog[]> {
    return this.ConvertList<PhoneLog>(this.getPhoneLog(userId, date));
  }

  private postPhoneLog(id: number, userId: number, notes: string): Observable<Response> {
    return this.http.patch(CONFIG.apiURL.core.phoneLog,
      {
        id: id,
        userId: userId,
        notes: notes
      }
    );
  }
  public UpdatePhoneLog(id: number, userId: number, notes: string): Observable<string> {
    return this.ConvertData(this.postPhoneLog(id, userId, notes));
  }

  private getVisibleBusinessUnitDropDownList(): Observable<Response> {
    return this.http.s_get(CONFIG.apiURL.core.visibleBusinessUnitDropDownList);
  }

  private getPayTypeHoursList(): Observable<Response> {
    return this.http.get(CONFIG.apiURL.core.paytypeHoursList);
  }

  public VisibleBusinessUnitDropDownList(): Observable<VisibleBusinessUnitDropDown[]> {
    return this.ConvertList<VisibleBusinessUnitDropDown>(this.getVisibleBusinessUnitDropDownList());
  }

  public PaytypeHourList(): Observable<PayTypeHour[]> {
    return this.ConvertList<PayTypeHour>(this.getPayTypeHoursList());
  }


  public getVisibleBusinessUnitById(id: number): VisibleBusinessUnitDropDown {
    if (!this.visibleBusinessUnitList || this.visibleBusinessUnitList.length === 0) {
      return null;
    }
    return this.visibleBusinessUnitList.find(x => x.id === id);
  }

  public getVisibleBusinessUnitByName(name: string): VisibleBusinessUnitDropDown {
    if (!this.visibleBusinessUnitList || this.visibleBusinessUnitList.length === 0) {
      return null;
    }
    return this.visibleBusinessUnitList.find(x => x.name === name);
  }

  public getVisibleBusinessUnitBydllName(ddlname: string): VisibleBusinessUnitDropDown {
    if (!this.visibleBusinessUnitList || this.visibleBusinessUnitList.length === 0) {
      return null;
    }
    return this.visibleBusinessUnitList.find(x => x.ddl_name === ddlname);
  }


  public getProfileValueByPropertyName(name: string): Observable<any> {
    return this.s_getData<any>(CONFIG.apiURL.core.profile + name)
      .map(res => JSON.parse(res));
  }

  public saveProfileValueByPropertyName(name: string, value: any): Observable<string> {
    return this.s_postData<string>(CONFIG.apiURL.core.profile + name, { data: JSON.stringify(value) });
  }

  public checkQuarterNumber(obj: Object): boolean {
    const sh = String(obj).trim()
    if (!(sh.indexOf('.') < 0
      || (sh.indexOf('.') > -1 &&
        (
          sh.endsWith('.00') ||
          sh.endsWith('.25') ||
          sh.endsWith('.75') ||
          sh.endsWith('.5') ||
          sh.endsWith('.') ||
          sh.endsWith('.0') ||
          sh.endsWith('.50')
        )
      )
    )
    ) {
      return true;
    }
    return false;
  }


  get isDev(): boolean {
    return CONFIG.ISDEV();
  }

  public loadBootstrapCSS() {
    const bootstrap: HTMLLinkElement = <HTMLLinkElement>document.getElementById('bootstrap');
    bootstrap.href = 'assets/css/bootstrap.min.css';
  }
  public unloadBootstrapCSS() {
    const bootstrap: HTMLLinkElement = <HTMLLinkElement>document.getElementById('bootstrap');
    bootstrap.href = 'assets/css/empty.css';
  }
}
