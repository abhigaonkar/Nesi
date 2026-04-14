import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { CoreService } from 'services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { tap, catchError } from 'rxjs/operators';
import {TimesheetShopProjects } from '../../pages/timesheet/interface/timesheetShopProjects'; 
import { UpdateTimesheetProject } from 'models/pages/timesheet/UpdateTimesheetProject';
import { ServiceBase } from 'services/shared/serviceBase';
import { HttpService } from '../../core/http.service';
import { InsertTimesheetProject } from 'models/pages/timesheet/InsertTimesheetProject';
@Injectable({
  providedIn: 'root'
})
export class Timesheet_shop_projectsService extends ServiceBase {
constructor(private cs:CoreService, 
            protected http: HttpService) { 
              super(http);
            }
getShopProjects():Observable<TimesheetShopProjects>  {
  const url=CONFIG.apiURL.page.timesheet.Projects;
  const data = this.cs.getObject<TimesheetShopProjects>(url);
  return data;
}
 /* return  this.cs.getObject<TimesheetShopProjects>(url)
        .pipe(
           tap(res => console.log(' Timesheet Project in service ', res)),
           catchError(this.handleErr('getShopProjects', []))
           );
 }*/
 public updateProject(model: UpdateTimesheetProject) :Observable <string>{
   return this.postString(CONFIG.apiURL.page.timesheet.InsertProject,model);
 }
 public insertProject(model: InsertTimesheetProject):Observable <string>{
   return this.postString(CONFIG.apiURL.page.timesheet.UpdateProject,model);
 }
 
 handleErr(error: any, []) {
  return (error.message || error);
}
}