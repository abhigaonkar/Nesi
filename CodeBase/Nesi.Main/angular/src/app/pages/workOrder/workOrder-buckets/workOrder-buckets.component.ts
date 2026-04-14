import { Component, OnInit } from '@angular/core';
import { CoreService } from '../../../services/shared/core.service';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Title } from '@angular/platform-browser';
import { CONFIG } from '../../../configuration';
import { WorkOrderService } from '../../../services/pages/workorder.service';

@Component({
  selector: 'nesi-workorder-buckets',
  templateUrl: './workOrder-buckets.component.html',
  styleUrls: ['./workOrder-buckets.component.css']
})
export class WorkOrderBucketsComponent implements OnInit {

  public business_unit_id = 0;
  public pm_id = 0;
  public order_by = 'WOProg_OpenDateTime';
  private sub: Subscription;
  private _profile: any;
  public submitting = false;

  public set profile(value: any) {
    this._profile = value;
  }

  public get profile() {
    return this._profile || { isnull: true, open_list: [], scanned_list: [], pm_approval_list: [], pmList: [] };
  }

  constructor(
    public cs: CoreService,
    public route: ActivatedRoute,
    public titleService: Title,
    public ws: WorkOrderService,
  ) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.business_unit_id = +params['business_unit_id'];
      this.titleService.setTitle('Workorder Buckets ' + this.business_unit_id);
      this.loadProfile();
    });
  }

  get url() {
    return CONFIG.apiURL.page.workOrder.buckets + this.business_unit_id.toString() + '/' + this.pm_id.toString() + '/' + this.order_by;
  }

  loadProfile() {
    this.submitting = true;
    this.cs.getObject<any>(this.url)
      .subscribe((res) => {
        this.profile = res;
        this.titleService.setTitle('Workorder Buckets ' + this.profile.business_unit_name);
        this.submitting = false;
      });
  }

  addWorkOrder() {
    this.openUrl('add', this.business_unit_id, 'WorkOrder')
  }

  openUrl(type, buId, from) {
    this.ws.openUrl(type, buId, from);
  }

  getList(type: string[], need_parent = false) {
    return this.profile.open_list.filter(x => type.includes(x.woprog_status)
      && (!need_parent || (need_parent && x.parent_woprog_id > 1)));
  }

  getOpenPos() {
    return this.profile.open_list.filter(x => x.po_list && x.po_list.length > 0);
  }
}
