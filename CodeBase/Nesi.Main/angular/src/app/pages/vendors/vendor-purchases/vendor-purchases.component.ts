import { Component, OnInit } from '@angular/core';
import { VendorGridBase } from '../_base/vendorGridBase';

@Component({
  selector: 'nesi-vendor-purchases',
  templateUrl: './vendor-purchases.component.html',
  styleUrls: ['./vendor-purchases.component.css']
})
export class VendorPurchasesComponent extends VendorGridBase implements OnInit {

  constructor() { super() }

}
