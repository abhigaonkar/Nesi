import { Component, OnInit, Input, Output,  EventEmitter } from '@angular/core';
import { TreeNode } from 'primeng/primeng';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { MessageBase } from '../../../core/messageBaseComponent';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';

@Component({
  selector: 'nesi-employee-files',
  templateUrl: './employee-files.component.html',
  styleUrls: ['./employee-files.component.css']
})
export class EmployeeFilesComponent extends MessageBase implements OnInit {
  @Output() loading = new EventEmitter();
  @Output() loaded = new EventEmitter();
  fileDirectory: TreeNode[];
  submitting = false;

  memberid: number;
  @Input() set employee(value: any) {
    if (value) {
      this.memberid = value.memberid || value.member_id;
      if (this.memberid && this.memberid > 0) {
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
    this.cs.getList<TreeNode>(CONFIG.apiURL.core.fileManager.employee + this.memberid)
      .subscribe(
        (res) => {
          this.fileDirectory = res;
          this.loaded.emit();
          this.submitting = false;
        },
        (err: any) => {
          super.PushErrorMessage(err);
          this.loaded.emit();
          this.submitting = false;
        });
  }

  copy_applicant_fiels() {
    this.submitting = true;
    this.cs.postString(CONFIG.apiURL.page.employee.files.copyApplicant + this.memberid, null)
      .subscribe(
        (res) => {
          this.PushResponseMessage(res);
          this.submitting = false;
        },
        (err: any) => {
          super.PushErrorMessage(err);
          this.loaded.emit();
          this.submitting = false;
        }
      );
  }

}
