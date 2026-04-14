import { Component, OnInit, Input } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';
import { Message } from '../../../models/banner/message';

@Component({
  selector: 'bar-messagesTable',
  templateUrl: './table.messages.component.html',
  styleUrls: ['./table.messages.component.css']
})
export class TableMessagesComponent implements OnInit {

  @Input()
  messageList: Message[];
  @Input()
  showAll = false;
  @Input()
  recentNumber = 10;
  constructor() { }

  ngOnInit() {
  }


  get MessageList(): Message[] {
    return this.showAll ? this.messageList : this.messageList.slice(0, this.recentNumber);
  }
}
