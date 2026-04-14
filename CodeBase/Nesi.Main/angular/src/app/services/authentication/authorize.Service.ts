import { TokenService } from './tokenService';
import { ServiceBase } from '../shared/serviceBase';
import { Injectable } from '@angular/core';
import { HttpService } from '../../core/http.service';
import { SignInData } from '../../models/authentication/signInData';
import { Observable } from 'rxjs/Observable';
import { CONFIG } from '../../configuration';
import { ChangePassword } from '../../models/authentication/changePassword';
import { NesiRequestOptions } from 'core/nesi-request-options';

@Injectable()
export class AuthorizeService extends ServiceBase {

  constructor(protected http: HttpService,
    private ts: TokenService,
  ) {
    super(http);
  }

  // Sign in request and set storage
  public signIn(signInData: SignInData): Observable<Response> {
    if (!signInData) { return; }
    const body = 'username=' + encodeURIComponent(signInData.username)
      + '&password=' + encodeURIComponent(signInData.password) + '&grant_type=password'

    return this.http.post(CONFIG.authentication.tokenBaseUrl, body);
  }

  public signInN1(): Observable<Response> {
    const body = {};
    const options = new NesiRequestOptions();
    options.headers = null;
    return this.http.post_n1(CONFIG.authentication.nesi1AuthUrl, body, options );
  }

  public switchUserN1(id: number): Observable<Response> {
    const body = {'Id' : id};
    const options = new NesiRequestOptions();
    options.headers = null;
    return this.http.post_n1(CONFIG.authentication.nesi1SwitchUser, body, options );
  }

  public signOutN1(): Observable<Response> {
    const body = {};
    return this.http.post_n1(CONFIG.authentication.nesi1AuthSignOutUrl, body);
  }


  public refreshToken(refresh_token: string): Observable<any> {

    const body = 'grant_type=refresh_token&refresh_token=' + encodeURIComponent(refresh_token);

    return this.http.post(CONFIG.authentication.tokenBaseUrl, body)

  }



  public getNeedToChangePassword(): Observable<boolean> {
    return this.ConvertData(this.http.get(CONFIG.apiURL.password.change));
  }

  public ChangePassword(entiy: ChangePassword): Observable<string> {
    return this.ConvertData(this.http.post(CONFIG.apiURL.password.change, entiy));
  }

  // Sign out request and delete storage
  public signOut(): Observable<Response> {
    return this.http.delete(CONFIG.apiURL.currentUser.signOutPath);
  }


  public validateToken(): Observable<any> {
    return this.getObject<any>(CONFIG.apiURL.currentUser.validateToken);
  }
}
