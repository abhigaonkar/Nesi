import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-rootEmployeeTermiantion',
  templateUrl: './rootEmployeeTermiantion.component.html',
  styleUrls: ['./rootEmployeeTermiantion.component.css']
})
export class RootEmployeeTermiantionComponent extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
  }

}
