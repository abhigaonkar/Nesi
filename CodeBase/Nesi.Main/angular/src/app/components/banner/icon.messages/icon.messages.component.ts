import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';
import { Message } from '../../../models/banner/message';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'bar-iconmessages',
  templateUrl: './icon.messages.component.html',
  styleUrls: ['./icon.messages.component.css']
})
export class IconMessagesComponent implements OnInit {

  @Input()
  messageList: Message[];

  @Output() click = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onClick(event: any) {
    this.click.emit(event);
  }

  get badgeColor() {
    if (this.messageList.length > 10) {
      return 'blue';
    } else if (this.messageList.length > 5) {
      return 'purple';
    }
    return 'orange';

  }
}
