import { Injectable } from '@angular/core';
import { ServiceBase } from '../shared/serviceBase';
import { HttpService } from '../../core/http.service';
import { CONFIG } from '../../configuration';
import { Observable } from 'rxjs/Observable';
import { Response } from '@angular/http';
import { ApiResult } from '../../models/core/ApiResult';

@Injectable()
export class PasswordService extends ServiceBase {

  constructor(protected http: HttpService) {
    super(http);
  }

  postRequestPassword(email: string): Observable<Response> {
    const body = { email: email };
    return this.http.post(CONFIG.apiURL.password.request, body);
  }

  postForgotPassword(username: string): Observable<Response> {
    const body = { username: username };
    return this.http.post(CONFIG.apiURL.password.forgot, body);
  }

  postChangePassword(body: any): Observable<Response> {
    return this.http.post(CONFIG.apiURL.password.change, body);
  }

  requestPassword(email: string): Observable<ApiResult> {
    return this.ConvertObject<ApiResult>(this.postRequestPassword(email));
  }

  fogotPassword(username: string): Observable<ApiResult> {
    return this.ConvertObject<ApiResult>(this.postForgotPassword(username));
  }

  changePassword(body: any): Observable<string> {
    return this.ConvertData<string>(this.postChangePassword(body));
  }
}
