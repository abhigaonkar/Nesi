import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nesi-timesheet-pel-summary',
  templateUrl: './timesheet-pel-summary.component.html',
  styleUrls: ['./timesheet-pel-summary.component.css']
})
export class TimesheetPelSummaryComponent implements OnInit {
  @Input() data: any;
  constructor() { }

  ngOnInit() {
  }

}
