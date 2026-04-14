import { TokenService } from './tokenService';
import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { HubEvent } from '../../models/authentication/hubEvent';
import * as fromCurrentUser from '../../actions/layout/currentUser';
import * as fromMessage from '../../actions/layout/growlMessage';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromLayoutProfiles from '../../actions/layout/layoutPorfile';
import { CONFIG } from '../../configuration';
import { CoreService } from '../shared/core.service';
import { Router } from '@angular/router';
import { ConnectionState } from 'app/services/authentication/channel.service';


declare var $: any;

@Injectable()
export class SignalRService {


  ToggleNotificationWhenOtherSignedIn$: Observable<string>;
  ToggleNotificationWhenOtherSignedOut$: Observable<string>;
  toggleNotificationWhenOtherSignedIn: string;
  toggleNotificationWhenOtherSignedOut: string;
  private subscriptionTSI: Subscription;
  private subscriptionTSO: Subscription;
  public releaseUserId: number;
  connection: any;
  proxy: any;

  constructor(
    private store: Store<fromRoot.State>,
    private ts: TokenService,
    public cs: CoreService,
    private router: Router,
  ) {
    this.ToggleNotificationWhenOtherSignedIn$ = store.select(fromRoot.getLayoutProfiles.ToggleNotificationWhenOtherSignedIn);
    this.ToggleNotificationWhenOtherSignedOut$ = store.select(fromRoot.getLayoutProfiles.ToggleNotificationWhenOtherSignedOut);
    this.subscriptionTSI = this.ToggleNotificationWhenOtherSignedIn$
      .subscribe((value: string) => {
        this.toggleNotificationWhenOtherSignedIn = value;
      });
    this.subscriptionTSO = this.ToggleNotificationWhenOtherSignedOut$
      .subscribe((value: string) => {
        this.toggleNotificationWhenOtherSignedOut = value;
      });
    // this.store.dispatch(new fromLayoutProfiles.LoadLayoutProfiles());
    this.connectSignalR();
  }

  public connectSignalR() {
    this.connection = $.hubConnection();
    this.connection.logging = true;
    this.connection.url = CONFIG.SignalR.host() + '/signalr';
    this.proxy = this.connection.createHubProxy(CONFIG.SignalR.hubName);
    this.proxy.on('Fire',
      (res) => {
        this.processHubEvent(res);
      }
    );
    this.connection.start({ transport: ['webSockets', 'serverSentEvents', 'longPolling'] })
      .then(
      () => {
        CONFIG.LOG('started hub', 'connection signalr');
      })
      .catch(
      err => CONFIG.LOG(err, 'connection failed singalr')
      );






    // if (!this.connection) {
    //   this.connection = this.signalR.createConnection(null);
    //   this.connection
    //     .start()
    //     .then(c => {
    //       this.listner = c.listenFor<HubEvent>('Fire');
    //       this.listner.subscribe((s: HubEvent) => {
    //         this.processHubEvent(s);
    //       },
    //         (err) => { CONFIG.LOG(err) });
    //       this.connecter = c;
    //       if (!this.ts.isSwitchedUser && !CONFIG.ISDEBUG()) {
    //         this.FireSignalREvent(CONFIG.SignalR.Events.SignIn.name);
    //       }
    //     })
    //     .catch(err => { CONFIG.LOG(err) });
    // }

  }

  private processHubEvent(event: HubEvent) {
    CONFIG.LOG(event.message, 'message in process hub event');
    CONFIG.LOG(event.name, 'name in process hub event');
    const message = event.message;
    const value = event.name;
    if (!this.ts.checkAuthentication() && !message.startsWith('self_')) { return };
    switch (message) {
      // Turn this off for now
      case CONFIG.SignalR.Events.SignIn.name:
        if (this.ts.currentUser.name !== value) {
          this.store.dispatch(new fromCurrentUser.LoadActiveUserList());
          if (this.toggleNotificationWhenOtherSignedIn === '1') {
            // this.store.dispatch(new fromMessage.PushInfoMessage(value + ' ' + CONFIG.SignalR.Events.SignIn.message));
          }
        }
        break;
      case CONFIG.SignalR.Events.SignOut.name:
        if (this.ts.currentUser && this.ts.currentUser.name !== value) {
          this.store.dispatch(new fromCurrentUser.LoadActiveUserList());
          if (this.toggleNotificationWhenOtherSignedOut === '1') {
            // this.store.dispatch(new fromMessage.PushInfoMessage(value + ' ' + CONFIG.SignalR.Events.SignOut.message))
          }
        }
        break;
      case CONFIG.SignalR.Events.newMessage.name:
        CONFIG.LOG(event.name, 'singalr new message fired');
        if (!value) {
          return;
        }
        try {
          // args includes businessunit name , userid.
          const args: string[] = value.split(',');
          const sentName = args[0]; // from name
          const userId = args[1]; // user id
          // if the message is not recived by this user do not show notification.
          if (this.ts.currentUser.id.toString() !== userId) {
            return;
          }
          CONFIG.LOG(sentName, 'sent name in signalr');
          this.store.dispatch(new fromCurrentUser.LoadMESSAGEList());
          this.store.dispatch(new fromMessage.PushInfoMessage(CONFIG.SignalR.Events.newMessage.message.replace('@name', sentName)));
        } catch (e) {

        }
        break;
      case CONFIG.SignalR.Events.chatDashMessage.name:
        CONFIG.LOG(event.name, 'singalr chatmessage fired');
        if (!value) {
          return;
        }
        try {
          // args includes businessunit name , userid.
          const args: string[] = value.split(',');
          const buName = args[0]; // businessunit name
          const userId = args[1]; // user id
          // if the message is sent by this user do not show notification.
          //  console.log(buName);
          //  console.log(this.ts.currentUser.id);
          if (this.ts.currentUser.id.toString() === userId) {
            return;
          }
          CONFIG.LOG(buName, 'bu name in signalr');
          // if  user's visible business unit list contains this businessunit then show notification.
          if (this.cs.getVisibleBusinessUnitBydllName(buName)) {
            this.store.dispatch(new fromMessage.PushInfoMessage(buName + ' ' + CONFIG.SignalR.Events.chatDashMessage.message));
          }
        } catch (e) {

        }
        break;
      case CONFIG.SignalR.Events.Test.name:
        this.store.dispatch(new fromMessage.PushInfoMessage(event.name + ' ' + CONFIG.SignalR.Events.Test.message));
        break;
      case CONFIG.SignalR.Events.reboot.name:
        this.store.dispatch(new fromCurrentUser.SetRebootTime(Number(value)));
        break;
      case CONFIG.SignalR.Events.self_signOutAllWindow.name:
        if (!this.ts.currentAuthData) { return; }
        if (!(this.ts.currentAuthData && this.ts.currentAuthData.guid) || event.name !== this.ts.currentAuthData.guid) {
          if (this.ts.isAuthentication) {
            this.store.dispatch(new fromCurrentUser.LoadActiveUserList());
          }
          return;
        }
        CONFIG.LOG(this.releaseUserId, 'session user in signalR self signout');
        if (this.releaseUserId && this.releaseUserId === this.ts.currentUser.id) {
          return;
        }
        if (this.isHomeOrOpen) {
          window.location.href = '/#/signin';
        }
        break;
      case CONFIG.SignalR.Events.self_signInAllWindow.name:
      if (!this.ts.currentAuthData) { return; }
      if (!(this.ts.currentAuthData && this.ts.currentAuthData.guid) || event.name !== this.ts.currentAuthData.guid) {
          if (this.ts.isAuthentication) {
            this.store.dispatch(new fromCurrentUser.LoadActiveUserList());
          }
          return;
        }
        if (!this.isSigin) {
          // this.router.navigate(['/signin']);
        } else {
          window.location.reload();
        }
        break;  // case CONFIG.SignalR.Events.self_KeepSiginAllWindow.name:
      //   if (!(this.ts.currentUser && this.ts.currentAuthData.guid) || event.name !== this.ts.currentAuthData.guid) {
      //     return;
      //   }
      //   if (this.isHomeOrOpen) {
      //     this.store.dispatch(new fromCurrentUser.SetRefreshTime(1));
      //   }
      //   break;
      default:
        break;
    }
  }

  get isHomeOrOpen() {
    return this.router.url.startsWith('/home') || this.router.url.startsWith('/opens');
  }
  get isSigin() {
    return this.router.url.startsWith('/signin');
  }
  public FireSignOutAll() {
    this.FireSelfAll(CONFIG.SignalR.Events.self_signOutAllWindow.name);
  }

  public FireSignInAll() {
    setTimeout(() => {
      this.FireSelfAll(CONFIG.SignalR.Events.self_signInAllWindow.name);
    }, 1000);
  }

  // public FireKeepSiginAll() {
  //   this.FireSelfAll(CONFIG.SignalR.Events.self_KeepSiginAllWindow.name);
  // }

  public FireSelfAll(eventName: string) {
    this.FireSignalRAllNameValue(eventName, this.ts.currentAuthData.guid);
  }

  public FireSignalREvent(message: string) {
    this.FireSignalRAllNameValue(message, this.ts.currentUser.name)
  }

  public FireSignalRAllNameValue(message: string, value: string) {
    this.FireSignalREventNameValue(CONFIG.SignalR.fireToAllEvent, message, value);
  }

  public FireSignalREventNameValue(eventName: string, message: string, value: string) {
    //  if (!this.ts.checkAuthentication()) { return };
    CONFIG.LOG(this.connection.State, 'connection state singalr');
    //  this.connection.State = ConnectionState.Connected;

    // if (this.connection.State === ConnectionState.Connected) {
    this.sendevent(eventName, message, value);
    // } else {
    //   setTimeout(() => {
    //     this.connection.State = ConnectionState.Connected;
    //     CONFIG.LOG(this.connection.State, 'connection state');
    //     this.sendevent(eventName, message, value);
    //   }, 2000);
    // }
    // if (this.connecter) {
    //   CONFIG.LOG(message + ':' + value, 'fire signal r event');
    //   this.connecter.invoke(hub_name, value, message)
    //     .then(res => { CONFIG.LOG(res, 'firesingalRevent') })
    //     .catch(err => { CONFIG.LOG(err, 'firesingalRevent') });
    // }
  }


  private sendevent(eventName: string, message: string, value: string) {
    CONFIG.LOG(eventName + ' ' + value + ' ' + message, 'send event signalr');
    this.proxy.invoke(eventName, value, message)
      .then(res => { CONFIG.LOG(res, 'firesingalRevent signalr') })
      .catch(err => { CONFIG.LOG(err, 'firesingalRevent signalr') });
  }
  // when save chat messge at bottom menu fire a signalr event.
  public FireSignalREventFromBusinessUnit(businessUnitname: string, userId: number) {
    this.FireSignalRAllNameValue(CONFIG.SignalR.Events.chatDashMessage.name, businessUnitname + ',' + userId.toString());
  }
}
