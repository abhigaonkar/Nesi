import { WorkOrderFormBase } from '../../_base/workorderFormBase';
import { Component, OnInit, Input, Output, EventEmitter ,ViewChild} from '@angular/core';
import { FormMessageBase } from '../../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { Subscription } from 'rxjs/Subscription';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService } from 'primeng/primeng';


@Component({
  selector: 'nesi-workorder-main-tabs',
  templateUrl: './workorder-main-tabs.component.html',
  styleUrls: ['./workorder-main-tabs.component.css']
})
export class WorkorderMainTabsComponent extends  WorkOrderFormBase implements OnInit {

 
  Customer_id: number;
  Business_unit_id:number;
  Woprog_id:number;
  //IsVisiable:boolean;

  showTabs;
  
  private sub: any;

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public titleService: Title,
    public route: ActivatedRoute,
    public router: Router,
    private cf: ConfirmationService
    //private cus: CustomersService
  ) {
    super(store, cs);
    this.Init(CONFIG.apiURL.page.workOrder.edit.main_tabs)
  }

  activeTableView = 0;

  ngOnInit() {
    this.tabLabels = ['General','Customer','Line Items', 'PO\'s',' Time Entries', 'History','Analysis','Project Notes','Project Folder','Linked WOs' ];
    //this.tabName = this.tabLabels[0];
    this.showTabs={
         "General":true,
         "Customer":false,
         "Line Items":true,
         "PO\'s":true,
         "Time Entries":true,
         "History":true,
         "Analysis":true,
         "Project Notes":true,
         "Project Folder":true,
         "Linked WOs":true
    };
    
    this.sub = this.route.params.subscribe(params => {
        this.Business_unit_id = this.workorder.business_unit_id;//+params['business_unit_id']; // (+) converts string 'id' to a number
        this.Woprog_id = this.workorder.str_wo_id;
        this.Customer_id=this.workorder.customer_id;
        
    });

  }

  tabviewOnChange(event: any) {
    const currentIndex = this.activeTableView;
    this.activeTableView = event.index;
    if (event.index !== 0) {
      this.cf.confirm({
        message: 'Changes you made may not be saved, do you really want to leave this tab?',
        accept: () => {
                //alert(this.Customer_id);.\buil
        },
        reject: () => {
          this.activeTableView = 0;
        }
      });
    }
  }

}
