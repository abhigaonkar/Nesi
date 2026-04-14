import { OrderSummaryBusinessUnitBase } from 'app/components/shared/Bases/OrderSummaryBusinessUnitBase';
import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { WorkOrderService } from 'app/services/pages/workorder.service';
@Component({
  selector: 'nesi-purchaseOrder-Summary-BusinessUnit',
  templateUrl: './purchaseOrder-Summary-BusinessUnit.component.html',
  styleUrls: ['./purchaseOrder-Summary-BusinessUnit.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PurchaseOrderSummaryBusinessUnitComponent extends OrderSummaryBusinessUnitBase implements OnInit {
    
  constructor(
    public cs: CoreService,
    public ws: WorkOrderService,
  ) {
    super(cs, ws);
  }

}
