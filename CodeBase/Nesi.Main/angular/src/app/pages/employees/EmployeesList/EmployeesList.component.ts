import { Component, OnInit, ViewChild } from '@angular/core';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { EmployeeService } from '../_base/employeeService';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { WindowRef } from '../../../services/shared/windowRef';
import { TokenService } from '../../../services/authentication/tokenService';
import { MenuService } from '../../../services/layout/menuService';
import { MessageBase } from '../../../core/messageBaseComponent';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-EmployeesList',
  templateUrl: './EmployeesList.component.html',
  styleUrls: ['./EmployeesList.component.css']
})
export class EmployeesListComponent extends MessageBase implements OnInit {
  public displayEdit = false;
  public entity: any;
  public member_access_visible = false;
  public suggestion_list: any[];
  public selectedSuggestionItem: any;
  public search_query: string;

  @ViewChild(DatatableComponent) dt: DatatableComponent;
  _list_profile: any;

  set list_profile(value: any) {
    this._list_profile = value;
  }

  get list_profile() {
    return this._list_profile || {};
  }

  public open_edit_tab = 0;

  public contact_member_id: number;
  public displayContact = false;

  constructor(
    public cs: CoreService,
    private win: WindowRef,
    protected store: Store<fromRoot.State>,
    private ms: MenuService,
  ) { 
    super(store);
  }


  ngOnInit() {
    //  this.getbase_profile();
    this.member_access_visible = this.ms.checkMenuPrivilege(96);
  }

  getbase_profile() {
    this.cs.getObject(CONFIG.apiURL.page.employee.profile)
      .subscribe(
        (res) => {
          this.list_profile = res;
        },
        (err:any)=>{
          super.PushErrorMessage(err);
        }
      )
  }

  filterEmployee(event) {
    this.search_query = event.query;
    if (!this.search_query || this.search_query.length <= 2) {
      return;
    }
    this.cs.postList(CONFIG.apiURL.page.employee.advanceSearch, { data: this.search_query })
      .subscribe(res => this.suggestion_list = res,
        (err:any)=>{
          super.PushErrorMessage(err);
        });
  }

  replaceSearchString(str: string): string {
    const p = new RegExp(this.search_query, 'gi');
    return str.replace(p, '<b>' + this.search_query + '</b>');
  }

  openEdit(event, index = 0) {
    this.entity = event;
    this.open_edit_tab = index;
    this.displayEdit = true;
  }

  loadGrid() {
    this.dt.after_onRefresh();
  }

  open_member_access() {
    this.win.boingNesi1(CONFIG.Nesi1URL.member_access, 'member_access');
  }

  open_contact(event) {
    this.contact_member_id = event.id || (event.data && event.data.memberid);
    this.displayContact = true;
  }
}
