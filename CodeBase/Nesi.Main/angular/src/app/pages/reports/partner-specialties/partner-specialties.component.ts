import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';

@Component({
  selector: 'nesi-partner-specialties',
  templateUrl: './partner-specialties.component.html',
  styleUrls: ['./partner-specialties.component.css']
})
export class PartnerSpecialtiesComponent implements OnInit {
  @ViewChild(DatatableComponent) dt: DatatableComponent;

  constructor() { }

  ngOnInit() {
  }

  loadDetail(refreshbutton = false) {
    this.dt.loadReport();
  }
}
