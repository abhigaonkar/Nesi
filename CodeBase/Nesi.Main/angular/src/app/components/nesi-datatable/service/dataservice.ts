import { Injectable } from '@angular/core';
import {
  Headers, Http, Response, URLSearchParams, RequestOptions,
  RequestOptionsArgs,
  Request,
  XHRBackend
} from '@angular/http';
import 'rxjs/add/operator/toPromise';
import * as Api from 'environments/environment';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import { NesiRequestOptions } from 'app/core/nesi-request-options';
import { CONFIG } from 'app/configuration';
import { saveAs } from 'file-saver/FileSaver';
import { HttpService } from 'app/core/http.service';
import { Observable } from 'rxjs/Observable';



@Injectable()
export class DataService {

  constructor(private http: HttpService,
    private router: Router) {
  }

  /*   private setHeaders(): Headers {
      let user;
      if (localStorage.getItem(CONFIG.authentication.authDataString)) {
        user = JSON.parse(atob(localStorage.getItem(CONFIG.authentication.authDataString)));
      }
      const token = user && user.access_token;
      if (token) {
        let headersConfig = new Headers();
        headersConfig.append('Content-Type', 'application/json');
        headersConfig.append('Accept', 'application/json');
        headersConfig.append('Authorization', 'Bearer ' + token);
        return headersConfig;
      } else {
        let headersConfig = {
          'Content-Type': 'application/json',
          'Accept': 'application/json'
      }
        return new Headers(headersConfig);
      }
    } */

  getData(url, columns) {
    const dataURL = url;
    return this.http.post(dataURL, columns).map(res => res.json())
  }

  getSchema(url) {
    const dataURL = url;
    return this.http.get(dataURL).map(res => res.json())
  }

  getFile(url: String, columns: any, cb: Function) {
    let user;
    if (localStorage.getItem(CONFIG.authentication.authDataString)) {
      user = JSON.parse(atob(localStorage.getItem(CONFIG.authentication.authDataString)));
    }
    const token = user && user.access_token;
    const dataURL = CONFIG.apiURL.host() + url;
    const xhr = new XMLHttpRequest();
    xhr.open('POST', dataURL, true);
    xhr.setRequestHeader('Content-type', 'application/json');
    xhr.setRequestHeader('Accept', 'application/json');
    xhr.setRequestHeader('Authorization', 'Bearer ' + token);
    xhr.responseType = 'blob';
    xhr.onreadystatechange = function () {
      if (xhr.readyState === 4) {
        if (xhr.status === 200) {
          cb(xhr);
        } else if (xhr.status === 401) {
          window.location.href = '/index.html'; // Redirect to login page when token expires.
        } else {
          CONFIG.LOG('Error');
        }
      }
    }
    xhr.send(JSON.stringify(columns));
  }

  addUpdate_v2(url, entity) {
    const dataURL = url;
    return this.http.post(dataURL, entity)
  }

  delete_v2(url, data) {
    const dataURL = url;
    return this.http.post(dataURL, data)
      .toPromise()
      .then(this.extractData);
  }

  bulkEdit_v2(entity, url) {
    const dataURL = url;
    return this.http.post(dataURL, entity)
      .toPromise()
      .then(this.extractData);
  }

  getUrlParam(key) {
    return this.router.url.replace('/', '').toUpperCase() + key;
  }

  deleteRecord(apiUrl, id) {
    const dataURL = apiUrl;
    const urlSearchParams = new URLSearchParams();
    urlSearchParams.append('id', id);
    const body = urlSearchParams.toString();
    return this.http.post(dataURL, { 'id': id })
      .toPromise()
      .then(this.extractData);
  }

  editRecord(apiUrl, entity) {
    const dataURL = apiUrl;
    return this.http.post(dataURL, entity)
      .toPromise()
      .then(this.extractData);
  }


  // getFilterData(columnName) {
  //   return this.http.get(Api.ApiUrls.FILTER_DATA + columnName, { headers: this.setHeaders() })
  //     .toPromise()
  //     .then(res => res.json())
  //     .then(data => {
  //       return formatList(columnName, data);
  //     });

  //   function formatList(column, dataArray) {
  //     let customArray = [];
  //     dataArray = JSON.parse(dataArray);
  //     if (!dataArray || dataArray.length === 0) {
  //       return;
  //     } else {
  //       for (let i = 0; i < dataArray[column].length; i++) {
  //         customArray.push({ 'label': column, 'value': dataArray[column][i] });
  //       }
  //     }
  //     return customArray;
  //   }
  // }

  getLayouts(gridId, memberId) {

    return this.http.get(CONFIG.ApiUrls.GET_LAYOUTS + '/' + gridId + '/' + memberId)
      .toPromise()
      .then(res => res.json());
  }

  addLayout(params) {
    return this.http.post(CONFIG.ApiUrls.ADD_LAYOUT, params)
      .toPromise()
      .then(res => res.text())
      .then(data => {
        return data;
      });
  }

  deleteLayout(params) {
    return this.http.post(CONFIG.ApiUrls.DELETE_LAYOUT + params.id, null)
      .toPromise()
      .then(res => res.text())
      .then(data => {
        return data;
      })

  }

  updateLayout(params): Observable<Response> {
    return this.http.post(CONFIG.ApiUrls.UPDATE_LAYOUT, params)
      .map(response => response.json())
      .catch((error: any) => Observable.throw(error));
  };

  formatLayoutRequest(params) {
    return {
      'id': params.index,
      'memberId': params.userId,
      'name': params.name,
      'layout': params.layout,
      'gridId': params.gridId,
      'isDefault': params.isDefault
    }
  }

  private extractData(res: Response) {
    const body = res.json();
    return body || {};
  }

  // private extractData_V2(res: Response) {
  //   let body = String(res.text());
  //   return body || '';
  // }

  private handleErrorPromise(error: Response | any) {
    CONFIG.LOG(error.message || error);
    return Promise.reject(error.message || error);
  }
}
