import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { Errorlog } from '../integ';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { CoreService } from 'services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { DxSelectBoxComponent } from "devextreme-angular";
import { AdmintoolsService } from '../../../services/pages/admintools.service';

@Component({
  selector: 'nesi-select-duration',
  templateUrl: './select-duration.component.html',
  styleUrls: ['./select-duration.component.css']
})
export class SelectDurationComponent implements OnInit {
  @ViewChild(DxSelectBoxComponent) selectBox: DxSelectBoxComponent;
  months: Errorlog[] = [];

  display = false;
  selectedmonth;
  @Output() record = new EventEmitter<Errorlog>();
  constructor(private cs: CoreService, private serv: AdmintoolsService) {
  }
  onValueChanged(event) {
    this.selectedmonth = event.value;
    this.display = true;
    this.record.emit(this.selectedmonth);
  }

  ngOnInit() {
    this.serv.getMonths()
      .subscribe((res: any[]) => { this.months = res; });
    console.log(this.months);
  }
}


