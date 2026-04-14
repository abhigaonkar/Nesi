import { Component, OnInit, Input } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-workOrder-summary-sumtotal',
  templateUrl: './workOrder-summary-sumtotal.component.html',
  styleUrls: ['./workOrder-summary-sumtotal.component.css']
})
export class WorkOrderSummarySumtotalComponent implements OnInit {
  @Input() value: any;

  constructor() { }

  ngOnInit() {
  }

}
