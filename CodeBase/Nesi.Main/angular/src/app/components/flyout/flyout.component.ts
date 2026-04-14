import { Component, OnInit, Input, Output, EventEmitter, ViewChild, AfterViewInit } from '@angular/core';
import { FormMessageBase } from '../../core/formMessageBaseComponent';
import { CONFIG } from '../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../reducers';
import { FormBuilder, Validators } from '@angular/forms';
import { CoreService } from '../../services/shared/core.service';
import { LabelValueInt } from '../../models/Shared/labelValueString';
import { element } from 'protractor';
import { Observable } from 'rxjs/Observable';
import { WindowRef } from 'app/services/shared/windowRef';
import { TokenService } from '../../services/authentication/tokenService';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-flyout',
  templateUrl: './flyout.component.html',
  styleUrls: ['./flyout.component.css']
})
export class FlyoutComponent extends FormMessageBase implements OnInit, AfterViewInit {
  groupUrl = CONFIG.apiURL.page.ticketFlyOut.group;
  types: LabelValueInt[] = [];
  perts: LabelValueInt[] = [];
  pasteImage: string;
  uploadFileData: any;
  PageId$: Observable<number>;
  pageid = 1;
  filename = '1.png';
  relatedTickets: any[] = [];
  loading = false;
  @Input() visible: boolean;
  oldpageid: number;


  get fileUrl(): string {
    return CONFIG.apiURL.page.ticketFlyOut.file + this.filename;
  }

  @Output()
  close = new EventEmitter();
  @ViewChild('#pasteDiv')
  pasteDiv;

  constructor(
    public cs: CoreService,
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    private winRef: WindowRef,
    public ts: TokenService,
  ) {
    super(store, cs);
    super.Init(CONFIG.apiURL.page.ticketFlyOut.save);
    this.PageId$ = store.select(fromRoot.getCurrentuser.getCurrentPageId);
  }

  ngAfterViewInit() {
  }

  ngOnInit() {
    this.loadTypesAndPerts(1);
    // this.loadRelatedTickets(1);
    this.PageId$.subscribe((value: number) => {
      if (this.oldpageid !== value && this.visible) {
        this.oldpageid = this.pageid;
        this.pageid = value;
        if (this.pageid) {
          this.loadTypesAndPerts(1);
          this.loadRelatedTickets(this.pageid);
        }
      }
    });

  }
  openTicket(id: string) {
    this.winRef.boingNesi1('/sections/member/tickets/ticketpage.aspx?issue=' + id, 'ticket_' + id);
  }

  createForm() {
    this.userform = this.fb.group({
      'group': ['', Validators.required],
      'type': ['', Validators.required],
      'pert': ['', Validators.required],
      'subject': ['', Validators.required],
      'is_private': false,
      'body': ['', Validators.required],
      'file': '',
      'has_file': '',
    });

    this.initFormvalue = {
      'group': '',
      'type': '',
      'pert': '',
      'subject': '',
      'is_private': false,
      'body': '',
      'file': '',
      'has_file': false,
    };
  }

  loadTypesAndPerts(groupId: number) {
    if (!this.visible) { return; }
    if (this.groupUrl && groupId && this.pageid && this.pageid !== NaN) {
      this.cs.getList<LabelValueInt>(this.groupUrl + '/' + groupId.toString() + '/' + this.pageid.toString())
        .subscribe(
        (res: any) => {
          this.types = res.types;
          this.perts = res.perts;
          this.fullPath = res.filePath;
          if (this.types) {
            this.userform.get('type').setValue(3);
          }
          if (this.perts) {
            super.LOG(this.pageid, 'pageid in loadtypesand perts');
            this.userform.get('pert').setValue(res.ticketPageId);
          }
        }
        );
    }
  }

  submitSuccess() {
    this.deletePasteImage();
    this.userform.get('group').setValue(1);
    this.loadTypesAndPerts(1);
    this.close.emit(event);
  }

  groupChanged(event: any) {
    this.loadTypesAndPerts(event.value);
  }

  groupLoaded(event: any) {
    this.userform.get('group').setValue(1);
    this.loadTypesAndPerts(1);
  }

  closeClick(event: any) {
    this.visible = false;
    this.close.emit(event);
  }

  handlePaste(e: any, div: any) {
    for (let i = 0; i < e.clipboardData.items.length; i++) {
      const item = e.clipboardData.items[i];
      super.LOG('Item: ' + item.type, 'handle paste');
      if (item.type.indexOf('image') > -1) {
        this.uploadFileData = item.getAsFile();
        this.uploadFile(this.uploadFileData);
      }
    }
  }


  onUploaded(event: any) {
    super.onUploaded(event);
    if (this.uploadedFiles && this.uploadedFiles.length > 0) {
      this.filename = this.uploadedFiles[0].name;
      this.loadImage();
      // this.userform.get('file').setValue(this.uploadedFiles[0].name);
    }
    super.LOG(this.userform.get('file').value, 'file uploaded timesheet expense');

  }

  get backgroundImg() {
    try {
      if (this.pasteImage) {
        return 'url(data:image/png;base64,' + this.pasteImage + ')';
      } else {
        return 'url(./assets/images/ticket/bg[paste].png)';
      }
    } catch (e) {

    }
  }

  deletePasteImage() {
    this.uploadFileData = null;
    this.pasteImage = null;
    this.userform.get('file').setValue('');
    this.userform.get('has_file').setValue(false);
    this.cs.deleteString(this.fileUrl).subscribe(
      (res: string) => {
        super.LOG(res, 'delete paste image file');
      }
    );
  }

  loadImage() {
    this.cs.getString(this.fileUrl)
      .subscribe(
      (d: string) => {
        this.pasteImage = d;
        this.userform.get('file').setValue(this.filename);
        this.userform.get('has_file').setValue(true);
      }
      );
  }

  uploadFile(data: any) {
    super.LOG(data, 'paste file data');
    this.cs.uploadFile(this.fileUrl, this.filename, data.type.toString(), data).subscribe(
      (res: string) => {
        this.loadImage();
        super.LOG(res, 'uploadfile response');
      },
      (err: any) => {
        super.LOG(err, 'uploadfile error');
      }
    )
  }

  PertainingChanged(event: any) {
    this.loadRelatedTickets(event.value);
  }

  public loadRelatedTickets(page_id: number) {
    CONFIG.LOG(page_id, 'pageid in load realted tickets');
    CONFIG.LOG(this.oldpageid, 'oldpageid in load realted tickets');
    this.relatedTickets = [];
    if (!this.visible) { return; }
    if (page_id <= 0) {
      return;
    }
    this.loading = true;
    this.cs.getList<any>(CONFIG.apiURL.page.ticketFlyOut.relatedTickets + page_id.toString())
      .subscribe(
      (res) => {
        this.relatedTickets = res;
        this.oldpageid = page_id;
        this.loading = false;
      }
      );
  }

}
