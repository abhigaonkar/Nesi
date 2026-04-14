import { Injectable } from '@angular/core';

@Injectable()

export class UtilityService {

  //  EXPORT TO #PDFAND  #EXCEL HANDLERS.

  getGroupedItems(groupArr) {

    return groupArr.join('^')
  }

  getSelectcolumn(selectClmn) {

    const filterClmnArr = [];
    for (let i = 0; i < selectClmn.length; i++) {
      if (selectClmn[i] && selectClmn[i].field && selectClmn[i].display) {
        filterClmnArr.push(selectClmn[i].field);
      }
    }
    return filterClmnArr.join();
  }

  getMultiSortMeta(multiSortMeta) {

    const multiSortMetaArr = [];
    for (let i = 0; i < multiSortMeta.length; i++) {
      multiSortMetaArr.push(multiSortMeta[i].field + (multiSortMeta[i].order === 1 ? '' : '||DESC'));
    }
    return multiSortMetaArr.join('^');
  }

  getFilterColumn(filterObj) {

    const filterObjArr = [];
    const filterObjKeys = Object.keys(filterObj);
    for (let i = 0; i < filterObjKeys.length; i++) {
      let filterValueString;
      if (Array.isArray(filterObj[filterObjKeys[i]].value)) {
        filterValueString = filterObj[filterObjKeys[i]].value.join('^');
      }
      filterObjArr.push(filterObjKeys[i] + '||'
        + (filterValueString ? filterValueString : filterObj[filterObjKeys[i]].value) + '||' + filterObj[filterObjKeys[i]].matchMode);
    }
    return filterObjArr.join('`');
  }

  getUniqueArrayOrObjects(firstArray, secondArray, field) {

    const tmpHash = {};
    for (let i = 0; i < firstArray.length; i++) {
      tmpHash[firstArray[i].field] = 1;
    }

    for (let j = 0; j < secondArray.length; j++) {
      if (!tmpHash[secondArray[j].field]) {
        firstArray.push(secondArray[j]);
      } else {
        firstArray.splice(Object.keys(tmpHash).indexOf(secondArray[j].field), 1, secondArray[j])
      }
    }
    return firstArray;
  }

  getIndexFromArrayOfObjects(array, field, value) {
    return array.findIndex(x => x[field] === value);
  }

}
