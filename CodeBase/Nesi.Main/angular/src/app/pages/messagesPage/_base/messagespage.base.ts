
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { MessageBase } from 'app/core/messageBaseComponent';
import * as fromCurrentUser from '../../../actions/layout/currentUser';

export class MessagespageBase extends MessageBase implements OnInit {
    items: any[];
    selectedItems: any[];
    clickItem: any;

    loading = false;
    @Input() type: number;
    @Output() deleted = new EventEmitter();
    @Output() reply = new EventEmitter();
    @Output() replyAll = new EventEmitter();

    get url(): string {
        switch (this.type) {
            case 1:
                return CONFIG.apiURL.page.messages.inbox;
            case 2:
                return CONFIG.apiURL.page.messages.sent;
            case 3:
                return CONFIG.apiURL.page.messages.deleted;
        }
    }

    constructor(
        protected store: Store<fromRoot.State>,
        public cs: CoreService

    ) {
        super(store);

    }

    ngOnInit() {
        this.loadItems();
    }


    public loadItems() {
        CONFIG.LOG(this.type, 'loaditem in type');
        this.loading = true;
        this.cs.getList<any>(this.url)
            .subscribe(
                (res) => {
                    this.items = res;
                    this.loading = false;
                });
    }

    applyRowStyle(row: any): string {
        if (row.type !== 1) { return; }
        return String(row.status).toLowerCase() === 'unread' ? 'bold italic' : '';
    }

    readMessage(event) {
        if (this.type !== 1) { return; }
        const row = event.data;
        // refresh message icon badage.
        // if (String(row.status).toLowerCase() === 'unread') {
        // }
        this.items.find(x => x.messageTo_ID == row.messageTo_ID).status = 'Read';

    }

    deleteMessages() {
        if (this.type === 3) { return; }
        const ids = this.selectedItems.map(x => x.messageTo_ID);
        const containsUnread: boolean = this.selectedItems.findIndex(x => String(x.status).toLowerCase() === 'unread') > -1;
        this.cs.postString(CONFIG.apiURL.page.messages.delete + this.type.toString(), ids)
            .subscribe(
                (res: string) => {
                    if (this.PushResponseMessage(res)) {
                        this.items = this.items.filter(x => ids.indexOf(x.messageTo_ID) == -1);
                        this.selectedItems = null;
                        // if deleted items contain unread message, refresh message icon badage.
                        if (containsUnread) {
                            this.store.dispatch(new fromCurrentUser.LoadMESSAGEList());
                        }
                        this.deleted.emit();
                    }
                }
            );
    }

    onReply(body: string, item) {
        if (item) {
            item.body = body;
            this.reply.emit(item);
        }
    }

    onReplyAll(data, item) {
        if (item && data) {
            item.body = data.body;
            item.ids = data.ids;
            this.replyAll.emit(item);
        }
    }
}