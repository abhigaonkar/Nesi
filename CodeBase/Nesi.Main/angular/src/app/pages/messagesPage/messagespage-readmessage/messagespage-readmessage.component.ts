import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from 'app/core/messageBaseComponent';
import * as fromCurrentUser from '../../../actions/layout/currentUser';
import { FileInfo } from '../../../models/component/fileManager/file';
import { saveAs as importedSaveAs } from 'file-saver';

@Component({
  selector: 'nesi-messagespage-readmessage',
  templateUrl: './messagespage-readmessage.component.html',
  styleUrls: ['./messagespage-readmessage.component.css']
})
export class MessagespageReadmessageComponent extends MessageBase implements OnInit {
  @Input() messageId: number;
  @Input() typeId: number;
  @Output() reply = new EventEmitter();
  @Output() replyAll = new EventEmitter();

  filename: string;
  file_extension: string;
  file_size: number;
  file_mime_type: string;
  file_last_modified: Date;
  body: string;
  ids: number[];
  constructor
    (
    private cs: CoreService,
    protected store: Store<fromRoot.State>,
  ) {
    super(store);
  }

  ngOnInit() {
    this.cs.getObject<any>(CONFIG.apiURL.page.messages.read + this.typeId.toString() + '/' + this.messageId.toString())
      .subscribe(
        (res: any) => {
          this.body = res.body;
          this.ids = res.ids;
          this.filename = res.fullName;
          this.file_extension = res.extension;
          this.store.dispatch(new fromCurrentUser.LoadMESSAGEList());
        }
      );
  }


  public downloadFile(filename: string, extension: string) {
    const file: FileInfo = {
      name: 'attachment_' + this.messageId + extension,
      fullName: filename,
      extension: extension,
      size: 0,
      lastModified: new Date(),
      mimeType: ''
    };

    this.cs.downloadFile(file).subscribe(
      (blob) => {
        importedSaveAs(blob, file.name);
      }
    );
  }

}
