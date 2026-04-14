import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  selector: 'nesi-rootEmployeeDisciplinary',
  templateUrl: './rootEmployeeDisciplinary.component.html',
  styleUrls: ['./rootEmployeeDisciplinary.component.css']
})
export class RootEmployeeDisciplinaryComponent  extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
 
}
