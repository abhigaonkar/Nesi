// Import the core angular services.
import { Injectable, Inject } from '@angular/core';
import { Response, Http } from '@angular/http';
import { HttpService } from '../../core/http.service';
import { CONFIG } from '../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromMessage from '../../actions/layout/growlMessage';
import { LoggingErrorHandlerOptions, ErrorOutputOptions } from './loggingErrorHandlerOptions';
import { ServiceBase } from '../../services/shared/serviceBase';
import { OnlineUser } from '../../models/authentication/onlineUser';
import { AuthData } from '../../models/authentication/authorData';

@Injectable()
export class ErrorLogService extends ServiceBase {
  prevError: any;

  // I initialize the service.
  constructor(
    protected http: HttpService,
    private store: Store<fromRoot.State>
  ) {
    super(http);
  }


  // ---
  // PUBLIC METHODS.
  // ---


  // I log the given error to various aggregation and tracking services.
  public logError(error: any, options: ErrorOutputOptions): void {
    if (this.prevError && (!error || !error.message || !this.prevError.message || error.message === this.prevError.message)) {
      return;
    }
    this.prevError = error;
    // Internal tracking.
    if (options.sendToConsole && (CONFIG.ISDEBUG() || CONFIG.ISDEV())) {
      this.sendToConsole(error);
    }
    if (options.sendToGrowlMessage && (CONFIG.ISDEBUG() || CONFIG.ISDEV())) {
      this.sendToMessage(error);
    }
    if (options.sendToServer) {
      this.sendToServer(error);
    }

  }


  // ---
  // PRIVATE METHODS.
  // ---


  // I send the error the browser console (safely, if it exists).
  private sendToConsole(error: any): void {

    if (console && console.group && console.error) {

      console.group('Error Log Service');
      console.log(error);
      console.warn(error.message || 'unknow error.');
      console.warn(error.stack || '');
      console.groupEnd();

    }

  }

  public sendToMessage(error: any): void {
    if (this.store) {
      // if (CONFIG.ISDEBUG()) {
      this.store.dispatch(new fromMessage.PushErrorMessage(error.message || error || 'unknow error.'));
      // }else {
      // this.store.dispatch(new fromMessage.PushErrorMessage('An error occurred, please cut a ticket if this continues. '));
      // }
    }
  }



  // I send the error to the server-side error tracking end-point.
  private sendToServer(error: any): void {


    let currentUser: OnlineUser;
    if (localStorage.getItem(CONFIG.authentication.currentUserDataString)) {
      currentUser = JSON.parse(atob(localStorage.getItem(CONFIG.authentication.currentUserDataString)));
    }
    let currentAuthData: AuthData;
    if (localStorage.getItem(CONFIG.authentication.authDataString)) {
      currentAuthData = JSON.parse(atob(localStorage.getItem(CONFIG.authentication.authDataString)));
    }
    CONFIG.LOG(error, 'send error to server');
    this.http
      .post(
      CONFIG.apiURL.logging.error, // Doesn't really exist in demo.
      {
        id: 0,
        member_ID: (currentUser && currentUser.id) || 0,
        dt: new Date(),
        error_desc: error.Message || error.message || '',
        error_short: error.message || '',
        level: 1,
        user_ip: '',
        origin: 'Angular',
        full_stacktrace: error.stack || '',
        error_on_page: window.location.href || '',
        error_on_line: '',
        host_url: window.location.hostname || ''
      }
      )
      .subscribe(
      (httpResponse: Response): void => {

        // ... nothing to do here.

      },
      (httpError: any): void => {



      }
      )
      ;

  }

}
