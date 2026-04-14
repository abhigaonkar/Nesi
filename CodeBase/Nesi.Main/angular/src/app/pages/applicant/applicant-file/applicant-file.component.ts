import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { TreeNode } from 'primeng/primeng';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { MessageBase } from '../../../core/messageBaseComponent';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';

@Component({
  selector: 'nesi-applicant-file',
  templateUrl: './applicant-file.component.html',
  styleUrls: ['./applicant-file.component.css']
})
export class ApplicantFileComponent extends MessageBase implements OnInit {
  fileDirectory: TreeNode[];
  submitting = false;
  @Output() loading = new EventEmitter();
  @Output() loaded = new EventEmitter();
  applicantid: number;
  @Input() set applicant(value: any) {
    if (value) {
      this.applicantid = value.applicantid || value.applicant_id;
      if (this.applicantid && this.applicantid > 0) {
        this.loadDir();
      }
    }
  }
  constructor(
    private cs: CoreService,
    protected store: Store<fromRoot.State>,
  ) {
    super(store);

  }

  ngOnInit() {

  }


  loadDir() {
    this.submitting = true;
    this.loading.emit();
    this.cs.getList<TreeNode>(CONFIG.apiURL.core.fileManager.applicant + this.applicantid)
      .subscribe(
        (res) => {
          this.fileDirectory = res;
          this.loaded.emit();
          this.submitting = false;
        },
        (err:any) => {
          this.PushErrorMessage(err);
          this.loaded.emit();
          this.submitting = false;
        });
       
  }
}
