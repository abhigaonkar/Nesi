import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-rootEmployeeUserInfo',
  templateUrl: './rootEmployeeUserInfo.component.html',
  styleUrls: ['./rootEmployeeUserInfo.component.css']
})
export class RootEmployeeUserInfoComponent extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
  }

}
