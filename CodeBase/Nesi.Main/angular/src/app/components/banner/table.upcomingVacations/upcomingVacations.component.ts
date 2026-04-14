import { Component, OnInit, Input } from '@angular/core';
import { OnlineUser } from '../../../models/authentication/onlineUser';
import { UpcomingVacations } from '../../../models/layout/upcomingVacations';

@Component({
  selector: 'bar-upcomingVacationsTable',
  templateUrl: './upcomingVacations.component.html',
  styleUrls: ['./upcomingVacations.component.css']
})
export class UpcomingVacationsTableComponent implements OnInit {

@Input()
  upcomingVacations: UpcomingVacations[];
  @Input()
  showAll = false;
  @Input()
  recentNumber = 10;

  constructor() { }

  ngOnInit() {
  }
  get UpcomingVacationsList(): UpcomingVacations[] {
    return this.showAll ? this.upcomingVacations : this.upcomingVacations.slice(0, this.recentNumber);
  }
}
