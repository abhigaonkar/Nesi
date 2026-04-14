import { Component, OnInit, Input,Output, ViewChild }
  from '@angular/core';
import { CONFIG } from '../../../configuration';
import { TimesheetValue } from '../../../models/pages/timesheet/timesheetValue';
import { TimeSheetComponentBase } from '../interface/timesheetComponentBase';
import { TimesheetList } from '../../../models/pages/timesheet/timesheet-list';
import { TimesheetWorkOrderCommentComponent } from '../timesheetWorkOrderComment/timesheetWorkOrderComment.component';
import { UpdateTimesheetShop } from '../../../models/pages/timesheet/updateTimesheetShop';
import { Store } from '@ngrx/store';
import * as fromMessage from '../../../actions/layout/growlMessage';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { TimesheetService } from '../../../services/pages/timesheet.service';
import { DefaultValueFromScheduler } from '../../../models/pages/timesheet/defaultValueFromScheduler';
import { TokenService } from '../../../services/authentication/tokenService';
import { InsertTimeSheetShop } from '../../../models/pages/timesheet/InsertTimesheetShop';
import * as DATE from '../../../services/helper/datetime';
import { BindDropDownComponent } from 'app/components/shared/BindDropDown/BindDropDown.component';
import { TimesheetShopProjects } from '../interface/timesheetShopProjects';
import { Timesheet_shop_projectsService } from '../../../services/pages/timesheet_shop_projects.service';
import { DxSelectBoxComponent } from 'devextreme-angular';
import { EventEmitter } from '@angular/core';
import { Timesheet_Shop_ProjectsComponent } from '../timesheet_Shop_Projects/timesheet_Shop_Projects.component';
import { Observable } from '../../../../../node_modules/rxjs';
import { map } from '../../../../../node_modules/rxjs-compat/operator/map';
import { async } from '../../../../../node_modules/@types/q';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetShop',
  templateUrl: './timesheetShop.component.html',
  styleUrls: ['./timesheetShop.component.css']
})
export class TimesheetShopComponent implements OnInit, TimeSheetComponentBase {


  @Input()
  disabled = false;
  @Input('edited') edited: boolean;
  public comment: string;

  public rating: number;
  public hours: number;

  public selectedShopTimeType = 1;

  public APIURLshopTimeType: string;
  public selectedRow: TimesheetList;

  public inputValue: TimesheetValue;

  public selectedComment: number;
  @ViewChild(TimesheetWorkOrderCommentComponent)
  shopComment: TimesheetWorkOrderCommentComponent;
  @ViewChild(BindDropDownComponent)
  shopType: BindDropDownComponent;

  public defaultValue: DefaultValueFromScheduler;
  public minvalue:number = 0;
  public maxvalue:number;
  public currenthourValue:number;
  TSprojects: TimesheetShopProjects[] = [];
  projectsVisible = false;
  popupVisible = false;
 
  public bindValue: { value: 0, label:"N/A"};
  valid: boolean = false;
  @ViewChild('projectItem') projectItem: DxSelectBoxComponent;

  constructor(
    private store: Store<fromRoot.State>,
    public cs: CoreService,
    private tss: TimesheetService,
    private ts: TokenService,
    private ps: Timesheet_shop_projectsService
  ) { }

  ngOnInit() {
  }
  public LoadInputvalue(value: TimesheetValue) {
    this.inputValue = value;
    this.APIURLshopTimeType = CONFIG.apiURL.page.timesheet.ShopShopType;
    this.CancelSelectedRow();
    this.loadComments();
  }

  AfterLoadUrl() {
    this.selectRow();
  }
  SelectRow(item: TimesheetList, value: TimesheetValue): void {
    if (!item) { return; }
    this.selectedRow = item;
    CONFIG.LOG(item, 'selectrow in shop timesheet');
    CONFIG.LOG(value, 'inputvalue in shop timesheet');

    if (this.APIURLshopTimeType) {
      this.selectRow();
    } else {
      this.inputValue = value;
      this.APIURLshopTimeType = CONFIG.apiURL.page.timesheet.ShopShopType;
    }
  }

  public setDefaultValue() {
    CONFIG.LOG(this.defaultValue, 'defaultvalue setdefaultvalue')


    if (this.defaultValue && this.ts.currentUser.id === this.inputValue.userId) {
      CONFIG.LOG(this.defaultValue, 'defaultvalue setdefaultvalue')
      this.selectedShopTimeType = this.defaultValue.customer_Id;
     // this.hours = this.defaultValue.hours;
      // if(this.defaultValue.hours>=0)
      // { 
      //   this.minvalue=0;
      //   this.maxvalue=null;
      // }
      // else
      // {
      //   this.minvalue=null;
      //   this.maxvalue=0;
      // }
      // this.currenthourValue=this.defaultValue.hours;
    } else {
      this.selectedShopTimeType = null;
      this.hours = null;
      this.minvalue=null;
      this.maxvalue=null;
    }
  }


  private selectRow() {
    if (this.selectedRow) {
      this.selectedShopTimeType = this.selectedRow.membertime_shop_type_id;
      this.comment = this.selectedRow.comments;
      if(this.selectedRow.hours>=0)
      { 
        this.minvalue=0;
        this.maxvalue=null;
      }
      else
      {
        this.minvalue=null;
        this.maxvalue=0;
      }
      this.hours = this.selectedRow.hours;
      this.currenthourValue=this.selectedRow.hours;
      this.rating = this.selectedRow.rating;
      this.shopComment.showOne('', this.selectedRow.woCommentId);
      this.selectedComment = this.selectedRow.woCommentId;
      if (this.selectedShopTimeType == 15) {
        const pro = this.ps.getShopProjects();
        setTimeout(() => {
        pro.subscribe((res: any) => {
          this.TSprojects = res;
          console.log(this.TSprojects, res);
          this.bindValue = <any>this.getvalue();
            this.projectsVisible = true;
        },
        (err:any)=>{
          this.store.dispatch(new fromMessage.PushErrorMessage(err));
        }
        ),1000});
      
      }
      else {
        this.projectsVisible = false;
      }
    }
  }
  getvalue() {
    this.valid = false;
    if(this.TSprojects.length == 0 || (!this.selectedRow) ){
      return { value: 0, label:"N/A"};
    }
    let foundItem =  null;
    for(let i = 0; i< this.TSprojects.length; i++) {
      const item = this.TSprojects[i];
      if((<any>item).value === this.selectedRow.internal_project_id) {
        foundItem = item;
        break;
      }
      continue;
    }
    let data = foundItem;
    console.log(data);
    return {value: data.value, label: data.label};
  }

    
 
  CancelSelectedRow(): void {
    this.selectedShopTimeType = 10;
    this.selectedRow = null;
    this.comment = '';
    this.hours = null;
    this.rating = null;
    this.shopComment.loadDropDown();
    this.projectsVisible = false;
  }


  get OutputUpdateValue(): UpdateTimesheetShop {

    return {
      date: DATE.ToDateOnly(this.inputValue.selectedDate),
      selectedBusinessUnitId: this.inputValue.businessUnitId,
      selectedUserId: this.inputValue.userId,

      memberTime_ID: this.selectedRow.id,
      numberOfHours: this.hours,
      rating: this.rating,
      memberTime_WoComment_ID: this.selectedComment ? this.selectedComment : 0,
      memberTime_WoComment: this.comment,
      internal_project_id: this.selectedRow.internal_project_id> 0 ? this.projectItem.value:0
    }
  }

  UpdateRow(items: TimesheetList[],
    success: (msg: string) => void,
    failed: (msg: string) => void): void {
    CONFIG.LOG(this.OutputUpdateValue, 'update timeshop');
    this.tss.UpdateShop(this.OutputUpdateValue)
      .subscribe((res: string) => {
        const updateItem = this.OutputUpdateValue;
        const item = items.find(x => x.id === updateItem.memberTime_ID);
        CONFIG.LOG(JSON.stringify(item), 'update shop found item');
        item.hours = updateItem.numberOfHours;
        item.rating = updateItem.rating;
        item.comments = updateItem.memberTime_WoComment;
        item.internal_project_id = updateItem.internal_project_id;
        this.CancelSelectedRow();
        success(res);
      },
      (err: any) => {
        failed(err);
      });

  }

  get OutputInsertValue(): InsertTimeSheetShop {

    return {
      date: DATE.ToDateOnly(this.inputValue.selectedDate),
      selectedBusinessUnitId: this.inputValue.businessUnitId,
      selectedUserId: this.inputValue.userId,
      payTypeId: 1,

      memberTime_WoComment_ID: 0,
      numberOfHours: this.hours,
      rating: this.rating ? this.rating : 0,

      percentComplete: 100,
      memberTime_WoComment: this.comment,

      selectedShopTimeTypeId: this.selectedShopTimeType ? this.selectedShopTimeType : 10,
      selectedShopTimeTypeName: this.shopType.getLabelNameByValue((this.selectedShopTimeType ? this.selectedShopTimeType : 1).toString()),
      internal_project_id: (this.selectedShopTimeType == 15) ? this.projectItem.value : 0,
   
    }
  }


  InsertRow(items: TimesheetList[],
    success: (msg: string) => void,
    failed: (msg: string) => void): void {

    CONFIG.LOG(this.OutputInsertValue, 'insert workorder timesheet');
    this.tss.InsertShop(this.OutputInsertValue)
      .subscribe((res: string) => {
        this.CancelSelectedRow();
        success(res);
      },
      (err: any) => {
        failed(err);
      });
      this.projectsVisible = false;  
  }

  ValidateValue(): boolean {
    const model = this.OutputInsertValue;
    CONFIG.LOG(model, 'validate shop insert');
    if (!(model.selectedShopTimeTypeId > 0)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select a Shop Type.'));
      return false;
    }
    if (!this.hours) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please input hours.'));
      return false;
    }
    if (!((this.comment && this.comment.length >= 10) || (this.selectedComment && this.selectedComment > 0))) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select or input comment (at least 10 characters)'));
      return false;
    }
    if (model.selectedShopTimeTypeId == 15 && !(this.projectItem.value)) {
      this.store.dispatch(new fromMessage.PushInfoMessage('Please select a project'));
      return false;
    }
    return true;
  }

  ClearDropDownList(): void {
  }
  OnChangeComments(event: any) {
    if (event && event.item) {
      this.comment = event.item.label;
    }
  }
  OnShopTypeChange(event) {
    // this.loadComments();
    if (event.value == 15) {
      console.log("shop type change and its 15");
      this.loadProjects();
    }
    else {
      //Reset
      this.TSprojects = [];
      if(this.projectItem)
      {this.projectItem.value = "";}
      this.projectsVisible = false;


    }

}

  loadComments() {
    // CONFIG.LOG(this.selectedWorkOrder, 'loadcomments selected workorder');
    // CONFIG.LOG(JSON.stringify(this.inputValue), 'loadcomments inputvalue');
    if (!this.inputValue.userId || !this.selectedShopTimeType) { return; }
    const url = CONFIG.apiURL.page.timesheet.WorkOrderComments +
      this.inputValue.businessUnitId.toString() + '/' +
      this.inputValue.userId.toString() + '/' +
      this.selectedShopTimeType.toString();

    CONFIG.LOG(url, 'loadcomments url');

    this.shopComment.getList(url);

  }

  onChange(event)
  {
      if(this.hours!=null)
      { 
        if(this.minvalue!=null)
        {
          if(this.hours>=this.minvalue)
          {
            this.currenthourValue=this.hours;
          }
          else{
            this.hours=this.currenthourValue;
          }

        }
        if(this.maxvalue!=null)
        {
          if(this.hours<=this.maxvalue)
          {
            this.currenthourValue=this.hours;
          }
          else{
            this.hours=this.currenthourValue;
          }
        }
      }

  }
  loadProjects() {
    this.ps.getShopProjects()
      .subscribe((res: any) => {
        if (res) {
          console.log("load");
          this.TSprojects = res;
          this.projectsVisible = true;
          this.bindValue = {value: res.value, label: res.label};
          console.log(this.TSprojects , res);
        }
        else {
          console.log("Didnt load");
          this.selectedShopTimeType = 0;  
          this.projectsVisible = false;   
        }
        this.projectsVisible = true;
      },
      (err:any)=>{
        this.store.dispatch(new fromMessage.PushErrorMessage(err));
      });
  }
  OnProjectsChanged(e) {
    this.popupVisible = false;
    this.loadProjects();
  }
  onProjectsClicked() {
    this.popupVisible = true;
   
  }
  selectionChanged(event) {
   
  }

}
