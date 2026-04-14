import { Component, OnInit, ViewChild, Input, EventEmitter, Output } from '@angular/core';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { ConfirmationService } from 'primeng/primeng';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { MessageBase } from '../../../core/messageBaseComponent';
import { LabelValueInt } from '../../../models/Shared/labelValueString';
import { LabelValueString } from '../../../models/Shared/labelValueInt';
import { WindowRef } from '../../../services/shared/windowRef';
@Component({
  selector: 'nesi-employee-footprints',
  templateUrl: './employee-footprints.component.html',
  styleUrls: ['./employee-footprints.component.css']
})
export class EmployeeFootprintsComponent extends MessageBase implements OnInit {

  @Output() loading = new EventEmitter();
  @Output() loaded = new EventEmitter();

  submitting = false;
  memberid: number;
  _employee: any;
  selected_id = -1;
  tabs: any[];

  selected_tab: string;
  selected_all = false;

  options: any[];
  employee_list: LabelValueInt[];
  employee_list_tickets: LabelValueInt[];

  assignList: LabelValueInt[];

  assinged_member_id: number;
  reassign_button_label: string;

  tab_count = 0;

  @Input() set employee(value: any) {
    if (value) {
      this._employee = value;
      this.memberid = this.employee.memberid || this.employee.member_id;
      this.loadProfile();
    }
  }

  get employee(): any {
    return this._employee || {};
  }


  constructor(
    private cs: CoreService,
    private cf: ConfirmationService,
    protected store: Store<fromRoot.State>,
    private win: WindowRef,
  ) {
    super(store);

  }
  ngOnInit() {

  }

  loadProfile() {
    this.submitting = true;
    this.loading.emit();
    this.cs.getObject<any>(CONFIG.apiURL.page.employee.footPrints.profile + this.memberid)
      .subscribe(
        (res) => {
          if (res && res.tabs && res.tabs.length > 0) {
            this.tabs = res.tabs;
            this.tab_count = this.tabs.length;

            this.selected_tab = this.tabs[0].value;
            if (res.options) {
              this.reassign_button_label = res.options.button;
              this.options = res.options.options;
            }
            this.employee_list = res.employee_list;
            this.employee_list_tickets = res.employee_list_tickets;
            this.load_footprint();
          } else {
            this.tab_count = -1;
          }
          this.submitting = false;
          this.loaded.emit();
        },
        (err:any)=>{
          super.PushErrorMessage(err);
          this.submitting = false;
        }
      );
  }

  load_footprint() {
    this.selected_all = false;
    this.submitting = true;
    if (this.selected_tab === 'tickets_assigned') {
      this.assignList = this.employee_list_tickets;
    } else {
      this.assignList = this.employee_list;
    }
    this.cs.getObject<any>(CONFIG.apiURL.page.employee.footPrints.options + this.memberid + '/' + this.selected_tab)
      .subscribe((res) => {
        if (res) {
          this.options = res.options;
          this.reassign_button_label = res.button;
        } else {
          this.options = null;
          this.reassign_button_label = 'Reassign';
        }
        this.submitting = false;
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      }
      );
  }

  get selectedOptions() {
    return this.options && this.options.filter(x => x.checked);
  }

  reassgin() {
    this.submitting = true;
    const postObject = {
      button: this.reassign_button_label,
      type: this.selected_tab,
      from_member_id: this.memberid,
      to_member_id: this.reassign_button_label === 'Reassign' ? this.assinged_member_id : 0,
      items: this.selectedOptions
    };
    this.cs.postDataExtra(CONFIG.apiURL.page.employee.footPrints.reassign, postObject)
      .subscribe(
        (res) => {
          if (this.PushResponseMessage(res.data) && res.extra) {
            this.tabs = res.extra.tabs;
            if (!this.tabs || this.tabs.length === 0) {
              this.tab_count = -1;
            } else {
              this.tab_count = this.tabs.length;
              this.options = res.extra.options && res.extra.options.options;
              this.reassign_button_label = res.extra.options && res.extra.options.button;

            }
          }
          this.submitting = false;
        },
      (err:any)=>{
        this.PushErrorMessage(err);
        this.submitting = false;
      }
      );
  }

  open_link(v) {
    this.win.boingNesi1(v.link, v.label);
  }

  selected_all_click(event) {
    if (this.options) {
      this.options.forEach(x => x.checked = this.selected_all);
    }
  }
}
