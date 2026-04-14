import { Injectable, OnInit } from '@angular/core';
import { Response } from '@angular/http';
import { CONFIG } from '../../configuration';
import { HttpService } from '../../core/http.service';
import { Observable } from 'rxjs/Observable';
import { DataExtra } from '../../models/core/dataExtra';

@Injectable()
export class ServiceBase {

  constructor(protected http: HttpService) {
  }

  public getAPIURL(url: string): string {
    return this.http.getFullUrl(url);
  }

  public SetTokenBeforeUpload(event) {

    let user;
    if (localStorage.getItem(CONFIG.authentication.authDataString)) {
      user = JSON.parse(atob(localStorage.getItem(CONFIG.authentication.authDataString)));
    }
    const token = user && user.access_token;
    const xhr: XMLHttpRequest = event.xhr;
    if (token) {
      xhr.setRequestHeader('Authorization', 'Bearer ' + token);
    }
  }

  protected ConvertList<T>(ob: Observable<Response>): Observable<T[]> {

    return ob
      .map((res: Response) => {
        const body = <T[]>res.json();
        return body || [];
      });
  }

  protected ConvertObject<T>(ob: Observable<Response>): Observable<T> {

    return ob
      .map((res: Response) => {
        const body = <T>res.json();
        return body || null;
      });
  }

  protected ConvertData<T>(ob: Observable<Response>): Observable<T> {

    return ob
      .map((res: Response) => {
        const body = res.json();
        return <T>body.data || null;
      });
  }

  protected ConvertMap<T>(keyName: string, ob: Observable<Response>): Observable<Map<string, T>> {

    return ob
      .map((res: Response) => {
        const list = <T[]>res.json();
        if (!list) {
          return null;
        }
        const map = new Map<string, T>();
        list.forEach(
          (x: T) => {
            map.set(x[keyName], x);
          }
        );
        return map;
      });
  }

  public getMap<T>(url: string, keyName: string): Observable<Map<string, T>> {
    return this.ConvertMap<T>(keyName, this.http.get(url));
  }
  public postMap<T>(url: string, body: any, keyName: string): Observable<Map<string, T>> {
    return this.ConvertMap<T>(keyName, this.http.post(url, body));
  }

  public deleteString(url: string): Observable<string> {
    return this.ConvertData<string>(this.http.delete(url));
  }

  public deleteObject<T>(url: string): Observable<T> {
    return this.ConvertObject<T>(this.http.delete(url));
  }
  public deleteDataExtra(url: string): Observable<DataExtra> {
    return this.deleteObject<DataExtra>(url);
  }

  public postList<T>(url: string, body: any): Observable<T[]> {
    return this.ConvertList<T>(this.http.post(url, body));
  }
  public postObject<T>(url: string, body: any): Observable<T> {
    return this.ConvertObject<T>(this.http.post(url, body));
  }

  public getList<T>(url: string): Observable<T[]> {
    return this.ConvertList<T>(this.http.get(url));
  }

  public s_getList<T>(url: string): Observable<T[]> {
    return this.ConvertList<T>(this.http.s_get(url));
  }
  public s_postData<T>(url: string, body: any): Observable<T> {
    return this.ConvertData<T>(this.http.s_post(url, body));
  }
  public getData<T>(url: string): Observable<T> {
    return this.ConvertData<T>(this.http.get(url));  // WEBAPI: OkD
  }

  public getObject<T>(url: string): Observable<T> {
    return this.ConvertObject<T>(this.http.get(url)); // WEBAPI: Ok
  }
  public s_getObject<T>(url: string): Observable<T> {
    return this.ConvertObject<T>(this.http.s_get(url));
  }
  public s_getData<T>(url: string): Observable<T> {
    return this.ConvertData<T>(this.http.s_get(url));
  }

  public getBoolean(url: string): Observable<boolean> {
    return this.getData<any>(url).map(
      (res) => {
        if (!res) { return false; }
        return res.toString().toLowerCase() === 'true';
      }
    );
  }
  
  public getString(url: string): Observable<string> {
    return this.getData<string>(url);
  }

  public getNumber(url: string): Observable<number> {
    return this.getData<number>(url);
  }

  public patchData<T>(url: string, body: any): Observable<T> {
    return this.ConvertData<T>(this.http.patch(url, body));
  }

  public patchObject<T>(url: string, body: any): Observable<T> {
    return this.ConvertObject<T>(this.http.patch(url, body));
  }

  public postData<T>(url: string, body: any): Observable<T> {
    return this.ConvertData<T>(this.http.post(url, body));
  }

  public patchString(url: string, body: any): Observable<string> {
    return this.patchData<string>(url, body);
  }

  public postString(url: string, body: any): Observable<string> {
    return this.postData<string>(url, body);
  }

  public postDataExtra(url: string, body: any): Observable<DataExtra> {
    return this.postObject<DataExtra>(url, body);
  }
}




