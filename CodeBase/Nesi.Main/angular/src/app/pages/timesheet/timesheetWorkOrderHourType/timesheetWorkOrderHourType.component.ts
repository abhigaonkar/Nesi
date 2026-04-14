import { Component, OnInit, Output, EventEmitter, forwardRef, ViewChild, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, NgModel, ControlValueAccessor } from '@angular/forms';
import { ValueAccessorBase } from '../../../services/shared/value-accessor';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';
import { AdvanceValueAccessorBase } from '../../../services/shared/advanceValue-accessor';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-timesheetWorkOrderHourType',
  template: `
  <p-dropdown  [editable]="editable" [disabled]="(!editable && !loaded) || disabled  || !(options && options.length >0)"
  [style]="{'width':'100%'}"
  [options]="options" [(ngModel)]="value"
  (onChange)="onchanged($event)" [required]="required"
  [filter]="false">
  <ng-template let-item pTemplate="item">
    <div>{{item.label}}</div>
  </ng-template></p-dropdown>
  `,
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => TimesheetWorkOrderHourTypeComponent), multi: true }]

})

export class TimesheetWorkOrderHourTypeComponent extends AdvanceValueAccessorBase<any> implements OnInit {
  
  canApplyMileage=false;
  canApplyTravel=false;
  constructor(
    coresvs: CoreService,
  ) {
    super();
    this.cs = coresvs;
  }
  ngOnInit() {

  }
  public getHourType() 
    {
      this.items.forEach(hrtype => {
        return {
          label: hrtype[this.labelField],
          value: hrtype[this.valueField]
        }
      }); 
}
  public filterHourType(allowedPaytypeforJobtype: string[], filterbool: boolean) {
    this.filter = filterbool;   
        if (!(this.filter && this.items)) {
          return
        };       
        this.options = [];
        this.items.forEach(a => {
          for (let i = 0; i <= allowedPaytypeforJobtype.length; i++) {
            if (a && a['payTypeHours_ID'] === allowedPaytypeforJobtype[i]) {
              this.options.push({ label: a[this.labelField], value: a[this.valueField] });  
                            
            }
          }
        }) ;
       
      }
 public getExtrapaytype(extraPaytypes: string[]){
    this.canApplyMileage= false;
    this.canApplyTravel= false;
    
        this.items.forEach(paytype => {
          if(extraPaytypes!=undefined &&extraPaytypes.length>0){
            for(let i= 0;i<extraPaytypes.length; i++){  
                 if(extraPaytypes[i]=="8")  {
                   this.canApplyMileage= true;
                 }  
                 if(extraPaytypes[i]=="7")  {
                  this.canApplyTravel= true;
                }  
                                   
            }            
          }
        })
      }
}
