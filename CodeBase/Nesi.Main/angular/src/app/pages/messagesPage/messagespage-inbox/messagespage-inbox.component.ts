
import { Component, OnInit } from '@angular/core';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { MessageBase } from 'app/core/messageBaseComponent';
import { MessagespageBase } from 'app/pages/messagesPage/_base/messagespage.base';

@Component({
  selector: 'nesi-messagespage-inbox',
  templateUrl: './messagespage-inbox.component.html',
  styleUrls: ['./messagespage-inbox.component.css']
})
export class MessagespageInboxComponent extends MessagespageBase implements OnInit {
 
  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    }


}
