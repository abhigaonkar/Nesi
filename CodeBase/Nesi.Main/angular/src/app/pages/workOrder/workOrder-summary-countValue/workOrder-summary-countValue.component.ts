import { Component, OnInit, Input } from '@angular/core';
import { TokenService } from 'app/services/authentication/tokenService';
import { Router } from '@angular/router';
import { WindowRef } from 'app/services/shared/windowRef';
import { WorkOrderService } from 'app/services/pages/workorder.service';
import { CoreService } from 'app/services/shared/core.service';
import { OrderSummaryCountValueBase } from 'app/components/shared/Bases/OrderSummaryCountValueBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-workOrder-summary-countValue',
  templateUrl: './workOrder-summary-countValue.component.html',
  styleUrls: ['./workOrder-summary-countValue.component.css']
})
export class WorkOrderSummaryCountValueComponent extends OrderSummaryCountValueBase implements OnInit {
  @Input() can_cut_WO: boolean;

  constructor(
    public ts: TokenService,
    public router: Router,
    public cs: CoreService,
    public ws: WorkOrderService,
  ) {
    super(ts, router, cs, ws);
  }

  ngOnInit() {
    this.link_names = ['open', 'init_prep', 'open_pos', 'rework', 'questions', 'pm_approval', 'bm_approval'];
    this.from = 'WorkOrder';
  }
}
