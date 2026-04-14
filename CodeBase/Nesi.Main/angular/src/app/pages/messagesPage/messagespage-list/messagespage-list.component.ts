import { Component, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { CoreService } from 'app/services/shared/core.service';
import { MessagespageInboxComponent } from 'app/pages/messagesPage/messagespage-inbox/messagespage-inbox.component';
import { MessagespageNewComponent } from 'app/pages/messagesPage/messagespage-new/messagespage-new.component';
import { CONFIG } from 'app/configuration';
import { MessageBase } from 'app/core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-messagespage-list',
  templateUrl: './messagespage-list.component.html',
  styleUrls: ['./messagespage-list.component.css']
})
export class MessagespageListComponent extends MessageBase implements OnInit {

  displayNew = false;
  type = 1;

  @ViewChildren(MessagespageInboxComponent)
  inboxs: MessagespageInboxComponent[];
  @ViewChild(MessagespageNewComponent)
  newPop: MessagespageNewComponent;

  constructor(
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
  ) {
    super(store);
  }

  ngOnInit() {
    this.tabLabels = ['Inbox', 'Sent', 'Deleted'];
    this.tabChanged({ index: 0 });
  }

  afterNewMessage() {
    this.displayNew = false;
    this.getInbox(1).loadItems();
    this.getInbox(2).loadItems();
  }

  getInbox(type: number): MessagespageInboxComponent {
    let o: MessagespageInboxComponent;
    this.inboxs.forEach(e => {
      if (e.type === type) {
        o = e;
      }
    });
    return o;
  }
  newMessage() {
    this.type = 1;
    this.newPop.setFormValue('to', null);
    this.newPop.setFormValue('subject', '');
    this.newPop.setFormValue('body', '');
    this.displayNew = true;
  }

  onDeleted() {
    CONFIG.LOG('delted row', 'on deleted in message list');
    this.getInbox(3).loadItems();
  }

  reply(data) {
    this.newPop.setFormValue('to', [data.message_LeftBy_Member_ID]);
    this.setBody(data);
  }

  setBody(data) {
    this.type = 2;
    this.newPop.setFormValue('subject', 'RE: ' + data.subject);
    this.newPop.setFormValue('body', '\n\n<br />--------ORIGINAL MESSAGE-------------\n\n<br />' +
      'Date :' + String(data.date).replace('T', ' ') + '\n<br />' + 'From:' + data.from_name + '\n\n<br />Message Body:\n\n<br /><br />' + data.body);
    this.displayNew = true;
  }


  replyAll(data) {

    this.newPop.setFormValue('to', data.ids);
    this.setBody(data);
  }
}
