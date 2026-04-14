import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  selector: 'nesi-rootEmployeeWage',
  templateUrl: './rootEmployeeWage.component.html',
  styleUrls: ['./rootEmployeeWage.component.css']
})
export class RootEmployeeWageComponent  extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
 
}