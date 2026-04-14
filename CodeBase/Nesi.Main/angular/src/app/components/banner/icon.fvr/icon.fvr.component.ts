import { Component, OnInit, Input } from '@angular/core';
import { Fvr } from '../../../models/layout/fvr';

@Component({
  selector: 'bar-iconfvr',
  templateUrl: './icon.fvr.component.html',
  styleUrls: ['./icon.fvr.component.css']
})
export class IconFvrComponent implements OnInit {
  @Input()
  fvrs: Fvr[];

  constructor() { }

  ngOnInit() {
  }

}
