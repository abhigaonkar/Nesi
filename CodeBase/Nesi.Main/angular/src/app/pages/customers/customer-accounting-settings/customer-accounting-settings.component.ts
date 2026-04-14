import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nesi-customer-accounting-settings',
  templateUrl: './customer-accounting-settings.component.html',
  styleUrls: ['./customer-accounting-settings.component.css']
})
export class CustomerAccountingSettingsComponent implements OnInit {
  @Input() customer_id: number;
  @Input() customer_base_profile: any;
  @Input() disabled:boolean;
  constructor() { }

  ngOnInit() {
  }

}
