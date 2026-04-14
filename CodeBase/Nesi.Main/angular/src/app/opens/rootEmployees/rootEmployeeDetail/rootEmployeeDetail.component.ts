import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CoreService } from '../../../services/shared/core.service';
import { Subscription } from 'rxjs';
import { RootEmployeeBase } from '../rootEmployeeBase';

@Component({
  selector: 'nesi-rootEmployeeDetail',
  templateUrl: './rootEmployeeDetail.component.html',
  styleUrls: ['./rootEmployeeDetail.component.css']
})
export class RootEmployeeDetailComponent extends RootEmployeeBase implements OnInit {


  constructor(
    public route: ActivatedRoute,
  ) {
    super(route);
   }
 
}
