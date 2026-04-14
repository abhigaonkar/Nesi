import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import {
  BranchBreakTimeRequirementInputParameter,
  BreakTimeQueryParameter,
  BreakTimeRecord,
  BreakTimeRecordRequirementRecord,
  BreakTimeRecordResult,
  OperationType,
  IBreakTimeService,
} from 'models/pages/timesheet/breaktime';

@Component({
  selector: 'app-breaktime-popup',
  templateUrl: './breaktime-popup.component.html',
  styleUrls: ['./breaktime-popup.component.css']
})
export class BreaktimePopupComponent implements OnInit {
  public showup: boolean;
  public record: BreakTimeRecord;
  public userform: FormGroup;
  public updatePermission = false;

  @Input()
  set display(value: any) {
    this.showup = <any>value.val;
    this.record = value.record;

    if(!this.record) {
      return;
    }

    this.userform.reset({
      id: this.record.id,
      date: this.record.date,
      business_unit_id: this.record.business_unit_id,
      member_id: this.record.member_id,
      
      start_time: this.record.start_time.substr(0, 5),
      morning_break_start: this.record.morning_break_start.substr(0, 5),
      morning_break_duration: this.record.morning_break_duration,
      
      lunch_break_start: this.record.lunch_break_start.substr(0, 5),
      lunch_break_duration: this.record.lunch_break_duration,
      afternoon_break_start: this.record.afternoon_break_start.substr(0, 5),
      afternoon_break_duration:this.record.afternoon_break_duration,
      updatePermission: !this.record.locked
    });

    this.updatePermission = this.userform.get("updatePermission").value;
  }

  @Input()
  set currentRecord(value: BreakTimeRecord) {
    this.record = value;
  }

  @Output() updateBreaktimeRecord = new EventEmitter<BreakTimeRecord>();

  constructor(private fb: FormBuilder) {
    this.createForm();
   }

  ngOnInit() {
  }

  close() {
    this.showup = false;
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
      entered_by_member_id: '',
      updatePermission: true
    });
  }

  onSubmit() {
    const record = new BreakTimeRecord();
    record.id = this.userform.get("id").value;
    record.date = this.userform.get("date").value;
    record.business_unit_id = this.userform.get("business_unit_id").value;
    record.member_id = this.userform.get("member_id").value;

    if(this.userform.get("start_time").value.length === 5){
      record.start_time = this.userform.get("start_time").value;
    } else{
      const d = (<Date>this.userform.get("start_time").value);
      record.start_time = d.getHours() + ":" + d.getMinutes();
    }

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

    this.updateBreaktimeRecord.emit(record);
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
