import { Component, OnInit } from '@angular/core';
import { VendorGridBase } from '../_base/vendorGridBase';

@Component({
  selector: 'nesi-vendor-phone-calls',
  templateUrl: './vendor-phone-calls.component.html',
  styleUrls: ['./vendor-phone-calls.component.css']
})
export class VendorPhoneCallsComponent  extends VendorGridBase implements OnInit {
  constructor() { super() }
}
