import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nesi-purchaseOrder-summary-sumtotal',
  templateUrl: './purchaseOrder-summary-sumtotal.component.html',
  styleUrls: ['./purchaseOrder-summary-sumtotal.component.css']
})
export class PurchaseOrderSummarySumtotalComponent implements OnInit {
  @Input() value: any;

  constructor() { }

  ngOnInit() {
  }

}
