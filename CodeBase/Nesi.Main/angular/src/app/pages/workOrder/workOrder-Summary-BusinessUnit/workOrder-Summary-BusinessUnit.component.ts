import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { WorkOrderService } from 'app/services/pages/workorder.service';
import { OrderSummaryBusinessUnitBase } from 'app/components/shared/Bases/OrderSummaryBusinessUnitBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-workOrder-Summary-BusinessUnit',
  templateUrl: './workOrder-Summary-BusinessUnit.component.html',
  styleUrls: ['./workOrder-Summary-BusinessUnit.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class WorkOrderSummaryBusinessUnitComponent extends OrderSummaryBusinessUnitBase implements OnInit {
  @Input() can_cut_WO: boolean;

  constructor(
    public cs: CoreService,
    public ws: WorkOrderService,
  ) {
    super(cs, ws);
  }

}
