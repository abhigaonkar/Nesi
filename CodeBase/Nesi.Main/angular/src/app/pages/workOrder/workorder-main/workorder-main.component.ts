import { WorkOrderFormBase } from '../_base/workorderFormBase';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormMessageBase } from '../../../core/formMessageBaseComponent';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../reducers';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { Subscription } from 'rxjs/Subscription';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'nesi-workorder-main',
  templateUrl: './workorder-main.component.html',
  styleUrls: ['./workorder-main.component.css']
})
export class WorkorderMainComponent extends WorkOrderFormBase implements OnInit {

  private sub: Subscription;
  @Output() pdf_path = new EventEmitter();

  public display_wocomment = false;

  constructor(
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    public titleService: Title,
    public route: ActivatedRoute,
    public router: Router,
  ) {
    super(store, cs);
    this.Init(CONFIG.apiURL.page.workOrder.edit.main)
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      const business_unit_id = +params['business_unit_id'];
      const str_wo_id = +params['str_wo_id'];
      //const customerId=this.entity.customerId;
      
      this.workorder = { str_wo_id: str_wo_id, business_unit_id: business_unit_id,customer_id:0};
      
    });
  }

  AfterProfileLoaded() {
    if (this.profile.pdf_path) {
      this.pdf_path.emit({ data: this.profile.pdf_path });
    }
    this.titleService.setTitle(this.wo_title);
   
    //alert(this.entity.customerId);
  }

  get wo_title(): string {
     this.workorder.customer_id=this.entity.customerId;
    return `Work Order: ${this.entity.orderNumber || ''} - ${this.entity.customerName || ''} - ${this.entity.customerId || ''}`;

  }

  open_wo(woid) {
    this.router.navigate(['/home/12/workorder/edit/' + this.business_unit_id + '/' + woid]);
  }

  unlink_scan() {
    this.router.navigate(['/home/12/workorder/rename/' + this.business_unit_id + '/' + this.woprog_id]);
  }

  open_wocomments() {
    this.display_wocomment = true;
  }


  button_click(e) {
    switch (e.item.id) {
      case 'back':
        this.router.navigate(['/home/12/workorder/buckets/' + this.business_unit_id]);
        break;
      default:
        break;
    }
  }
}
