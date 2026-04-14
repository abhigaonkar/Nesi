import {
    NgModule, Component, ElementRef, AfterContentInit, AfterViewInit, AfterViewChecked, OnInit, OnDestroy, Input,
    ViewContainerRef, ViewChild, IterableDiffers,
    Output, EventEmitter, ContentChild, ContentChildren, Renderer2, QueryList, TemplateRef,
    ChangeDetectorRef, Inject, forwardRef, EmbeddedViewRef, NgZone, OnChanges, SimpleChanges, HostListener
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'
import { SharedModule } from '../common/shared';
import { Paginator, PaginatorModule } from '../../primeng-custom-library/paginator/paginator';
import { Column, Header, Footer, HeaderColumnGroup, FooterColumnGroup, PrimeTemplate } from 'primeng/primeng';
import { LazyLoadEvent } from 'primeng/primeng';
import { FilterMetadata } from 'primeng/primeng';
import { SortMeta } from 'primeng/primeng';
import { DomHandler } from 'primeng/primeng';
import { ObjectUtils } from '../utils/objectutils';
import { Subscription } from 'rxjs/Subscription';
import { BlockableUI } from 'primeng/primeng';
import { Injectable, Pipe, PipeTransform } from '@angular/core';
import { DialogModule } from 'primeng/primeng';
import { ButtonModule } from 'primeng/primeng';
import { DataService } from '../../../service/dataservice';
import { AddEditFormModule } from '../../../../shared/add-edit-form/add-edit-form.module';


@Pipe({
    name: 'sortgrid',
    pure: false
})

@Injectable()
export class SortGridPipe implements PipeTransform {
    transform(array: Array<any>, arg1: string, arg2: number, arg3: string): Array<any> {
        if (array) {
            array.sort((data1: any, data2: any) => {
                const value1 = data1[arg1];
                const value2 = data2[arg1];
                let result = null;

                if (value1 == null && value2 != null) {
                    result = -1;
                } else if (value1 != null && value2 == null) {
                    result = 1;
                } else if (value1 == null && value2 == null) {
                    result = 0;
                } else if (typeof value1 === 'string' && typeof value2 === 'string') {
                    result = value1.localeCompare(value2);
                } else {
                    result = (value1 < value2) ? -1 : (value1 > value2) ? 1 : 0;
                }
                if (result !== 0) {
                    if (arg2 === 1) {
                        return result;
                    } else {
                        return result * -1;
                    }
                } else {
                    result = (data1.id < data2.id) ? -1 : (data1.id > data2.id) ? 1 : 0;
                    return result;
                }
                // return (this.sortOrder * result);
            });
        }
        return array;
    }
}

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'p-dtRadioButton',
    template: `
        <div class="ui-radiobutton ui-widget">
            <div class="ui-helper-hidden-accessible">
                <input type="radio" [checked]="checked">
            </div>
            <div class="ui-radiobutton-box ui-widget ui-radiobutton-relative ui-state-default" (click)="handleClick($event)"
                        (mouseenter)="hover=true" (mouseleave)="hover=false"
                        [ngClass]="{'ui-state-hover':hover,'ui-state-active':checked}">
                <span class="ui-radiobutton-icon ui-clickable" [ngClass]="{'fa fa-circle':checked}"></span>
            </div>
        </div>
    `
})
// tslint:disable-next-line:component-class-suffix
export class DTRadioButton {

    @Input() checked: boolean;

    @Output() onClick: EventEmitter<any> = new EventEmitter();

    public hover: boolean;

    handleClick(event) {
        this.onClick.emit(event);
    }
}

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'p-dtCheckbox',
    template: `
        <div class="ui-chkbox ui-widget">
            <div class="ui-helper-hidden-accessible">
                <input type="checkbox" [checked]="checked">
            </div>
            <div class="ui-chkbox-box ui-widget ui-corner-all ui-state-default" (click)="handleClick($event)"
                        (mouseover)="hover=true" (mouseout)="hover=false"
                        [ngClass]="{'ui-state-hover':hover&&!disabled,'ui-state-active':checked&&!disabled,'ui-state-disabled':disabled}">
                <span class="ui-chkbox-icon ui-clickable" [ngClass]="{'fa fa-check':checked}"></span>
            </div>
        </div>
    `
})
// tslint:disable-next-line:component-class-suffix
export class DTCheckbox {

    @Input() checked: boolean;

    @Input() disabled: boolean;

    @Output() onChange: EventEmitter<any> = new EventEmitter();

    public hover: boolean;

    handleClick(event) {
        if (!this.disabled) {
            this.onChange.emit({ originalEvent: event, checked: !this.checked });
        }
    }
}

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'p-rowExpansionLoader',
    template: ``
})
// tslint:disable-next-line:component-class-suffix
export class RowExpansionLoader implements OnInit, OnDestroy {

    @Input() template: TemplateRef<any>;

    @Input() rowData: any;

    @Input() rowIndex: any;

    view: EmbeddedViewRef<any>;


    constructor(public viewContainer: ViewContainerRef) { }

    ngOnInit() {
        this.view = this.viewContainer.createEmbeddedView(this.template, {
            '\$implicit': this.rowData,
            'rowIndex': this.rowIndex
        });
    }

    ngOnDestroy() {
        this.view.destroy();
    }
}

@Component({
    // tslint:disable-next-line:component-selector
    selector: '[pColumnHeaders]',
    template: `
        <ng-template ngFor let-col [ngForOf]="columns" let-lastCol="last">
            <th #headerCell [attr.id]="col.colId"
             [ngStyle]="col.headerStyle || col.style" [class]="col.headerStyleClass || col.styleClass" [attr.colspan]="col.colspan"
             [attr.rowspan]="col.rowspan"
                [ngClass]="{'ui-state-default ui-unselectable-text':true, 'ui-sortable-column': col.sortable,
                'ui-state-active': dt.isSorted(col), 'ui-resizable-column': dt.resizableColumns, 'ui-selection-column':col.selectionMode,
                            'ui-helper-hidden': col.hidden}"
                (dragstart)="dt.onColumnDragStart($event)"
                (dragleave)="dt.onColumnDragleave($event)"
                (drop)="dt.onColumnDrop($event)" (mousedown)="dt.onHeaderMousedown($event,headerCell)"
                [attr.tabindex]="col.sortable ? tabindex : null" (keydown)="dt.onHeaderKeydown($event,col)"
                [attr.scope]="col.scope||(col.colspan ? 'colgroup' : 'col')">
                <span class="ui-column-resizer ui-clickable"
                *ngIf="dt.resizableColumns && col.resizable
                && ((dt.columnResizeMode == 'fit' && !lastCol) || dt.columnResizeMode == 'expand')"
                (mousedown)="dt.initColumnResize($event)"></span>
                <span class="ui-column-title" *ngIf="!col.selectionMode&&!col.headerTemplate">{{col.header}}</span>
                <span class="ui-column-title" *ngIf="col.headerTemplate">
                    <p-columnHeaderTemplateLoader [column]="col"></p-columnHeaderTemplateLoader>
                </span>
                <span class="ui-sortable-column-icon fa fa-fw fa-sort" (click)="dt.sort($event,col)" *ngIf="col.sortable"
                     [ngClass]="{'fa-sort-down': (dt.getSortOrder(col) == -1),'fa-sort-up': (dt.getSortOrder(col) == 1)}"></span>
                <input [attr.type]="col.filterType"
                class="ui-column-filter ui-inputtext ui-widget ui-state-default ui-corner-all" [attr.maxlength]="col.filterMaxlength"
                [attr.placeholder]="col.filterPlaceholder"
                 *ngIf="col.filter&&!col.filterTemplate"
                  [value]="dt.filters[col.filterField||col.field] ? dt.filters[col.filterField||col.field].value : ''"
                    (click)="dt.onFilterInputClick($event)"
                    (input)="dt.onFilterKeyup($event, col.filterField||col.field, col.filterMatchMode, null)"/>
                <p-columnFilterTemplateLoader [column]="col" *ngIf="col.filterTemplate"></p-columnFilterTemplateLoader>
                <p-dtCheckbox *ngIf="col.selectionMode=='multiple'"
                (onChange)="dt.toggleRowsWithCheckbox($event)"
                 [checked]="dt.allSelected" [disabled]="dt.isEmpty()"></p-dtCheckbox>
            </th>
        </ng-template>
    `
})
// tslint:disable-next-line:component-class-suffix
export class ColumnHeaders {

    // tslint:disable-next-line:no-input-rename
    @Input('pColumnHeaders') columns: Column[];
    public tabindex = 0;

    constructor(@Inject(forwardRef(() => DataTable)) public dt: DataTable) { }

    // tslint:disable-next-line:member-ordering
}

@Component({
    // tslint:disable-next-line:component-selector
    selector: '[pColumnFooters]',
    template: `
        <td *ngFor="let col of columns" [ngStyle]="col.footerStyle||col.style" [class]="col.footerStyleClass||col.styleClass"
            [attr.colspan]="col.colspan" [attr.rowspan]="col.rowspan"
            [ngClass]="{'ui-state-default':true, 'ui-helper-hidden': col.hidden}">
            <span class="ui-column-footer" *ngIf="!col.footerTemplate">{{col.footer}}</span>
            <span class="ui-column-footer" *ngIf="col.footerTemplate">
                <p-columnFooterTemplateLoader [column]="col"></p-columnFooterTemplateLoader>
            </span>
        </td>
    `
})
// tslint:disable-next-line:component-class-suffix
export class ColumnFooters {

    // tslint:disable-next-line:no-input-rename
    @Input('pColumnFooters') columns: Column[];
    constructor(@Inject(forwardRef(() => DataTable)) public dt: DataTable) { }

}

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'nestedGroup',
    template: `
        <div *ngIf = "parentGroup">
            <div *ngFor="let subGroup of parentGroup.subGroups | sortgrid: 'name':findGroupSortOrder(index+1)">
                <table class="nested-table">
                    <tbody>
                        <tr>
                            <td class="datatable-tbl-border-1" [style.paddingLeft.px]="(index+1)*1">
                                <span>
                                <a (click)="dt.toggleFunction(subGroup);">
                                    <span class="fa fa-fw" [ngClass]="subGroup.expand ? dt.expandedIcon : dt.collapsedIcon"></span>
                                </a>
                                  <span>{{dt.getHeader(dt.groupFieldArray[index+1])}}: </span>
                                  <span class="ui-rowgroup-header-name">{{subGroup.name}}</span>
                                  <span> {{dt.getSummary(subGroup.leaves[0],dt.groupFieldArray[index+1], false)}}</span>
                                  </span>
                                <div *ngIf="(subGroup.level <  dt.groupFieldArray.length-1) && subGroup.expand">
                                    <nestedGroup [tableKeyField]="tableKeyField"
                                     [groupSummary]="groupSummary"
                                     (edit)="editRow($event)"
                                     (delete)="deleteRow($event)"
                                     [columns]="columns" [parentGroup]="subGroup"
                                      [index]="index + 1"></nestedGroup>
                                </div>
                                <div
                                 *ngIf="(subGroup.level ==  dt.groupFieldArray.length-1) && subGroup.expand">
                                    <table>
                                        <tbody>
                                            <ng-container *ngFor="let leave of subGroup.leaves  let i=index">
                                            <tr #rowElement class="datatable-tbl-border-2"
                                            [ngClass]="{
                                                'ui-datatable-even': i %2==0,
                                                'ui-datatable-odd': i%2==1
                                            }"
                                            >
                                                <table class="nested-table-data" style="margin-left:-4px">
                                                        <tr
                                                        >
                                                        <ng-container *ngFor="let col of columns">
                                                            <td #cell *ngIf="col.field"
                                                                (click)="dt.switchCellToEditMode(cell,col,leave)" rowspan=1
                                                                [class]="col.bodyStyleClass||col.styleClass"
                                                                [ngClass]="{'ui-editable-column':col.editable,
                                                                'ui-selection-column':col.selectionMode,
                                                                'ui-helper-hidden': col.hidden
                                                            }">
                                                              <span class="ui-column-title" *ngIf="dt.responsive">{{col.header}}</span>
                                                              <span class="ui-cell-data"
                                                              *ngIf="!col.bodyTemplate">{{leave[col.field]}}</span>
                                                                    <span class="ui-cell-data" *ngIf="col.bodyTemplate">
                                                                    <p-columnBodyTemplateLoader
                                                                    [column]="col" [rowData]="leave">
                                                                    </p-columnBodyTemplateLoader>
                                                                </span>
                                                            </td>
                                                        </ng-container>
                                                        </tr>
                                                </table>
                                            </tr>
                                            </ng-container>
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

        </div>
        <p-dialog header="Edit Form"
        [styleClass]="'add--new-customer'"
         [(visible)]="displayEditDialog" [responsive]="true" showEffect="fade"
        [modal]="true">
        <app-add-edit-form
        *ngIf="displayEditDialog"
        [rowData]="leaveData"
        [columnArray]="dt.schema"
        [genericApiObject]="dt.genericApiObject"
        actionType="edit"
        (emitRecord)="editRecord($event)"
        (closeDialog)="displayEditDialog=false;"></app-add-edit-form>
        </p-dialog>
        <p-dialog header="Are you sure?" [(visible)]="confirmDeleteDialog" [responsive]="true" showEffect="fade"
        [modal]="true">
            <div class="ui-grid ui-grid-responsive ui-fluid">
            <div class="ui-grid-row">
            Are you sure you want to delete this asset?  This history will also be deleted..
            </div>
            </div>
        <p-footer>
            <div class="ui-dialog-buttonpane ui-helper-clearfix">
            <button type="button" class="btn btn-danger" pButton icon="fa-check" label="Delete"
            (click)="deleteRecord(subGroupData,leaveData,id);confirmDeleteDialog=false"></button>
            <button type="button" pButton icon="fa-times-circle" label="Cancel" (click)="confirmDeleteDialog=false"></button>
            </div>
        </p-footer>
        </p-dialog>
    `
})
// tslint:disable-next-line:component-class-suffix
export class NestedGroup {

    @Input() parentGroup;
    @Input() index;
    @Input() columns;
    @Input() groupSummary: any[];
    @Output() delete: EventEmitter<any> = new EventEmitter();
    @Output() edit: EventEmitter<any> = new EventEmitter();
    @Input() tableKeyField;
    @Input() groupSummaryTable: any[];
    objectKeys = Object.keys;
    public displayEditDialog = false;
    public leaveData: Object;
    public subGroupData: Object;
    public id: any;
    private customer_name: any;
    confirmDeleteDialog = false;
    showSummary = false;
    constructor(private dataService: DataService, @Inject(forwardRef(() => DataTable)) public dt: DataTable) { };

    getFieldType(value) {
        if (value.indexOf('String') !== -1) {
            return 'string';
        } else if (value.indexOf('Boolean') !== -1) {
            return 'boolean';
        } else if (value.indexOf('DateTime') !== -1) {
            return 'date';
        } else {
            return 'number';
        }
    }
    convertStringToLowerCase(str) {
        return str ? str.toString().toLowerCase() : str;
    }

    changeValue(value, itemKey) {
        this.leaveData[itemKey] = !value;
    }
    // tslint:disable-next-line:use-life-cycle-interface
    ngOnInit() {
        // Hide summary brackets in grouping when no summary is provided
        this.showSummary = this.groupSummary.length > 0;
    }


    openEditDialog(subGroup, leave) {
        this.leaveData = Object.assign({}, leave);
        this.subGroupData = subGroup;
        this.displayEditDialog = true;
    }

    onChange(event, itemKey) {
        this.leaveData[itemKey.lookup.values[0].Key] = event.target.options[event.target.selectedIndex].innerText;
        this.leaveData[itemKey.name] = event.target.value;
    }

    editRecord(event) {
        for (let i = 0; i < this.subGroupData['leaves'].length; i++) {
            if (this.subGroupData['leaves'][i][this.tableKeyField] === event.record[this.tableKeyField]) {
                for (const key in this.subGroupData['leaves'][i]) {
                    if (this.subGroupData['leaves'][i][key] !== event.record[key]) {
                        this.subGroupData['leaves'][i][key] = event.record[key];
                    }
                }
                break;
            }
        }
        this.edit.emit({
            pGroup: this.parentGroup,
            leave: event.record
        });
    }

    editRow(event) {
        for (let i = 0; i < event.pGroup.leaves.length; i++) {
            if (event.pGroup.leaves[i][this.tableKeyField] === event.leave[this.tableKeyField]) {
                for (const key in event.pGroup.leaves[i]) {
                    if (event.pGroup.leaves[i][key] !== event.leave[key]) {
                        event.pGroup.leaves[i][key] = event.leave[key];
                    }
                }
                break;
            }
        }
        this.edit.emit({
            pGroup: this.parentGroup,
            leave: event.leave
        });
    }

    openDeleteDialog(subGroup, leave, id) {
        this.confirmDeleteDialog = true;
        this.subGroupData = subGroup;
        this.leaveData = leave;
        this.id = id;
    }

    deleteRecord(subGroup, leave, id) {
        for (let i = 0; i < subGroup.leaves.length; i++) {
            if (subGroup.leaves[i][this.tableKeyField] === id) {
                subGroup.leaves = subGroup.leaves.filter((val, x) => x !== i);
                break;
            }
        }

        this.delete.emit({
            leave: leave,
            pGroup: this.parentGroup,
            id: id
        });
    }

    deleteRow(event) {
        for (let i = 0; i < event.pGroup.leaves.length; i++) {
            if (event.pGroup.leaves[i][this.tableKeyField] === event.id) {
                event.pGroup.leaves = event.pGroup.leaves.filter((val, x) => x !== i);
            }
        }

        this.delete.emit({
            leave: event.leave,
            pGroup: this.parentGroup,
            id: event.id
        })
    }

    findGroupSortOrder(level) {
        const column = this.dt.groupFieldArray[level];
        let sortOrder: number;
        for (let i = 0; i < this.dt.multiSortMeta.length; i++) {
            if (this.dt.multiSortMeta[i].field === column) {
                sortOrder = this.dt.multiSortMeta[i].order;
                break;
            }
        }
        return sortOrder;
    }

}

@Component({
    // tslint:disable-next-line:component-selector
    selector: '[pTableBody]',
    template: `
        <ng-template ngFor let-rowData [ngForOf]="data" let-even="even" let-odd="odd" let-rowIndex="index" [ngForTrackBy]="dt.rowTrackBy">
            <tr #rowGroupElement class="ui-widget-header ui-rowgroup-header"
                *ngIf="dt.rowGroupMode=='subheader'
                 && (rowIndex === 0||(dt.resolveFieldData(rowData,dt.groupFieldArray[0])
                 != dt.resolveFieldData(dt.dataToRender[rowIndex - 1],
                     dt.groupFieldArray[0])))"
                (click)="dt.onRowGroupClick($event)" [ngStyle]="{'cursor': dt.sortableRowGroup ? 'pointer' : 'auto'}">
                <td [attr.colspan]="dt.visibleColumns().length">
                    <a href="#" *ngIf="dt.expandableRowGroups"
                    (click)="dt.toggleRowGroup($event,rowData,0,dt.resolveFieldData(rowData,dt.groupFieldArray[0]))">
                        <span class="fa fa-fw" [ngClass]="dt.isRowGroupExpanded(rowData) ? dt.expandedIcon : dt.collapsedIcon"></span>
                    </a>
                    <span class="ui-rowgroup-header-name">
                        {{dt.getHeader(dt.groupFieldArray[0])}}:
                         <p-templateLoader [template]="dt.rowGroupHeaderTemplate" [data]="rowData"></p-templateLoader>
                    </span>
                    <span *ngIf="false">{{dt.getSummary(rowData,dt.groupFieldArray[0], true)}}</span>
                    <nestedGroup [tableKeyField]="dt.tableKeyField"
                     [groupSummary]="dt.groupSummary"
                     (edit)="dt.editLeave($event)" (delete)="dt.deleteLeave($event)"
                      [columns]="dt.columns" *ngIf="dt.expandableRowGroups && dt.isRowGroupExpanded(rowData)
                       && dt.groupFieldArray.length > 1" [parentGroup]="dt.getTopGroup(dt.selectedNode)"
                        [index]=0></nestedGroup>
                </td>
            </tr>
            <tr #rowElement
            *ngIf="(!dt.expandableRowGroups || dt.isRowGroupExpanded(rowData))
            && (dt.groupFieldArray ? dt.groupFieldArray.length === 1 : true)"
                (click)="dt.handleRowClick($event, rowData, rowIndex)"
                 (dblclick)="dt.rowDblclick($event,rowData)"
                 (contextmenu)="dt.onRowRightClick($event,rowData)"
                 (touchend)="dt.handleRowTouchEnd($event)"
                [ngClass]="[even&&dt.rowGroupMode!='rowspan'? 'ui-datatable-even':'',
                            odd&&dt.rowGroupMode!='rowspan'?'ui-datatable-odd':'',
                            dt.isSelected(rowData)? 'ui-state-highlight': '',
                            dt.getRowStyleClass(rowData,rowIndex)]">
                <ng-template ngFor let-col [ngForOf]="columns" let-colIndex="index">
                    <td #cell *ngIf="!dt.rowGroupMode || (dt.rowGroupMode == 'subheader') ||
                        (dt.rowGroupMode=='rowspan' &&
                        ((dt.sortField==col.field && dt.rowGroupMetadata[dt.resolveFieldData(rowData,dt.sortField)].index == rowIndex)
                         || (dt.sortField!=col.field)))"
                        [ngStyle]="col.bodyStyle||col.Style"
                        [class]="dt.getColumnStyleClass(rowData,rowIndex, col.field)
                         || col.bodyStyleClass||col.styleClass" (click)="dt.switchCellToEditMode(cell,col,rowData)"
                        [ngClass]="{'ui-editable-column':col.editable,
                        'ui-selection-column':col.selectionMode, 'ui-helper-hidden': col.hidden}"
                        [attr.rowspan]="(dt.rowGroupMode=='rowspan'
                        && dt.sortField == col.field && dt.rowGroupMetadata[dt.resolveFieldData(rowData,dt.sortField)].index == rowIndex)
                         ? dt.rowGroupMetadata[dt.resolveFieldData(rowData,dt.sortField)].size : null">
                        <span class="ui-column-title" *ngIf="dt.responsive">{{col.header}}</span>
                        <span class="ui-cell-data ui-resizable-cell-data"
                         *ngIf="!col.bodyTemplate && !col.expander && !col.selectionMode">{{dt.resolveFieldData(rowData,col.field)}}</span>
                        <span class="ui-cell-data ui-resizable-cell-data" *ngIf="col.bodyTemplate">
                            <p-columnBodyTemplateLoader [column]="col"
                            [rowData]="rowData" [rowIndex]="rowIndex + dt.first"></p-columnBodyTemplateLoader>
                        </span>
                        <div class="ui-cell-editor" *ngIf="col.editable">
                            <input *ngIf="!col.editorTemplate" type="text" [(ngModel)]="rowData[col.field]" required="true"
                                (keydown)="dt.onCellEditorKeydown($event, col, rowData, rowIndex)"
                                 class="ui-inputtext ui-widget ui-state-default ui-corner-all"/>
                            <a *ngIf="col.editorTemplate" class="ui-cell-editor-proxy-focus" href="#"
                            (focus)="dt.onCustomEditorFocusPrev($event, colIndex)"></a>
                            <p-columnEditorTemplateLoader *ngIf="col.editorTemplate" [column]="col"
                            [rowData]="rowData" [rowIndex]="rowIndex"></p-columnEditorTemplateLoader>
                            <a *ngIf="col.editorTemplate" class="ui-cell-editor-proxy-focus" href="#"
                            (focus)="dt.onCustomEditorFocusNext($event, colIndex)"></a>
                        </div>
                        <a href="#" *ngIf="col.expander" (click)="dt.toggleRow(rowData,$event)">
                            <span class="ui-row-toggler fa fa-fw ui-clickable"
                            [ngClass]="dt.isRowExpanded(rowData) ? dt.expandedIcon : dt.collapsedIcon"></span>
                        </a>
                        <p-dtRadioButton *ngIf="col.selectionMode=='single'"
                        (onClick)="dt.selectRowWithRadio($event, rowData)" [checked]="dt.isSelected(rowData)"></p-dtRadioButton>
                        <p-dtCheckbox *ngIf="col.selectionMode=='multiple'"
                        (onChange)="dt.toggleRowWithCheckbox($event,rowData)"
                         [checked]="dt.isSelected(rowData)"></p-dtCheckbox>
                    </td>
                </ng-template>
            </tr>
            <tr class="ui-widget-header"
             *ngIf="(dt.groupFieldArray ? dt.groupFieldArray.length == 1 : true)
             && dt.rowGroupFooterTemplate && dt.rowGroupMode=='subheader'
             && ((rowIndex === dt.dataToRender.length - 1)
             ||(dt.resolveFieldData(rowData,dt.groupFieldArray[0])
             !== dt.resolveFieldData(dt.dataToRender[rowIndex + 1],dt.groupFieldArray[0])))
             && (!dt.expandableRowGroups || dt.isRowGroupExpanded(rowData))">
                <p-templateLoader class="ui-helper-hidden" [data]="rowData" [template]="dt.rowGroupFooterTemplate"></p-templateLoader>
            </tr>
            <tr *ngIf="dt.expandableRows && dt.isRowExpanded(rowData)">
                <td [attr.colspan]="dt.visibleColumns().length">
                    <p-rowExpansionLoader [rowData]="rowData"
                     [rowIndex]="rowIndex" [template]="dt.rowExpansionTemplate">
                     </p-rowExpansionLoader>
                </td>
            </tr>
        </ng-template>
        <tr *ngIf="dt.isEmpty()" class="ui-widget-content ui-datatable-emptymessage-row">
            <td [attr.colspan]="dt.visibleColumns().length" class="ui-datatable-emptymessage">{{dt.emptyMessage}}</td>
        </tr>
    `
})
// tslint:disable-next-line:component-class-suffix
export class TableBody {

    // tslint:disable-next-line:no-input-rename
    @Input('pTableBody') columns: Column[];


    // tslint:disable-next-line:member-ordering
    @Input() data: any[];

    // tslint:disable-next-line:member-ordering
    public showSummary = false;

    constructor(@Inject(forwardRef(() => DataTable)) public dt: DataTable) { }

    visibleColumns() {
        return this.columns ? this.columns.filter(c => !c.hidden) : [];
    }

    // tslint:disable-next-line:use-life-cycle-interface
    ngOnInit() {
        // Hide summary brackets in grouping when no summary is provided
        if (this.dt.groupSummary) {
            this.showSummary = this.dt.groupSummary.length > 0;
        }
    }

}

@Component({
    // tslint:disable-next-line:component-selector
    selector: '[pScrollableView]',
    template: `
        <div #scrollHeader class="ui-widget-header ui-datatable-scrollable-header" [ngStyle]="{'width': width}">
            <div #scrollHeaderBox  class="ui-datatable-scrollable-header-box">
                <table [ngClass]="dt.tableStyleClass" [ngStyle]="dt.tableStyle">
                    <thead class="ui-datatable-thead">
                        <tr *ngIf="!dt.headerColumnGroup" class="ui-state-default" [pColumnHeaders]="columns"></tr>
                        <ng-template [ngIf]="dt.headerColumnGroup">
                            <tr *ngFor="let headerRow of dt.headerColumnGroup.rows"
                             class="ui-state-default" [pColumnHeaders]="headerRow.columns"></tr>
                        </ng-template>
                    </thead>
                    <tbody *ngIf="dt.frozenValue"
                     [ngClass]="{'ui-datatable-data ui-widget-content': true,
                     'ui-datatable-hoverable-rows': (dt.rowHover||dt.selectionMode)}"
                      [pTableBody]="columns" [data]="dt.frozenValue">
                      </tbody>
                </table>
            </div>
        </div>
        <div #scrollBody class="ui-datatable-scrollable-body" [ngStyle]="{'width': width,'max-height':dt.scrollHeight}">
            <div #scrollTableWrapper class="ui-datatable-scrollable-table-wrapper" style="position:relative">
                <table #scrollTable [class]="dt.tableStyleClass"
                [ngStyle]="dt.tableStyle" [ngClass]="{'ui-datatable-virtual-table':virtualScroll}" style="top:0px">
                    <colgroup class="ui-datatable-scrollable-colgroup">
                        <col *ngFor="let col of columns"
                        [ngStyle]="col.headerStyle||col.style" [ngClass]="{'ui-helper-hidden': col.hidden}"/>
                    </colgroup>
                    <tbody [ngClass]="{'ui-datatable-data ui-widget-content': true,
                    'ui-datatable-hoverable-rows': (dt.rowHover||dt.selectionMode)}"
                    [pTableBody]="columns" [data]="dt.dataToRender"></tbody>
                </table>
            </div>
        </div>
        <div #scrollFooter class="ui-widget-header ui-datatable-scrollable-footer" [ngStyle]="{'width': width}" *ngIf="dt.hasFooter()">
            <div #scrollFooterBox  class="ui-datatable-scrollable-footer-box">
                <table [ngClass]="dt.tableStyleClass" [ngStyle]="dt.tableStyle">
                    <tfoot class="ui-datatable-tfoot">
                        <tr *ngIf="!dt.footerColumnGroup" [pColumnFooters]="columns" class="ui-state-default"></tr>
                        <ng-template [ngIf]="dt.footerColumnGroup">
                            <tr
                            *ngFor="let footerRow of dt.footerColumnGroup.rows"
                            class="ui-state-default" [pColumnFooters]="footerRow.columns"></tr>
                        </ng-template>
                    </tfoot>
                </table>
            </div>
        </div>
    `
})
// tslint:disable-next-line:component-class-suffix
export class ScrollableView implements AfterViewInit, AfterViewChecked, OnDestroy {


    // tslint:disable-next-line:member-ordering
    // tslint:disable-next-line:no-input-rename
    @Input('pScrollableView') columns: Column[];

    @ViewChild('scrollHeader') scrollHeaderViewChild: ElementRef;

    @ViewChild('scrollHeaderBox') scrollHeaderBoxViewChild: ElementRef;

    @ViewChild('scrollBody') scrollBodyViewChild: ElementRef;

    @ViewChild('scrollTable') scrollTableViewChild: ElementRef;

    @ViewChild('scrollTableWrapper') scrollTableWrapperViewChild: ElementRef;

    @ViewChild('scrollFooter') scrollFooterViewChild: ElementRef;

    @ViewChild('scrollFooterBox') scrollFooterBoxViewChild: ElementRef;

    @Input() frozen: boolean;

    @Input() width: string;

    @Input() virtualScroll: boolean;

    @Output() onVirtualScroll: EventEmitter<any> = new EventEmitter();

    public scrollBody: HTMLDivElement;

    public scrollHeader: HTMLDivElement;

    public scrollHeaderBox: HTMLDivElement;

    public scrollTable: HTMLDivElement;

    public scrollTableWrapper: HTMLDivElement;

    public scrollFooter: HTMLDivElement;

    public scrollFooterBox: HTMLDivElement;

    public bodyScrollListener: Function;

    public headerScrollListener: Function;

    public scrollBodyMouseWheelListener: Function;

    public scrollFunction: Function;

    public rowHeight: number;

    public scrollTimeout: any;

    // tslint:disable-next-line:max-line-length
    constructor(@Inject(forwardRef(() => DataTable)) public dt: DataTable, public domHandler: DomHandler, public el: ElementRef, public renderer: Renderer2, public zone: NgZone) { }


    ngAfterViewInit() {
        this.initScrolling();
    }

    ngAfterViewChecked() {
        if (this.virtualScroll && !this.rowHeight) {
            const row = this.domHandler.findSingle(this.scrollTable, 'tr.ui-widget-content:not(.ui-datatable-emptymessage-row)');
            if (row) {
                this.rowHeight = this.domHandler.getOuterHeight(row);
            }
        }

        if (!this.frozen) {
            this.zone.runOutsideAngular(() => {
                setTimeout(() => {
                    this.alignScrollBar();
                }, 1);
            });
        }
    }

    initScrolling() {
        this.scrollHeader = <HTMLDivElement>this.scrollHeaderViewChild.nativeElement;
        this.scrollHeaderBox = <HTMLDivElement>this.scrollHeaderBoxViewChild.nativeElement;
        this.scrollBody = <HTMLDivElement>this.scrollBodyViewChild.nativeElement;
        this.scrollTable = <HTMLDivElement>this.scrollTableViewChild.nativeElement;
        this.scrollTableWrapper = <HTMLDivElement>this.scrollTableWrapperViewChild.nativeElement;
        this.scrollFooter = this.scrollFooterViewChild ? <HTMLDivElement>this.scrollFooterViewChild.nativeElement : null;
        this.scrollFooterBox = this.scrollFooterBoxViewChild ? <HTMLDivElement>this.scrollFooterBoxViewChild.nativeElement : null;

        if (!this.frozen) {
            this.zone.runOutsideAngular(() => {
                this.scrollHeader.addEventListener('scroll', this.onHeaderScroll.bind(this));
                this.scrollBody.addEventListener('scroll', this.onBodyScroll.bind(this));
            });
        }

        if (!this.frozen) {
            this.alignScrollBar();
        } else {
            this.scrollBody.style.paddingBottom = this.domHandler.calculateScrollbarWidth() + 'px';
        }
    }

    onBodyScroll(event) {
        const frozenView = this.el.nativeElement.previousElementSibling;
        let frozenScrollBody;
        if (frozenView) {
            // tslint:disable-next-line:prefer-const
            frozenScrollBody = this.domHandler.findSingle(frozenView, '.ui-datatable-scrollable-body');
        }

        this.scrollHeaderBox.style.marginLeft = -1 * this.scrollBody.scrollLeft + 'px';
        if (this.scrollFooterBox) {
            this.scrollFooterBox.style.marginLeft = -1 * this.scrollBody.scrollLeft + 'px';
        }

        if (frozenScrollBody) {
            frozenScrollBody.scrollTop = this.scrollBody.scrollTop;
        }

        if (this.virtualScroll) {
            const viewport = this.domHandler.getOuterHeight(this.scrollBody);
            const tableHeight = this.domHandler.getOuterHeight(this.scrollTable);
            const pageHeight = this.rowHeight * this.dt.rows;
            const virtualTableHeight = this.domHandler.getOuterHeight(this.scrollTableWrapper);
            const pageCount = (virtualTableHeight / pageHeight) || 1;

            if (this.scrollBody.scrollTop + viewport > parseFloat(this.scrollTable.style.top)
                + tableHeight || this.scrollBody.scrollTop < parseFloat(this.scrollTable.style.top)) {
                const page = Math.floor((this.scrollBody.scrollTop * pageCount) / (this.scrollBody.scrollHeight)) + 1;
                this.onVirtualScroll.emit({
                    page: page
                });
                this.scrollTable.style.top = ((page - 1) * pageHeight) + 'px';
            }
        }
    }

    onHeaderScroll(event) {
        this.scrollHeader.scrollLeft = 0;
    }

    hasVerticalOverflow() {
        return this.domHandler.getOuterHeight(this.scrollTable) > this.domHandler.getOuterHeight(this.scrollBody);
    }

    alignScrollBar() {
        const scrollBarWidth = this.hasVerticalOverflow() ? this.domHandler.calculateScrollbarWidth() : 0;
        this.scrollHeaderBox.style.marginRight = scrollBarWidth + 'px';
        if (this.scrollFooterBox) {
            this.scrollFooterBox.style.marginRight = scrollBarWidth + 'px';
        }
    }

    ngOnDestroy() {
        this.scrollHeader.removeEventListener('scroll', this.onHeaderScroll);
        this.scrollBody.removeEventListener('scroll', this.onBodyScroll);
    }
}

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'p-dataTable',
    template: `
        <div [ngStyle]="style" [class]="styleClass" [style.width]="containerWidth"
            [ngClass]="{'ui-datatable ui-widget':true,
            'ui-datatable-reflow':responsive,
            'ui-datatable-stacked':stacked,
            'ui-datatable-resizable':resizableColumns,
            'ui-datatable-scrollable':scrollable}">
            <div class="ui-datatable-loading ui-widget-overlay" *ngIf="loading"></div>
            <div class="ui-datatable-loading-content" *ngIf="loading">
                <i [class]="'fa fa-spin fa-2x ' + loadingIcon"></i>
            </div>
            <div class="ui-datatable-header ui-widget-header" *ngIf="header">
                <ng-content select="p-header"></ng-content>
            </div>
            <p-paginator [rows]="rows" [first]="first" [totalRecords]="totalRecords"
            [pageLinkSize]="pageLinks" styleClass="ui-paginator-top" [alwaysShow]="alwaysShowPaginator"
                (onPageChange)="onPageChange($event)"
                [rowsPerPageOptions]="rowsPerPageOptions"
                 *ngIf="paginator && paginatorPosition =='top' || paginatorPosition =='both'"></p-paginator>
            <div class="ui-datatable-tablewrapper" (scroll)="scroll($event)"  *ngIf="!scrollable">
                <table id="pData_table_1" [ngClass]="tableStyleClass" [ngStyle]="tableStyle" >
                    <thead class="ui-datatable-thead"
                           [style.opacity]="fixed_header?'0':'1'"
                    >
                        <tr *ngIf="!headerColumnGroup" class="ui-state-default" [pColumnHeaders]="columns"></tr>
                        <ng-template [ngIf]="headerColumnGroup">
                            <tr *ngFor="let headerRow of headerColumnGroup.rows" class="ui-state-default"
                             [pColumnHeaders]="headerRow.columns"></tr>
                        </ng-template>
                    </thead>
                    <tfoot *ngIf="hasFooter()" class="ui-datatable-tfoot">
                        <tr *ngIf="!footerColumnGroup" class="ui-state-default" [pColumnFooters]="columns"></tr>
                        <ng-template [ngIf]="footerColumnGroup">
                            <tr *ngFor="let footerRow of footerColumnGroup.rows"
                            class="ui-state-default" [pColumnFooters]="footerRow.columns"></tr>
                        </ng-template>
                    </tfoot>
                    <tbody [ngClass]="{'ui-datatable-data ui-widget-content': true,
                    'ui-datatable-hoverable-rows': (rowHover||selectionMode)}" [pTableBody]="columns"
                     [data]="dataToRender"></tbody>
                </table>

                <table id="pData_table_fixed" [ngClass]="tableStyleClass" [ngStyle]="tableStyle"
               [style.display]="fixed_header?'block':'none'"
                >
                <thead class="ui-datatable-thead fixed_header">
                    <tr *ngIf="!headerColumnGroup" class="ui-state-default" [pColumnHeaders]="columns"></tr>
                    <ng-template [ngIf]="headerColumnGroup">
                        <tr *ngFor="let headerRow of headerColumnGroup.rows" class="ui-state-default"
                         [pColumnHeaders]="headerRow.columns"></tr>
                    </ng-template>
                </thead>
            </table>
            </div>
            <ng-template [ngIf]="scrollable">
                <div class="ui-datatable-scrollable-wrapper ui-helper-clearfix" [ngClass]="{'max-height':scrollHeight}">
                    <div *ngIf="hasFrozenColumns()" [pScrollableView]="frozenColumns" frozen="true"
                        [ngStyle]="{'width':this.frozenWidth}" class="ui-datatable-scrollable-view ui-datatable-frozen-view"></div>
                    <div [pScrollableView]="scrollableColumns" [ngStyle]="{'width':this.unfrozenWidth, 'left': this.frozenWidth}"
                        class="ui-datatable-scrollable-view" [virtualScroll]="virtualScroll" (onVirtualScroll)="onVirtualScroll($event)"
                        [ngClass]="{'ui-datatable-unfrozen-view': hasFrozenColumns()}"></div>
                </div>
            </ng-template>

            <p-paginator [rows]="rows" [first]="first" [totalRecords]="totalRecords"
            [pageLinkSize]="pageLinks" styleClass="ui-paginator-bottom" [alwaysShow]="alwaysShowPaginator"
                (onPageChange)="onPageChange($event)"
                [rowsPerPageOptions]="rowsPerPageOptions"
                *ngIf="paginator && paginatorPosition =='bottom' || paginatorPosition =='both'"></p-paginator>
            <div class="ui-datatable-footer ui-widget-header" *ngIf="footer">
                <ng-content select="p-footer"></ng-content>
            </div>

            <div class="ui-column-resizer-helper ui-state-highlight" style="display:none"></div>
            <span class="fa fa-arrow-down ui-datatable-reorder-indicator-up" style="position: absolute; display: none;"></span>
            <span class="fa fa-arrow-up ui-datatable-reorder-indicator-down" style="position: absolute; display: none;"></span>
        </div>
    `,
    providers: [DomHandler, ObjectUtils]
})
// tslint:disable-next-line:component-class-suffix
export class DataTable implements AfterViewChecked, AfterViewInit, AfterContentInit, OnInit, OnDestroy, BlockableUI {
    @Input() bulkEnableForPN: boolean;

    @Input() tableKeyField: any;

    @Input() fieldLabelHashTable: any = {}

    @Input() schema: any[];

    @Input() paginator: boolean;

    @Input() currentPage: number;

    @Input() rows: number;

    @Input() pageLinks = 5;

    @Input() rowsPerPageOptions: number[];

    @Input() responsive: boolean;

    @Input() stacked: boolean;

    @Input() selectionMode: string;

    @Output() selectionChange: EventEmitter<any> = new EventEmitter();

    @Input() editable: boolean;

    @Output() onRowClick: EventEmitter<any> = new EventEmitter();

    @Output() onRowSelect: EventEmitter<any> = new EventEmitter();

    @Output() onRowUnselect: EventEmitter<any> = new EventEmitter();

    @Output() onRowDblclick: EventEmitter<any> = new EventEmitter();

    @Output() onHeaderCheckboxToggle: EventEmitter<any> = new EventEmitter();

    @Input() headerCheckboxToggleAllPages: boolean;

    @Output() onContextMenuSelect: EventEmitter<any> = new EventEmitter();

    @Output() onDtViewInitilized: EventEmitter<any> = new EventEmitter()

    @Input() filterDelay = 300;

    @Input() lazy: boolean;

    @Output() onLazyLoad: EventEmitter<any> = new EventEmitter();

    @Input() resizableColumns: boolean;

    @Input() columnResizeMode = 'fit';

    @Output() onColResize: EventEmitter<any> = new EventEmitter();

    @Input() reorderableColumns: boolean;

    @Output() onColReorder: EventEmitter<any> = new EventEmitter();

    @Output() onScroll: EventEmitter<any> = new EventEmitter();

    @Input() scrollable: boolean;

    @Input() virtualScroll: boolean;

    @Input() scrollHeight: any;

    @Input() scrollWidth: any;

    @Input() frozenWidth: any;

    @Input() unfrozenWidth: any;

    @Input() style: any;

    @Input() styleClass: string;

    @Input() tableStyle: any;

    @Input() tableStyleClass: string;

    @Input() globalFilter: any;

    @Input() sortMode = 'single';

    @Input() sortField: string;

    @Input() sortOrder = 1;

    @Input() defaultSortOrder = 1;

    @Input() groupFieldArray: any[];

    @Input() multiSortMeta: SortMeta[];

    @Input() contextMenu: any;

    @Input() csvSeparator = ',';

    @Input() exportFilename = 'download';

    @Input() emptyMessage = 'No records found';

    @Input() paginatorPosition = 'bottom';

    @Input() alwaysShowPaginator = true;

    @Input() metaKeySelection = true;



    @Input() immutable = true;

    @Input() frozenValue: any[];

    @Input() compareSelectionBy = 'deepEquals';

    @Output() onEditInit: EventEmitter<any> = new EventEmitter();

    @Output() onEditComplete: EventEmitter<any> = new EventEmitter();

    @Output() onEdit: EventEmitter<any> = new EventEmitter();

    @Output() onEditCancel: EventEmitter<any> = new EventEmitter();

    @Output() onPage: EventEmitter<any> = new EventEmitter();

    @Output() onSort: EventEmitter<any> = new EventEmitter();

    @Output() onFilter: EventEmitter<any> = new EventEmitter();

    @Output() deleteSubRow: EventEmitter<any> = new EventEmitter();

    @Output() editSubRow: EventEmitter<any> = new EventEmitter();

    @ContentChild(Header) header;

    @ContentChild(Footer) footer;

    @Input() expandableRows: boolean;

    @Input() expandedRows: any[];

    @Input() expandableRowGroups: boolean;

    @Input() rowExpandMode = 'multiple';

    @Input() public expandedRowsGroups: any[];

    @Input() expandedIcon = 'fa-chevron-circle-down';

    @Input() collapsedIcon = 'fa-chevron-circle-right';

    @Input() tabindex = 1;

    @Input() rowStyleClass: Function;

    @Input() rowStyleMap: Object;

    @Input() rowGroupMode: string;

    @Input() sortableRowGroup = true;

    @Input() sortFile: string;

    @Input() rowHover: boolean;

    @Input() public filters: { [s: string]: FilterMetadata; } = {};

    @Input() dataKey: string;

    @Input() loading: boolean;

    @Input() loadingIcon = 'fa-spinner';

    @Output() valueChange: EventEmitter<any[]> = new EventEmitter<any[]>();

    @Output() firstChange: EventEmitter<number> = new EventEmitter<number>();

    @Output() onRowExpand: EventEmitter<any> = new EventEmitter();

    @Output() onRowCollapse: EventEmitter<any> = new EventEmitter();

    @Output() onRowGroupExpand: EventEmitter<any> = new EventEmitter();

    @Output() onRowGroupCollapse: EventEmitter<any> = new EventEmitter();

    @ContentChildren(PrimeTemplate) templates: QueryList<PrimeTemplate>;

    @ContentChildren(Column) cols: QueryList<Column>;

    @ContentChild(HeaderColumnGroup) headerColumnGroup: HeaderColumnGroup;

    @ContentChild(FooterColumnGroup) footerColumnGroup: FooterColumnGroup;

    @Input() groupSummary: any;

    @ViewChild(Paginator) rfpgn: Paginator;

    @Input() genericApiObject: any;

    @Output() onNestedGroupCollapse: EventEmitter<any> = new EventEmitter();

    @Output() onNestedGroupExpand: EventEmitter<any> = new EventEmitter();

    @Output() askConfirmation: EventEmitter<any> = new EventEmitter();

    @Input() gridName: string;
    @Input() groupSummaryTable: any[];

    @Input() public columnStyleClass: Function;

    @Input() public fixed_header = false;

    public selectedNode = '';
    public expandIndexCount = 0;

    public isGroupDataUpdate = false;

    public expandedGroupInfo: any[] = [];

    // public groupFieldArray: any[] = [];

    public _value: any[];

    public dataToRender: any[];

    public page = 0;

    public filterTimeout: any;

    public filteredValue: any[];

    public columns: Column[];

    public frozenColumns: Column[];

    public scrollableColumns: Column[];

    public columnsChanged = false;

    public sortColumn: Column;

    public columnResizing: boolean;

    public lastResizerHelperX: number;

    public documentEditListener: Function;

    public documentColumnResizeEndListener: Function;

    public resizerHelper: any;

    public resizeColumn: any;

    public reorderIndicatorUp: any;

    public reorderIndicatorDown: any;

    public iconWidth: number;

    public iconHeight: number;

    public draggedColumn: any;

    public dropPosition: number;

    public tbody: any;

    public rowTouched: boolean;

    public rowGroupToggleClick: boolean;

    public editingCell: any;

    public virtualTableHeight: number;

    public rowGroupMetadata: any;

    public rowGroupHeaderTemplate: TemplateRef<any>;

    public rowGroupFooterTemplate: TemplateRef<any>;

    public rowExpansionTemplate: TemplateRef<any>;

    public scrollBarWidth: number;

    public editorClick: boolean;

    public _first = 0;

    public selectionKeys: any;

    public preventSelectionKeysPropagation: boolean;

    public preventSortPropagation: boolean;

    public preventRowClickPropagation: boolean;

    public previousRowIndex = -1;

    @Input() rowGroupExpandMode = 'single';

    differ: any;

    _selection: any;

    _totalRecords: number;

    globalFilterFunction: any;

    columnsSubscription: Subscription;

    totalRecordsChanged: boolean;

    anchorRowIndex: number;

    rangeRowIndex: number;

    initialized: boolean;

    // tslint:disable-next-line:member-ordering
    // tslint:disable-next-line:no-input-rename
    // tslint:disable-next-line:member-ordering
    // tslint:disable-next-line:no-input-rename
    @Input('currentPageNumber') currentPageNumber = 0;
    // tslint:disable-next-line:member-ordering
    // tslint:disable-next-line:no-input-rename
    @Input('sortObject') sortObject = { order: 1, field: null, multisortmeta: [] };
    private expandedGroup: object;
    @Input() rowTrackBy: Function = (index: number, item: any) => item;

    // tslint:disable-next-line:member-ordering
    @ViewChild(Paginator) pag: Paginator;


    constructor(public el: ElementRef, public domHandler: DomHandler, public differs: IterableDiffers,
        public renderer: Renderer2, public changeDetector: ChangeDetectorRef, public objectUtils: ObjectUtils,
        public zone: NgZone) {
        this.differ = differs.find([]).create(null);
    }

    getSummary(row, col, root) {
        if (!this.groupSummaryTable) {
            return;
        }

        // if (row && row['metadata']) {
        //     return row.metadata[this.groupFieldArray.length - this.groupFieldArray.indexOf(col) - 1];
        // } else return '';

        const level = this.groupFieldArray.indexOf(col) + 1;

        for (let i = 0; i < this.groupSummaryTable.length; i++) {
            const rowLevel = +this.groupSummaryTable[i].level;
            if (rowLevel !== level) {
                continue;
            }

            let matched = true;
            for (let j = 0; j < this.groupFieldArray.length; j++) {
                const column: string = this.groupFieldArray[j];

                const empty1 = this.groupSummaryTable[i][column] === null ?
                    true :
                    (this.groupSummaryTable[i][column] === '' ? true : false);
                const empty2 = row[column] === null ? true : (row[column] === '' ? true : false);

                if (this.groupSummaryTable[i] && row && (this.groupSummaryTable[i][column] !== row[column] && !empty1 && !empty2)) {
                    matched = false;
                    break;
                }
            }

            if (matched) {
                // Found one
                return this.groupSummaryTable[i].des;
            }
        }


        if (root === true) {

            // Level 2
            // Problem: now we may have the leaf node (g1, g2, g3, g4), but we need to calculate the middle summary node.
            // the currrent implementation is to support 2 level.(this.groupFieldArray.length - 1).
            // The correct solution to reduce the node dims. (g1, g2, g3, g4) -> (g1, g2, g3)
            // tslint:disable-next-line:no-shadowed-variable
            const level = this.groupFieldArray.indexOf(col) + 1;

            for (let i = 0; i < this.groupSummaryTable.length; i++) {
                const rowLevel = +this.groupSummaryTable[i].level;
                if (rowLevel !== level) {
                    continue;
                }

                let matched = true;
                for (let j = 0; j < 1; j++) {
                    const column: string = this.groupFieldArray[j];

                    // tslint:disable-next-line:max-line-length
                    const empty1 = this.groupSummaryTable[i][column] === null ?
                        true :
                        (this.groupSummaryTable[i][column] === '' ? true : false);
                    const empty2 = row[column] === null ? true : (row[column] === '' ? true : false);

                    if (this.groupSummaryTable[i][column] !== row[column] && !empty1 && !empty2) {
                        matched = false;
                        break;
                    }
                }

                if (matched) {
                    // Found one
                    return this.groupSummaryTable[i].des;
                }
            }
        }

        if (this.groupFieldArray.length > 2) {

            // Level 3
            // tslint:disable-next-line:no-shadowed-variable
            const level = this.groupFieldArray.indexOf(col) + 1;

            for (let i = 0; i < this.groupSummaryTable.length; i++) {
                const rowLevel = +this.groupSummaryTable[i].level;
                if (rowLevel !== level) {
                    continue;
                }

                let matched = true;
                for (let j = 0; j < 2; j++) {
                    const column: string = this.groupFieldArray[j];
                    const empty1 = this.groupSummaryTable[i][column] === null ?
                        true :
                        (this.groupSummaryTable[i][column] === '' ? true : false);
                    const empty2 = row[column] === null ? true : (row[column] === '' ? true : false);

                    if (this.groupSummaryTable[i][column] !== row[column] && !empty1 && !empty2) {
                        matched = false;
                        break;
                    }
                }

                if (matched) {
                    // Found one
                    return this.groupSummaryTable[i].des;
                }
            }
        }

        // Not found
        return '';
    }

    getHeader(col) {
        for (let i = 0; i < this.columns.length; i++) {
            if (this.columns[i].field === col) {
                return this.columns[i].header;
            }
        }
    }

    deleteLeave(event) {
        for (let i = 0; i < this.value.length; i++) {
            if (this.value[i][this.tableKeyField] === event.id) {
                this.value = this.value.filter((val, x) => x !== i);
                break;
            }
        };
        this.deleteSubRow.emit(event.leave);
    }

    editLeave(event) {
        for (let i = 0; i < this.value.length; i++) {
            if (this.value[i][this.tableKeyField] === event.leave[this.tableKeyField]) {
                for (const key in this.value[i]) {
                    if (this.value[i][key] !== event.leave[key]) {
                        this.value[i][key] = event.leave[key];
                    }
                }
                break;
            }
        }
        this.editSubRow.emit(event.leave)
    }

    // tslint:disable-next-line:use-life-cycle-interface
    ngOnChanges(changes: SimpleChanges) {
        if (changes.multiSortMeta) {
            this.sortMultiple();
            if (this.lazy) {
                this.onLazyLoad.emit(this.createLazyLoadMetadata());
            }
        } else if (changes.filters) {

            if (changes.filters.previousValue) {
                this._filter();
            }
        }

        /*  if (changes.value && this.expandedRowsGroups && this.groupFieldArray) {
              if (this.groupFieldArray.length > 0) {
                  if (this.expandedRowsGroups.length > 0) {
                      if (this.expandedRowsGroups[0]) {
                          if (this.expandedRowsGroups[0].subGroups) {
                              if (this.expandIndexCount != this.first) {
                                  if (this.expandedGroupInfo.length < this.groupFieldArray.length) {
                                      this.expandedRowsGroups[0].subGroups = [];
                                  }
                              }
                          }
                      }
                  }
              }
          }*/
    }

    clearLeaves(group) {
        if (group) {
            group.forEach(element => {
                if (element.subGroups && element.subGroups.length > 0) {
                    this.clearLeaves(element.subGroups);
                }
                if (this.expandedGroup) {
                    if (element.name !== this.expandedGroup['name'] && element.level === this.expandedGroup['level']) {
                        element.expand = false;
                    }
                }
                const leave = element.leaves[0];
                element.leaves = [];
                element.leaves.push(leave);
            });
        }
    }

    ngOnInit() {
        if (this.lazy) {
            if (this.currentPageNumber > 0) {
                this.rows = this.currentPageNumber;
            }

            // when the transition from non-group grid to
            // group grid or inverse, we need to bring one from anther due to two grids in the view.
            this.multiSortMeta = this.sortObject.multisortmeta;

            this.onLazyLoad.emit(this.createLazyLoadMetadata());
        }
    }

    ngAfterContentInit() {
        this.initColumns();
        this.columnsSubscription = this.cols.changes.subscribe(_ => {
            this.initColumns();
            this.changeDetector.markForCheck();
        });

        this.templates.forEach((item) => {

            switch (item.getType()) {
                case 'rowexpansion':
                    this.rowExpansionTemplate = item.template;
                    break;

                case 'rowgroupheader':
                    this.rowGroupHeaderTemplate = item.template;
                    break;

                case 'rowgroupfooter':

                    this.rowGroupFooterTemplate = item.template;
                    break;
            }
        });
    }

    ngAfterViewChecked() {
        if (this.columnsChanged && this.el.nativeElement.offsetParent) {
            if (this.resizableColumns) {
                this.initResizableColumns();
            }

            if (this.reorderableColumns) {
                this.initColumnReordering();
            }

            this.columnsChanged = false;
        }

        if (this.totalRecordsChanged && this.virtualScroll) {
            const scrollableTable = this.domHandler.findSingle(this.el.nativeElement, 'div.ui-datatable-scrollable-table-wrapper');
            const row = this.domHandler.findSingle(scrollableTable, 'tr.ui-widget-content');
            const rowHeight = this.domHandler.getOuterHeight(row);
            this.virtualTableHeight = this._totalRecords * rowHeight;
            scrollableTable.style.height = this.virtualTableHeight + 'px';
            this.totalRecordsChanged = true;
        }
    }

    ngAfterViewInit() {
        if (this.globalFilter) {
            this.globalFilterFunction = this.renderer.listen(this.globalFilter, 'keyup', () => {
                if (event['keyCode'] === 13) {
                    this.filterTimeout = setTimeout(() => {
                        this._filter();
                        this.filterTimeout = null;
                    }, this.filterDelay);
                }
            });
        }

        this.initialized = true;
        this.onDtViewInitilized.emit(this);
    }

    @Input() get value(): any[] {
        return this._value;
    }

    set value(val: any[]) {
        if (this.immutable) {
            this._value = val ? [...val] : null;
            this.handleDataChange();
        } else {
            this._value = val;
        }

        this.valueChange.emit(this.value);
    }

    @Input() get first(): number {
        return this._first;
    }

    set first(val: number) {
        let shouldPaginate = this.initialized && this._first !== val;
        if (this.expandedGroupInfo && this.expandedGroupInfo.length > 0) {
            shouldPaginate = false;
        }
        this._first = val;
        if (shouldPaginate) {
            this.paginate();
        }
    }

    @Input() get totalRecords(): number {
        return this._totalRecords;
    }

    set totalRecords(val: number) {
        this._totalRecords = val;
        this.totalRecordsChanged = true;
    }

    @Input() get selection(): any {
        return this._selection;
    }

    set selection(val: any) {
        this._selection = val;

        if (this.dataKey && !this.preventSelectionKeysPropagation) {
            this.selectionKeys = {};
            if (this._selection) {
                for (const data of this._selection) {
                    this.selectionKeys[String(this.objectUtils.resolveFieldData(data, this.dataKey))] = 1;
                }
            }
        }
        this.preventSelectionKeysPropagation = false;
    }

    // tslint:disable-next-line:use-life-cycle-interface
    ngDoCheck() {
        if (!this.immutable) {
            const changes = this.differ.diff(this.value);
            if (changes) {
                this.handleDataChange();
            }
        }
    }

    handleDataChange() {
        // this.loading = false;

        if (this.paginator) {
            this.updatePaginator();
        }

        if (!this.lazy) {
            // To reinitialise clear global while grouping.
            if (this.groupFieldArray && this.groupFieldArray.length > 0) {
                if (this.globalFilter) {
                    this.globalFilter.value = null;
                }
            }


            if (this.hasFilter()) {
                this._filter();
            }

            if (this.preventSortPropagation) {
                this.preventSortPropagation = false;
            } else if (this.sortField || this.multiSortMeta) {
                if (!this.sortColumn && this.columns) {
                    this.sortColumn = this.columns.find(col => col.field === this.sortField && col.sortable === 'custom');
                }

                if (this.sortMode === 'single') {
                    this.sortSingle();
                } else if (this.sortMode === 'multiple') {
                    this.sortMultiple();
                }
            }
        }

        this.updateDataToRender(this.filteredValue || this.value);
    }

    initColumns(): void {
        if (this.cols) {
            this.columns = this.cols.toArray();
        }
        if (this.scrollable) {
            this.scrollableColumns = [];
            this.frozenColumns = [];
            this.cols.forEach((col) => {
                if (col.frozen) {
                    this.frozenColumns.push(col);
                } else {
                    this.scrollableColumns.push(col);
                }
            });
        }

        this.columnsChanged = true;
    }

    resolveFieldData(data: any, field: string): any {
        ;
        if (data && field) {
            if (field.indexOf('.') === -1) {
                return data[field];
            } else {
                const fields: string[] = field.split('.');
                let value = data;
                for (let i = 0, len = fields.length; i < len; ++i) {
                    if (value == null) {
                        return null;
                    }
                    value = value[fields[i]];
                }
                return value;
            }
        } else {
            return null;
        }
    }

    updateRowGroupMetadata() {

        this.rowGroupMetadata = {};
        if (this.dataToRender) {
            for (let i = 0; i < this.dataToRender.length; i++) {
                const rowData = this.dataToRender[i];
                const group = this.resolveFieldData(rowData, this.sortField);
                if (i === 0) {
                    this.rowGroupMetadata[group] = { index: 0, size: 1 };
                } else {
                    const previousRowData = this.dataToRender[i - 1];
                    const previousRowGroup = this.resolveFieldData(previousRowData, this.sortField);
                    if (group === previousRowGroup) {
                        this.rowGroupMetadata[group].size++;
                    } else {
                        this.rowGroupMetadata[group] = { index: i, size: 1 };
                    }
                }
            }
        }
    }

    updatePaginator() {
        // total records
        this.updateTotalRecords();

        // first
        if (this.totalRecords && this.first >= this.totalRecords) {
            const numberOfPages = Math.ceil(this.totalRecords / this.rows);
            this._first = Math.max((numberOfPages - 1) * this.rows, 0);
        }
    }

    updateTotalRecords() {
        this.totalRecords = this.lazy ? this.totalRecords : (this.value ? this.value.length : 0);
    }

    onPageChange(event) {
        if (!this.bulkEnableForPN) {
            this._first = event.first;
            this.firstChange.emit(this.first);
            this.rows = event.rows;
            this.paginate();
        } else {

            const promise = new Promise((resolve, reject) => {
                this.askConfirmation.emit(resolve);
            });

            promise.then(val => {
                this.f(val, event.first, event.rows, event.prevValue);
            });

        }
    }

    f(val, first, rows, prevFirst) {
        if (val === 1) {
            this._first = first;
            this.firstChange.emit(first);
            this.rows = rows;
            this.paginate();
        } else {
            console.log(first, rows);
            console.dir(this.pag);
            this.pag.first = prevFirst;
            this.pag.updatePageLinks();
        }
    }

    paginate() {

        if (this.lazy) {
            this.onLazyLoad.emit(this.createLazyLoadMetadata());
        } else {
            this.updateDataToRender(this.filteredValue || this.value);
        }

        this.onPage.emit({
            first: this.first,
            rows: this.rows
        });
    }


    updateDataToRender(datasource) {

        if ((this.paginator || this.virtualScroll) && datasource) {
            this.dataToRender = [];
            const startIndex: number = this.lazy ? 0 : this.first;
            const endIndex: number = this.virtualScroll ? this.first + this.rows * 2 : startIndex + this.rows;

            for (let i = startIndex; i < endIndex; i++) {
                if (i >= datasource.length) {
                    break;
                }

                this.dataToRender.push(datasource[i]);
            }
        } else {
            this.dataToRender = datasource;
        }

        if (this.rowGroupMode) {

            this.updateRowGroupMetadata();
            //  setTimeout(() => {
            // if (!this.isGroupDataUpdate) {
            this.updateRowGroupData();
            // }
            // }, 300);
        }
    }

    updateRowGroupData() {
        this.isGroupDataUpdate = true;
        if (this.expandedRowsGroups && this.expandedGroupInfo) {
            const grp = this.expandedRowsGroups[0];
            this.clearExpandedRowGroup(grp, 0);
            if (this.dataToRender.length > 0) {
                this.dataToRender.forEach(p => {
                    this.updateExpandedRowGroup(p, grp, 0);
                });
            }
        }

    }
    updateExpandedRowGroup(row, grp, i) {
        if (i === this.expandedGroupInfo.length) {
            return;
        }
        if (row && grp) {
            if (grp.name === row[this.expandedGroupInfo[i].columnname]) {
                if (grp.subGroups) {
                    if (grp.subGroups.length > 0) {
                        grp.subGroups.forEach(p => {
                            this.updateExpandedRowGroup(row, p, i + 1);
                        });
                    }

                }
                this.addSubGroupIfNotExists(grp, row);
                this.addLeaveIfNotExist(grp, row);
            }
        }
    }

    clearExpandedRowGroup(grp, i) {
        if (i === this.expandedGroupInfo.length) {
            return;
        }
        if (grp) {
            if (grp.name === this.expandedGroupInfo[i].value) {
                if (grp.subGroups) {
                    if (grp.subGroups.length > 0) {
                        let index = grp.subGroups.length - 1;
                        while (index >= 0) {
                            if (grp.subGroups[index].expand === false
                                || (grp.subGroups[index].name !== this.expandedGroupInfo[i + 1].value)) {
                                grp.subGroups.splice(index, 1);
                            } else {
                                this.clearExpandedRowGroup(grp.subGroups[index], i + 1);
                            }
                            index -= 1;
                        }
                    }
                }
                grp.leaves = [];
            }
        }
    }


    onVirtualScroll(event) {
        this._first = (event.page - 1) * this.rows;

        if (this.lazy) {
            this.onLazyLoad.emit(this.createLazyLoadMetadata());
        } else {
            this.updateDataToRender(this.filteredValue || this.value);
        }
    }

    onHeaderKeydown(event, column: Column) {
        if (event.keyCode === 13) {
            this.sort(event, column);
            event.preventDefault();
        }
    }

    onHeaderMousedown(event, header: any) {
        if (this.reorderableColumns) {
            if (event.target.nodeName !== 'INPUT') {
                header.draggable = true;
            } else if (event.target.nodeName === 'INPUT') {
                header.draggable = false;
            }
        }
    }

    sort(event, column: Column) {
        if (!column.sortable) {
            return;
        }


        if (this.bulkEnableForPN) {
            const promise = new Promise((resolve, reject) => {
                this.askConfirmation.emit(resolve);
            });


            promise.then(val => { this.sort2(event, column, val) })
        } else {
            this.sort2(event, column, 1)
        }

    }

    sort2(event, column: Column, val) {
        if (val === 0) {
            return;
        }

        const targetNode = event.target.nodeName;
        if ((targetNode === 'TH'
            && this.domHandler.hasClass(event.target, 'ui-sortable-column'))
            || ((targetNode === 'SPAN' || targetNode === 'DIV')
                && !this.domHandler.hasClass(event.target, 'ui-clickable'))) {
            if (!this.immutable) {
                this.preventSortPropagation = true;
            }

            const columnSortField = column.sortField || column.field;
            this.sortOrder = (this.sortField === columnSortField) ? this.sortOrder * -1 : this.defaultSortOrder;
            this.sortField = columnSortField;
            this.sortColumn = column;
            const metaKey = event.metaKey || event.ctrlKey;

            if (this.sortMode === 'multiple') {
                if (this.groupFieldArray && this.groupFieldArray.length > 0) {
                    for (let i = 0; i < this.multiSortMeta.length; i++) {
                        if (this.groupFieldArray.indexOf(this.multiSortMeta[i].field) === -1) {
                            this.multiSortMeta.splice(i, 1);
                        }
                    }
                } else {
                    if (!this.multiSortMeta || !metaKey) {
                        this.multiSortMeta = [];
                    }
                }

                this.addSortMeta({ field: this.sortField, order: this.sortOrder });
            }

            if (this.lazy) {
                this._first = 0;
                this.onLazyLoad.emit(this.createLazyLoadMetadata());
            } else {
                if (this.sortMode === 'multiple') {
                    this.sortMultiple();
                } else {
                    this.sortSingle();
                }
            }

            this.onSort.emit({
                field: this.sortField,
                order: this.sortOrder,
                multisortmeta: this.multiSortMeta
            });
        }
        this.updateDataToRender(this.filteredValue || this.value);
    }

    sortSingle() {
        if (this.value) {

            if (this.sortColumn && this.sortColumn.sortable === 'custom') {
                this.preventSortPropagation = true;
                this.sortColumn.sortFunction.emit({
                    field: this.sortField,
                    order: this.sortOrder
                });
            } else {
                this.value.sort((data1, data2) => {
                    const value1 = this.resolveFieldData(data1, this.sortField);
                    const value2 = this.resolveFieldData(data2, this.sortField);
                    let result = null;

                    if (value1 == null && value2 != null) {
                        result = -1;
                    } else if (value1 != null && value2 == null) {
                        result = 1;
                    } else if (value1 == null && value2 == null) {
                        result = 0;
                    } else if (!isNaN(Number(value1)) && !isNaN(Number(value2))) {
                        result = (Number(value1) < Number(value2)) ? -1 : (Number(value1) > Number(value2)) ? 1 : 0;
                    } else if (typeof value1 === 'string' && typeof value2 === 'string') {
                        result = value1.localeCompare(value2);
                    } else {
                        result = (value1 < value2) ? -1 : (value1 > value2) ? 1 : 0;
                    }

                    return (this.sortOrder * result);
                });
            }

            this._first = 0;

            if (this.hasFilter()) {
                this._filter();
            }
        }
    }

    sortMultiple() {
        if (this.value) {
            this.value.sort((data1, data2) => {
                return this.multisortField(data1, data2, this.multiSortMeta, 0);
            });

            if (this.hasFilter()) {
                this._filter();
            }
        }
    }

    multisortField(data1, data2, multiSortMeta, index) {
        const value1 = this.resolveFieldData(data1, multiSortMeta[index].field);
        const value2 = this.resolveFieldData(data2, multiSortMeta[index].field);
        let result = null;
        if (!isNaN(Number(value1)) && !isNaN(Number(value2))) {
            result = (Number(value1) < Number(value2)) ? -1 : (Number(value1) > Number(value2)) ? 1 : 0;
        } else if (typeof value1 === 'string' || value1 instanceof String) {
            if (value1.localeCompare && (value1 !== value2)) {
                return (multiSortMeta[index].order * value1.localeCompare(value2));
            }
        } else {
            result = (value1 < value2) ? -1 : 1;
        }

        if (value1 === value2) {
            return (multiSortMeta.length - 1) > (index) ? (this.multisortField(data1, data2, multiSortMeta, index + 1)) : 0;
        }

        return (multiSortMeta[index].order * result);
    }

    addSortMeta(meta) {
        let index = -1;
        for (let i = 0; i < this.multiSortMeta.length; i++) {
            if (this.multiSortMeta[i].field === meta.field) {
                index = i;
                break;
            }
        }

        if (index >= 0) {
            this.multiSortMeta[index] = meta;
        } else {
            this.multiSortMeta.push(meta);
        }
    }

    isSorted(column: Column) {
        if (!column.sortable) {
            return false;
        }

        const columnSortField = column.sortField || column.field;

        if (this.sortMode === 'single') {
            return (this.sortField && columnSortField === this.sortField);
        } else if (this.sortMode === 'multiple') {
            let sorted = false;
            if (this.multiSortMeta) {
                for (let i = 0; i < this.multiSortMeta.length; i++) {
                    if (this.multiSortMeta[i].field === columnSortField) {
                        sorted = true;
                        break;
                    }
                }
            }
            return sorted;
        }
    }

    getSortOrder(column: Column) {
        let order = 0;
        const columnSortField = column.sortField || column.field;

        if (this.sortMode === 'single') {
            if (this.sortField && columnSortField === this.sortField) {
                order = this.sortOrder;
            }
        } else if (this.sortMode === 'multiple') {
            if (this.multiSortMeta) {
                for (let i = 0; i < this.multiSortMeta.length; i++) {
                    if (this.multiSortMeta[i].field === columnSortField) {
                        order = this.multiSortMeta[i].order;
                        break;
                    }
                }
            }
        }
        return order;
    }

    onRowGroupClick(event) {
        if (this.rowGroupToggleClick) {
            this.rowGroupToggleClick = false;
            return;
        }

        if (this.groupFieldArray.length === 1) {
            if (this.sortableRowGroup) {
                const targetNode = event.target.nodeName;
                if ((targetNode === 'TD' || (targetNode === 'SPAN' && !this.domHandler.hasClass(event.target, 'ui-clickable')))) {
                    if (this.sortField !== this.groupFieldArray[0]) {
                        this.sortField = this.groupFieldArray[0];
                        this.sortSingle();
                    } else {
                        this.sortOrder = -1 * this.sortOrder;
                        this.sortSingle();
                    }
                }
            }
        }
    }

    clearSelectionRange() {
        let rangeStart, rangeEnd;

        if (this.rangeRowIndex > this.anchorRowIndex) {
            rangeStart = this.anchorRowIndex;
            rangeEnd = this.rangeRowIndex;
        } else if (this.rangeRowIndex < this.anchorRowIndex) {
            rangeStart = this.rangeRowIndex;
            rangeEnd = this.anchorRowIndex;
        } else {
            rangeStart = this.rangeRowIndex;
            rangeEnd = this.rangeRowIndex;
        }

        for (let i = rangeStart; i <= rangeEnd; i++) {
            const rangeRowData = this.dataToRender[i];
            const selectionIndex = this.findIndexInSelection(rangeRowData);
            // tslint:disable-next-line:no-shadowed-variable
            this._selection = this.selection.filter((val, ii) => ii !== selectionIndex);
            const dataKeyValue: string = this.dataKey ? String(this.resolveFieldData(rangeRowData, this.dataKey)) : null;
            if (dataKeyValue) {
                delete this.selectionKeys[dataKeyValue];
            }
            this.onRowUnselect.emit({ originalEvent: event, data: rangeRowData, type: 'row' });
        }
    }

    selectRange(rowIndex: number) {
        let rangeStart, rangeEnd;

        if (this.anchorRowIndex > rowIndex) {
            rangeStart = rowIndex;
            rangeEnd = this.anchorRowIndex;
        } else if (this.anchorRowIndex < rowIndex) {
            rangeStart = this.anchorRowIndex;
            rangeEnd = rowIndex;
        } else {
            rangeStart = rowIndex;
            rangeEnd = rowIndex;
        }

        for (let i = rangeStart; i <= rangeEnd; i++) {
            const rangeRowData = this.dataToRender[i];
            this._selection = [...this.selection, rangeRowData];
            this.selectionChange.emit(this.selection);
            const dataKeyValue: string = this.dataKey ? String(this.resolveFieldData(rangeRowData, this.dataKey)) : null;
            if (dataKeyValue) {
                this.selectionKeys[dataKeyValue] = 1;
            }
            this.onRowSelect.emit({ originalEvent: event, data: rangeRowData, type: 'row' });
        }
    }

    handleRowClick(event: MouseEvent, rowData: any, index: number) {
        if (this.preventRowClickPropagation) {
            this.preventRowClickPropagation = false;
            return;
        }

        if (this.previousRowIndex === index) {
            return;
        }

        this.previousRowIndex = index;

        const targetNode = (<HTMLElement>event.target).nodeName;

        if (targetNode === 'INPUT'
            || targetNode === 'BUTTON'
            || targetNode === 'A'
            || (this.domHandler.hasClass(event.target, 'ui-clickable'))) {
            return;
        }

        this.onRowClick.next({ originalEvent: event, data: rowData });

        if (this.selectionMode) {
            if (this.isMultipleSelectionMode() && event.shiftKey && this.anchorRowIndex != null) {
                this.domHandler.clearSelection();
                if (this.rangeRowIndex != null) {
                    this.clearSelectionRange();
                }

                this.rangeRowIndex = index;
                this.selectRange(index);
            } else {
                const selected = this.isSelected(rowData);
                const metaSelection = this.rowTouched ? false : this.metaKeySelection;
                const dataKeyValue: string = this.dataKey ? String(this.resolveFieldData(rowData, this.dataKey)) : null;
                this.anchorRowIndex = index;
                this.rangeRowIndex = index;

                if (metaSelection) {
                    const metaKey = event.metaKey || event.ctrlKey;

                    if (selected && metaKey) {
                        if (this.isSingleSelectionMode()) {
                            this._selection = null;
                            this.selectionKeys = {};
                            this.selectionChange.emit(null);
                        } else {
                            const selectionIndex = this.findIndexInSelection(rowData);
                            this._selection = this.selection.filter((val, i) => i !== selectionIndex);
                            this.selectionChange.emit(this.selection);
                            if (dataKeyValue) {
                                delete this.selectionKeys[dataKeyValue];
                            }
                        }

                        this.onRowUnselect.emit({ originalEvent: event, data: rowData, type: 'row' });
                    } else {
                        if (this.isSingleSelectionMode()) {
                            this._selection = rowData;
                            this.selectionChange.emit(rowData);
                            if (dataKeyValue) {
                                this.selectionKeys = {};
                                this.selectionKeys[dataKeyValue] = 1;
                            }
                        } else if (this.isMultipleSelectionMode()) {
                            if (metaKey) {
                                this._selection = this.selection || [];
                            } else {
                                this._selection = [];
                                this.selectionKeys = {};
                            }

                            this._selection = [...this.selection, rowData];
                            this.selectionChange.emit(this.selection);
                            if (dataKeyValue) {
                                this.selectionKeys[dataKeyValue] = 1;
                            }
                        }

                        this.onRowSelect.emit({ originalEvent: event, data: rowData, type: 'row' });
                    }
                } else {
                    if (this.isSingleSelectionMode()) {
                        if (selected) {
                            this._selection = null;
                            this.selectionKeys = {};
                            this.onRowUnselect.emit({ originalEvent: event, data: rowData, type: 'row' });
                        } else {
                            this._selection = rowData;
                            this.onRowSelect.emit({ originalEvent: event, data: rowData, type: 'row' });
                            if (dataKeyValue) {
                                this.selectionKeys = {};
                                this.selectionKeys[dataKeyValue] = 1;
                            }
                        }
                    } else {
                        if (selected) {
                            const selectionIndex = this.findIndexInSelection(rowData);
                            this._selection = this.selection.filter((val, i) => i !== selectionIndex);
                            this.onRowUnselect.emit({ originalEvent: event, data: rowData, type: 'row' });
                            if (dataKeyValue) {
                                delete this.selectionKeys[dataKeyValue];
                            }
                        } else {
                            this._selection = [...this.selection || [], rowData];
                            this.onRowSelect.emit({ originalEvent: event, data: rowData, type: 'row' });
                            if (dataKeyValue) {
                                this.selectionKeys[dataKeyValue] = 1;
                            }
                        }
                    }

                    this.selectionChange.emit(this.selection);
                }
            }

            this.preventSelectionKeysPropagation = true;
        }

        this.rowTouched = false;
    }

    handleRowTouchEnd(event: Event) {
        this.rowTouched = true;
    }

    selectRowWithRadio(event: Event, rowData: any) {
        if (this.selection !== rowData) {
            this._selection = rowData;
            this.selectionChange.emit(this.selection);
            this.onRowSelect.emit({ originalEvent: event, data: rowData, type: 'radiobutton' });

            if (this.dataKey) {
                this.selectionKeys = {};
                this.selectionKeys[String(this.resolveFieldData(rowData, this.dataKey))] = 1;
            }
        } else {
            this._selection = null;
            this.selectionChange.emit(this.selection);
            this.onRowUnselect.emit({ originalEvent: event, data: rowData, type: 'radiobutton' });
        }

        this.preventSelectionKeysPropagation = true;
        this.preventRowClickPropagation = true;
    }

    toggleRowWithCheckbox(event, rowData: any) {
        const selectionIndex = this.findIndexInSelection(rowData);
        this.selection = this.selection || [];
        const dataKeyValue: string = this.dataKey ? String(this.resolveFieldData(rowData, this.dataKey)) : null;

        if (selectionIndex !== -1) {
            this._selection = this.selection.filter((val, i) => i !== selectionIndex);
            this.onRowUnselect.emit({ originalEvent: event, data: rowData, type: 'checkbox' });
            if (dataKeyValue) {
                delete this.selectionKeys[dataKeyValue];
            }
        } else {
            this._selection = [...this.selection, rowData];
            this.onRowSelect.emit({ originalEvent: event, data: rowData, type: 'checkbox' });
            if (dataKeyValue) {
                this.selectionKeys[dataKeyValue] = 1;
            }
        }

        this.selectionChange.emit(this.selection);
        this.preventSelectionKeysPropagation = true;
        this.preventRowClickPropagation = true;
    }

    toggleRowsWithCheckbox(event) {
        if (event.checked) {
            this.selection = this.headerCheckboxToggleAllPages ? this.value.slice() : this.dataToRender.slice();
        } else {
            this.selection = [];
        }

        this.selectionChange.emit(this.selection);

        this.onHeaderCheckboxToggle.emit({ originalEvent: event, checked: event.checked });
    }

    onRowRightClick(event, rowData) {
        if (this.contextMenu) {
            const selectionIndex = this.findIndexInSelection(rowData);
            const selected = selectionIndex !== -1;
            const dataKeyValue: string = this.dataKey ? String(this.resolveFieldData(rowData, this.dataKey)) : null;

            if (!selected) {
                if (this.isSingleSelectionMode()) {
                    this.selection = rowData;
                    this.selectionChange.emit(rowData);
                } else if (this.isMultipleSelectionMode()) {
                    this.selection = [rowData];
                    this.selectionChange.emit(this.selection);
                }

                if (this.dataKey) {
                    this.selectionKeys[String(this.resolveFieldData(rowData, this.dataKey))] = 1;
                }
            }

            this.contextMenu.show(event);
            this.onContextMenuSelect.emit({ originalEvent: event, data: rowData });
        }

        this.preventSelectionKeysPropagation = true;
    }

    scroll(e) {
        this.onScroll.emit(e);
    }
    rowDblclick(event, rowData) {
        console.log('rowDblclick');
        this.onRowDblclick.emit({ originalEvent: event, data: rowData });
    }

    isSingleSelectionMode() {
        return this.selectionMode === 'single';
    }

    isMultipleSelectionMode() {
        return this.selectionMode === 'multiple';
    }

    findIndexInSelection(rowData: any) {
        let index = -1;
        if (this.selection) {
            for (let i = 0; i < this.selection.length; i++) {
                if (this.equals(rowData, this.selection[i])) {
                    index = i;
                    break;
                }
            }
        }

        return index;
    }

    isSelected(rowData) {
        if (rowData && this.selection) {
            if (this.dataKey) {
                return this.selectionKeys[this.objectUtils.resolveFieldData(rowData, this.dataKey)] !== undefined;
            } else {
                if (this.selection instanceof Array) {
                    return this.findIndexInSelection(rowData) > -1;
                } else {
                    return this.equals(rowData, this.selection);
                }
            }
        }

        return false;
    }

    equals(data1, data2) {
        return this.compareSelectionBy === 'equals' ? (data1 === data2) : this.objectUtils.equals(data1, data2, this.dataKey);
    }

    get allSelected() {
        if (this.headerCheckboxToggleAllPages) {
            return this.selection && this.value && this.selection.length === this.value.length;
        } else {
            let val = true;
            if (this.dataToRender && this.selection && (this.dataToRender.length <= this.selection.length)) {
                for (const data of this.dataToRender) {
                    if (!this.isSelected(data)) {
                        val = false;
                        break;
                    }
                }
            } else {
                val = false;
            }
            return val;
        }
    }

    onFilterKeyup(event, field, matchMode, type) {
        if (this.filterTimeout) {
            clearTimeout(this.filterTimeout);
        }

        this.filterTimeout = setTimeout(() => {
            this.filter(event, field, matchMode, type);
            this.filterTimeout = null;
        }, this.filterDelay);
    }

    getColumnFromSchema(field: String) {
        let column;
        for (let i = 1; i < this.schema.length; i++) {
            if (this.schema[i].field === field) {
                column = this.schema[i];
                break;
            }
        }
        return column;
    }

    isBlankOrIsNotBlank(matchMode) {
        return matchMode === 'isBlank' || matchMode === 'isNotBlank';
    }

    filter(event, field, matchMode, type) {
        const fieldArr = field.split('^');
        field = fieldArr[0];
        // Filter works only on enter
        if (event.value !== undefined && event.value !== null) {
            if (!this.isFilterBlank(event.value) || this.isBlankOrIsNotBlank(matchMode)) {
                if (type === 'boolean') {
                    let value;
                    if (Array.isArray(event.value)) {
                        value = event.value.map(elem => {
                            elem = (elem === 'Checked') ? 1 : (elem === 'Unchecked' ? 0 : '');
                            return elem;
                        })
                    } else {
                        value = (event.value === '-1') ? '' : event.value;
                    }
                    this.filters[field] = { value: value, matchMode: matchMode };
                } else {
                    this.filters[field] = { value: event.value, matchMode: matchMode };
                }
            } else if (this.filters[field]) {
                //    this.filters[field] = { value: '', matchMode: 'ontains' };
                delete this.filters[field];
            }
            this._filter();
        } else if (event.keyCode === 13) { // 'Enter' key is pressed
            if (!this.isFilterBlank(event.target.value)) {
                if (type === 'percent') {
                    const decimalPlace = Number(this.getColumnFromSchema(field).format.format.split('.')[1].split('-')[0]);
                    const length = event.target.value.toString().length;
                    let value;
                    if (length > decimalPlace) {
                        const a = Number(event.target.value) / 100;
                        const b = Math.pow(10, length);
                        value = Math.round(a * b) / b;
                    } else {
                        value = Math.round((Number(event.target.value) / 100) * (10 * decimalPlace)) / (10 * decimalPlace);
                    }
                    this.filters[field] = { value: value, matchMode: matchMode };
                } else {
                    this.filters[field] = { value: event.target.value, matchMode: matchMode };
                }

            } else if (this.filters[field]) {
                delete this.filters[field];
            }
            this._filter();
            event.target.blur();
        } else if ((fieldArr.length > 1 && fieldArr[1] === 'dateTime') || (type === 'date' && fieldArr.length === 1)) {
            const len = fieldArr.length;

            if (len > 1) {
                const date = event.getFullYear() + '-' + (event.getMonth() + 1) + '-' + event.getDate();
                if (!this.isFilterBlank(event)) {
                    this.filters[field] = { value: date, matchMode: matchMode };
                } else if (this.filters[field]) {
                    delete this.filters[field];
                }
                this._filter();
            } else {
                // Only filter model changed.
                if (this.filters[field] == null) {
                    if (event.datevalue == null) {
                        return;
                    }

                    fieldArr.push('dateTime');
                    this.filters[field] = { value: event.datevalue, matchMode: matchMode };
                    this._filter();
                    return;
                } else {
                    // value in filter
                    // delete this.filters[field];
                    fieldArr.push('dateTime');
                    const value = this.filters[field];
                    this.filters[field] = { value: value.value, matchMode: matchMode };
                    this._filter();
                }
            }
        } else {

        }
    }

    isFilterBlank(filter: any): boolean {
        if (filter !== null && filter !== undefined) {
            if ((typeof filter === 'string' && filter.trim().length === 0) || (filter instanceof Array && filter.length === 0)) {
                return true;
            } else {
                return false;
            }
        }
        return true;
    }

    _filter() {

        if (this.bulkEnableForPN) {
            const promise = new Promise((resolve, reject) => {
                this.askConfirmation.emit(resolve);
            });


            promise.then(val => { this._filter2(val) })
        } else {
            this._filter2(1)
        }

    }

    _filter2(val) {

        if (val === 0) {
            return;
        }
        this._first = 0;
        if (this.columns === undefined) {
            this.initColumns();
        }
        this.expandedRowsGroups = [];
        if (this.lazy) {
            this.onLazyLoad.emit(this.createLazyLoadMetadata());
        } else {
            this.filteredValue = [];

            for (let i = 0; i < this.value.length; i++) {
                let localMatch = true;
                let globalMatch = false;

                for (let j = 0; j < this.columns.length; j++) {
                    const col = this.columns[j],
                        filterMeta = this.filters[col.filterField || col.field];

                    // local
                    if (filterMeta) {
                        const filterValue = filterMeta.value,
                            filterField = col.filterField || col.field,
                            filterMatchMode = filterMeta.matchMode || 'startsWith',
                            dataFieldValue = this.resolveFieldData(this.value[i], filterField);
                        const filterConstraint = this.filterConstraints[filterMatchMode];

                        if (!filterConstraint(dataFieldValue, filterValue)) {
                            localMatch = false;
                        }

                        if (!localMatch) {
                            break;
                        }
                    }

                    // global
                    if (this.globalFilter && !globalMatch) {
                        globalMatch = this.filterConstraints['contains'](
                            this.resolveFieldData(this.value[i], col.filterField || col.field),
                            this.globalFilter.value);
                    }
                }

                let matches = localMatch;
                if (this.globalFilter) {
                    matches = localMatch && globalMatch;
                }

                if (matches) {
                    this.filteredValue.push(this.value[i]);
                }
            }

            if (this.filteredValue.length === this.value.length) {
                this.filteredValue = null;
            }

            if (this.paginator) {
                this.totalRecords = this.filteredValue ? this.filteredValue.length : this.value ? this.value.length : 0;
            }

            this.updateDataToRender(this.filteredValue || this.value);
        }

        this.onFilter.emit({
            filters: this.filters,
            filteredValue: this.filteredValue || this.value
        });
    }

    hasFilter() {
        let empty = true;
        for (const prop in this.filters) {
            if (this.filters.hasOwnProperty(prop)) {
                empty = false;
                break;
            }
        }
        return !empty || (this.globalFilter && this.globalFilter.value && this.globalFilter.value.trim().length);
    }

    onFilterInputClick(event) {
        event.stopPropagation();
    }

    // tslint:disable-next-line:member-ordering
    filterConstraints = {
        lessThan(value, filter): boolean {
            const filterValue = Number(filter);
            if (filter === undefined || filter === null || filter.trim() === '' || isNaN(filterValue)) {
                return true;
            }

            if (value === undefined || value === null || isNaN(Number(value))) {
                return false;
            }
            return Number(value) < filterValue;
        },

        greaterThan(value, filter): boolean {
            const filterValue = Number(filter);
            if (filter === undefined || filter === null || filter.trim() === '' || isNaN(filterValue)) {
                return true;
            }

            if (value === undefined || value === null || isNaN(Number(value))) {
                return false;
            }
            return Number(value) > filterValue;
        },

        lessThanOrEqual(value, filter): boolean {
            const filterValue = Number(filter);
            if (filter === undefined || filter === null || filter.trim() === '' || isNaN(filterValue)) {
                return true;
            }

            if (value === undefined || value === null || isNaN(Number(value))) {
                return false;
            }
            return Number(value) <= filterValue;
        },

        greaterThanOrEqual(value, filter): boolean {
            const filterValue = Number(filter);
            if (filter === undefined || filter === null || filter.trim() === '' || isNaN(filterValue)) {
                return true;
            }

            if (value === undefined || value === null || isNaN(Number(value))) {
                return false;
            }
            return Number(value) >= filterValue;
        },

        startsWith(value, filter): boolean {
            if (filter === undefined || filter === null || filter.trim() === '') {
                return true;
            }

            if (value === undefined || value === null) {
                return false;
            }

            const filterValue = filter.toLowerCase();
            return value.toString().toLowerCase().slice(0, filterValue.length) === filterValue;
        },

        contains(value, filter): boolean {
            if (filter === undefined || filter === null || (typeof filter === 'string' && filter.trim() === '')) {
                return true;
            }

            if (value === undefined || value === null) {
                return false;
            }

            return value.toString().toLowerCase().indexOf(filter.toLowerCase()) !== -1;
        },

        endsWith(value, filter): boolean {
            if (filter === undefined || filter === null || filter.trim() === '') {
                return true;
            }

            if (value === undefined || value === null) {
                return false;
            }

            const filterValue = filter.toString().toLowerCase();
            return value.toString().toLowerCase().indexOf(filterValue, value.toString().length - filterValue.length) !== -1;
        },

        equals(value, filter): boolean {
            if (filter === undefined || filter === null || (typeof filter === 'string' && filter.trim() === '')) {
                return true;
            }

            if (value === undefined || value === null) {
                return false;
            }

            return value.toString().toLowerCase() === filter.toString().toLowerCase();
        },

        notEquals(value, filter): boolean {
            if (filter === undefined || filter === null || (typeof filter === 'string' && filter.trim() === '')) {
                return false;
            }

            if (value === undefined || value === null) {
                return true;
            }

            return value.toString().toLowerCase() !== filter.toString().toLowerCase();
        },

        in(value, filter: any[]): boolean {
            if (filter === undefined || filter === null || filter.length === 0) {
                return true;
            }

            if (value === undefined || value === null) {
                return false;
            }

            for (let i = 0; i < filter.length; i++) {
                if (filter[i] === value) {
                    return true;
                }
            }

            return false;
        }
    }

    switchCellToEditMode(cell: any, column: Column, rowData: any) {
        if (this.editable && column.editable) {
            this.editorClick = true;
            this.bindDocumentEditListener();

            if (cell !== this.editingCell) {
                if (this.editingCell && this.domHandler.find(this.editingCell, '.ng-invalid.ng-dirty').length === 0) {
                    this.domHandler.removeClass(this.editingCell, 'ui-cell-editing');
                }

                this.editingCell = cell;
                this.onEditInit.emit({ column: column, data: rowData });
                this.domHandler.addClass(cell, 'ui-cell-editing');
                const focusable = this.domHandler.findSingle(cell, '.ui-cell-editor input');
                if (focusable) {
                    setTimeout(() => this.domHandler.invokeElementMethod(focusable, 'focus'), 50);
                }
            }
        }
    }

    switchCellToViewMode(element: any) {
        this.editingCell = null;
        const cell = this.findCell(element);
        this.domHandler.removeClass(cell, 'ui-cell-editing');
        this.unbindDocumentEditListener();
    }

    closeCell() {
        if (this.editingCell) {
            this.domHandler.removeClass(this.editingCell, 'ui-cell-editing');
            this.editingCell = null;
            this.unbindDocumentEditListener();
        }
    }

    bindDocumentEditListener() {
        if (!this.documentEditListener) {
            this.documentEditListener = this.renderer.listen('document', 'click', (event) => {
                if (!this.editorClick) {
                    this.closeCell();
                }
                this.editorClick = false;
            });
        }
    }

    unbindDocumentEditListener() {
        if (this.documentEditListener) {
            this.documentEditListener();
            this.documentEditListener = null;
        }
    }

    onCellEditorKeydown(event, column: Column, rowData: any, rowIndex: number) {
        if (this.editable) {
            this.onEdit.emit({ originalEvent: event, column: column, data: rowData, index: rowIndex });
            if (event.keyCode !== 13 && event.keyCode !== 27
                && event.keyCode !== 9 && event.keyCode !== 37 && event.keyCode !== 38 && event.keyCode !== 39
                && event.keyCode !== 40) {
                this.onEditComplete.emit({ column: column, data: rowData, index: rowIndex });
            }
            // enter
            if (event.keyCode === 13) {
                // this.onEditComplete.emit({column: column, data: rowData, index: rowIndex});
                this.domHandler.invokeElementMethod(event.target, 'blur');
                this.switchCellToViewMode(event.target);
                event.preventDefault();
            } else if (event.keyCode === 27) {
                this.onEditCancel.emit({ column: column, data: rowData, index: rowIndex });
                this.domHandler.invokeElementMethod(event.target, 'blur');
                this.switchCellToViewMode(event.target);
                event.preventDefault();
            } else if (event.keyCode === 9) {
                // this.onEditComplete.emit({column: column, data: rowData, index: rowIndex});

                if (event.shiftKey) {
                    this.moveToPreviousCell(event);
                } else {
                    this.moveToNextCell(event);
                }
            }
        }
    }

    moveToPreviousCell(event: KeyboardEvent) {
        const currentCell = this.findCell(event.target);
        const row = currentCell.parentElement;
        const targetCell = this.findPreviousEditableColumn(currentCell);

        if (targetCell) {
            this.domHandler.invokeElementMethod(targetCell, 'click');
            event.preventDefault();
        }
    }

    moveToNextCell(event: KeyboardEvent) {
        const currentCell = this.findCell(event.target);
        const row = currentCell.parentElement;
        const targetCell = this.findNextEditableColumn(currentCell);

        if (targetCell) {
            this.domHandler.invokeElementMethod(targetCell, 'click');
            event.preventDefault();
        }
    }

    findPreviousEditableColumn(cell: Element) {
        let prevCell = cell.previousElementSibling;

        if (!prevCell) {
            const previousRow = cell.parentElement.previousElementSibling;
            if (previousRow) {
                prevCell = previousRow.lastElementChild;
            }
        }

        if (prevCell) {
            if (this.domHandler.hasClass(prevCell, 'ui-editable-column')) {
                return prevCell;
            } else {
                return this.findPreviousEditableColumn(prevCell);
            }
        } else {
            return null;
        }
    }

    findNextEditableColumn(cell: Element) {
        let nextCell = cell.nextElementSibling;

        if (!nextCell) {
            const nextRow = cell.parentElement.nextElementSibling;
            if (nextRow) {
                nextCell = nextRow.firstElementChild;
            }
        }

        if (nextCell) {
            if (this.domHandler.hasClass(nextCell, 'ui-editable-column')) {
                return nextCell;
            } else {
                return this.findNextEditableColumn(nextCell);
            }
        } else {
            return null;
        }
    }

    onCustomEditorFocusPrev(event: KeyboardEvent) {
        this.moveToPreviousCell(event);
    }

    onCustomEditorFocusNext(event: KeyboardEvent) {
        this.moveToNextCell(event);
    }

    findCell(element) {
        if (element) {
            let cell = element;
            while (cell && cell.tagName !== 'TD') {
                cell = cell.parentElement;
            }

            return cell;
        } else {
            return null;
        }
    }

    initResizableColumns() {
        this.tbody = this.domHandler.findSingle(this.el.nativeElement, 'tbody.ui-datatable-data');
        this.resizerHelper = this.domHandler.findSingle(this.el.nativeElement, 'div.ui-column-resizer-helper');
        this.fixColumnWidths();
    }

    onDocumentMouseMove(event) {
        if (this.columnResizing) {
            this.onColumnResize(event);
        }
    }

    onDocumentMouseUp(event) {
        if (this.columnResizing) {
            this.columnResizing = false;
            this.onColumnResizeEnd(event);
        }
    }

    bindColumnResizeEvents() {
        this.zone.runOutsideAngular(() => {
            window.document.addEventListener('mousemove', this.onDocumentMouseMove.bind(this));
        });

        this.documentColumnResizeEndListener = this.renderer.listen('document', 'mouseup', (event) => {
            if (this.columnResizing) {
                this.columnResizing = false;
                this.onColumnResizeEnd(event);
            }
        });
    }

    unbindColumnResizeEvents() {
        window.document.removeEventListener('mousemove', this.onDocumentMouseMove);

        if (this.documentColumnResizeEndListener) {
            this.documentColumnResizeEndListener();
            this.documentColumnResizeEndListener = null;
        }
    }

    initColumnResize(event) {
        this.bindColumnResizeEvents();

        const container = this.el.nativeElement.children[0];
        const containerLeft = this.domHandler.getOffset(container).left;
        this.resizeColumn = event.target.parentElement;
        this.columnResizing = true;
        this.lastResizerHelperX = (event.pageX - containerLeft + container.scrollLeft);
    }

    onColumnResize(event) {
        const container = this.el.nativeElement.children[0];
        const containerLeft = this.domHandler.getOffset(container).left;
        this.domHandler.addClass(container, 'ui-unselectable-text');
        this.resizerHelper.style.height = container.offsetHeight + 'px';
        this.resizerHelper.style.top = 0 + 'px';
        this.resizerHelper.style.left = (event.pageX - containerLeft + container.scrollLeft) + 'px';

        this.resizerHelper.style.display = 'block';
    }

    onColumnResizeEnd(event) {
        const delta = this.resizerHelper.offsetLeft - this.lastResizerHelperX;
        const columnWidth = this.resizeColumn.offsetWidth;
        const newColumnWidth = columnWidth + delta;
        const minWidth = this.resizeColumn.style.minWidth || 15;

        if (columnWidth + delta > parseInt(minWidth, 10)) {
            if (this.columnResizeMode === 'fit') {
                const nextColumn = this.resizeColumn.nextElementSibling;
                const nextColumnWidth = nextColumn.offsetWidth - delta;

                if (newColumnWidth > 15 && nextColumnWidth > 15) {
                    this.resizeColumn.style.width = newColumnWidth + 'px';
                    if (nextColumn) {
                        nextColumn.style.width = nextColumnWidth + 'px';
                    }

                    if (this.scrollable) {
                        const colGroup = this.domHandler.findSingle(this.el.nativeElement, 'colgroup.ui-datatable-scrollable-colgroup');
                        const resizeColumnIndex = this.domHandler.index(this.resizeColumn);
                        colGroup.children[resizeColumnIndex].style.width = newColumnWidth + 'px';

                        if (nextColumn) {
                            colGroup.children[resizeColumnIndex + 1].style.width = nextColumnWidth + 'px';
                        }
                    }
                }
            } else if (this.columnResizeMode === 'expand') {
                this.tbody.parentElement.style.width = this.tbody.parentElement.offsetWidth + delta + 'px';
                this.resizeColumn.style.width = newColumnWidth + 'px';
                const containerWidth = this.tbody.parentElement.style.width;

                if (this.scrollable) {
                    this.domHandler.findSingle(this.el.nativeElement,
                        '.ui-datatable-scrollable-header-box').children[0].style.width
                        = containerWidth;
                    const colGroup = this.domHandler.findSingle(this.el.nativeElement, 'colgroup.ui-datatable-scrollable-colgroup');
                    const resizeColumnIndex = this.domHandler.index(this.resizeColumn);
                    colGroup.children[resizeColumnIndex].style.width = newColumnWidth + 'px';
                } else {
                    this.el.nativeElement.children[0].style.width = containerWidth;
                }
            }

            this.onColResize.emit({
                element: this.resizeColumn,
                delta: delta
            });
        }

        this.resizerHelper.style.display = 'none';
        this.resizeColumn = null;
        this.domHandler.removeClass(this.el.nativeElement.children[0], 'ui-unselectable-text');
        this.unbindColumnResizeEvents();
    }

    fixColumnWidths() {
        const v_columns = this.domHandler.find(this.el.nativeElement, 'th.ui-resizable-column');
        let bodyCols;

        for (let i = 0; i < v_columns.length; i++) {
            v_columns[i].style.width = v_columns[i].offsetWidth + 'px';
            // if (this.columns && this.columns[i]) {
            //     this.columns[i].style.width = v_columns[i].clientWidth + 'px';
            // }
        }

        if (this.scrollable) {
            const colGroup = this.domHandler.findSingle(this.el.nativeElement, 'colgroup.ui-datatable-scrollable-colgroup');
            bodyCols = colGroup.children;

            if (bodyCols) {
                for (let i = 0; i < v_columns.length; i++) {
                    bodyCols[i].style.width = v_columns[i].offsetWidth + 'px';
                }
            }
        }
    }

    onColumnDragStart(event) {
        this.changeDetector.markForCheck();
        if (this.columnResizing) {
            event.preventDefault();
            return;
        }

        this.draggedColumn = this.findParentHeader(event.target);
        event.dataTransfer.setData('text', 'b'); // Firefox requires this to make dragging possible
        this.zone.runOutsideAngular(() => {
            window.document.addEventListener('dragover', this.onColumnDragover.bind(this));
        });
    }

    onColumnDragover(event) {
        const dropHeader = this.findParentHeader(event.target);
        if (this.reorderableColumns && this.draggedColumn && dropHeader) {
            event.preventDefault();
            const container = this.el.nativeElement.children[0];
            const containerOffset = this.domHandler.getOffset(container);
            const dropHeaderOffset = this.domHandler.getOffset(dropHeader);

            if (this.draggedColumn !== dropHeader) {
                const targetLeft = dropHeaderOffset.left - containerOffset.left;
                const targetTop = containerOffset.top - dropHeaderOffset.top;
                const columnCenter = dropHeaderOffset.left + dropHeader.offsetWidth / 2;

                this.reorderIndicatorUp.style.top = dropHeaderOffset.top - containerOffset.top - (this.iconHeight - 1) + 'px';
                this.reorderIndicatorDown.style.top = dropHeaderOffset.top - containerOffset.top + dropHeader.offsetHeight + 'px';

                if (event.pageX > columnCenter) {
                    this.reorderIndicatorUp.style.left = (targetLeft + dropHeader.offsetWidth - Math.ceil(this.iconWidth / 2)) + 'px';
                    this.reorderIndicatorDown.style.left = (targetLeft + dropHeader.offsetWidth - Math.ceil(this.iconWidth / 2)) + 'px';
                    this.dropPosition = 1;
                } else {
                    this.reorderIndicatorUp.style.left = (targetLeft - Math.ceil(this.iconWidth / 2)) + 'px';
                    this.reorderIndicatorDown.style.left = (targetLeft - Math.ceil(this.iconWidth / 2)) + 'px';
                    this.dropPosition = -1;
                }

                this.reorderIndicatorUp.style.display = 'block';
                this.reorderIndicatorDown.style.display = 'block';
            } else {
                event.dataTransfer.dropEffect = 'none';
            }
        }
    }

    onColumnDragleave(event) {
        if (this.reorderableColumns && this.draggedColumn) {
            event.preventDefault();
            this.reorderIndicatorUp.style.display = 'none';
            this.reorderIndicatorDown.style.display = 'none';
            window.document.removeEventListener('dragover', this.onColumnDragover);
        }
    }

    onColumnDrop(event) {
        event.preventDefault();
        if (this.draggedColumn) {
            const dragIndex = this.domHandler.index(this.draggedColumn);
            const dropIndex = this.domHandler.index(this.findParentHeader(event.target));

            const dragged = this.columns[dragIndex].field;
            const dropped = this.columns[dropIndex].field;
            if (dragged === 'firstColumn' || dropped === 'firstColumn') {
                this.reorderIndicatorUp.style.display = 'none';
                this.reorderIndicatorDown.style.display = 'none';
                this.draggedColumn.draggable = false;
                this.draggedColumn = null;
                this.dropPosition = null;
                return;
            }

            let allowDrop = (dragIndex !== dropIndex && dropIndex > 0
                && (dragIndex > 0 ||
                    (dragIndex === 0 && this.draggedColumn.draggable === true)));
            // let allowDrop = (dragIndex != dropIndex);
            if (allowDrop && ((dropIndex - dragIndex === 1 && this.dropPosition === -1)
                || (dragIndex - dropIndex === 1 && this.dropPosition === 1))) {
                allowDrop = false;
            }

            if (allowDrop || dropIndex === 0) {
                // this.columns.splice(dropIndex, 0, this.columns.splice(dragIndex, 1)[0]);
                this.objectUtils.reorderArray(this.columns, dragIndex, dropIndex);
                this.onColReorder.emit({
                    dragIndex: dragIndex,
                    dropIndex: dropIndex,
                    columns: this.columns
                });
            }
            // if (this.reorderIndicatorUp && this.reorderIndicatorUp.style && this.reorderIndicatorUp.style.display){
            this.reorderIndicatorUp.style.display = 'none';
            this.reorderIndicatorDown.style.display = 'none';
            // }
            this.draggedColumn.draggable = false;
            this.draggedColumn = null;
            this.dropPosition = null;
        }
    }

    initColumnReordering() {
        this.reorderIndicatorUp = this.domHandler.findSingle(this.el.nativeElement.children[0], 'span.ui-datatable-reorder-indicator-up');
        this.reorderIndicatorDown =
            this.domHandler.findSingle(this.el.nativeElement.children[0],
                'span.ui-datatable-reorder-indicator-down');
        this.iconWidth = this.domHandler.getHiddenElementOuterWidth(this.reorderIndicatorUp);
        this.iconHeight = this.domHandler.getHiddenElementOuterHeight(this.reorderIndicatorUp);
    }

    findParentHeader(element) {
        if (element.nodeName === 'TH') {
            return element;
        } else {
            let parent = element.parentElement;
            while (parent.nodeName !== 'TH') {
                parent = parent.parentElement;
                if (!parent) { break; }
            }
            return parent;
        }
    }

    hasFooter() {
        if (this.footerColumnGroup) {
            return true;
        } else {
            if (this.columns) {
                for (let i = 0; i < this.columns.length; i++) {
                    if (this.columns[i].footer || this.columns[i].footerTemplate) {
                        return true;
                    }
                }
            }

        }
        return false;
    }

    isEmpty() {
        return !this.dataToRender || (this.dataToRender.length === 0);
    }

    createLazyLoadMetadata(): LazyLoadEvent {
        // this.loading = true;
        return {
            first: this.first,
            rows: this.virtualScroll ? this.rows * 2 : this.rows,
            sortField: this.sortField,
            sortOrder: this.sortOrder,
            filters: this.filters,
            globalFilter: this.globalFilter ? this.globalFilter.value : null,
            multiSortMeta: this.multiSortMeta
        };
    }

    getCurrentMultiSortMetaValues() {
        return this.multiSortMeta;
    }

    setCurrentMultiSortMetaValues(multiSortMeta) {
        this.multiSortMeta = Object.assign([], multiSortMeta);
        this.handleDataChange();
    }

    toggleRow(row: any, event?: Event) {
        if (!this.expandedRows) {
            this.expandedRows = [];
        }

        const expandedRowIndex = this.findExpandedRowIndex(row);

        if (expandedRowIndex !== -1) {
            this.expandedRows.splice(expandedRowIndex, 1);
            this.onRowCollapse.emit({
                originalEvent: event,
                data: row
            });
        } else {
            if (this.rowExpandMode === 'single') {
                this.expandedRows = [];
            }

            this.expandedRows.push(row);
            this.onRowExpand.emit({
                originalEvent: event,
                data: row
            });
        }

        if (event) {
            event.preventDefault();
        }
    }

    findExpandedRowIndex(row: any): number {
        let index = -1;
        if (this.expandedRows) {
            for (let i = 0; i < this.expandedRows.length; i++) {
                if (this.expandedRows[i] === row) {
                    index = i;
                    break;
                }
            }
        }
        return index;
    }

    isRowExpanded(row: any): boolean {
        return this.findExpandedRowIndex(row) !== -1;
    }

    toggleFunction(obj) {
        obj.expand = !obj.expand;
        if (!obj.expand) {
            obj.subGroups = [];
        }
        this.isGroupDataUpdate = false;
        this.processGroup(obj, obj.level);
    }

    getTopGroup(selectedValue) {
        if (this.expandedRowsGroups) {
            return this.expandedRowsGroups[0];
        }
        return null;
    }

    emptyLeave(row) {
        let result = true
        // tslint:disable-next-line:forin
        for (const key in row) {
            if (this.tableKeyField) {
                if (key === this.tableKeyField) {
                    if (row[key] === 0 || !row[key] || row[key] === '' || row[key] == null || row[key] === undefined) {
                        return true;
                    }
                }
            }

            if (row[key] !== '' && row[key] != null && row[key] !== undefined) {
                result = false
                break
            }
        }
        return result;
    }

    findCollection(obj: any, level: any, selectedNode: string) {
        let result = null;
        if (obj instanceof Array) {
            for (let i = 0; i < obj.length; i++) {
                result = this.findCollection(obj[i], level, selectedNode);
                if (result) {
                    break;
                }
            }
        } else {
            // tslint:disable-next-line:forin
            for (const prop in obj) {
                if (prop === 'level') {
                    if (obj[prop] === level
                        && (obj['name'] ? (obj['name'].toString() === selectedNode)
                            : ((selectedNode === '' || selectedNode == null) ? true : false))) {
                        return obj;
                    }
                }
                if (obj[prop] instanceof Object || obj[prop] instanceof Array) {
                    result = this.findCollection(obj[prop], level, selectedNode);
                    if (result) {
                        break;
                    }
                }
            }
        }
        return result;
    }

    checkIfLeaveExist(grp, row) {
        let leaveExist = false;
        if (grp.leaves === undefined || grp.leaves === null) {
            grp.leaves = [];
        }
        for (let leave = 0; leave < grp.leaves.length; leave++) {
            if (JSON.stringify(grp.leaves[leave]) === JSON.stringify(row)) {
                leaveExist = true;
                break;
            }
        }
        return leaveExist;
    }
    addLeaveIfNotExist(grp, row) {
        if (!this.checkIfLeaveExist(grp, row)) {
            grp.leaves.push(row);
            if (grp.leaves.length > 1) {
                this.clearEmptyRow(grp);
            }
        }
    }

    addSubGroupIfNotExists(grp: any, row: any): void {
        if (grp.subGroups == null) {
            grp.subGroups = [];
        }
        const grouplevel = grp.level + 1;
        if (grouplevel < this.groupFieldArray.length) {
            const value2Compare = this.resolveFieldData(row, this.groupFieldArray[grouplevel]);
            let groupExist = false;
            for (let index = 0; index < grp.subGroups.length; index++) {
                if (grp.subGroups[index].name === value2Compare) {
                    groupExist = true;
                    this.addLeaveIfNotExist(grp.subGroups[index], row);
                    return;
                }
            }
            if (!groupExist) {
                // var value=this.resolveFieldData(row, this.groupFieldArray[grouplevel])
                grp.subGroups.push({ name: value2Compare, subGroups: null, level: grp.level + 1, leaves: [row], expand: false });
                if (grp.subGroups.length > 1) {
                    for (let i = 0; i < grp.subGroups.length; i++) {
                        // if (grp.subGroups[i].name === '' || grp.subGroups[i].name === null || grp.subGroups[i].name===undefined) {
                        if (grp.subGroups[i].name === undefined) {
                            grp.subGroups.splice(i, 1);
                            break;
                        }
                    }
                }
            }

        }
    }


    clearEmptyRow(group) {
        if (group.leaves) {
            if (group.leaves.length > 1) {
                for (let i = 0; i < group.leaves.length; i++) {
                    if (this.emptyLeave(group.leaves[i])) {
                        group.leaves.splice(i, 1);
                        // break;
                    }
                }

            }
        }
    }



    isRowGroupExpanded(rowdata: any, level: any, selectedNode: any): boolean {
        let expand = true;
        if (this.expandedGroupInfo && this.expandedGroupInfo.length > 0) {
            const element = this.expandedGroupInfo[0];
            expand = (rowdata[element.columnname] === element.value);
        } else {
            expand = false;
        }
        return expand;
    }

    resolveRowGroup(row, data, type) {
        data.forEach((item) => {
            if (item.name === row) {
                if (type === 'subGroups') {
                    return item.subGroups;
                } else if (type === 'level') {
                    return item.level + 1;
                }
            }
        });
    }

    toggleRowGroup(event: any, row: any, level: any, nextElementSiblingValue: any): void {
        this.isGroupDataUpdate = false;
        //  this.selectedNode = event.currentTarget.nextElementSibling.innerHtml.split(': ')[1];
        this.rowGroupToggleClick = true;
        const rowGroupField = nextElementSiblingValue; // this.resolveFieldData(row, this.groupFieldArray[level]);
        this.selectedNode = rowGroupField;
        let done = false;
        if (this.expandedRowsGroups) {
            if (this.expandedRowsGroups.length > 0) {
                if (this.expandedRowsGroups[0].name === rowGroupField) {
                    this.expandedRowsGroups = [];
                    this.expandedGroupInfo = [];
                    this._first = this.expandIndexCount * this.rows;
                    this.selectedNode = null;
                    this.onRowGroupCollapse.emit({
                        originalEvent: event,
                        group: rowGroupField,
                        row: row,
                        lazyLoadData: this.createLazyLoadMetadata()
                    });
                    done = true;
                }
            }

        }

        if (!done) {
            if (this.rowGroupExpandMode === 'single') {
                this.expandedRowsGroups = [];
                this.expandedGroupInfo = [];
            }
            this.expandedRowsGroups = this.expandedRowsGroups || [];
            // add this to maintain information on expandeded group info
            this.expandedGroupInfo.push({ columnname: this.groupFieldArray[level], value: rowGroupField });
            this.expandedRowsGroups.push({ name: rowGroupField, subGroups: null, leaves: [row], level: 0, expand: true });
            this.expandIndexCount = (this.first / this.rows);
            this.onRowGroupExpand.emit({
                label: this.groupFieldArray[level],
                originalEvent: event,
                group: rowGroupField,
                row: row,
                level: level,
                lazyLoadData: this.createLazyLoadMetadata()
            });
        }
        // }
        event.preventDefault();
    }

    processGroup(group: any, level: number) {
        this.expandedGroup = this.expandedGroup || {};
        this.expandedGroup['name'] = group.name;
        this.expandedGroup['level'] = group.level;
        if (group.expand) {
            this.expandIndexCount = (this.first / this.rows);
            if (this.expandedGroupInfo.length > level) {
                this.expandedGroupInfo.splice(level, (this.expandedGroupInfo.length - level));
            }
            this.expandedGroupInfo.push({ columnname: this.groupFieldArray[level], value: group.name });
            this.onNestedGroupExpand.emit({
                label: this.groupFieldArray[level],
                originalEvent: event,
                group: group.name,
                level: level,
                row: group.leaves[0],
                lazyLoadData: this.createLazyLoadMetadata()
            });
        } else {
            for (let index1 = 0; index1 < this.expandedGroupInfo.length; index1++) {
                const element1 = this.expandedGroupInfo[index1];
                if (element1.columnname === this.groupFieldArray[level] && element1.value === group.name) {
                    this.expandedGroupInfo.splice(index1, this.expandedGroupInfo.length - index1);
                    break;
                }
            }
            this.first = this.expandIndexCount * this.rows;
            this.onNestedGroupCollapse.emit({
                label: this.groupFieldArray[level],
                originalEvent: event,
                group: group.name,
                level: level,
                row: group.leaves[0],
                lazyLoadData: this.createLazyLoadMetadata()
            })
        }
    }

    public reset() {
        this.sortField = null;
        this.sortOrder = 1;

        this.filteredValue = null;
        this.filters = {};

        this._first = 0;
        this.firstChange.emit(this._first);
        this.updateTotalRecords();

        if (this.lazy) {
            this.onLazyLoad.emit(this.createLazyLoadMetadata());
        } else {
            this.updateDataToRender(this.value);
        }
    }

    public exportCSV() {
        const data = this.filteredValue || this.value;
        let csv = '\ufeff';

        // headers
        for (let i = 0; i < this.columns.length; i++) {
            if (this.columns[i].field) {
                csv += '"' + (this.columns[i].header || this.columns[i].field) + '"';

                if (i < (this.columns.length - 1)) {
                    csv += this.csvSeparator;
                }
            }
        }

        // body
        data.forEach((record, i) => {
            csv += '\n';
            for (let ii = 0; ii < this.columns.length; ii++) {
                if (this.columns[ii].field) {
                    csv += '"' + this.resolveFieldData(record, this.columns[ii].field) + '"';

                    if (ii < (this.columns.length - 1)) {
                        csv += this.csvSeparator;
                    }
                }
            }
        });

        const blob = new Blob([csv], {
            type: 'text/csv;charset=utf-8;'
        });

        if (window.navigator.msSaveOrOpenBlob) {
            navigator.msSaveOrOpenBlob(blob, this.exportFilename + '.csv');
        } else {
            const link = document.createElement('a');
            link.style.display = 'none';
            document.body.appendChild(link);
            if (link.download !== undefined) {
                link.setAttribute('href', URL.createObjectURL(blob));
                link.setAttribute('download', this.exportFilename + '.csv');
                link.click();
            } else {
                csv = 'data:text/csv;charset=utf-8,' + csv;
                window.open(encodeURI(csv));
            }
            document.body.removeChild(link);
        }
    }

    getBlockableElement(): HTMLElement {
        return this.el.nativeElement.children[0];
    }

    getRowStyleClass(rowData: any, rowIndex: number) {
        let styleClass = 'ui-widget-content';
        if (this.rowStyleClass) {
            const rowClass = this.rowStyleClass.call(this, rowData, rowIndex);
            if (rowClass) {
                styleClass += ' ' + rowClass;
            }
        } else if (this.rowStyleMap && this.dataKey) {
            const rowClass = this.rowStyleMap[rowData[this.dataKey]];
            if (rowClass) {
                styleClass += ' ' + rowClass;
            }
        }

        return styleClass;
    }

    visibleColumns() {
        return this.columns ? this.columns.filter(c => !c.hidden) : [];
    }

    get containerWidth() {
        if (this.scrollable) {
            if (this.scrollWidth) {
                return this.scrollWidth;
            } else if (this.frozenWidth && this.unfrozenWidth) {
                return parseFloat(this.frozenWidth) + parseFloat(this.unfrozenWidth) + 'px';
            }
        } else {
            return this.style ? this.style.width : null;
        }
    }

    hasFrozenColumns() {
        return this.frozenColumns && this.frozenColumns.length > 0;
    }

    ngOnDestroy() {
        // remove event listener
        if (this.globalFilterFunction) {
            this.globalFilterFunction();
        }

        if (this.resizableColumns) {
            this.unbindColumnResizeEvents();
        }

        this.unbindDocumentEditListener();

        if (this.columnsSubscription) {
            this.columnsSubscription.unsubscribe();
        }
    }

    getColumnStyleClass(rowData: any, rowIndex: number, columnName) {
        let columnClass = '';

        if (this.columnStyleClass !== undefined && this.columnStyleClass) {
            columnClass = this.columnStyleClass.call(this, rowData, rowIndex, columnName);
        }

        return columnClass;
    }
}

@NgModule({
    imports: [CommonModule, ButtonModule, SharedModule, PaginatorModule, FormsModule, DialogModule, AddEditFormModule],
    exports: [DataTable, SharedModule],
    // tslint:disable-next-line:max-line-length
    declarations: [DataTable, SortGridPipe, NestedGroup, DTRadioButton, DTCheckbox, ColumnHeaders, ColumnFooters, TableBody, ScrollableView, RowExpansionLoader]
})
export class DataTableModule { }
