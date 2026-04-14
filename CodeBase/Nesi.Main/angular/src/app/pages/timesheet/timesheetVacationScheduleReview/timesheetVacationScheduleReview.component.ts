import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetVacationScheduleReview',
  templateUrl: './timesheetVacationScheduleReview.component.html',
  styleUrls: ['./timesheetVacationScheduleReview.component.css']
})
export class TimesheetVacationScheduleReviewComponent implements OnInit {

  @Output() cancel = new EventEmitter();
  @Output() confirm = new EventEmitter();
  @Input() value: any;

  constructor() { }

  ngOnInit() {
  }

}
