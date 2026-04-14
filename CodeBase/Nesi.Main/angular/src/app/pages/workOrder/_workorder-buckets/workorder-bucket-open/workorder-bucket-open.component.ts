import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'nesi-workorder-bucket-open',
  templateUrl: './workorder-bucket-open.component.html',
  styleUrls: ['./workorder-bucket-open.component.css']
})
export class WorkorderBucketOpenComponent implements OnInit {
  _list: any;

  @Input() height = 200;
  @Input() title: string;
  @Input() business_unit_id: number;

  @Input() set list(value: any[]) {
    this._list = value;
  }

  get list(): any[] {
    return this._list || [];
  }

  constructor() { }

  ngOnInit() {
  }

  get total() {
    return this.list.reduce((p, v) => p + (v.woprog_stilltobebilled || 0), 0);
  }
}
