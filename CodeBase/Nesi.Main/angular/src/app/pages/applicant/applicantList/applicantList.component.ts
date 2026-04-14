import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { MessageBase } from '../../../core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { EmployeeService } from '../../employees/_base/employeeService';
import { LabelValueString } from '../../../models/Shared/labelValueInt';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-applicantList',
  templateUrl: './applicantList.component.html',
  styleUrls: ['./applicantList.component.css']
})
export class ApplicantListComponent extends MessageBase implements OnInit {
  public displayEdit = false;
  public displayNew = false;
  public entity: any;

  public statusList: LabelValueString[];

  public show_all = false;
  @ViewChild(DatatableComponent) dt: DatatableComponent;
  _list_profile: any;

  set list_profile(value: any) {
    this._list_profile = value;
  }

  get list_profile() {
    return this._list_profile || {};
  }

  constructor(
    public cs: CoreService,
    protected store: Store<fromRoot.State>,
    private es: EmployeeService,
  ) {
    super(store);
    this.statusList = es.applicant_statusList;
  }

  ngOnInit() {
  }


  openEdit(event) {
    event.applicantid = event.id;
    event.memberid = 0;
    event.member_id = 0;
    event.is_applicant = true;
    event.isapplicant = true;
    this.entity = event;
    this.displayEdit = true;
  }

  openCreate(event) {
    this.entity = {};
    this.entity.applicantid = 0;
    this.entity.memberid = 0;
    this.entity.member_id = 0;
    this.entity.is_applicant = true;
    this.entity.isapplicant = true;
    this.displayNew = true;
  }

  loadGrid(refresh_button = false) {
    this.dt.reportQueryParam = [{ coulumnname: 'show_all', value: this.show_all ? 1 : 0 }];
    this.dt.refreshCache = true;
    if (refresh_button) {
      this.dt.loadReport();
    } else {
      this.dt.showReport('ApplicantHomeGrid');
    }
  }

  refreshGrid(event) {
    this.loadGrid();
  }

  update(event) {
    this.cs.postString(CONFIG.apiURL.page.applicant.updateGridField + event.data.id,
      {
        label: event.field,
        value: event.data[event.field]
      }
    ).subscribe(
      (res) => {
        if (!this.CheckResponseMessage(res)) {
          this.PushErrorMessage(res);
        }
      },
      (err:any)=>{
        this.PushErrorMessage(err);
      });
  }
}
