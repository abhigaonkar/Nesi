import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';
import { Ticket } from '../../../models/layout/ticket';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'bar-icongetSupport',
  templateUrl: './icon.getSupport.component.html',
  styleUrls: ['./icon.getSupport.component.css']
})
export class IconGetSupportComponent implements OnInit {

  @Input()
  tickets: Ticket[];

  @Output() click = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onClick(event: any) {
    this.click.emit(event);
  }

  get badgeColor() {
    if (this.tickets.length > 10) {
      return 'blue';
    } else if (this.tickets.length > 5) {
      return 'purple';
    }
    return 'orange';

  }
}
