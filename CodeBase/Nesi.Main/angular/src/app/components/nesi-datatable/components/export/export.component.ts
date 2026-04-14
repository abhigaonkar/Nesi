import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UtilityService } from '../../service/utilityService';
import { CONFIG } from 'app/configuration'
import { DataService } from '../../service/dataservice';
import { saveAs } from 'file-saver/FileSaver';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-export',
  templateUrl: './export.component.html',
  styleUrls: ['./export.component.css']
})
export class ExportComponent implements OnInit {

  @Input() name;

  @Input() type;

  @Input() reportTitle;

  @Input() fileName;

  @Input() groupColumns;

  @Input() defaultColumns;

  @Input() sortingObject;

  @Input() filterObject;

  @Input() apiObject;

  @Input() golbalFilter;
  @Input() reportQueryParam;
  @Input() visible_business_string;

  @Input() filterBuilder;

  @Input() otherFilterBuilders;
  @Input() otherFilterConditions;
  @Output() start = new EventEmitter();
  @Output() end = new EventEmitter();

  private exportUrl: String;

  constructor(private utilityService: UtilityService, private dataService: DataService) { }

  ngOnInit() {
  }

  getColumn(field) {
    for (let i = 0; i < this.defaultColumns.length; i++) {
      if (this.defaultColumns[i].field === field) {
        return this.defaultColumns[i];
      }
    }
  }

  exportExcel(): void {
    this.start.emit();
    let exportParameter;
    if (this.type === 'excel') {
      exportParameter = 'exportToExcel';
    } else if (this.type === 'pdf') {
      exportParameter = 'exportToPdf';
    }
    this.exportUrl = this.apiObject[exportParameter];

    const visibleColumns = [];
    for (let i = 1; i < this.defaultColumns.length; i++) {
      if (this.defaultColumns[i].display) {
        visibleColumns.push(this.defaultColumns[i].field);
      }
    }

    const postObject = {};
    postObject['groupby'] = this.utilityService.getGroupedItems(this.groupColumns);
    postObject['sortby'] = this.utilityService.getMultiSortMeta(this.sortingObject);
    postObject['globalfilter'] = this.golbalFilter.value;
    postObject['queryparam'] = this.reportQueryParam;
    postObject['bu_ids'] = this.visible_business_string;
    postObject['filterBuilder'] = this.filterBuilder;
    postObject['otherFilterBuilders'] = this.otherFilterBuilders;
    postObject['otherFilterConditions'] = this.otherFilterConditions;

    const filterObject = Object.keys(this.filterObject);
    filterObject.forEach((element, index) => {
      if (this.filterObject[element].matchMode === '') {
        const column = this.getColumn(element);
        this.filterObject[element].matchMode = (column && column['type'] === 'number') ? 'equals' : 'startsWith';
      }
    });

    postObject['filtercolumn'] = this.utilityService.getFilterColumn(this.filterObject);
    postObject['columns'] = visibleColumns ;

    this.dataService.getFile(this.exportUrl, postObject, (xhr) => {
      let contentType: string;
      let extension: string;
      // const currentDateTime = new Date().toLocaleString('en-US',{ hour12: false });
      const currentDateTime = new Date().toISOString().replace('T', ' ').split('.')[0];
      if (this.type === 'excel') {
        contentType = 'vnd.openxmlformats-officedocument.spreadsheetml.sheet';
        extension = 'xlsx';
      } else if (this.type === 'pdf') {
        contentType = 'application/pdf';
        extension = 'pdf';
      }
      let nameWithDateAndExtension = `${this.fileName}_${currentDateTime}.${extension}`;
      nameWithDateAndExtension = nameWithDateAndExtension.replace(/ /g, '').replace(/-/g, '').replace(/:/g, '');
      const blob = new Blob([xhr.response], { type: contentType });
      saveAs(blob, nameWithDateAndExtension);
      this.end.emit();
    });
  }

  getClass(type): string {
    let className;
    if (type === 'pdf') {
      className = 'fa fa-file-pdf';
    } else if (type === 'excel') {
      className = 'fa fa-file-excel';
    }
    return className;
  }
}
