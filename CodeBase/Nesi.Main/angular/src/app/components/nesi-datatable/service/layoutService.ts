import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { DataService } from 'app/components/nesi-datatable/service/dataservice';
import { MemberLayout } from 'app/components/nesi-datatable/components/datatable/datatable.component';
import { TokenService } from 'app/services/authentication/tokenService';

@Injectable()
export class LayoutService {


  constructor(private dataService: DataService, private tokenService: TokenService) {

  }


  loadMemberLayoutsByGridId(gridId: string, memberId: number, memberName: string): Observable<any> {
    const getLayoutBaseUrl = 'api/Layout/';
    const getLayoutMemberGridUrl: string = getLayoutBaseUrl + 'GetLayouts/' + gridId + '/' + memberId;
    return this.loadGridSchemaByGrid(gridId)
      .flatMap((gridSchemaData: any) => {
        return this.dataService.getSchema(getLayoutMemberGridUrl)
          .flatMap((memberLayouts: any) => {
            if (!(memberLayouts.data && memberLayouts.data.length)) {
              memberLayouts.data = this.setLayoutDefault(gridId, memberId, memberName, gridSchemaData.columns);
            }
            return Observable.of(memberLayouts);
          });
      });
  };

  loadGridSchemaByGrid(gridName): Observable<any> {
    const reportURL = 'api/Page/' + gridName;
    return this.dataService.getSchema(reportURL)
  }

  loadGridSchemaByGridName(gridId: string, memberId: number, memberName: string): Observable<any> {
    return this.loadMemberLayoutsByGridId(gridId, memberId, memberName)
      .flatMap((memberLayouts: ResponseData) => {
        return this.loadGridSchemaByGrid(gridId)
          .flatMap((gridSchema: any) => {
            return Observable.of({ memberLayouts, gridSchema });
          });
      });
  }

  loadMemberLayouts(gridId: string, memberId: number, memberName: string): Observable<any> {
    return this.loadMemberLayoutsByGridId(gridId, memberId, memberName)
      .map((schemaResponseData: ResponseData) => {
        return this.loadLayoutModelsCallback(schemaResponseData);
      });
  };

  getDefaultLayout(gridId: string, memberId: number, memberName: string, columnArray: any[]): MemberLayout {
    const defaultLayout = {
      'name': 'Default',
      'firstName': memberName,
      'fullName': memberName,
      'layout': JSON.stringify({
        'active': true,
        'groupArray': [],
        'columnArray': columnArray,
        'currentPage': 0,
        'paginationRows': 0,
        'filterObj': {},
        'multiSortMeta': {},
        'columnWidthList': []
      }),
      'gridId': gridId,
      'memberId': memberId,
      'isDefault': 0,
      'isMemberDefault': true
    }
    return defaultLayout as MemberLayout;
  };



  getFieldType(value) {
    if (value.toLowerCase().indexOf('string') !== -1) {
      return 'string';
    } else if (value.toLowerCase().indexOf('boolean') !== -1) {
      return 'boolean';
    } else if (value.toLowerCase().indexOf('datetime') !== -1) {
      return 'date';
    } else if (value.toLowerCase().indexOf('phone') !== -1) {
      return 'phone';
    } else if (value.toLowerCase().indexOf('currency') !== -1) {
      return 'currency';
    } else if (value.toLowerCase().indexOf('notes') !== -1) {
      return 'notes';
    } else if (value.toLowerCase().indexOf('textbox') !== -1) {
      return 'textbox';
    } else if (value.toLowerCase().indexOf('dropdown') !== -1) {
      return 'dropdown';
    } else if (value.toLowerCase().indexOf('email') !== -1) {
      return 'email';
    } else if (value.toLowerCase().indexOf('percent') !== -1) {
      return 'percent';
    } else if (value.toLowerCase().indexOf('edit') !== -1) {
      return 'edit';
    } else if (value.toLowerCase().indexOf('popuplink') !== -1) {
      return 'popuplink';
    } else if (value.toLowerCase().indexOf('fa') !== -1) {
      return 'fa';
    } else if (value.toLowerCase().indexOf('newnts') !== -1) {
      return 'newnts';
    } else if (value.toLowerCase().indexOf('spinner') !== -1) {
      return 'spinner';
    } else {
      return 'number';
    }
  }

  setColumnProperties(columnPropertyList: any): any {
    const summaryConfig = {};
    const columnArray = [];
    const columnOptions = [];
    let len: number = columnPropertyList.length;
    let count = 0;
    columnPropertyList.forEach(element => {
      if (JSON.parse(element.display)) {
        count++;
      }
    });
    len = count;
    let width = '1em';
    let widthPercent = 1;
    if (len) {
      width = (50 / (len + 1)).toFixed(2);
      widthPercent = (100 / (len + 1));
    }
    const defaultStyle = { 'fontweight': 'normal' };

    columnArray.push({
      first: true, display: true, order: -10
      , Style: { 'font-weight': 'normal' }
      , widthPercent: widthPercent
      , field: 'firstColumn', label: ''
    });

    columnPropertyList.forEach((columnPropertyListItem: any, index: number) => {
      if (columnPropertyListItem.summary) {
        summaryConfig[columnPropertyListItem.name] = columnPropertyListItem.summary;
      }
      const style = columnPropertyListItem.style ? JSON.parse(columnPropertyListItem.style) : defaultStyle;
      const type = this.getFieldType(columnPropertyListItem.rawType);
      let defaultFormat;
      if (type === 'date') {
        defaultFormat = {};
        defaultFormat.format = 'yyyy-MM-dd';
      } else if (type === 'currency') {
        defaultFormat = {};
        defaultFormat.currency = 'CAD';
        defaultFormat.format = '1.2-2'
      } else if (type === 'percent') {
        defaultFormat = {};
        defaultFormat.format = '1.2-2';
      }
      columnArray.push({
        format: columnPropertyListItem.displayFormat ? JSON.parse(columnPropertyListItem.displayFormat) : defaultFormat,
        defaultFilter: columnPropertyListItem.defaultFilter,
        isKey: columnPropertyListItem.isKey,
        field: columnPropertyListItem.name,
        label: columnPropertyListItem.header,
        filter: columnPropertyListItem.rawType !== 'edit',
        order: columnPropertyListItem.ordinal,
        filterType: 'custom',
        editable: columnPropertyListItem.editable,
        lookup: columnPropertyListItem.lookup,
        type: this.getFieldType(columnPropertyListItem.rawType),
        display: JSON.parse(columnPropertyListItem.display),
        first: false,
        sort: columnPropertyListItem.rawType !== 'edit',
        summary: {},
        validations: columnPropertyListItem.validation,
        color: columnPropertyListItem.backcolor,
        currency: columnPropertyListItem.currency,
        hyperlink: columnPropertyListItem.hyperlink,
        hyperlinkUrl: columnPropertyListItem.hyperlinkUrl,
        hyperlinkUrlParam: columnPropertyListItem.hyperlinkUrlParam,
        mobileHyperlinkUrl: columnPropertyListItem.mobileHyperlinkUrl,
        Style: { 'font-weight': style.fontweight ? style.fontweight : 'normal' },
        widthPercent: widthPercent,
        rawType: columnPropertyListItem.rawType,
        toolTip: columnPropertyListItem.toolTip,
        hideItemFilter: columnPropertyListItem.hideItemFilter
      });

    });

    columnArray.sort(function (a, b) {
      return a['order'] - b['order'];
    })

    columnArray.forEach((eachOjs) => {
      if (eachOjs.display) {
        columnOptions.push({ label: eachOjs.label, value: eachOjs });
      }
      // columnOptions.push({ label: eachOjs.label, value: eachOjs });
    });

    columnOptions.sort((a, b) => {
      if (a.label > b.label) {
        return 1;
      } else {
        return -1;
      }
    });

    // baseCoulmnArray = Object.assign([], columnArray);
    return { columnOptions, columnArray, summaryConfig };
  };




  setLayoutDefault(gridId: string, memberId: number, memberName: string, columnProperties: any[]): any[] {
    const columnInformation = this.setColumnProperties(columnProperties);
    return [this.getDefaultLayout(gridId, memberId, memberName, columnInformation.columnArray)];
  };



  private isJSON(value: string) {
    try {
      JSON.parse(value);
      return true;
    } catch (e) {
      return false;
    }
  }
  private loadLayoutModelsCallback(response: ResponseData) {
    // show all layouts of selected member
    const layoutDetails = [];
    const layoutData: Array<LayoutModel> = <LayoutModel[]>response.data;
    layoutData.forEach((member) => {
      if (this.isJSON(member.layout)) {
        const layout: LayoutModel = member;
        layout.owner = member.fullName;
        layoutDetails.push(layout);
      };
    });
    layoutDetails.sort((a: LayoutModel, b: LayoutModel): number => {
      return (b.isDefault - a.isDefault);
    });

    return layoutDetails;
  }

}

export class ResponseData {
  totalCount: number;
  data: any[] = [];
}

export class LayoutModel {
  id: number;
  firstName: string;
  memberId: number;
  name: string;
  fullName: string;
  layout: string;
  isDefault: number;
  gridId: string;
  owner: string;
}
