import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Fvr } from '../../../models/layout/fvr';
import { WindowRef } from '../../../services/shared/windowRef';

@Component({
  selector: 'nesi-fvrTable',
  templateUrl: './fvrTable.component.html',
  styleUrls: ['./fvrTable.component.css']
})
export class FvrTableComponent implements OnInit {

  @Input()
  fvrs: Fvr[];

  constructor(
    public winRef: WindowRef,
  ) {
  }

  ngOnInit() {
  }

  boing(id: number) {
    this.winRef.boingFvr(id);
  }

}
