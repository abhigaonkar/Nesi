import {Injectable} from '@angular/core';



@Injectable()
export class LocalDbService {
  localDB: any = window.localStorage;

  constructor () {}

  getItemFromLocalDb (hashKey, dataFormat) {
    const catchedGroupArray = this.localDB.getItem(hashKey);
    return catchedGroupArray ? dataFormat.toLowerCase() === 'json' ? JSON.parse(catchedGroupArray) : catchedGroupArray : '';
  }

  setItemIntoLocalDb (hashKey, data, dataFormat) {
    if (dataFormat && dataFormat.toLowerCase() === 'json') {
      data = JSON.stringify(data);
    }
    this.localDB.setItem(hashKey, data);
    return 'DONE';
  }
}
