import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  selector: 'nesi-rootEmployeeDaysOff',
  templateUrl: './rootEmployeeDaysOff.component.html',
  styleUrls: ['./rootEmployeeDaysOff.component.css']
})
export class RootEmployeeDaysOffComponent  extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
 
}
