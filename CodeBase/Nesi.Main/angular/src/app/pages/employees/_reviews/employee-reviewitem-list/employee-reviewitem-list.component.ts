import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { DataTable } from 'primeng/primeng';

@Component({
  selector: 'nesi-employee-reviewitem-list',
  templateUrl: './employee-reviewitem-list.component.html',
  styleUrls: ['./employee-reviewitem-list.component.css']
})
export class EmployeeReviewitemListComponent implements OnInit {

  _list = [];
  @Input() set list(value: any[]) {
    if (value) {
      this._list = value;
    }
  }

  get list() {
    return this._list;
  }

  @Input() locked = false;
  @Output() itemChanged = new EventEmitter();
  @ViewChild(DataTable) dt: DataTable;
  expand_all = false;
  constructor() { }

  ngOnInit() {
  }

  expandall(e) {
    this.dt.expandedRowsGroups = [];
    if (e) {
      this.dt.expandedRowsGroups = this.getGroupList();
    }
  }

  getGroupList() {
    return Array.from(new Set(this.list.map(x => x.group)).values());
  }

  submit(data) {
    setTimeout(() => {
      this.itemChanged.emit(data)
    }, 300);
  }

  get_score(x): number {
    const glist = this.list.filter(y => y.group === x);
    let sum = 0;
    let count = 0;
    glist.forEach(z => {
      const s = Number(z.score);
      if (s > 0) {
        sum += s;
        count++
      }
    });
    if (count > 0) {
      const score = Math.round(sum * 100 / count) / 100;
      return score;
    } else {
      return 0;
    }
  }

  get_score_text(x): string {
    const s = this.get_score(x);
    if (s > 0) {
      return s.toString();
    } else {
      return ' Not Scored'
    }
  }
}
