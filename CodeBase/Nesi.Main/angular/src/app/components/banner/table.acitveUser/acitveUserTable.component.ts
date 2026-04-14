import { Component, OnInit, Input } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';

@Component({
  selector: 'bar-acitveUserTable',
  templateUrl: './acitveUserTable.component.html',
  styleUrls: ['./acitveUserTable.component.css']
})
export class AcitveUserTableComponent implements OnInit {

  @Input()
  allOnlineUsers: OnlineUser[];
  @Input()
  showAll = false;
  @Input()
  recentNumber = 10;
  constructor() { }

  ngOnInit() {
  }
  get OnlineUserList(): OnlineUser[] {
    return this.showAll ? this.allOnlineUsers : this.allOnlineUsers.slice(0, this.recentNumber);
  }
}
