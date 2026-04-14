import { Injectable, OnInit } from '@angular/core';
import { Response } from '@angular/http';
import { CONFIG } from '../../configuration';
import { HttpService } from '../../core/http.service';

@Injectable()
export class BusinessUnitService implements OnInit {

  constructor(private http: HttpService)  {

  }

   ngOnInit(): void {

  }

  getBusinessUnitList(): Promise<Response> {
    return this.http.get(CONFIG.apiURL.page.businessUnitList).toPromise();
  }





}
