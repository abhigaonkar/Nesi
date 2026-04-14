import { Component, OnInit, Input, ViewChild, Output } from '@angular/core';
import { TimesheetShopProjects } from '../interface/timesheetShopProjects';
import { Timesheet_shop_projectsService } from '../../../services/pages/timesheet_shop_projects.service';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators, FormGroup, FormControl, MinLengthValidator } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import DataSource from 'devextreme/data/data_source';
import { DataExtra } from 'app/models/core/dataExtra';
import { CONFIG } from '../../../configuration';
import { DxListComponent } from 'devextreme-angular';
import { EventEmitter } from '@angular/core';
import { stringify } from 'querystring';
import { get } from 'https';
@Component({
  selector: 'nesi-timesheet-shop-Projects',
  templateUrl: './timesheet_Shop_Projects.component.html',
  styleUrls: ['./timesheet_Shop_Projects.component.css'],
})
export class Timesheet_Shop_ProjectsComponent extends FormMessageBase implements OnInit{
  // @Input('selectedProject') selectedProject: TimesheetShopProjects;
  @Input() projectId: number;
  @Output() edited = new EventEmitter<boolean>();
  @Output() close = new EventEmitter();
  @ViewChild('list') list: DxListComponent;
  projectList: TimesheetShopProjects[] = [];
  listproject: DataSource;
  public projectName: string;
  public editFlag = false;
  constructor(
    private ps: Timesheet_shop_projectsService,
    public cs: CoreService,
    private fb: FormBuilder,
    protected store: Store<fromRoot.State>,
  ) {
    super(store, cs);
    this.projectName = "";
    this.createForm();
  }
  ngOnInit() {
    this.refresh();
   
  }
  refresh() {
    this.loadProjects();
    this.editFlag=false;
    this.createForm();
  }
 
  createForm() {
    this.userform = this.fb.group({
      'name': ['', Validators.required]
    });
    this.initFormvalue = ({
      'name': ''
    });
  }
  loadProjects() {
    this.ps.getShopProjects()
      .subscribe((res: any) => {
        this.listproject = res;
      });
  }
  public changed() {
    this.edited.emit(true);
    this.editFlag = false;
    this.list.clearChangedOptions();
    this.projectName = "";
  }
  itemClick(e) {
    this.projectName = (this.list.selectedItems.map(a => a.label)).toString();
   
    this.editFlag = true;
  }
  onNewClicked(e) {
    this.projectName = "";
    this.editFlag = false;
    this.userform.reset();
    this.userform.markAsUntouched();
  }
  submitValidate() {
    if (this.editFlag) {
      if (this.userform.get('id')) {
        console.log("has id already");
      }
      else {
        this.userform.addControl('id', this.fb.control(this.list.selectedItems.map(a => a.value)[0], Validators.required));
        this.userform.updateValueAndValidity();
      }
      this.submitedValue = this.userform.value;
      this.postUrl=CONFIG.apiURL.page.timesheet.UpdateProject;
      this.editFlag = false;
      return true;
      }
    else
      {
        if (this.userform.get('id')) 
        {
        this.userform.removeControl('id');
        this.userform.updateValueAndValidity();       
        this.postUrl = CONFIG.apiURL.page.timesheet.InsertProject;         
       
       }
       this.postUrl = CONFIG.apiURL.page.timesheet.InsertProject;
             
        return true;
      }
     
    }
  
    submitSuccess() {
 
    if (this.postUrl === CONFIG.apiURL.page.timesheet.UpdateProject) {
       console.log("update url");
       this.changed();
       this.refresh();
       this.close.emit();
     }
     else {
       console.log("save");
      
     } 
     this.userform.reset();
     this.userform.markAsUntouched();
     this.refresh();
     setTimeout(() => {
       if(this.responseMessageAfterPost){
         this.editFlag=false;
       }
     }, 500);
 }
 
}
