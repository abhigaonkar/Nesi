import { TokenService } from '../services/authentication/tokenService';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import {
  Http,
  RequestOptionsArgs,
  Response,
  Request,
  Headers,
  XHRBackend
} from '@angular/http';


import { LoaderService } from './loader/loader.service';
import { NesiRequestOptions } from './nesi-request-options';
import { CONFIG, ERROR_OUTPUT_OPTIONS } from '../configuration';
import { ErrorOutputOptions } from './errorhandler/loggingErrorHandlerOptions';
import { AuthData } from 'models/authentication/authorData';

@Injectable()
export class HttpService extends Http {

  apiUrl = CONFIG.apiURL.host();
  n1ApiUrl = CONFIG.Nesi1URL.host();

  private errorOutputOptions: ErrorOutputOptions = ERROR_OUTPUT_OPTIONS;
  user: AuthData;
  public get lastActiveTime(): Date {

    const o = localStorage.getItem(CONFIG.authentication.lastActiveTime);
    CONFIG.LOG(o, 'lastactive time in http service');
    if (o) {
      return new Date(o);
    } else {
      return new Date();
    }
  }

  public superGet(url: string) {
    return super.get(url);
  }
  public set lastPingTime(value: Date) {
    localStorage.setItem(CONFIG.authentication.lastPingTime, value.toUTCString());
  }

  public get lastPingTime(): Date {

    const o = localStorage.getItem(CONFIG.authentication.lastPingTime);
    CONFIG.LOG(o, 'lastPingTime time in http service');
    if (o) {
      return new Date(o);
    } else {
      return new Date();
    }
  }

  public set lastActiveTime(value: Date) {
    localStorage.setItem(CONFIG.authentication.lastActiveTime, value.toUTCString());
  }

  public showLoading() {
    this.loaderService.show();
  }

  public hideLoading() {
    this.loaderService.hide();
  }
  constructor(
    backend: XHRBackend,
    defaultOptions: NesiRequestOptions,
    private loaderService: LoaderService,
  ) {
    super(backend, defaultOptions);
    this.lastActiveTime = new Date();
    this.lastPingTime = new Date();
    if (localStorage.getItem(CONFIG.authentication.authDataString)) {
      this.user = JSON.parse(atob(localStorage.getItem(CONFIG.authentication.authDataString)));
    }
  }

  private afterHttp(request: Observable<any>, resetActiveTime = true): Observable<any> {
    if (resetActiveTime) {
      this.lastActiveTime = new Date();
      this.lastPingTime = new Date();
    }
    return request.catch(this.onCatch)
      .do((res: Response) => {
        this.onSuccess(res);
      }, (error: any) => {
        this.onError(error);
      })
      .finally(() => {
        this.onEnd();
      });
  }

  private s_afterHttp(request: Observable<any>): Observable<any> {
    return this.afterHttp(request, false);
  }



  // silent get, not show loading bar
  s_get(url: string, options: RequestOptionsArgs = null): Observable<any> {
    return this.s_afterHttp(super.get(this.getFullUrl(url), this.requestOptions(options)));
  }

  get(url: string, options: RequestOptionsArgs = null): Observable<any> {
    this.showLoader();
    return this.afterHttp(super.get(this.getFullUrl(url), this.requestOptions(options)));
  }

  post(url: string, body: any, options: RequestOptionsArgs = null): Observable<any> {

    this.showLoader();
    return this.afterHttp(super.post(this.getFullUrl(url), body, this.requestOptions(options)));
  }

  post_n1(url: string, body: any, options: RequestOptionsArgs = null): Observable<any> {

    this.showLoader();
    return this.afterHttp(super.post(this.getN1FullUrl(url), body, this.requestOptions(options)));
  }

  s_post(url: string, body: any, options: RequestOptionsArgs = null): Observable<any> {
    return this.s_afterHttp(super.post(this.getFullUrl(url), body, this.requestOptions(options)));
  }

  put(url: string, body: any, options: RequestOptionsArgs = null): Observable<any> {

    this.showLoader();

    return this.afterHttp(super.put(this.getFullUrl(url), body, this.requestOptions(options)));


  }

  delete(url: string, options: RequestOptionsArgs = null): Observable<any> {

    this.showLoader();

    return this.afterHttp(super.delete(this.getFullUrl(url), this.requestOptions(options)));

  }

  patch(url: string, body: any, options: RequestOptionsArgs = null): Observable<any> {

    this.showLoader();

    return this.afterHttp(super.patch(this.getFullUrl(url), body, this.requestOptions(options)));


  }

  public fileOptions(type: string): RequestOptionsArgs {
    const options = new NesiRequestOptions();
    options.headers = new Headers();

    let user;
    if (localStorage.getItem(CONFIG.authentication.authDataString)) {
      user = JSON.parse(atob(localStorage.getItem(CONFIG.authentication.authDataString)));
    }
    const token = user && user.access_token;
    if (token) {
      options.headers.append('Content-Type', type);
      options.headers.append('Authorization', 'Bearer ' + token);
    }
    return options;
  }

  private requestOptions(options: RequestOptionsArgs = null): RequestOptionsArgs {

    if (options == null) {
      options = new NesiRequestOptions();
    }

    if (options.headers == null) {
      options.headers = new Headers();
      let user;
      if (localStorage.getItem(CONFIG.authentication.authDataString)) {
        user = JSON.parse(atob(localStorage.getItem(CONFIG.authentication.authDataString)));
      }
      const token = user && user.access_token;
      if (token) {
        options.headers.append('Content-Type', 'application/json');
        options.headers.append('Authorization', 'Bearer ' + token);
      }
    }

    return options;
  }

  public getFullUrl(url: string): string {
    return this.apiUrl + url;
  }

  public getN1FullUrl(url: string): string {
    return this.n1ApiUrl + url;
  }

  private onCatch(error: any, caught: Observable<any>): Observable<any> {
    // console.log(error);
    return Observable.throw(error);
  }

  private onSuccess(res: Response): void {
    // console.log('Request successful');
  }

  private onError(error: Response): void {
    switch (error.status) {
      case 401: {
        if (window.location.href.toString().toLowerCase().indexOf('signin') < 0) {
          CONFIG.LOG(error, '401 redirect onError http service');
          sessionStorage.setItem(CONFIG.authentication.deniedUrl, window.location.href);
          window.location.href = '/index.html';
        } else {
          CONFIG.LOG(error, '401 not redirect onError http service');
        }
        break;
      }
      case 0:{
        CONFIG.LOG(error, '0 onError http service');
        throw ('HTTP Connection Error, Please contact with Web administrator.');
      }
      case 500:{
        CONFIG.LOG(error, '500 onError http service');
        throw (error.status+' '+error.statusText);
      }
      
      default: {
        CONFIG.LOG(error, 'onError http service');
        throw (error);
      }
    }
  }

  private onEnd(): void {
    this.hideLoader();
  }

  private showLoader(): void {
    this.loaderService.show();
  }

  private hideLoader(): void {
    this.loaderService.hide();
  }

}
