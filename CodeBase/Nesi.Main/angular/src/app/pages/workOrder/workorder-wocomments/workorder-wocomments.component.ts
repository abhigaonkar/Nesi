import { WorkOrderFormBase } from '../_base/workorderFormBase';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { Subscription } from 'rxjs/Subscription';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { DataExtra } from '../../../models/core/dataExtra';

@Component({
  selector: 'nesi-workorder-wocomments',
  templateUrl: './workorder-wocomments.component.html',
  styleUrls: ['./workorder-wocomments.component.css']
})
export class WorkorderWocommentsComponent extends WorkOrderFormBase implements OnInit {

  private sub: Subscription;
  public new_comments: string;
  public timesheet_comments: string;
  public printComments = false;

  public try_add_comment = false;

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public titleService: Title,
    public route: ActivatedRoute,
    public router: Router,
  ) {
    super(store, cs);
    this.Init(CONFIG.apiURL.page.workOrder.edit.wocomments)
  }

  AfterProfileLoaded() {
    if (this.profile && this.profile.timesheet_comments) {
      this.printComments = this.profile.timesheet_comments.printComments;
      this.timesheet_comments = this.profile.timesheet_comments.comments;
    }
  }

  addComment() {
    this.try_add_comment = true;
    if (!this.new_comments) {
      return;
    }
    this.cs.postDataExtra(this.getUrl(this.postUrl), { data: this.new_comments })
      .subscribe(
        res => {
          if (this.PushResponseMessage(res.data)) {
            this.new_comments = '';
            this.try_add_comment = false;
            this.profile.comments = res.extra;
          }
        }
      );
  }

  saveTimesheetComments() {
    this.profile.timesheet_comments.comments = this.timesheet_comments;
    this.profile.timesheet_comments.printComments = this.printComments;
    this.cs.patchObject<DataExtra>(this.getUrl(this.postUrl), this.profile.timesheet_comments)
      .subscribe(
        res => {
          if (this.PushResponseMessage(res.data)) {
            this.profile.timesheet_comments = res.extra;
            this.AfterProfileLoaded();
          }
        }
      );
  }
}
