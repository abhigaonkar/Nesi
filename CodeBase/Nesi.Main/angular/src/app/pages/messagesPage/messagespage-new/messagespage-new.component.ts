
import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { SignalRService } from 'app/services/authentication/signalR.service';

@Component({
  selector: 'nesi-messagespage-new',
  templateUrl: './messagespage-new.component.html',
  styleUrls: ['./messagespage-new.component.css']
})
export class MessagespageNewComponent extends FormMessageBase implements OnInit {

  @Output() cancel = new EventEmitter();
  users: any[];

  constructor(
    private fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private sr: SignalRService,

  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.messages.base);

  }

  createForm() {
    this.userform = this.fb.group({
      'to': ['', Validators.required],
      'subject': ['', Validators.required],
      'body': ['', Validators.required],
      'file_name': '',
    });

    this.initFormvalue = {
      'to': null,
      'subject': '',
      'body': '',
      'file_name': '',
    }
  }


  onUploaded(event: any) {
    super.onUploaded(event);
    if (this.uploadedFiles && this.uploadedFiles.length > 0) {
      this.userform.get('file_name').setValue(this.uploadedFiles[0].name);
    }
    super.LOG(this.userform.get('file_name').value, 'file uploaded new message');

  }

  ngOnInit() {
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.messages.userList)
      .subscribe(
      (res) => {
        this.users = res;
      }
      );
      this.cs.getString(CONFIG.apiURL.page.messages.filePath)
      .subscribe(
      (res) => {
        this.fullPath = res + '\\' + this.ts.currentAuthData.guid;
      }
      );
  }


  submitSuccess() {
    const to: number[] = this.submitedValue.to;
    to.forEach(x => {
      this.sr.FireSignalRAllNameValue(CONFIG.SignalR.Events.newMessage.name, this.ts.currentUser.name + ',' + x);
    });
  }

  public setFormValue(field: string, value: any) {
    this.userform.get(field).setValue(value);
  }


}
