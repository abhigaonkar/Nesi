import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

@Component({
  selector: 'nesi-timesheet-pel-review',
  templateUrl: './timesheet-pel-review.component.html',
  styleUrls: ['./timesheet-pel-review.component.css']
})
export class TimesheetPelReviewComponent implements OnInit {

  @Output() cancel = new EventEmitter();
  @Output() confirm = new EventEmitter();
  @Input() value: any;
  @Input() submitting = false;
  constructor() { }

  ngOnInit() {
  }

}
