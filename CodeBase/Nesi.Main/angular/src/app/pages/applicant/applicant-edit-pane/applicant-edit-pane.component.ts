import { Component, OnInit, EventEmitter, Output, Input, ViewChild } from '@angular/core';
import { ApplicantFileComponent } from '../applicant-file/applicant-file.component';
import { ApplicantListComponent } from '../applicantList/applicantList.component';

@Component({
  selector: 'nesi-applicant-edit-pane',
  templateUrl: './applicant-edit-pane.component.html',
  styleUrls: ['./applicant-edit-pane.component.css']
})
export class ApplicantEditPaneComponent implements OnInit {
  submitting = false;
  @Output() close = new EventEmitter();
  @Input() applicant: any;

  @Output() refreshGrid = new EventEmitter();

  get applicant_id() {
    return this.applicant && (this.applicant.applicant_id || this.applicant.id) || 0;
  }
  public tabIndex = 0;
  public currentIndex = 0;

  constructor() { }

  ngOnInit() {
  }

  tabChanged(event) {
    this.tabIndex = event.index;
  }

  applicant_saved(event) {
    if (this.applicant_id === 0) {
      this.applicant.id = event;
      this.applicant.applicant_id = event;
      this.applicant.is_applicant = true;
     }
     this.refreshGrid.emit();
  }
}
