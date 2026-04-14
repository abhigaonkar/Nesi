import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';
import { CONFIG } from '../../../configuration';
import { Observable } from 'rxjs/Observable';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { TokenService } from 'app/services/authentication/tokenService';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'bar-icononlineUser',
  templateUrl: './icon.onlineUser.component.html',
  styleUrls: ['./icon.onlineUser.component.css']
})
export class IconOnlineUserComponent implements OnInit {

  //@Input()

  @Input()
  set allOnlineUsers(inOnlineUsers: OnlineUser[]) {
    this.onlineUsers = inOnlineUsers;
    this.setBadgeNumber();
  }
  get allOnlineUsers() {
    return this.onlineUsers;
  }



  @Output() click = new EventEmitter();
  @Output() iconmouseover = new EventEmitter();

  public badgeNum: Number;
  private onlineUsers: OnlineUser[];
  private isContact = false;
  ToggleOnlineUserBadge$: Observable<string>;
  private subscriptionTOUBadge: Subscription;
  toggleOnlineUserBadge: boolean;

  constructor(
    protected store: Store<fromRoot.State>,
    public ts: TokenService
  ) {

    this.isContact = this.ts.isContact;
    if (!this.isContact) {
      this.ToggleOnlineUserBadge$ = store.select(fromRoot.getLayoutProfiles.ToggleOnlineUserBadge);
    }
  }

  ngOnInit() {

    if (!this.isContact) {
      this.subscriptionTOUBadge = this.ToggleOnlineUserBadge$
        .subscribe((value: string) => {
          this.toggleOnlineUserBadge = value === '0';
          this.setBadgeNumber();
        });
    }
  }

  setBadgeNumber() {
    if (!this.toggleOnlineUserBadge) {
      this.badgeNum = this.allOnlineUsers.length;
    } else {
      this.badgeNum = 0;
    }
  }
  onClick(event: any) {
    this.click.emit(event);
  }

  onMouseOver(event: any) {
    CONFIG.LOG('mouseover', 'onlineuser icon');
    this.iconmouseover.emit(event);
  }
}
