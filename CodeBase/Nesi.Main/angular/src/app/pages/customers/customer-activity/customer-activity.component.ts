
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { CustomerFormBase } from 'app/pages/customers/_base/customerFormBase';

@Component({
  selector: 'nesi-customer-activity',
  templateUrl: './customer-activity.component.html',
  styleUrls: ['./customer-activity.component.css']
})
export class CustomerActivityComponent extends CustomerFormBase implements OnInit {
  tabIndex = 0;
  salesnotes: string;
  publicnotes: string;
  @Input() disabled:boolean;

  constructor(
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(store, cs);
    super.InitCustomer(
      CONFIG.apiURL.page.customers.edit.notes,
      CONFIG.apiURL.page.customers.edit.notesProfile
    );
  }

  tabChange(event) {
    this.tabIndex = event.index;
  }


  get isBilling(): boolean {
    return this.profile && this.profile.is_admin_authenticated && this.profile.is_billAddress;
  }

  get isBackOffice(): boolean {
    return this.profile && this.profile.is_back_office && this.profile.is_billAddress;
  }

  get isEditPastNotes(): boolean {
    return this.profile && this.profile.is_edit_past_notes_allowed;
  }

  add_salesnotes() {
    if (!this.salesnotes) {
      this.PushWarnMessage('Please input notes.');
      return;
    }
    this.submitting = true;
    this.cs.postDataExtra(this.getUrl(this.postUrl), {
      id: 4,
      value: this.salesnotes
    }).subscribe(
      (res) => {
        this.submitting = false;
        if (this.PushResponseMessage(res.data)) {
          this.profile.customer_salesnotes = res.extra;
          this.salesnotes = null;
        }
      }
      );
  }

  save_salesnotes() {
    if (!this.profile.customer_salesnotes) {
      this.PushWarnMessage('Please input notes.');
      return;
    }
    this.submitting = true;
    this.cs.postDataExtra(this.getUrl(this.postUrl), {
      id: 5,
      value: this.profile.customer_salesnotes
    }).subscribe(
      (res) => {
        this.submitting = false;
        if (this.PushResponseMessage(res.data)) {
          this.profile.customer_salesnotes = res.extra;
        }
      }
      );
  }


  add_publicnotes() {
    if (!this.publicnotes) {
      this.PushWarnMessage('Please input notes.');
      return;
    }
    this.submitting = true;
    this.cs.postDataExtra(this.getUrl(this.postUrl), {
      id: 3,
      value: this.publicnotes
    }).subscribe(
      (res) => {
        this.submitting = false;
        if (this.PushResponseMessage(res.data)) {
          this.profile.public_notes = res.extra;
          this.publicnotes = null;
        }
      }
      );
  }


  save_arnotes() {
    if (!this.profile.customer_arnotes) {
      this.PushWarnMessage('Please input notes.');
      return;
    }
    this.submitting = true;
    this.cs.postDataExtra(this.getUrl(this.postUrl), {
      id: 1,
      value: this.profile.customer_arnotes
    }).subscribe(
      (res) => {
        this.submitting = false;
        if (this.PushResponseMessage(res.data)) {
          this.profile.customer_arnotes = res.extra;
        }
      }
      );
  }


  save_invocing_instructions() {
    if (!this.profile.customer_memo) {
      this.PushWarnMessage('Please input notes.');
      return;
    }
    this.submitting = true;
    this.cs.postDataExtra(this.getUrl(this.postUrl), {
      id: 2,
      value: this.profile.customer_memo
    }).subscribe(
      (res) => {
        this.submitting = false;
        if (this.PushResponseMessage(res.data)) {
          this.profile.customer_memo = res.extra;
        }
      }
      );
  }

  save_workorder_instructions() {
    if (!this.profile.customer_workorder_memo) {
      this.PushWarnMessage('Please input notes.');
      return;
    }
    this.submitting = true;
    this.cs.postDataExtra(this.getUrl(this.postUrl), {
      id: 6,
      value: this.profile.customer_workorder_memo
    }).subscribe(
      (res) => {
        this.submitting = false;
        if (this.PushResponseMessage(res.data)) {
          this.profile.customer_workorder_memo = res.extra;
        }
      }
      );
  }
}
