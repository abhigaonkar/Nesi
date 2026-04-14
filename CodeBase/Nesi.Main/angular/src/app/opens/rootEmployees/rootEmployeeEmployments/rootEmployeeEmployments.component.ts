import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  selector: 'app-rootEmployeeEmployments',
  templateUrl: './rootEmployeeEmployments.component.html',
  styleUrls: ['./rootEmployeeEmployments.component.css']
})
export class RootEmployeeEmploymentsComponent extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
 
}
