import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CustomersService } from 'app/services/pages/customer.services';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-customer-new',
  templateUrl: './customer-new.component.html',
  styleUrls: ['./customer-new.component.css']
})
export class CustomerNewComponent implements OnInit {
  @Output() openDetail = new EventEmitter();
  constructor(
    private custS: CustomersService,
  ) { }

  ngOnInit() {
  }

  open_customer(event) {
    this.openDetail.emit({ data: { customer_id: event.result } });
    // this.custS.openCustomer(event.result);
  }
}
