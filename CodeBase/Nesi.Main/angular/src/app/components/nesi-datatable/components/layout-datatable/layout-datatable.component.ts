import { TokenService } from './../../../../services/authentication/tokenService';
import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { DataTableModule } from 'primeng/primeng';
import { ListboxModule, LazyLoadEvent } from 'primeng/primeng';
import { DataService } from '../../service/dataservice';
import { CONFIG } from '../../../../configuration';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import * as fromMessage from '../../../../actions/layout/growlMessage';
import { Observable } from 'rxjs/Observable';
import { LayoutService } from 'app/components/nesi-datatable/service/layoutService';
// import { RadioButtonModule } from "primeng/primeng";

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-layout-datatable',
  templateUrl: './layout-datatable.component.html',
  styleUrls: ['./layout-datatable.component.css']
})
export class LayoutDatatableComponent implements OnInit {
  _memberLayoutData: MemberLayout[];
  @Input() gridId: string;
  @Output() populateGridEvent: EventEmitter<any> = new EventEmitter();
  @Input() set memberLayoutData(value: MemberLayout[]) {
    this._memberLayoutData = value;
    this.setSelectedLayout();
  }

  get memberLayoutData(): MemberLayout[] {
    return this._memberLayoutData;
  }

  @Input() columnProperties: any[];
  public deletedLayout: MemberLayout;
  public loading: boolean;
  public confirmDisplayDialog: boolean;
  public layoutDetails: MemberLayout[] = [];
  public members: MappedData[] = [];
  private getLayoutBaseUrl = 'api/Layout/';
  public selectedMember: MappedDataValue;
  public currentMemberid: any;
  private alternateMemberid: any;
  private currentMemberName: string;
  public totalRecords: number;
  public layoutErrorMessage = 'Loading..';
  public defaultLayoutName = 'Default';
  public selectedLayout: MemberLayout;
  public selectedLayoutId: number;

  public business_unit_name: string;
  constructor(private dataService: DataService,
    private tokenService: TokenService,
    protected store: Store<fromRoot.State>,
    private layoutService: LayoutService,
    private cd: ChangeDetectorRef, ) { }

  ngOnInit() {

    // load all members
    this.getLayoutMembers();

    // get current member id and load its layouts
    this.currentMemberid = this['tokenService']['currentUser']['id'];
    this.currentMemberName = this['tokenService']['currentUser']['fullName'];
    this.business_unit_name = this.tokenService.currentUser.businessUnitName;
  }


  setSelectedLayout(id: number = 0) {
    if (id) {
      this.selectedLayoutId = id;
    }
    this.selectedLayout = this.memberLayoutData.find(x => x.id === this.selectedLayoutId);
  }


  get can_delete_global_layout() {
    return this.tokenService.currentUser && this.tokenService.currentUser.save_global_layout;
  }
  /****************************************** private methods *****************************************/

  /**
   * get all members
   */
  getLayoutMembers() {
    this.loading = true;
    const layoutUrl = this.getLayoutBaseUrl + 'Members/' + this.gridId;
    this.dataService.getSchema(layoutUrl)
      .subscribe((response: Array<Member>) => {
        this.getLayoutMembersCallback(response);
        if (this.selectedLayoutId) {
          this.setSelectedLayout();
        }
      },
        (err) => {
          this.loading = false;
          throw (err);
        });

  }

  mapMembers(members: Array<Member>) {
    const m1: MappedData = new MappedData();
    // let m2: MappedData = new MappedData();
    // let m3: MappedData = new MappedData();
    m1.label = '(All)';
    m1.value = { id: -3, name: '(All)' }
    // m2.label = '(Blanks)';
    // m1.value = {id: -2, name: '(Blanks)'}
    // m3.label = '(Non blanks)';
    // m1.value = {id: -1, name: '(Non blanks)'}
    this.members.push(m1);
    // this.members.push(m2);
    // this.members.push(m3);
    if (members) {
      members.forEach((member) => {
        const m: MappedData = new MappedData();
        m.label = member.member_fullname + ` (${member.ddlName})`;
        m.value = {
          id: member.member_ID,
          name: member.member_fullname
        }
        this.members.push(m);
      })
    } else {
      CONFIG.LOG('Unable to fetch layout members')
    }

  }

  /****************************************** event handlers *****************************************/

  onMemberSelected(data: MappedData) {
    event.stopPropagation();
    const memberId: number = data.value.id;
    this.alternateMemberid = memberId;
    this.loading = true;
    this.layoutService.loadMemberLayouts(this.gridId, memberId, data.value.name).subscribe((memberLayoutData: MemberLayout[]) => {
      this.memberLayoutData = memberLayoutData;


      if (!(this.memberLayoutData && this.memberLayoutData.length)) {
        // tslint:disable-next-line:max-line-length
        const defaultLayout = this.layoutService.setLayoutDefault(this.gridId, (this.alternateMemberid || this.currentMemberid), data.value.name, this.columnProperties)[0];
        this.memberLayoutData.push(defaultLayout);
      }

      // this.populateGridEvent.emit({
      //   memberId: this.alternateMemberid || this.currentMemberid,
      //   layout: this.memberLayoutData[0],
      // });
      this.loading = false;
    });
  }

  public get has_global_layout() {
    return this.memberLayoutData && this.memberLayoutData.findIndex(x => x.memberId === 0) > -1
  }

  public get has_default_layout() {
    return this.memberLayoutData.findIndex(x => x.isDefault === 1 && x.memberId !== 0) > -1;
  }

  public refreshLayouts() {
    const item = this.memberLayoutData.find(x => x.memberId === 0);
    //  CONFIG.LOG(item, 'default layout item');
    if (item && !this.has_default_layout) {
      item.isDefault = 1;
    } else {
      if (item) {
        item.isDefault = 0;
      }
    }
  }

  onSetDefaultLayout(layout: MemberLayout) {
    if (layout.memberId !== 0) {
      layout.isDefault = 1;
    }
    this.loading = true;
    //  this.onLayoutSelection(layout);
    const updateLayoutUrl = this.getLayoutBaseUrl + 'UpdateLayout/Status/' + this.gridId + '/' + this.currentMemberid;
    try {
      this.dataService.getData(updateLayoutUrl, layout)
        .subscribe((success: boolean) => {
          this.onSetDefaultLayoutCallback(success);
          this.onLayoutSelection(layout);
          if (layout.memberId === 0) {
            this.memberLayoutData.forEach(x => x.isDefault = 0);
          }
          this.refreshLayouts();
        },
          (err) => { this.loading = false; })
    } catch {
      this.loading = false;
    }

  }

  onLayoutSelection(layout: MemberLayout) {
    this.selectedLayout = layout;

    // this.alternateMemberid = layout.memberId;
    this.populateGridEvent.emit({
      memberId: this.currentMemberid,
      layout: layout,
      deletedIndex: undefined
    });
  }

  onLayoutDelete(layout: MemberLayout) {
    this.loading = true;
    const deleteLayoutUrl: string = this.getLayoutBaseUrl + 'DeleteLayout/' + layout.id;
    this.dataService.getData(deleteLayoutUrl, null)
      .subscribe((success: boolean) => {
        this.onLayoutDeleteCallback(success, layout);

      },
        (err) => { this.loading = false; })
  }
  openDeleteDialog(layout: MemberLayout) {
    this.deletedLayout = layout;
    this.confirmDisplayDialog = true;
  }
  /****************************************** Callbacks *****************************************/

  /**
   * callback when all members loaded
   */
  getLayoutMembersCallback(members: Array<Member>) {
    this.mapMembers(members);
    this.loading = false;
  }


  private onSetDefaultLayoutCallback(success: boolean) {
    if (success) {
      this.store.dispatch(new fromMessage.PushSuccessMessage('You have successfully set the default layout for this report.'));
    }
    this.loading = false;
  }

  private onLayoutDeleteCallback(success: boolean, layout: MemberLayout) {
    if (success) {
      const currentIndex = this.memberLayoutData.findIndex(x => x.id === layout.id);
      this.memberLayoutData.splice(currentIndex, 1);
      this.memberLayoutData = this.memberLayoutData.slice();
      this.store.dispatch(new fromMessage.PushSuccessMessage('Layout deleted successfully.'));
      if (!(this.memberLayoutData && this.memberLayoutData.length)) {
        // tslint:disable-next-line:max-line-length
        const defaultLayout = this.layoutService.setLayoutDefault(this.gridId, (this.alternateMemberid || this.currentMemberid), layout.firstName, this.columnProperties)[0];
        this.memberLayoutData.push(defaultLayout);
      }
      this.loading = false;
      this.selectedLayout = this.memberLayoutData[0];
      this.populateGridEvent.emit({
        memberId: this.alternateMemberid || this.currentMemberid,
        layout: this.memberLayoutData[0],
        deletedIndex: currentIndex
      });
    }
  };

}

export class ResponseData {
  totalCount: number;
  data: any[];
}


export class MappedData {
  label: string;
  value: MappedDataValue;
}


class MappedDataValue {
  id: number;
  name: string;
}


export class Member {
  member_ID: number;
  member_fullname: string;
  ddlName: string;
}

export class MemberLayout {
  id: number;
  firstName: string;
  memberId: number;
  name: string;
  layout: string;
  isDefault: number;
  fullName: string;
  gridId: string;
  owner: string;
}
