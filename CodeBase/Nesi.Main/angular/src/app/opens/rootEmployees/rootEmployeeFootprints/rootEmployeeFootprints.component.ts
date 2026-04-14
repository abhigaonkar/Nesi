import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  selector: 'nesi-rootEmployeeFootprints',
  templateUrl: './rootEmployeeFootprints.component.html',
  styleUrls: ['./rootEmployeeFootprints.component.css']
})
export class RootEmployeeFootprintsComponent extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
 
}
