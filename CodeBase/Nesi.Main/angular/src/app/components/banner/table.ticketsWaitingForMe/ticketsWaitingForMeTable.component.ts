import { Component, OnInit, Input } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';
import { Ticket } from '../../../models/layout/ticket';

@Component({
  selector: 'bar-ticketsWaitingForMeTable',
  templateUrl: './ticketsWaitingForMeTable.component.html',
  styleUrls: ['./ticketsWaitingForMeTable.component.css']
})
export class TicketsWaitingForMeTableComponent implements OnInit {

  @Input()
  tickets: Ticket[];
  @Input()
  showAll = false;
  @Input()
  recentNumber = 10;

  constructor() { }

  ngOnInit() {
  }
  get TicketsWaitingForMe(): Ticket[] {
    return this.showAll ? this.tickets : this.tickets.slice(0, this.recentNumber);
  }
}
