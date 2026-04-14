import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { MessageBase } from '../../../core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { EmployeeService } from '../../employees/_base/employeeService';
import { LabelValueString } from '../../../models/Shared/labelValueInt';
import { CustomersService } from 'app/services/pages/customer.services';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customerrequestadminList',
  templateUrl: './customerrequestadminList.component.html',
  styleUrls: ['./customerrequestadminList.component.css']
})
export class CustomerRequestAdminListComponent extends MessageBase implements OnInit {
  public displayEdit = false;
  public displayNew = false;
  public entity: any;
  public selectedEntityKey: LabelValueString;

  public availableEntities: LabelValueString[];

  public tablename: string;
  public tablenamedisplay: string;

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
    private custS: CustomersService,
   // private es: EmployeeService,
  ) {
    super(store);
    // this.statusList = es.applicant_statusList;
    this.availableEntities = custS.tableValues;
  }

  ngOnInit(): void {
    this.selectedEntityKey = this.availableEntities[0];
    this.loadGrid();
  }

  onEntityChange() {
    this.loadGrid();
  }

  openEdit(event) {
    event.id = event.id;
    event.name = event.name;
    event.is_active = event.is_active;
    this.entity = { ...event };
    this.displayEdit = true;
    this.tablename = this.selectedEntityKey.value;
    this.tablenamedisplay = this.selectedEntityKey.label;
  }

  openCreate(event) {
    this.entity = {};
    this.entity.applicantid = 0;
    this.entity.memberid = 0;
    this.entity.member_id = 0;
    this.entity.is_applicant = true;
    this.entity.isapplicant = true;
    this.displayNew = true;
    this.tablename = this.selectedEntityKey.value;
  }

  onModalClose() {
    this.displayNew = false; // close sidebar
    this.displayEdit = false;
    this.loadGrid();         // reload grid data from API
  }

  loadGrid(refresh_button = false) {
    this.dt.reportQueryParam = [{ coulumnname: 'show_all', value: this.show_all ? 1 : 0 },
      { columnname: 'entity', value: this.selectedEntityKey.value }];

    this.dt.refreshCache = true;
    if (refresh_button) {
      this.dt.loadReport();
    } else {
      this.dt.showReport('CustomerRequestAdminGrid');
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
