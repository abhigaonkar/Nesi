import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { DataTable } from 'primeng/primeng';

@Component({
  selector: 'nesi-employee-reviewitem-milestone',
  templateUrl: './employee-reviewitem-milestone.component.html',
  styleUrls: ['./employee-reviewitem-milestone.component.css']
})
export class EmployeeReviewitemMilestoneComponent implements OnInit {
  @Input() list = [];
  @Input() locked = false;
  @Output() itemChanged = new EventEmitter();
  constructor() { }

  ngOnInit() {
  }


  submit(data) {
    setTimeout(() => {
      this.itemChanged.emit(data)
    }, 300);
  }
}
