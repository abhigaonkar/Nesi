import { ComplexDateTimeObject, LabelValue, dateMetadata, COMPLEXDATE_CONSTANT, FilterRoot } from './../../../../models/component/filterBuilder/filterBuilder';
import { Component, EventEmitter, Input, Output, ViewChild, ChangeDetectionStrategy } from '@angular/core';
import { CONFIG } from 'app/configuration';
import { Calendar } from 'primeng/primeng';
import { setTimeout } from 'timers';

@Component({
    selector: 'date-control',
    templateUrl: './date.control.component.html'
    // changeDetection: ChangeDetectionStrategy.OnPush
})

export class DateControl {
    @Input('field') field: any;

    private _filterRoot: FilterRoot;
    @Input('filterRoot') 
    set filterRoot( filterRoot: FilterRoot)
    {
        console.log(`${filterRoot} -> called at ${new Date()}`);
        this._filterRoot = filterRoot;

        if (this._filterRoot.complexDateTimeList == undefined || this._filterRoot.complexDateTimeList == null) {
            this._list = [];
            this.updateUI();
            return;
        }

        let complexDateTimeList: Array<ComplexDateTimeObject > = [];
        this._filterRoot.complexDateTimeList.forEach(element => {
            if (element.columnName === this.field) {
                complexDateTimeList.push(element);
            }
        });

        this._list = complexDateTimeList;
        this.updateUI();
    }

    @Input() 
    set complexDatetimeObjectList(list: Array<ComplexDateTimeObject>) {
        this._list = list;
        this.updateUI();
    }

    get complexDatetimeObjectList(): Array<ComplexDateTimeObject> {
        return this._list;
    }

    @ViewChild('from') private from: Calendar;
    @ViewChild('end') private end: Calendar;

    private _list: Array<ComplexDateTimeObject>
    public fromDate: Date;
    public toDate: Date;
    public checkedValues: string[] = [];
    public checkBoxOptions: string[] = []; //  ['Yesterday', 'Last Month', 'Today', 'This Month', 'Tomorrow', 'Next Month', 'Last Week', 'Last Year', 'This Week', 'This Year', 'Next Week', 'Next Year'];
    public complexDatetimeObjects: Array<ComplexDateTimeObject>=[]
    @Output() filterRecordsBetweenDates: EventEmitter<any> = new EventEmitter();
    @Output() hideOverlay: EventEmitter<any> = new EventEmitter();

    submit(event) {
        const queryString = this.createQueryString();
        this.filterRecordsBetweenDates.emit({
            field: this.field,
            value: queryString,
            complexDatetimeObjects: this.complexDatetimeObjects
        });
    }


    /**
     * Returns filter query string for checked options
     * @returns string
     */
    createQueryString(): string {
        this.complexDatetimeObjects=[];
        let queryString: string = '';
        if (this.checkedValues.indexOf('Yesterday') > -1) {
            queryString += this.field + '||greaterThanOrEqual LocalDateTimeYesterday^lessThan LocalDateTimeToday||complexDateTime`';
            this.createComplexDateTimeObject('Yesterday', this.field);
        }
        if (this.checkedValues.indexOf('Today') > -1) {
            queryString += this.field + '||greaterThanOrEqual LocalDateTimeToday^lessThan LocalDateTimeTomorrow||complexDateTime`';
            this.createComplexDateTimeObject('Today', this.field);
        }
        if (this.checkedValues.indexOf('Tomorrow') > -1) {
            queryString += this.field + '||greaterThanOrEqual LocalDateTimeTomorrow^lessThan LocalDateTimeDayAfterTomorrow||complexDateTime`';
            this.createComplexDateTimeObject('Tomorrow', this.field);
        }
        if (this.checkedValues.indexOf('Last Week') > -1) {
            queryString += this.field + '||greaterThanOrEqual LocalDateTimeLastWeek^lessThan LocalDateTimeThisWeek||complexDateTime`';
            this.createComplexDateTimeObject('Last Week', this.field);
        }
        if (this.checkedValues.indexOf('This Week') > -1) {
            queryString += this.field + '||greaterThanOrEqual LocalDateTimeThisWeek^lessThan LocalDateTimeNextWeek||complexDateTime`';
            this.createComplexDateTimeObject('This Week', this.field);
        }
        if (this.checkedValues.indexOf('Next Week') > -1) {
            queryString += this.field + '||greaterThanOrEqual LocalDateTimeNextWeek^lessThan LocalDateTimeTwoWeeksAway||complexDateTime`';
            this.createComplexDateTimeObject('Next Week', this.field);
        }
        if (this.checkedValues.indexOf('Next Year') > -1) {
            queryString += this.field + '||greaterThanOrEqual LocalDateTimeNextYear^lessThan AddYears(LocalDateTimeNextYear,1)||complexDateTime`';
            this.createComplexDateTimeObject('Next Year', this.field);
        }
        if (this.checkedValues.indexOf('This Year') > -1) {
            queryString += this.field + '||greaterThanOrEqual LocalDateTimeThisYear^lessThan LocalDateTimeNextYear||complexDateTime`';
            this.createComplexDateTimeObject('This Year', this.field);
        }
        if (this.checkedValues.indexOf('Last Year') > -1) {
            queryString += this.field + '||greaterThanOrEqual AddYears(LocalDateTimeThisYear,-1)^lessThan LocalDateTimeThisYear||complexDateTime`';
            this.createComplexDateTimeObject('Last Year', this.field);
        }
        if (this.checkedValues.indexOf('Next Month') > -1) {
            queryString += this.field + '||greaterThanOrEqual LocalDateTimeNextMonth^lessThan AddMonths(LocalDateTimeNextMonth,1)||complexDateTime`';
            this.createComplexDateTimeObject('Next Month', this.field);
        }
        if (this.checkedValues.indexOf('This Month') > -1) {
            queryString += this.field + '||greaterThanOrEqual LocalDateTimeThisMonth^lessThan LocalDateTimeNextMonth||complexDateTime`';
            this.createComplexDateTimeObject('This Month', this.field);
        }
        if (this.checkedValues.indexOf('Last Month') > -1) {
            queryString += this.field + '||greaterThanOrEqual AddMonths(LocalDateTimeThisMonth,-1)^lessThan LocalDateTimeThisMonth||complexDateTime`';
            this.createComplexDateTimeObject('Last Month', this.field);
        }


        //const from = this.fromDate ? this.fromDate.getMonth() + 1 + "/" + this.fromDate.getDate() + "/" + this.fromDate.getFullYear() : '';
        const from = this.fromDate ? this.fromDate.getFullYear() + "-" + (this.fromDate.getMonth() + 1) + "-" + this.fromDate.getDate() : '';

        const milliSecsADay = 24 * 60 * 60 * 1000;
        
        let to;
        if (this.toDate) {
        const nextToDate = new Date(this.toDate.getTime() + milliSecsADay); // Always add a day to this.toDate
            // to = nextToDate ? nextToDate.getMonth() + 1 + "/" + nextToDate.getDate() + "/" + nextToDate.getFullYear() : '';
            to = this.toDate ? this.toDate.getFullYear() + "-" + ( this.toDate.getMonth() + 1) + "-" + this.toDate.getDate() : '';
        }
        
        // Desired Format: vendor_QC_DateTime||greaterThanOrEqual 12/27/2017^lessThan 12/28/2017||complexDateTime
        if (this.fromDate && this.toDate) {
            queryString += this.field + '||greaterThanOrEqual ' + from + '^lessThan ' + to + '||complexDateTime`';
            this.createComplexDateTimeObject('fromdateandtodate', this.field, from, to);
        } else if (this.fromDate && !this.toDate) {
            queryString += this.field + '||greaterThanOrEqual ' + from + '||complexDateTime`';
            this.createComplexDateTimeObject('fromdateonly',this.field, from);
        } else if (!this.fromDate && this.toDate) {
            queryString += this.field + '||lessThan ' + to + '||complexDateTime`';
            this.createComplexDateTimeObject('todateonly', this.field, null, to);
        }
        return queryString.slice(0, -1); // Remove last `
    }

    cancel(event) {
        this.hideOverlay.emit();
    }

    createComplexDateTimeObject(type: string, field: string, from?:string, to?:string){
        let complexDateTime: ComplexDateTimeObject = new ComplexDateTimeObject();
        complexDateTime.columnName = this.field;

        if(type.toLowerCase() === 'yesterday'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMEYESTERDAY,
                displayValue: 'LocalDateTimeYesterday',
                type: 'dropdownDateTime'
            };
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETODAY,
                displayValue: 'LocalDateTimeToday',
                type: 'dropdownDateTime'
            }
            complexDateTime.alias="Is Yesterday";
        }
        else if(type.toLowerCase() === 'today'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETODAY,
                displayValue:'LocalDateTimeToday',
                type: 'dropdownDateTime'
            };
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETOMORROW,
                displayValue:'LocalDateTimeTomorrow',
                type: 'dropdownDateTime'
            }
            complexDateTime.alias="Is Today";
        }
        else if(type.toLowerCase() === 'tomorrow'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETOMORROW,
                displayValue: 'LocalDateTimeTomorrow',
                type: 'dropdownDateTime'
            };
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMEDAYAFTERTOMORROW,
                displayValue: 'LocalDateTimeDayAfterTomorrow',
                type: 'dropdownDateTime'
            }
            complexDateTime.alias="Is Tomorrow";
        }
        else if(type.toLowerCase() === 'last week'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMELASTWEEK,
                displayValue: 'LocalDateTimeLastWeek',
                type: 'dropdownDateTime'
            };
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETHISWEEK,
                displayValue: 'LocalDateTimeThisWeek',
                type: 'dropdownDateTime'
            }
            complexDateTime.alias="Is Last Week";
        }
        else if(type.toLowerCase() === 'this week'){
            
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETHISWEEK,
                displayValue: 'LocalDateTimeThisWeek',
                type: 'dropdownDateTime'
            };
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMENEXTWEEK,
                displayValue: 'LocalDateTimeNextWeek',
                type: 'dropdownDateTime'
            }
            complexDateTime.alias="Is This Week";
        }
        else if(type.toLowerCase() === 'next week'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMENEXTWEEK,
                displayValue: 'LocalDateTimeNextWeek',
                type: 'dropdownDateTime'
            };
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETWOWEEKSAWAY,
                displayValue: 'LocalDateTimeTwoWeeksAway',
                type: 'dropdownDateTime'
            }
            complexDateTime.alias="Is Next Week";
        }
        else if(type.toLowerCase() === 'next year'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMENEXTYEAR,
                displayValue: 'LocalDateTimeNextYear',
                type: 'dropdownDateTime'
            },
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_NEXTYEAR,
                displayValue: 'AddYears(LocalDateTimeNextYear, 1)',
                type: 'calendar'
            }
            complexDateTime.alias="Is Next Year";
        }
        else if(type.toLowerCase() === 'this year'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETHISYEAR,
                displayValue: 'LocalDateTimeThisYear',
                type: 'dropdownDateTime'
            },
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMENEXTYEAR,
                displayValue: 'LocalDateTimeNextYear',
                type: 'dropdownDateTime'
            }
            complexDateTime.alias="Is This Year";
        }
        else if(type.toLowerCase() === 'last year'){
            complexDateTime.condition1={
                label: 'Is Greater than',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LASTYEAR,
                displayValue: 'AddYears(LocalDateTimeThisYear, -1)',
                type: 'calendar'
            },
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETHISYEAR,
                displayValue: 'LocalDateTimeThisYear',
                type: 'dropdownDateTime'
            }
            complexDateTime.alias="Is Last Year";
        }
        else if(type.toLowerCase() === 'next month'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATETHANOREQUAL_LOCALDATETIMENEXTMONTH,
                displayValue: 'LocalDateTimeNextMonth',
                type: 'dropdownDateTime'
            },
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_NEXTMONTH,
                displayValue: 'AddMonths(LocalDateTimeNextMonth, 1)',
                type: 'calendar'
            }
            complexDateTime.alias="Is Next Month";
        }
        else if(type.toLowerCase() === 'this month'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETHISMONTH,
                displayValue: 'LocalDateTimeThisMonth',
                type: 'dropdownDateTime'
            },
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMENEXTMONTH,
                displayValue: 'LocalDateTimeNextMonth',
                type: 'dropdownDateTime'
            }
            complexDateTime.alias="Is This Month";
        }
        else if(type.toLowerCase() === 'last month'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LASTMONTH,
                displayValue: 'AddMonths(LocalDateTimeThisMonth, -1)',
                type: 'calendar'
            },
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETHISMONTH,
                displayValue: 'LocalDateTimeThisMonth',
                type: 'dropdownDateTime'
            }
            complexDateTime.alias="Is Last Month";
        }
        else if(type.toLowerCase() === 'fromdateandtodate'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.FROMDATE_GREATERTHANOREQUAL,
                displayValue: from,
                type: 'calendar'
            },
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.TODATE_LESSTHAN,
                displayValue: to,
                type: 'calendar'
            }
            complexDateTime.alias="";
        }
        else if(type.toLowerCase() === 'fromdateonly'){
            complexDateTime.condition1={
                label: 'Is greater than or equal to',
                value: COMPLEXDATE_CONSTANT.FROMDATE_GREATERTHANOREQUAL,
                displayValue: from,
                type: 'calendar'
            }
            complexDateTime.alias="";
        }
        else if(type.toLowerCase() === 'todateonly'){
            complexDateTime.condition2={
                label: 'Is less than',
                value: COMPLEXDATE_CONSTANT.TODATE_LESSTHAN,
                displayValue: to,
                type: 'calendar'
            }
            complexDateTime.alias="";
        }

        complexDateTime.operand="And";
        this.complexDatetimeObjects.push(complexDateTime);
    }

    private updateUI() {
        this._list.forEach(complexDateTime => {
            // handle from + to
            if (complexDateTime.alias === "") {
                this.handleFromAndToCase(complexDateTime);
            }
        });
    }

    private handleFromAndToCase(complexDateTimeObject: ComplexDateTimeObject) {
        if (complexDateTimeObject.condition1 != null) {
           this.fromDate = new Date(complexDateTimeObject.condition1.displayValue);
        }

        if (complexDateTimeObject.condition2 != null) {
            this.toDate = new Date(complexDateTimeObject.condition2.displayValue);
        }
    }
}
