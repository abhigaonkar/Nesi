import { Injectable, OnInit } from '@angular/core';
import { Response } from '@angular/http';
import { CONFIG } from '../../configuration';
import { HttpService } from '../../core/http.service';
import { Observable } from 'rxjs/Observable';
import { Profile } from '../../models/layout/profile';
import { ServiceBase } from '../shared/serviceBase';

@Injectable()
export class LayoutProfileService extends ServiceBase implements OnInit {

  constructor(protected http: HttpService, ) {
    super(http);
  }

  ngOnInit(): void {

  }


  getLayoutProfiles(): Observable<Profile[]> {
    return this.http.get(CONFIG.apiURL.layout.layoutProfiles).catch(() => [])
      .map((res: Response) => {
        const body = res.json();
        return body || {};
      })
      .map((payload: Profile[]) => {
        return payload;
      });
  }

  saveLayoutProfiles(profiles: Profile[]): Observable<Response> {
    // console.log(profiles);
    return this.http.put(CONFIG.apiURL.layout.layoutProfiles, JSON.stringify(profiles));
  }


}
