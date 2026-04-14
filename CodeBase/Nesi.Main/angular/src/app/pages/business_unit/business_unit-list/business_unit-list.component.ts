import { Component, OnInit } from '@angular/core';
import { BusinessUnitList } from '../../../models/pages/business_unit-list';
import { BusinessUnitService } from '../../../services/pages/business_unit.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-businessunitlist',
  templateUrl: './business_unit-list.component.html',
  styleUrls: ['./business_unit-list.component.css']
})
export class BusinessUnitListComponent implements OnInit {
public items: BusinessUnitList[];
public totalRecords: number;
public totalPages: number;

  constructor(private dataService: BusinessUnitService) { }

  ngOnInit() {
    this.loadTable();
  }

  loadTable() {
    this.dataService.getBusinessUnitList().then(
      (res) => {
          this.items = <BusinessUnitList[]> res.json();
         console.log(this.items);
        });
  }

}
