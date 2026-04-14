import { Component, OnInit } from '@angular/core';
import { RootWorkorderBase } from '../rootWorkorderBase';
import { ActivatedRoute } from '@angular/router';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-rootWorkorder-wocomments',
  templateUrl: './rootWorkorder-wocomments.component.html',
  styleUrls: ['./rootWorkorder-wocomments.component.css']
})
export class RootWorkorderWocommentsComponent extends RootWorkorderBase implements OnInit {

  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
  }

  ngOnInit() {
  }

}
