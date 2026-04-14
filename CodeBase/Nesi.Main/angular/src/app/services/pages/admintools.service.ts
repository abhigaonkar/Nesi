import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { CoreService } from 'app/services/shared/core.service';
import { SelectDurationComponent } from '../../pages/admintools/select-duration/select-duration.component';
import { Errorlog } from '../../pages/admintools/integ';
import { CONFIG } from 'app/configuration';
import { tap, catchError } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class AdmintoolsService {

  constructor(private cs: CoreService) { }

  getErrorlogs(duration: string): Observable<Errorlog> {
    const url = CONFIG.apiURL.page.admintools.integerrors + (<any>duration).ym.toString();
    return this.cs.getObject<Errorlog>(url);
  }

  getMonths(): Observable<Errorlog[]> {
    return this.cs.getList<Errorlog>(CONFIG.apiURL.page.admintools.selectmonth)
      .pipe(
        tap(res => console.log('Got audit error month', res)),
        catchError(this.handleErr('getMonths', []))
      );
  }
  handleErr(error: any, []) {
    return (error.message || error);
  }

}
