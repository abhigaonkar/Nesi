import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  selector: 'nesi-rootEmployeeIT',
  templateUrl: './rootEmployeeIT.component.html',
  styleUrls: ['./rootEmployeeIT.component.css']
})
export class RootEmployeeITComponent  extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
 
}
