import { OverlayPanel } from 'primeng/primeng';
import {
    FilterRoot,
    FilterRow, LabelValue, COMPLEX_DATE_CONDITIONS, FILTER_OPERATORS
} from './../../../../models/component/filterBuilder/filterBuilder';
import { Component, OnInit, Input, ViewChild } from '@angular/core';

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'filter-builder',
    templateUrl: 'filter-builder.component.html'
})

export class FilterBuilderComponent implements OnInit {
    public complexDateConditions: Array<LabelValue> = [];
    public filterOperators: Array<any> = FILTER_OPERATORS;

    private currentFilterRoot: FilterRoot;
    private currentFilterRowIndex: number;
    public filterOperands: any[] = [];

    // tslint:disable-next-line:no-input-rename
    @Input('filters') filterRoot: FilterRoot;
    // tslint:disable-next-line:no-input-rename
    @Input('columns') columnArray: Array<any>;
    // tslint:disable-next-line:no-input-rename
    @Input('matchModeString') filterMatchModeString: Array<any>;
    // tslint:disable-next-line:no-input-rename
    @Input('matchModeNumber') filterMatchModeNumber: Array<any>;
    // tslint:disable-next-line:no-input-rename
    @Input('matchModeDate') filterMatchModeDate: Array<any>;
    // tslint:disable-next-line:no-input-rename
    @Input('matchModeBoolean') filterMatchModeBoolean: Array<any>;

    @ViewChild('opFilter1') filterOperatorOverlay: OverlayPanel;
    @ViewChild('opFilter2') filterColumnNameOverlay: OverlayPanel;
    @ViewChild('opFilter3') filterColumnOperandOverlay: OverlayPanel;



    constructor() { }

    ngOnInit() {

        this.populateComplexDateConditionsDropDown();

    }

    public stickyPosition(op, event, adjust, needToAdjust) {
        // if(needToAdjust) {
        //     const c = this.getVisibleColumns().length;
        //     if (c > 10) {
        //         adjust = (c / 2) *  30;
        //     }
        // }

        let top = event.y + event.offsetY - adjust;
        setTimeout(() => {
            op.container.style.position = "fixed";
            op.container.style.display = "block";
            op.container.style.top = `${top}px`;
        }, 1);

    }

    public showOperateMenu(op, event, i, operation) {
        if (String(operation.operand.label).includes('of')) {
            return;
        }

        this.stickyPosition(op, event, 0, false);

        op.toggle(event);
        this.onCurrentFilterOperationRowClick(i);
        this.setCurrentRoot(this.filterRoot);
        this.setFilterOperandsByType(operation);
    }

    private populateComplexDateConditionsDropDown() {
        this.complexDateConditions = COMPLEX_DATE_CONDITIONS
    }

    public addNewRow(filterRows: Array<FilterRow>) {
        const visibleColumns = this.getVisibleColumns()

        const filterOperationRow: FilterRow = new FilterRow();
        filterOperationRow.column = visibleColumns[0].label;
        filterOperationRow.key = visibleColumns[0].field;
        filterOperationRow.operand = { label: '', value: '' };
        filterOperationRow.operand.label = this.getFilterString(filterOperationRow.key)['label'];
        filterOperationRow.operand.value = this.getFilterString(filterOperationRow.key)['value'];
        filterOperationRow.value = '';

        const column = this.getColumnInformationByKey(visibleColumns[0].field);
        filterOperationRow.type = this.getColumnTypeByColumn(column);
        filterOperationRow.placeholder = this.getPlaceholderByColumnType(column);
        this.populateFilterOperandsByType(filterOperationRow.type);

        filterRows.push(filterOperationRow);
    }

    public setCurrentRoot(root: FilterRoot) {
        this.currentFilterRoot = root;
    }

    private onCurrentFilterOperationRowClick(rowIndex: number) {
        this.currentFilterRowIndex = rowIndex;
    }

    private changeFilterOperator(filterOperator: any) {
        const otherThanOperatorSelection: string = filterOperator['label'];
        if (otherThanOperatorSelection.toLowerCase().indexOf('add condition') > -1) {
            this.addNewRow(this.currentFilterRoot.filterRows)
        } else {
            this.currentFilterRoot.filterOperator = otherThanOperatorSelection;
        }

        this.filterOperatorOverlay.hide();
    }

    private setFilterOperandsByType(row: FilterRow) {
        this.populateFilterOperandsByType(row.type)
    }

    private changeFilterColumn(columnObject: any) {
        const index = this.currentFilterRowIndex;

        this.currentFilterRoot.filterRows[index].column = columnObject.label;
        this.currentFilterRoot.filterRows[index].key = columnObject.field;
        this.currentFilterRoot.filterRows[index].operand.label =
            this.getFilterString(this.currentFilterRoot.filterRows[index].key)['label'];
        this.currentFilterRoot.filterRows[index].operand.value =
            this.getFilterString(this.currentFilterRoot.filterRows[index].key)['value'];
        const column = this.getColumnInformationByKey(this.currentFilterRoot.filterRows[index].key);
        this.currentFilterRoot.filterRows[index].type = this.getColumnTypeByColumn(column);
        this.currentFilterRoot.filterRows[index].placeholder = this.getPlaceholderByColumnType(column);
        this.currentFilterRoot.filterRows[index].value = '';

        // this.currentFilterRoot.isDirty = true;


        this.populateFilterOperandsByType(this.currentFilterRoot.filterRows[index].type);
        this.filterColumnNameOverlay.hide();
    }


    private changeFilterColumnOperand(columnOperand: any) {
        const index = this.currentFilterRowIndex;
        this.currentFilterRoot.filterRows[index].reservedOperand = this.currentFilterRoot.filterRows[index].operand.value;
        this.currentFilterRoot.filterRows[index].operand.label = columnOperand.label;
        this.currentFilterRoot.filterRows[index].operand.value = columnOperand.value;
        if (this.isDateTypeOperand(columnOperand.label)) {
            this.currentFilterRoot.filterRows[index].useAliasing = columnOperand.isAlias;
        }
        // this.currentFilterRoot.filterRows[index].reservedOperand = columnOperand.value;
        this.filterColumnOperandOverlay.hide();
    }

    private removeFilterItem(filterRows: Array<FilterRow>, index) {
        filterRows.splice(index, 1);
    }

    public getVisibleColumns() {
        const visibleColumns = [];
        if (this.columnArray && this.columnArray.length > 0) {
            for (let i = 1; i < this.columnArray.length; i++) {
                if (this.columnArray[i].display) {
                    visibleColumns.push(this.columnArray[i]);
                }
            }
            visibleColumns.sort(function (a, b) { return (a.label > b.label) ? 1 : ((b.label > a.label) ? -1 : 0); });
            return visibleColumns;
        }
        return visibleColumns;
    }

    private getFilterString(key) {
        const operand = {};
        const filteredCol = this.columnArray.filter((col) => {
            return col['field'] === key;
        });
        if (filteredCol[0]['type'] === 'number'
            || filteredCol[0]['type'] === 'currency'
            || filteredCol[0]['type'] === 'boolean'
            || filteredCol[0]['type'] === 'date') {
            operand['label'] = 'Equals';
            operand['value'] = 'equals';
        } else {
            operand['label'] = 'Begins With';
            operand['value'] = 'startsWith';
        }
        return operand;
    }
    private getColumnInformationByKey(key: string) {
        return this.columnArray.filter((c) => {
            return c.field === key;
        })
    }

    private getColumnTypeByColumn(column: any[]) {
        let colType = '';
        if (column && column.length > 0) {
            colType = column[0]['type']
        }
        return colType;
    }

    private getPlaceholderByColumnType(column: any[]) {

        let placeholder = '<enter a value>';
        let colType = '';
        if (column && column.length > 0) {
            colType = column[0]['type']
        }
        if (colType.toLowerCase() === 'boolean') {
            placeholder = '?';
        }

        return placeholder;
    }

    private populateFilterOperandsByType(colType: string) {
        this.filterOperands = [];

        if (colType === 'string' || colType === 'notes' || colType === 'phone') {
            this.filterOperands = this.filterMatchModeString;
        } else if (colType === 'date' || colType === 'dropdownDateTime' || colType === 'calendar') {
            this.filterOperands = this.filterMatchModeDate;
        } else if (colType === 'boolean') {
            this.filterOperands = this.filterMatchModeBoolean;
        } else { // for number/currency/boolean
            this.filterOperands = this.filterMatchModeNumber;
        }
    }

    private isDateTypeOperand(label: string) {
        const matchModeDates = this.filterMatchModeDate.filter((item) => {
            return (item['label'] === label)
        });
        return (matchModeDates && matchModeDates.length > 0)
    }


    private validateInput(event, operationRow: FilterRow) {
        const valid: boolean = this.validateFilterQuery(event.target.value, operationRow.type);
        // if(!valid){
        //     event.target.value='';
        //     operationRow.value='';
        // }
        return valid;
    }

    /**
     * Validate value entered manually in header filters
     * @param  {string} value
     * @param  {string} type
     * @returns boolean
     */
    private validateFilterQuery(value: string, type: string): boolean {
        let regex;  // value contains only characters specified
        if (type === 'number') { // Allow numbers 0-9 only
            regex = /^[0-9]+$/;
            return regex.test(value);
        } else if (type === 'string' || type === 'notes') { // Do not allow ` ' [ | characters as they fail in SQL injection
            // regex = /[`'[|]/;
            regex = /[]/;
            return !regex.test(value);
        } else if (type === 'phone') { // Allow - () . , and numbers only
            regex = /[-().0-9,]+$/;
            return regex.test(value);
        } else if (type === 'currency' || type === 'percent') { // Allow numbers 0-9 and . only
            // regex = /(?:\+|\-|\$)?\d{1,}(?:\,?\d{3})*(?:\.\d+)?%?/; // Allow floating point numbers only
            regex = /^[0-9.]+$/;
            return regex.test(value);
        } else {
            return true;
        }
    }


}
