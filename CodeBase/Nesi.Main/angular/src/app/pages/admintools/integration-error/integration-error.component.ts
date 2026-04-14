import { Component, OnInit,Input, ViewChild, Output } from '@angular/core';
import {Errorlog} from '../integ';
import { Observable } from 'rxjs';
import { tap, catchError} from 'rxjs/operators';
import { CoreService } from 'services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { DxDataGridComponent } from 'devextreme-angular';
import{ AdmintoolsService} from '../../../services/pages/admintools.service';
@Component({
  selector: 'nesi-integration-error',
  templateUrl: './integration-error.component.html',
 styleUrls: ['./integration-error.component.css'],
})
export class IntegrationErrorComponent implements OnInit {
  @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;
  data : Errorlog;


  @Input('dur') dur: any;
 
  constructor(private cs: CoreService,private serv:AdmintoolsService) { }
 // checkIfExist(id: number) {
//    this.show=this.data.message_type != "";
 // return this.show}
  ngOnInit() {   
   this.serv.getErrorlogs(this.dur) 
                          .subscribe((res) => {this.data=res; console.dir(res) }); 
  
 }
  
}
  
 


