import { HttpService } from 'app/core/http.service';
import { ServiceBase } from './../../../services/shared/serviceBase';
import { Injectable } from '@angular/core';
import { DataService } from '../../nesi-datatable/service/dataservice';
import 'rxjs/add/operator/toPromise';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AddEditFormService extends ServiceBase {
  lookupPageCount: number;
  lookupOptions: Observable<any> = new Observable();

  constructor(private dataService: DataService, private httpService: HttpService) { super(httpService); }

  /**
  * Saves record to database
  * @param  {} newRecord
  * @returns void
  */
  saveRecord(newRecord, url, actionType): Observable<any> {
    if (actionType === 'create') {
      // this should be table key field. Right now only for customer asset
      delete newRecord['id'];
    }
    return this.dataService.addUpdate_v2(url, newRecord);
  }

  getLookupData(url, postObject, column, callback) {
    this.dataService.getData(url, postObject).subscribe((data) => {
      let lookupData = data.data || data;
      if (data.data) {
        lookupData.map((element) => {
          element.label = element[column.lookup.values[0].key];
          element.value = element[column.lookup.key];
          delete element[column.lookup.values[0].key];
          delete element[column.lookup.key];
        });
      }
      callback(lookupData);
    })
  }

}
