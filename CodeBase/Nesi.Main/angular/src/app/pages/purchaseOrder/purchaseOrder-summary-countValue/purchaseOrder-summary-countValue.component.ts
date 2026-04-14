import { Component, OnInit, Input } from '@angular/core';
import { TokenService } from 'app/services/authentication/tokenService';
import { Router } from '@angular/router';
import { WindowRef } from 'app/services/shared/windowRef';
import { WorkOrderService } from 'app/services/pages/workorder.service';
import { CoreService } from 'app/services/shared/core.service';
import { OrderSummaryCountValueBase } from 'app/components/shared/Bases/OrderSummaryCountValueBase';

@Component({
  selector: 'nesi-purchaseOrder-summary-countValue',
  templateUrl: './purchaseOrder-summary-countValue.component.html',
  styleUrls: ['./purchaseOrder-summary-countValue.component.css']
})
export class PurchaseOrderSummaryCountValueComponent extends OrderSummaryCountValueBase implements OnInit {

  constructor(
    public ts: TokenService,
    public router: Router,
    public cs: CoreService,
    public ws: WorkOrderService,
  ) {
    super(ts, router, cs, ws);
  }

  ngOnInit() {
    this.link_names = [];
    this.from = 'PurchaseOrder';
  }
}
