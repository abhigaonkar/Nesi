import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { EmployeeService } from '../../_base/employeeService';
import { EmployeeOfferFormBase } from '../employeeOfferBase';
import { WindowRef } from '../../../../services/shared/windowRef';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-employee-offer',
  templateUrl: './employee-offer.component.html',
  styleUrls: ['./employee-offer.component.css']
})
export class EmployeeOfferComponent extends EmployeeOfferFormBase implements OnInit {

  print_label = 'Print';
  print_visible = true;
  print_enabled = true;

  action_label = '';
  action_visible = false;
  action_enabled = false;

  accept_label = 'Accept';
  accept_visible = false;
  accept_enabled = false;
  expire_visible = true;
  redo_visible = false;

  save_enabled = true;

  displayConfirmWhenHaveMoreAcceptedOffers = false;
  confirmInfo = '';

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public es: EmployeeService,
    private fb: FormBuilder,
    private win: WindowRef,

  ) {
    super(store, cs, es);
    super.InitEmployee(
      CONFIG.apiURL.page.employee.offer.notes
    );
    this.disabled_form_with_save_enabled = false;
  }

  createForm() {
    this.userform = this.fb.group({
      'member_id': '',
      'offer_id': '',
      'history_notes': '',
      'new_notes': ['', Validators.required],
    });
  }

  private sendForApprovalCheck(): boolean {
    if ( !(this.profile.has_compl === 0) ) {
      return false;
    }

    if (this.profile.manager_id === this.profile.current_user_id || this.profile.report_to === this.profile.current_user_id) {
      return true;
    }

    return false;
  }

  reloadComplPlanInfo() {
    this.cs.getObject<any>(this.getUrl(this.profileUrl))
      .subscribe(
          (res) => {
              this.profile.show_accept_with_compl_plan = res.show_accept_with_compl_plan;
              this.profile.has_compl = res.has_compl;
              this.loadProfile();
          },
          (err:any)=>
          {
            this.PushErrorMessage(err);
          }
      );
  }

  loadProfile() {
    super.loadProfile();
    if (!this.offer_id) {
      this.userform.disable();
    } else {
      this.userform.enable();
    }
    switch (this.profile.status) {
      case 'In Development':
        this.print_visible = true;
        this.accept_visible = false;
        this.save_enabled = true;
        this.expire_visible = true;
        this.redo_visible = false;

        if ( this.profile.has_compl === 1 ) {
          // Offer with complan
          if (this.profile.show_accept_with_compl_plan === 1) {
            this.action_visible = false;
            this.print_enabled = true;
            this.print_label = 'Print and Release';
            this.print_enabled = true;
          } else {
            this.action_visible = true;
            this.action_label = 'Send for Approval';
            this.action_enabled = true;
            this.print_enabled = false;
          }

          break;
        }

        if (this.profile.is_supervisor || this.profile.is_elevated || this.sendForApprovalCheck() ) {
          this.action_visible = false;
          this.print_enabled = true;
          this.print_label = 'Print and Release';
          this.print_enabled = true;
        } else {
          this.action_visible = true;
          this.action_label = 'Send for Approval';
          this.action_enabled = true;
          this.print_enabled = false;
        }
        break;
      case 'Approved':
        this.print_label = 'Print and Release';
        this.accept_visible = false;
        this.action_label = 'Revoke';
        this.action_visible = true;
        this.action_enabled = true;
        this.print_visible = true;
        this.print_enabled = true;
        this.save_enabled = false;
        this.expire_visible = true;
        this.redo_visible = false;
        break;
      case 'Waiting for Approval':
        this.print_label = 'Print';
        this.print_visible = false;
        this.save_enabled = false;
        this.expire_visible = true;
        this.accept_visible = false;
        this.action_visible = false;

        if ( this.profile.has_compl === 1 ) {
          // Offer with complan
          if (this.profile.show_accept_with_compl_plan === 1) {
            this.action_label = 'Approved';
            this.action_visible = true;
            this.action_enabled = true;
            this.print_visible = true;
            this.print_enabled = true;
            this.redo_visible = true;
          }
          break;
        }

        if (this.profile.is_supervisor || this.profile.is_elevated || this.sendForApprovalCheck()) {
          this.action_label = 'Approved';
          this.action_visible = true;
          this.action_enabled = true;
          this.print_visible = true;
          this.print_enabled = true;
          this.redo_visible = true;
        }
        break;
      case 'Released':
        this.save_enabled = false;
        this.print_label = 'Print';
        this.print_visible = true;
        this.print_enabled = true;
        this.accept_visible = true;
        this.accept_enabled = true;
        this.expire_visible = true;
        this.action_label = 'Revoke';
        this.action_visible = false;
        this.action_enabled = false;
        break;
      case 'Accepted':
      this.accept_visible = false;
        this.print_label = 'Print';
        this.expire_visible = false;
        this.action_visible = false;
        this.print_enabled = true;
        this.save_enabled = false;
        break;
      case 'Awaiting Start Date':
      this.accept_visible = false;
        this.print_label = 'Print';
        this.expire_visible = true;
        this.action_visible = false;
        this.print_enabled = true;
        this.save_enabled = false;
        break;
      case 'Previous':
        this.accept_visible = false;
        this.print_label = 'Print';
        this.expire_visible = false;
        this.action_visible = false;
        this.print_enabled = true;
        this.save_enabled = false;
        break;
      case 'Revoked':
      case 'Past':
      this.accept_visible = false;
      this.expire_visible = false;
     // this.action_label = 'Revive';
     // this.action_visible = true;
    //  this.action_enabled = true;
      this.print_label = 'Print';
      this.print_enabled = false;
      this.save_enabled = false;
      //  this.userform.disable();
      break;
      case 'Closed':
        this.accept_visible = false;
        this.expire_visible = false;
        this.action_label = 'Revive';
        this.action_visible = true;
        this.action_enabled = true;
        this.print_label = 'Print';
        this.print_enabled = false;
        this.save_enabled = false;
        //  this.userform.disable();
        break;
       
    }
  }


  action() {
    switch (this.action_label) {
      case 'Revive':
        this.change_status('In Development');
        break;
      case 'Revoke':
        this.change_status('Revoked');
        break;
      default:
        this.change_status(this.action_label);
        break;
    }
  }

  print() {
    if (this.print_label.includes('Release')) {
      this.change_status('Released', () => this.preview());
    } else {
      this.preview();
    }
  }

  preview() {
    // tslint:disable-next-line:max-line-length
    this.win.boingNesi1(CONFIG.Nesi1URL.printOffer.replace('@moid', this.offer_id.toString()), 'print_preview_offer_' + this.offer_id.toString());
  }

  accept() {
    this.change_status('Accepted', () => {
      this.cs.superGet(this.win.generateNesi1Url(CONFIG.Nesi1URL.upload_offer.replace('@moid', this.offer_id.toString())))
        .subscribe(res => {

        },
        (err:any)=>
        {
          this.PushErrorMessage(err);
        }
        );
    });
  }

  confirmAccept() {
    // Hide the popup first.
    this.displayConfirmWhenHaveMoreAcceptedOffers = false;
    this.change_status('Accepted', null, 'overwrite');
  }

  confirmNO() {
    this.displayConfirmWhenHaveMoreAcceptedOffers = false;
  }

  expire() {
    this.change_status('Closed');
  }

  detail_changed(e) {
    this.loadData();
    // this.loadProfile();
    this.onChange.emit(e);
  }

  change_status(status: string, callback: Function = null, confirm: string = '') {
    this.cs.postDataExtra(this.getUrl(CONFIG.apiURL.page.employee.offer.status), { data: status, data2: confirm })
      .subscribe((res) => {
        if (this.PushErrorResponseMessage(res.data)) {
          this.profile = res.extra;
          this.loadProfile();
          this.onChange.emit();
          if (callback) { callback() };
        } else {
          // Below code to handle the case of multiple accepted offers about
          // already had one accepted offer then want to have another accepted offer.
          if (res.extra.multipleAcceptedOffers === 1) {
            // Clear the warning message and show up the popup.
            this.ClearMessage();
            this.confirmInfo = res.data;
            this.displayConfirmWhenHaveMoreAcceptedOffers = true;
          }
        }
      },
      (err: any) => {
        CONFIG.LOG(err, 'error message');
        super.PushErrorMessage('HTTP Connection Error. Updating is failed.');
      }
      
      );
  }
}
