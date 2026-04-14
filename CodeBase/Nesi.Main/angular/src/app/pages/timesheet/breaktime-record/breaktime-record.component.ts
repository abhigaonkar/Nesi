import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import {   
  BranchBreakTimeRequirementInputParameter,
  BreakTimeQueryParameter,
  BreakTimeRecord,
  BreakTimeRecordRequirementRecord,
  BreakTimeRecordResult,
  OperationType,
  IBreakTimeService, } from 'models/pages/timesheet/breaktime';

@Component({
  selector: 'app-breaktime-record',
  templateUrl: './breaktime-record.component.html',
  styleUrls: ['./breaktime-record.component.css']
})
export class BreaktimeRecordComponent implements OnInit {
  @Input() selectedDate: Date;
  @Input() selectedUser: number;
  @Input() selectedBranch: number;
  @Input() signInUser: number;
  public cachedbranchSetting: BreakTimeRecordRequirementRecord;
  @Output() insertBreaktimeRecord = new EventEmitter<BreakTimeRecord>();

  @Input()
  set branchSetting(value: BreakTimeRecordRequirementRecord) {
    // we only need to pass the editable part.
    this.userform.reset({
      id: 0,
      date: '',
      business_unit_id: '',
      member_id: '',
      confirmed: false,

      start_time: value.def_start_time.substr(0, 5),
      morning_break_start: value.def_morning_start.substr(0, 5),
      morning_break_duration: value.def_morning_dur,
      
      lunch_break_start: value.def_lunch_start.substr(0, 5),
      lunch_break_duration: value.def_lunch_dur,
      afternoon_break_start: value.def_afternoon_start.substr(0, 5),
      afternoon_break_duration: value.def_afternoon_dur,
    });

    this.cachedbranchSetting = value;
  }

  public userform: FormGroup;

  constructor(private fb: FormBuilder) {
    this.createForm();
   }

  ngOnInit() {
  }

  createForm() {
    this.userform = this.fb.group({
      id: 0,
      date: '',
      business_unit_id: '',
      member_id: '',
      start_time: ['', Validators.required],
      morning_break_start: ['', Validators.required],
      morning_break_duration: ['', Validators.required],
      lunch_break_start: ['', Validators.required],
      lunch_break_duration: ['', Validators.required],
      afternoon_break_start: ['', Validators.required],
      afternoon_break_duration: ['', Validators.required],
      entered_by_member_id: ['', Validators.required],
      confirmed: false
    });
  }

  onSubmit() {
    const record = new BreakTimeRecord();
    record.id = 0;

    record.date = this.selectedDate;
    record.business_unit_id = this.selectedBranch;
    record.member_id = this.selectedUser;

    const s = this.userform.get('start_time');
    if(this.userform.get("start_time").value.length === 5){
      record.start_time = this.userform.get("start_time").value;
    } else{
      const d = (<Date>this.userform.get("start_time").value);
      record.start_time = d.getHours() + ":" + d.getMinutes();
    }

    // record.morning_break_start =  (<Date>this.userform.get("morning_break_start").value).toTimeString();
    if(this.userform.get("morning_break_start").value.length === 5){
      record.morning_break_start = this.userform.get("morning_break_start").value;
    } else{
      const d = (<Date>this.userform.get("morning_break_start").value);
      record.morning_break_start = d.getHours() + ":" + d.getMinutes();
    }
    record.morning_break_duration =  this.userform.get("morning_break_duration").value;

    if(this.userform.get("lunch_break_start").value.length === 5){
      record.lunch_break_start = this.userform.get("lunch_break_start").value;
    } else{
      const d = (<Date>this.userform.get("lunch_break_start").value);
      record.lunch_break_start = d.getHours() + ":" + d.getMinutes();
    }
    record.lunch_break_duration =  this.userform.get("lunch_break_duration").value;

    if(this.userform.get("afternoon_break_start").value.length === 5){
      record.afternoon_break_start = this.userform.get("afternoon_break_start").value;
    } else{
      const d = (<Date>this.userform.get("afternoon_break_start").value);
      record.afternoon_break_start = d.getHours() + ":" + d.getMinutes();
    }
    record.afternoon_break_duration =  this.userform.get("afternoon_break_duration").value;

    record.entered_by_member_id = this.signInUser;

    this.insertBreaktimeRecord.emit(record);
  }

  // For validation
  get start_time() { return this.userform.get('start_time'); }
  get morning_break_start() { return this.userform.get('morning_break_start'); }
  get morning_break_duration() { return this.userform.get('morning_break_duration'); }
  get lunch_break_start() { return this.userform.get('lunch_break_start'); }
  get lunch_break_duration() { return this.userform.get('lunch_break_duration'); }
  get afternoon_break_start() { return this.userform.get('afternoon_break_start'); }
  get afternoon_break_duration() { return this.userform.get('afternoon_break_duration'); }
}
