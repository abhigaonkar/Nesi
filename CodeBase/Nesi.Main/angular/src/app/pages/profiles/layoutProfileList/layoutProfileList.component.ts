import { Component, OnInit, NgModule, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromLayoutProfiles from '../../../actions/layout/layoutPorfile';
import * as fromRoot from '../../../reducers';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { Profile } from '../../../models/layout/profile';
import { LayoutProfileService } from '../../../services/layout/layoutProfile.services';
import { Status, getLayout_ToggleTicketsIcon } from '../../../reducers/layout/layoutProfile';
import * as fromMessage from '../../../actions/layout/growlMessage';
import { LayoutProfileHelper } from '../../../services/layout/layoutprofile.helper';
import { TokenService } from '../../../services/authentication/tokenService';
import { CONFIG } from '../../../configuration';
import { WindowRef } from '../../../services/shared/windowRef';
import { MessageBase } from 'app/core/messageBaseComponent';
import { CoreService } from '../../../services/shared/core.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-layoutProfileList',
  templateUrl: './layoutProfileList.component.html',
  styleUrls: ['./layoutProfileList.component.css']
})
export class LayoutProfileListComponent extends MessageBase implements OnInit, OnDestroy {

  menuMode: string;
  menuStyle: string;
  layoutColor: string;
  theme: string;
  messageLifeTime: number;
  signOutTime: number;
  Profiles: Profile[];
  toggleNotificationWhenOtherSignedIn: boolean;
  toggleNotificationWhenOtherSignedOut: boolean;
  toggleMessageIcon: boolean;
  toggleTasksIcon: boolean;
//  toggleTicketIcon: boolean;
  toggleOnlineUserIcon: boolean;
  toggleOnlineUserBadge: boolean;

  private subscriptionMenuMode: Subscription;
  private subscriptionMenuStyle: Subscription;
  private subscriptionLayoutColor: Subscription;
  private subscriptionTheme: Subscription;
  private subscriptionMessageLifeTime: Subscription;
  private subscriptionSignOutTime: Subscription;
  private subscriptionStatus: Subscription;
  private subscriptionHelper: Subscription;
  private subscriptionTOU: Subscription;
  private subscriptionTSI: Subscription;
  private subscriptionTSO: Subscription;
  private subscriptionTTicket: Subscription;
  private subscriptionTMessage: Subscription;
  private subscriptionTtask: Subscription;
  private subscriptionTOUBadge: Subscription;

  MenuMode$: Observable<string>;
  MenuStyle$: Observable<string>;
  LayoutColor$: Observable<string>;
  Theme$: Observable<string>;
  MessageLifeTime$: Observable<number>;
  SignOutTime$: Observable<number>;
  ToggleNotificationWhenOtherSignedIn$: Observable<string>;
  ToggleNotificationWhenOtherSignedOut$: Observable<string>;
  ToggleMessageIcon$: Observable<string>;
  ToggleTasksIcon$: Observable<string>;
 // ToggleTicketIcon$: Observable<string>;
  ToggleOnlineUserIcon$: Observable<string>;
  ToggleOnlineUserBadge$: Observable<string>;

  status$: Observable<Status>;
  Profiles$: Observable<Profile[]>;
  isDirty = false;
  isLdapUser = false;
  displayPasswordInput = false;
  currentPassword: string;
  
  constructor(
    protected store: Store<fromRoot.State>,
    private lps: LayoutProfileService,
    private winRef: WindowRef,
    public ts: TokenService,
    public cs: CoreService,
  ) {
    super(store);
    this.isLdapUser = this.ts.currentUser.isLdapUser;
    CONFIG.LOG(this.ts.currentUser.isLdapUser, 'isLdapUser')
    this.MenuMode$ = store.select(fromRoot.getLayoutProfiles.MenuMode);
    this.MenuStyle$ = store.select(fromRoot.getLayoutProfiles.MenuStyle);
    this.LayoutColor$ = store.select(fromRoot.getLayoutProfiles.LayoutColor);
    this.Theme$ = store.select(fromRoot.getLayoutProfiles.Theme);
    this.MessageLifeTime$ = store.select(fromRoot.getLayoutProfiles.MessageLifeTime);
    this.SignOutTime$ = store.select(fromRoot.getLayoutProfiles.SignOutTime);
    this.status$ = store.select(fromRoot.getLayoutProfilesStatus);
    this.Profiles$ = store.select(fromRoot.getLayoutProfiles.Profiles);
    this.ToggleOnlineUserIcon$ = store.select(fromRoot.getLayoutProfiles.ToggleOnlineUserIcon);
    this.ToggleTasksIcon$ = store.select(fromRoot.getLayoutProfiles.ToggleTasksIcon);
   // this.ToggleTicketIcon$ = store.select(fromRoot.getLayoutProfiles.ToggleTicketIcon);
    this.ToggleMessageIcon$ = store.select(fromRoot.getLayoutProfiles.ToggleMessageIcon);
    this.ToggleNotificationWhenOtherSignedIn$ = store.select(fromRoot.getLayoutProfiles.ToggleNotificationWhenOtherSignedIn);
    this.ToggleNotificationWhenOtherSignedOut$ = store.select(fromRoot.getLayoutProfiles.ToggleNotificationWhenOtherSignedOut);
    this.ToggleOnlineUserBadge$ = store.select(fromRoot.getLayoutProfiles.ToggleOnlineUserBadge);

  }

  ngOnInit() {
    this.subscriptionTOU = this.ToggleOnlineUserIcon$
      .subscribe((value: string) => {
        this.toggleOnlineUserIcon = value === '1';
      });
    this.subscriptionTOUBadge = this.ToggleOnlineUserBadge$
      .subscribe((value: string) => {
        this.toggleOnlineUserBadge = value === '1';
      });
   // this.subscriptionTTicket = this.ToggleTicketIcon$
    //  .subscribe((value: string) => {
    //    this.toggleTicketIcon = value === '1';
    //  });
    this.subscriptionTMessage = this.ToggleMessageIcon$
      .subscribe((value: string) => {
        this.toggleMessageIcon = value === '1';
      });
    this.subscriptionTtask = this.ToggleTasksIcon$
      .subscribe((value: string) => {
        this.toggleTasksIcon = value === '1';
      });

    this.subscriptionTSI = this.ToggleNotificationWhenOtherSignedIn$
      .subscribe((value: string) => {
        this.toggleNotificationWhenOtherSignedIn = value === '1';
      });
    this.subscriptionTSO = this.ToggleNotificationWhenOtherSignedOut$
      .subscribe((value: string) => {
        this.toggleNotificationWhenOtherSignedOut = value === '1';
      });

    this.subscriptionHelper = this.Profiles$
      .subscribe((value: Profile[]) => {
        this.Profiles = value;
      });
    this.subscriptionMenuMode = this.MenuMode$
      .subscribe((value: string) => {
        this.menuMode = value;
      });
    this.subscriptionMenuStyle = this.MenuStyle$
      .subscribe((value: string) => {
        this.menuStyle = value;
      });
    this.subscriptionMessageLifeTime = this.MessageLifeTime$
      .subscribe((value: number) => {
        this.messageLifeTime = value / 1000;
      });
    this.subscriptionSignOutTime = this.SignOutTime$
      .subscribe((value: number) => {
        CONFIG.LOG(value, 'signout time');
        this.signOutTime = value / 60 / 60;
        CONFIG.LOG(this.signOutTime, 'signout time');
      });
    this.subscriptionLayoutColor = this.LayoutColor$
      .subscribe((value: string) => {
        this.layoutColor = value;
      });
    this.subscriptionTheme = this.Theme$
      .subscribe((value: string) => {
        this.theme = value;
      });
    this.subscriptionStatus = this.status$
      .subscribe(status => {
        if (status === Status.saveSuccessed) {
          // show messagebox
          this.store.dispatch(new fromMessage.PushSuccessMessage('Profiles saved successfully.'));
        }
        if (status === Status.saveFailed) {
          // show messagebox
          this.store.dispatch(new fromMessage.PushErrorMessage('Profiles saved Failed.'));
        }
      })
  }


  saveProfiles() {
    this.isDirty = false;
    this.store.dispatch(new fromLayoutProfiles.SaveLayoutProfile(this.Profiles));
  }
  openUserSettings() {
    this.winRef.boingNesi1(CONFIG.Nesi1URL.contact.contactDisplay + this.ts.currentUser.id, 'topmenu', null);
  }
  syncLdapPassword() {
    this.cs.postString(CONFIG.apiURL.layout.syncLdap, { data: this.currentPassword })
      .subscribe((response) => {
        this.currentPassword = null;
        if (this.PushResponseMessage(response)) {
          this.displayPasswordInput = false;
        }
      });
  }

  setMenuMode() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile({ name: 'MenuMode', value: this.menuMode }));
    this.isDirty = true;
  }

  setMenuStyle() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile({ name: 'MenuStyle', value: this.menuStyle }));
    this.isDirty = true;
  }

  setLayoutColor() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile({ name: 'LayoutColor', value: this.layoutColor }));
    this.isDirty = true;
  }

  setTheme() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile({ name: 'Theme', value: this.theme }));
    this.isDirty = true;
  }
  setSignIn() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile(
      { name: 'ToggleNotificationWhenOtherSignedIn', value: this.toggleNotificationWhenOtherSignedIn ? '1' : '0' }));
    this.isDirty = true;
  }
  setSignOut() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile(
      { name: 'ToggleNotificationWhenOtherSignedOut', value: this.toggleNotificationWhenOtherSignedOut ? '1' : '0' }));
    this.isDirty = true;
  }

 /* setTicketIcon() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile(
      { name: 'ToggleTicketIcon', value: this.toggleTicketIcon ? '1' : '0' }));
    this.isDirty = true;
  }*/
  setTaskIcon() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile(
      { name: 'ToggleTasksIcon', value: this.toggleTasksIcon ? '1' : '0' }));
    this.isDirty = true;
  }
  setMessageIcon() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile(
      { name: 'ToggleMessageIcon', value: this.toggleMessageIcon ? '1' : '0' }));
    this.isDirty = true;
  }
  setOnlineUserIcon() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile(
      { name: 'ToggleOnlineUserIcon', value: this.toggleOnlineUserIcon ? '1' : '0' }));
    this.isDirty = true;
  }
  setOnlineUserBadge() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile(
      { name: 'ToggleOnlineUserBadge', value: this.toggleOnlineUserBadge ? '1' : '0' }));
    this.isDirty = true;
  }

  setMessageLifeTime() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile({ name: 'MessageLifeTime', value: String(this.messageLifeTime * 1000) }));
    this.store.dispatch(new fromMessage.PushInfoMessage('Set message life time to ' + this.messageLifeTime + ' seconds.'));
    this.isDirty = true;
  }

  setSignOutTime() {
    this.store.dispatch(new fromLayoutProfiles.SetLayoutProfile({ name: 'SignOutTime', value: String(this.signOutTime * 60 * 60) }));
    this.isDirty = true;
  }
  ngOnDestroy() {
    if (this.subscriptionMenuMode) {
      this.subscriptionMenuMode.unsubscribe();
    }
    if (this.subscriptionLayoutColor) {
      this.subscriptionLayoutColor.unsubscribe();
    }
    if (this.subscriptionMenuStyle) {
      this.subscriptionMenuStyle.unsubscribe();
    }
    if (this.subscriptionTheme) {
      this.subscriptionTheme.unsubscribe();
    }
    if (this.subscriptionStatus) {
      this.subscriptionStatus.unsubscribe();
    }
    if (this.subscriptionMessageLifeTime) {
      this.subscriptionMessageLifeTime.unsubscribe();
    }
    this.subscriptionHelper.unsubscribe();
    this.subscriptionTSI.unsubscribe();
    this.subscriptionTSO.unsubscribe();
    this.subscriptionTOU.unsubscribe();
    this.subscriptionTOUBadge.unsubscribe();
    this.subscriptionTtask.unsubscribe();
    this.subscriptionTTicket.unsubscribe();
    this.subscriptionTMessage.unsubscribe();
  }
}

