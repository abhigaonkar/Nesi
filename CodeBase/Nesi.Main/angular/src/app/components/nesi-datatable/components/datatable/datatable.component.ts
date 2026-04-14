import { FILTER_CONSTANTS, FilterRoot, FILTER_OPERATORS } from './../../../../models/component/filterBuilder/filterBuilder';
import { FilterRow, ComplexDateTimeObject, COMPLEXDATE_CONSTANT } from 'app/models/component/filterBuilder/filterBuilder';
import { BannerService } from './../../../../services/layout/banner.service';
import {
  ChangeDetectorRef,
  Input,
  Component,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
  ViewChildren,
  QueryList,
  HostListener,
  Inject,
  OnDestroy
} from '@angular/core';
import { DataService } from '../../service/dataservice';
import { OverlayPanel, InputTextareaModule, Column } from 'primeng/primeng';
import { LocalDbService } from '../../service/localDbService';
import { UtilityService } from '../../service/utilityService';
import { DomHandler } from 'primeng/primeng';
import { LazyLoadEvent } from 'primeng/primeng';
import { HttpService } from 'app/core/http.service';
import { CONFIG } from 'app/configuration';
import { ReportId } from './report-id.enum';
import { ActivatedRoute } from '@angular/router';
import { DataTable } from 'app/components/nesi-datatable/components/primeng-custom-library/datatable/datatable';
import * as fromMessage from '../../../../actions/layout/growlMessage';
import * as fromRoot from '../../../../reducers';
import { Store } from '@ngrx/store';
import { TokenService } from 'app/services/authentication/tokenService';
import * as fromCurrentUser from '../../../../actions/layout/currentUser';
import { LayoutDatatableComponent } from 'app/components/nesi-datatable/components/layout-datatable/layout-datatable.component';
import { Observable } from 'rxjs/Observable';
import { LayoutService } from 'app/components/nesi-datatable/service/layoutService';
import { validateCharacters } from '../../../shared/custom-validators/custom.validator';
import { FormControl } from '@angular/forms';
import { DatePipe, CurrencyPipe, PercentPipe, DecimalPipe } from '@angular/common';
import { WindowRef } from 'app/services/shared/windowRef';
import { PhoneFormatPipe } from 'app/pipes/phoneFormat.pipe';
import { DateControl } from 'app/components/nesi-datatable/components/date-control/date.control.component';
import { SaveFailLayoutProfile } from 'app/actions/layout/layoutPorfile';
import { MultiSelect } from 'app/components/nesi-datatable/components/primeng-custom-library/multiselect/multiselect';
import { TOGGLE_ACTION } from '@ngrx/store-devtools/src/actions';
import { IndexDirective } from '../directives/index.directive';
import { ConfirmationService } from 'primeng/components/common/api';
import { CoreService } from 'app/services/shared/core.service';
import { LabelValueInt } from '../../../../models/Shared/labelValueString';
import { AuthorizeService } from '../../../../services/authentication/authorize.Service';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Subscription } from 'rxjs';
import { MenuService } from '../../../../services/layout/menuService';
import { MessageBase } from 'app/core/messageBaseComponent';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-datatable',
  templateUrl: './datatable.component.html'
})
export class DatatableComponent extends MessageBase implements OnInit, OnDestroy {


  /************** Private Variables *******************/
  private hidden = false;
  private alternateMemberId: number;
  public filterType: any[] = [];
  public paginatorFlag = true;
  private draggedColumn = '';
  private swapStart = '';
  private localDBKey: any = {};
  private formOptType: any = 0;
  private schemaFields: any = {};
  private deleteLayoutIndexId: number;
  private memberId: any;
  private user: any = {};
  private layoutOptionShowHide: boolean;
  private errorList: any = {};
  private isDefaultLayoutActivated = false;
  private editedRows: any[];
  private backupOfEditRow: any[] = [];
  private expandedRowGroups: any = [];
  private filterKeyValueHash = {};
  private currentPageRowCount: number;
  private dtViewChild: any = {};
  private isNewLayout = false;
  public processing = false;
  private invalidRowsColumns: Map<string, boolean> = new Map();
  private reportFor = '';
  private unselectedRow: any[];
  private highlightedColumns: any[] = [];
  private sessionKey = 'updateDataSet_running';

  private filterString = '';
  public filterBuilder: FilterRoot = new FilterRoot();
  public lastSavedFilterRoot: FilterRoot = new FilterRoot();
  public currentFilterRoot: FilterRoot = new FilterRoot();
  private childRootDepth = 0;

  private firstLoad = false;
  private layoutApplied = false;

  /*****************Public Variables *****************/
  public layouts: any = [];
  public reportSchema: any;
  public loadLayouts = true;
  public groupArray: any[] = [];
  public display = true;
  public multiSortMeta: any[] = [];
  public columnArray: any[];
  public columnArrayForColumnsSelection: any[];
  public columnOptions: any[];
  public editDisplayDialog = false;
  public rowData: Object = {};
  public filterData: any = {};

  private _selectedValue: any[] = [];
  public get selectedValue(): any[] {
    this.calculateSelectedValue();
    return this._selectedValue;
  };

  public set selectedValue(value: any[]) {
    this._selectedValue = value;
  }

  public apply_fixed_header = false;
  public filterMatchModeString: any[];
  public filterMatchModeStringMulitple: any[];
  public filterMatchModeNumber: any[];
  public filterMatchModeDate: any[];
  public filterMatchModeBooelan: any[];
  public selectedItemIndex: number;
  public displayDialogAdd = false;
  public objectKeys = Object.keys;
  public filterObj: any = {};
  public filterBackup: any = {};
  public displayFilter = false;
  public filterBuilderObj: any = {};
  public confirmDisplayDialog = false;
  public confirmDisplayDialogLabel = '';
  public confirmDisplayDialogBtnLabel = '';
  public checked = true;
  public selectedLayout: any = {};
  public bulkEnable: boolean;
  public gridId: any;
  public fieldLabelHashTable: any = {};
  public genericDataSet: any[];
  public paginationConfig: any = {
    rows: 25,
    pageLinks: 6,
    rowPerPageOptions: [10, 25, 50, 100, 200]
  };
  public genericApiObject: any = {};
  public newReport = false;
  public summaryConfig: Object = {};
  public totalRecords: number;
  public lazy = false;
  public tableEditable: boolean;
  public distinctLoader = false;
  public tableKeyField: any;
  public baseCoulmnArray: any = [];
  public reportTitle: any;
  public isDragActive = -2;
  public selectedLayoutIndex = 0;
  public primeLoader: boolean;
  public booleanFilterValues: any[] = [];
  public defaultPageSettingMessage = '(Make this my default page)';
  public summary: Object = {};
  public groupSummary: any = {};
  public isInlineEditingValid = true;
  public groupMultiselect: any = [];
  public multiSelectFilter: any = [];
  public hasSummary = false;
  public filterClosable: boolean;
  
  public sortField: string;
  public sortOrder: number;
  public defaultNote = '';
  private saveButtonClicked = false;
  private old_scroll_position = 0;

  public groupSummaryTable: any[];
  private savedSelectionMode = '';

  private currentPostObject: any;
  private localfilters: { [s: string]: {}; };

  private no_copy_fields = ['name', 'type', 'rawType', 'editable', 'header',
    'isKey', 'hyperlink', 'hyperlinkUrl', 'hyperlinkUrlParam', 'mobileHyperlinkUrl', 'displayFormat', 'currency',
    'format'
  ];

  public filterOperators: Array<any> = FILTER_OPERATORS.slice(0, 4);
  public otherFilterObjects = [];
  public otherFilterConditions = [];
  public currentOtherFilterCondition_index = -1;
  public otherFilterNewCondtion = 'And';
  public visible_business_unit_list: any[];
  public selectedBusinessUnits: any[];
  public selectedBusinessUnitIds: number[] = [];

  private _selectedItems: Array<any> = [];

  public currentPageRowFromOnPageEvent = 0;
  public sortObject = { order: 1, field: null, multisortmeta: [] };

  public currentColumnWidths = [];
  public initialiazedOnClumnWidth = false;

  @ViewChild('opFilter1') filterOperatorOverlay: OverlayPanel;

  /*************Input Properties ***************/
  // tslint:disable-next-line:no-input-rename
  @Input('id') pageId: number;
  // tslint:disable-next-line:no-input-rename
  @Input('gridName') gridName: any;
  // tslint:disable-next-line:no-input-rename
  @Input('gridTitle') gridTitle: any;
  // tslint:disable-next-line:no-input-rename
  @Input('reportQueryParam') reportQueryParam: any = [];
  // tslint:disable-next-line:no-input-rename
  @Input('refreshCache') refreshCache: boolean;
  @Input() dropdownOptions: any;
  @Input() newNotesConfig: any;
  @Input() spinnerConfig: any;
  @Input() selectionMode = 'single';
  @Input() disableAddbtn: boolean;

  /**************Output Properties ************/
  @Output() onSort: EventEmitter<any> = new EventEmitter();
  @Output() onrowclick = new EventEmitter();
  @Output() oncellclick = new EventEmitter();
  @Output() onTextBoxChange = new EventEmitter();
  @Output() onDropDownChange = new EventEmitter();
  @Output() onCreate = new EventEmitter();
  @Output() onEdit = new EventEmitter();
  @Output() onDelete = new EventEmitter();
  @Output() onSave = new EventEmitter();
  @Output() onRefresh = new EventEmitter();
  @Output() onSeachFinish = new EventEmitter();

  @ViewChild('dp') custom_dt: ElementRef;
  @ViewChild('dt') default_dt: ElementRef;
  @ViewChild(DataTable) dataTable: DataTable;
  @ViewChild('gb') gb: ElementRef;
  @ViewChild(DataTable) customDataTable: DataTable;
  @ViewChild('op4') overlayPanel: OverlayPanel;

  @ViewChildren(DateControl) dateControlElem: QueryList<DateControl>;

  @ViewChild('appLayout') appLayout: LayoutDatatableComponent;


  @ViewChild('multiSelect') multiSelect: MultiSelect;

  @ViewChildren(IndexDirective) indexes: Array<IndexDirective>;

  @Input() public callback: Function;
  public _callbackForRow: Function;
  public fixed_header = false;

  public current_layout_visible_business_string: string;

  @Input() public fixed_header_scroll = 180;
  @Input() public visible_business_type = 'N';

  @Input() public need_query_param = false;

  @Input()
  set callbackForRow(value: Function) {
    this._callbackForRow = value;
  }


  public passed_params_apply = false;
  public passed_filterObject: any;
  public passed_business_unit_string: string;

  private oneTime = false;
  private oneTimeForGroup = false;

  fixed_header_scroll_x = 0;

  constructor(private dataService: DataService,
    private cd: ChangeDetectorRef,
    private localDbService: LocalDbService,
    private utilityService: UtilityService,
    private domHandler: DomHandler,
    public el: ElementRef,
    private httpService: HttpService,
    private route: ActivatedRoute,
    public bs: BannerService,
    private layoutService: LayoutService,
    protected store: Store<fromRoot.State>,
    public tokenService: TokenService,
    private datePipe: DatePipe,
    private currencyPipe: CurrencyPipe,
    private percentPipe: PercentPipe,
    private numberPipe: DecimalPipe,
    private phoneFormatPipe: PhoneFormatPipe,
    private winRef: WindowRef,
    public cs: CoreService,
    private as: AuthorizeService,
    private ms: MenuService,
    private confirmationService: ConfirmationService) {
    super(store);
    this.layoutOptionShowHide = true;
    this.groupSummaryTable = [];
    this.callback = this.defaultCallback;
    this._callbackForRow = this.setRowClass;
  }


  ngOnDestroy(): void {
  }

  selectBusinessUnitsChanged() {
    this.selectedBusinessUnitIds = this.selectedBusinessUnits.filter(x => x.display).map(x => x.field);
    if (!this.selectedBusinessUnitIds || this.selectedBusinessUnitIds.length === 0) {
      this.setDefaultSelectedVisibleBusinessUnit();
    }
    this.loadReport();
  }

  get visible_business_string() {
    this.selectedBusinessUnitIds = this.selectedBusinessUnits.filter(x => x.display).map(x => x.field);
    if ((!this.selectedBusinessUnitIds || this.selectedBusinessUnitIds.length === 0)
      && this.current_layout_visible_business_string) {
      return this.current_layout_visible_business_string;
    }
    return '[' + this.visible_business_type + ']' + this.selectedBusinessUnitIds.join(',');
  }


  /**
   *Updates multisortmeta object on sorting
   * @param  {} data
   */
  updateMultiSortMeta(data) {

    this.multiSortMeta = data.multisortmeta;
  }

  /**
   * Construct multisortmeta object on basis of groupArray
   * @param  {} groupArray
   */
  constructMultiSortMetaArray(groupArray) {
    const multiArraySort = [];
    this.groupArray.forEach((eachObj) => {
      multiArraySort.push({ field: eachObj, order: 1 })
    });
    return multiArraySort;
  }

  isArray(obj) {
    return Array.isArray(obj);
  }

  changeShowStatus() {
    this.layoutOptionShowHide = !this.layoutOptionShowHide;
  }

  public getVisibleBusinessUnitList() {
    this.as.validateToken().subscribe(
      res => {
        this.tokenService.visible_business_unit_list = this.visible_business_unit_list;
        this.visible_business_unit_list = this.tokenService.visible_business_unit_list;
        this.setDefaultSelectedVisibleBusinessUnit();
      }
    )
  }


  public setDefaultSelectedVisibleBusinessUnit(liststring = null) {
    this.selectedBusinessUnits = this.visible_business_unit_list
      .map(x => x.value);
    if (!liststring) {
      this.current_layout_visible_business_string = this.tokenService.currentUser.businessUnitId.toString();
      this.selectedBusinessUnits.forEach(x => x.display = (x.field === this.tokenService.currentUser.businessUnitId));
    } else {
      this.current_layout_visible_business_string = liststring;
      const list = liststring.substr(3).split(',');
      this.selectedBusinessUnits.forEach(x => x.display = (list.includes(String(x.field))));
    }
  }
  ngOnInit() {
    const userId = this['tokenService']['currentUser']['id'];
    const userName = this['tokenService']['currentUser']['fullName']
    this.user[userId] = userName;
    this.memberId = userId;
    this.bulkEnable = false;

    this.ms.subjectMenuMode.subscribe(x => {
      this.update_fixed_header_scroll_x();
    });

    if (this.tokenService.visible_business_unit_list && this.tokenService.visible_business_unit_list.length > 0) {
      this.visible_business_unit_list = this.tokenService.visible_business_unit_list;
      this.setDefaultSelectedVisibleBusinessUnit();
    } else {
      this.getVisibleBusinessUnitList();
    }
    this.filterMatchModeString = [];
    this.filterMatchModeStringMulitple = [];
    this.filterMatchModeNumber = [];
    this.filterMatchModeDate = [];
    this.filterMatchModeBooelan = [];

    /*********Filter options for number type column ***********/
    this.filterMatchModeNumber.push({ label: 'Equals', value: 'equals' });
    this.filterMatchModeNumber.push({ label: 'Doesn\'t equal', value: 'notEquals' });
    this.filterMatchModeNumber.push({ label: 'Is less than', value: 'lessThan' });
    this.filterMatchModeNumber.push({ label: 'Is less than or equal to', value: 'lessThanOrEqual' });
    this.filterMatchModeNumber.push({ label: 'Is greater than', value: 'greaterThan' });
    this.filterMatchModeNumber.push({ label: 'Is greater than or equal to', value: 'greaterThanOrEqual' });
    // this.filterMatchModeNumber.push({ label: 'Is blank', value: 'isBlank' });
    // this.filterMatchModeNumber.push({ label: 'Is not blank', value: 'isNotBlank' });

    /*********Filter options for string type column ***********/
    this.filterMatchModeString.push({ label: 'Begins with', value: 'startsWith' });
    this.filterMatchModeString.push({ label: 'Contains', value: 'contains' });
    this.filterMatchModeString.push({ label: 'Doesn\'t contain', value: 'notContains' });
    this.filterMatchModeString.push({ label: 'Ends with', value: 'endsWith' });
    this.filterMatchModeString.push({ label: 'Equals', value: 'equals' });
    this.filterMatchModeString.push({ label: 'Doesn\'t equal', value: 'notEquals' });
    this.filterMatchModeString.push({ label: 'Is blank', value: 'isBlank' });
    this.filterMatchModeString.push({ label: 'Is not blank', value: 'isNotBlank' });


    /*********Filter options for string type column and mulitple selected ***********/
    this.filterMatchModeStringMulitple.push({ label: 'Is any of', value: 'in' });
    this.filterMatchModeStringMulitple.push({ label: 'Is none of', value: 'not in' });

    /*********Dropdown options for boolean type column ***********/
    this.booleanFilterValues.push({ label: 'All', value: '-1' });
    this.booleanFilterValues.push({ label: 'Checked', value: '1' });
    this.booleanFilterValues.push({ label: 'Unchecked', value: '0' });

    this.filterMatchModeBooelan.push({ label: 'Equals', value: 'equals' });

    /*********Dropdown options for date type column ***********/
    this.filterMatchModeDate.push({ label: 'Equals', showPlaceholder: true, isAlias: false, value: 'equals' });
    this.filterMatchModeDate.push({ label: 'Doesn\'t equal', showPlaceholder: true, isAlias: false, value: 'notEquals' });
    this.filterMatchModeDate.push({ label: 'Is less than', showPlaceholder: true, isAlias: false, value: 'lessThan' });
    this.filterMatchModeDate.push({ label: 'Is less than or equal to', showPlaceholder: true, isAlias: false, value: 'lessThanOrEqual' });
    this.filterMatchModeDate.push({ label: 'Is greater than', showPlaceholder: true, isAlias: false, value: 'greaterThan' });
    // tslint:disable-next-line:max-line-length
    this.filterMatchModeDate.push({ label: 'Is greater than or equal to', showPlaceholder: true, isAlias: false, value: 'greaterThanOrEqual' });
    this.filterMatchModeDate.push({ label: 'Is blank', showPlaceholder: true, isAlias: false, value: 'isBlank' });
    this.filterMatchModeDate.push({ label: 'Is not blank', showPlaceholder: true, isAlias: false, value: 'isNotBlank' });

    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is Yesterday', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual LocalDateTimeYesterday^lessThan LocalDateTimeToday||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is Today', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual LocalDateTimeToday^lessThan LocalDateTimeTomorrow||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is Tomorrow', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual LocalDateTimeTomorrow^lessThan LocalDateTimeDayAfterTomorrow||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is Last Week', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual LocalDateTimeLastWeek^lessThan LocalDateTimeThisWeek||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is This Week', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual LocalDateTimeThisWeek^lessThan LocalDateTimeNextWeek||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is Next Week', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual LocalDateTimeNextWeek^lessThan LocalDateTimeTwoWeeksAway||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is Next Year', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual LocalDateTimeNextYear^lessThan AddYears(LocalDateTimeNextYear,1)||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is This Year', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual LocalDateTimeThisYear^lessThan LocalDateTimeNextYear||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is Last Year', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual AddYears(LocalDateTimeThisYear,-1)^lessThan LocalDateTimeThisYear||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is Next Month', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual LocalDateTimeNextMonth^lessThan AddMonths(LocalDateTimeNextMonth,1)||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is This Month', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual LocalDateTimeThisMonth^lessThan LocalDateTimeNextMonth||complexDateTime`' });
    // tslint:disable-next-line:max-line-length
    // this.filterMatchModeDate.push({ label: 'Is Last Month', showPlaceholder: false, isAlias: true, value: '||greaterThanOrEqual AddMonths(LocalDateTimeThisMonth,-1)^lessThan LocalDateTimeThisMonth||complexDateTime`' });
    this.sessionKey = 'updating_dataset_' + this.gridName;
    sessionStorage.setItem(this.sessionKey, '0');
    this.showReport(this.gridName);


  }


  confirm(resolve) {


    this.confirmationService.confirm({
      message: 'Are you sure you want to perform this action? All unsaved grid data will be lost.',
      accept: () => {
        this.bulkEnable = false;
        resolve(1);
      },
      reject: () => {

        resolve(0);

      }
    });
  }

  getColumnFromColumnArray(field: String) {
    let column;
    if (!this.columnArray) {
      return;
    }
    for (let i = 1; i < this.columnArray.length; i++) {
      if (this.columnArray[i].field === field) {
        column = this.columnArray[i];
        column.index = i;
        break;
      }
    }
    return column;
  }

  calculateSelectedValue() {
    this.columnArray.forEach((col) => {
      const c = this.getColumnFromColumnArray(col.field);
      if (c && c.index > -1) {
        let r = null;
        const o = this.filterBuilder.filterRows.filter(row => row.key === col.field);
        if (o) {
          if (o.length === 1) {
            r = o[0].value;
          } else {
            r = '';
            o.forEach((x, i, array) => {
              if (array[0].type === 'calendar' || array[0].type === 'date') {
                r = x.value;
                return;
              }
              r += x.value;

              if (i < array.length - 1) {
                r += ',';
              }
            });
          }
        }
        this._selectedValue[c.index] = r;
      }
    });

    // this.filterBuilder.filterRows.forEach((row) => {
    //   const c = this.getColumnFromColumnArray(row.key);
    //   if (c && c.index > -1) {
    //     this._selectedValue[c.index] = row.value;
    //   }
    // });
  }

  createHashTable(schema) {
    if (schema && schema.length > 0) {
      schema.forEach((objData) => {
        if (objData && objData.name) {
          this.schemaFields[objData.name] = objData;
          this.fieldLabelHashTable[objData.header.toLowerCase().trim()] = {
            field: objData.name,
            header: objData.header
          };
        }
      });
    }
  }

  /**
   * Construct column array on basis of column schema && Loads default layout on grid
   * @param  {} schema
   */
  updateToggleColumn(schema) {
    // create column array from schema in 1st API response

    const modifiledColumns = this.layoutService.setColumnProperties(schema);

    // if (modifiledColumns && modifiledColumns.columnArray && !this.showEditColumn) {
    //   modifiledColumns.columnArray = modifiledColumns.columnArray.filter(x => !x.type || x.type !== 'edit');
    //   modifiledColumns.columnOptions = modifiledColumns.columnOptions.filter(x => !x.value || !x.value.type || x.value.type !== 'edit');
    // }
    this.columnArray = modifiledColumns.columnArray;

    const first = this.columnArray && this.columnArray.find(x => x.first);
    if (first) {
      first.display = this.genericApiObject.edit || this.genericApiObject.delete || this.genericApiObject.viewInvoiceUrl;
      first.display = !!first.display;
    }

    this.columnOptions = modifiledColumns.columnOptions;
    this.summaryConfig = modifiledColumns.summaryConfig;
    this.baseCoulmnArray = this.columnArray;
    this.selectedLayout = this.layouts.find(x => x.isDefault);
    if (!this.selectedLayout) {
      this.selectedLayout = this.layouts[0];
      if (this.layouts && this.layouts.length === 1) {
        this.firstLoad = true;
      }
    }

    this.applyLayout();
  }

  /**
   * Set type of columns inside columnArray
   * @param  {} value
   */
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
    } else if (value.toLowerCase().indexOf('email') !== -1) {
      return 'email';
    } else if (value.toLowerCase().indexOf('percent') !== -1) {
      return 'percent';
    } else {
      return 'number';
    }
  }

  onDragEnter(data) {
    // tslint:disable-next-line:radix
    this.isDragActive = data.target.id ? parseInt(data.target.id) : -2;
  }

  onDragLeave(data) {
    this.isDragActive = -2;
  }

  log(x) {
    console.log(x);
  }

  /**
   * Construct multiselect filter with distinct values of a column
   * @param  {} event
   * @param  {string} columnName
   * @param  {OverlayPanel} overlaypanel
   */
  getFilterData(event, columnName, overlaypanel: OverlayPanel) {
    const column = this.getColumnFromColumnArray(columnName);
    if (!overlaypanel.visible && column.type !== 'date') {
      this.distinctLoader = true;
      const postObject = {};
      this.filterData = {};
      this.filterData[column.field] = [];
      const dataURL = this.genericApiObject['distinct'];
      postObject['pagesize'] = 500000;
      postObject['pagecount'] = 1;
      postObject['selectcolumn'] = column.field;
      postObject['bu_ids'] = this.visible_business_string;
      postObject['queryparam'] = this.reportQueryParam;

      if (this.checked) {
        this.setPostObjectFilter(postObject, columnName);
      }
      this.setPostObjectColumns(postObject);

      //  postObject['queryparam'] = this.reportQueryParam;
      // console.dir(postObject);
      this.dataService.getData(dataURL, postObject).subscribe(filterData => {
        filterData = filterData.map((current, index, arr) => {
          let label;
          if (column.type === 'percent') {
            label = current ? this.percentPipe.transform(current, column.format.format) : current;
          } else if (column.type === 'date') {
            label = current ? this.datePipe.transform(current, column.format.format) : current;
          } else if (column.type === 'currency') {
            // tslint:disable-next-line:radix
            label = current || parseInt(current) === 0 ?
              this.currencyPipe.transform(current, column.format.currency, 'symbol-narrow', column.format.format)
              : current;
          } else {
            label = current;
          }

          return current = {
            label: label,
            value: current
          }
        })
        this.filterData[column.field] = filterData;
        if (this.multiSelectFilter && this.multiSelectFilter[column.index] && this.multiSelectFilter[column.index].length > 0) {
          const newFilters = [];
          if (Array.isArray(this.multiSelectFilter[column.index])) { // A
            for (const x of this.multiSelectFilter[column.index]) { // B
              if (Array.isArray(x)) {
                // X is array object
                for (const e of x) {
                  const f = filterData.find(y => y.value.toLowerCase() === e.toLowerCase());
                  if (f) {
                    newFilters.push(f.value);
                  }
                }
              } else {
                // X is not array object.
                const f = filterData.find(y => y.value.toLowerCase() === x.toLowerCase());
                if (f) {
                  newFilters.push(f.value);
                }
              }
            } // B
            this.multiSelectFilter[column.index] = newFilters;
          } // A
        }

        this.distinctLoader = false;

        if (!filterData || filterData.length === 0) {
          this.store.dispatch(new fromMessage.PushInfoMessage(`No Data`)); // for ${column.header}.
        }

        // this.multiSelectFilter[column.index] = this.filterObj[column.index];
      },
        (err) => {

        });
    } else {
      this.filterData[column.field] = [{}];
      //  this.calculateSelectedValue();
    }
  }

  /**
   * Determines class of sort icon (Only for columns in droppable div)
   * @param  {} col
   */
  getFlag(col) {
    if (!this.multiSortMeta) {
      return;
    }
    for (let i = 0; i < this.multiSortMeta.length; i++) {
      if (this.multiSortMeta[i].field === col) {
        return this.multiSortMeta[i].order;
      }
    }
  }

  /**
   * Change sorting (Only for columns in droppable div)
   * @param  {} col
   */
  changeSort(col) {
    this.selectedValue = [];
    if (!this.multiSortMeta) {
      return;
    }
    for (let i = 0; i < this.multiSortMeta.length; i++) {
      if (this.multiSortMeta[i].field === col) {
        const previousSort = this.multiSortMeta[i].order;
        let newSort: number;
        if (previousSort === 1) {
          newSort = -1;
        } else if (previousSort === -1) {
          newSort = 1;
        }
        this.multiSortMeta.splice(i, 1, { field: col, order: newSort });
        this.multiSortMeta = this.multiSortMeta.slice();
        this.cd.detectChanges();
      }
    }
  }

  /**
   * Get field value of column dragged for grouping from column array
   * @param  {} targetChildren
   */
  getDraggedColumn(targetChildren) {
    if (!this.columnArray) {
      return;
    }
    for (let i = 0; i < this.columnArray.length; i++) {
      if ((this.columnArray[i].label ?
        (this.columnArray[i].label.toLowerCase().trim() === targetChildren) : false)
        && this.columnArray[i].display) {
        return this.columnArray[i].field;
      }
    }
  }

  // getSummary(filter, globalFilter, callback) {
  //   // // get summary for required columns
  //   // this.primeLoader = true;
  //   // let filterURL = '';
  //   // let globalFilterURL = '';
  //   // if (filter && (Object.keys(filter).length > 0)) {
  //   //   filterURL = "&filtercolumn="
  //   //   for (let key in filter) {
  //   //     filterURL = filterURL + key + ':' +
  //   // filter[key]['value'] + ':' + (filter[key]['matchMode']
  //   // ? filter[key]['matchMode'] : (this.schemaFields[key]
  //   // && this.schemaFields[key].type == 'System.Int32') ?
  //   // 'equals' : 'startsWith') + (Object.keys(filter).indexOf(key) == Object.keys(filter).length - 1 ? '' : ',');
  //   //   }
  //   // }
  //   // if (globalFilter && globalFilter != '') {
  //   //   globalFilterURL = "&globalfilter=" + globalFilter;
  //   // }
  //   // let summaryURL = '?summary=';
  //   // for (let key in this.summaryConfig) {
  //   //   summaryURL = summaryURL + key + ':' + this.summaryConfig[key];
  //   // }
  //   // let dataURL = this.genericApiObject['search'] + summaryURL + filterURL + globalFilterURL;
  //   // let visibleColumns = [];
  //   // for (let i = 1; i < this.columnArray.length; i++) {
  //   //   if (this.columnArray[i].display) {
  //   //     visibleColumns.push(this.columnArray[i].field);
  //   //   }
  //   // }
  //   // this.dataService.getData(dataURL, visibleColumns).subscribe((data) => {
  //   //   callback(data);
  //   // })

  //   //
  //   // 'this.primeLoader = false' will make the blocked UI available to touch if the summary schema in there.
  //   //

  //   this.primeLoader = false;
  // }

  onRowClick(event) {
    this.onrowclick.emit(event);
  }

  allowDrop(event) {
    event.preventDefault();
  }

  drop(event) {
    if (this.customDataTable) {
      this.draggedColumn = this.getDraggedColumn(
        this.customDataTable.draggedColumn.getElementsByTagName('span')[1].innerText.toLowerCase().trim());
    }
    this.primeLoader = true;
    this.genericDataSet = [];
    this.expandedRowGroups = [];
    if (this.groupArray && this.groupArray.length === 1) {
      this.bulkEditCancel();
    }
    this.isDragActive = -2;
    this.bulkEnable = false;
    if (this.groupArray.indexOf(this.draggedColumn) === -1) {
      this.groupMultiselect = [];
      this.multiSelectFilter = [];
      this.newReport = false;
      this.newReport = true;
      this.filterObj = {};
      this.selectedValue = [];
      this.display = false;
      this.primeLoader = true;
      this.groupArray.push(this.draggedColumn);
      this.multiSortMeta = this.constructMultiSortMetaArray(this.groupArray);
      if (this.sortObject.multisortmeta) {
        const list: Array<any> = (<any>(this.sortObject)).multisortmeta;
        list.forEach(item => {
          let found = false;
          for (let i = 0; i < this.multiSortMeta.length; i++) {
            if (this.multiSortMeta[i].field === item.field) {
              found = true;
              break;
            }
          }
          if (!found) {
            this.multiSortMeta.push(item);
          }
        });
      }

      this.localDbService.setItemIntoLocalDb(this.dataService.getUrlParam(this.localDBKey.groupArray), this.groupArray, 'json');
      this.draggedColumn = '';
      setTimeout(() => {
        this.display = true;
        this.cd.detectChanges();
        this.fixColumnWidthOnCurrentLayout(this.getDefaultScreenWidth());
      }, 300);
      this.cd.detectChanges();
    }
  }


  viewReady(event) {
  }


  setPostObjectFilter(postObject, distinctColumn = null) {
    let filterKey = '';

    let fromFilterDialog = false;
    if (this.filterObj && (Object.keys(this.filterObj) && Object.keys(this.filterObj).length > 0)) {
      // matchModes are not defined at back end currently.

      fromFilterDialog = (Object.keys(this.filterObj)[0] === FILTER_CONSTANTS.FROM_FILTER_DIALOG);
      if (!fromFilterDialog) {

        if (this.filterBuilder.level === 1) {
          // this.level1FilterRows = this.filterBuilder.filterRows;
          // this.filterBuilder.complexDateTimeList=[];
          const children = this.filterBuilder;
          const complexDateTimeList = this.filterBuilder.complexDateTimeList;
          this.filterBuilder = new FilterRoot();
          // this.filterBuilder.filterRows=[];
          this.filterBuilder.filterOperator = 'And';
          this.filterBuilder.children.push(children);
          this.filterBuilder.complexDateTimeList = complexDateTimeList;
        }
        if (!this.filterBuilder.level) {

          const filterDialogMadeFields = this.filterBuilder.filterRows.filter((row) => {
            // tslint:disable-next-line:max-line-length
            return row.rowCreationSource === FILTER_CONSTANTS.FROM_FILTER_DIALOG && row.type !== 'dropdownDateTime' && row.type !== 'calendar';
          });


          this.filterBuilder.filterRows = [];
          if (filterDialogMadeFields && filterDialogMadeFields.length > 0) {
            this.filterBuilder.filterRows = this.filterBuilder.filterRows.concat(filterDialogMadeFields);
          }
        }

        if (distinctColumn && this.filterObj[distinctColumn]) {
          const col = this.getColumnFromColumnArray(distinctColumn);
          if (!this.multiSelectFilter[col.index]) {
            this.multiSelectFilter[col.index] = [this.filterObj[distinctColumn]['value']];
          }
        }

        // tslint:disable-next-line:forin
        for (const key in this.filterObj) {
          // create a filter builder object
          if (!distinctColumn || key !== distinctColumn) {
            this.createFilterBuilderObject(this.filterBuilder, key, this.filterObj[key]['value'], this.filterObj[key]['matchMode']);
            const others = this.filterObj[key]['others'];
            if (others && others.length > 0) {
              // tslint:disable-next-line:forin
              for (const other of others) {
                this.createFilterBuilderObject(this.filterBuilder, key, other['value'], other['matchMode']);
              }
            }
          }
        }

        if (this.filterBuilder.complexDateTimeList && this.filterBuilder.complexDateTimeList.length > 0) {
          // this.plugComplexDateTime(this.filterBuilder.complexDateTimeList);
          this.createComplexDateTree(this.filterBuilder.complexDateTimeList);
          // this.filterBuilder.complexDateTimeList = [];
        }

        filterKey = this.buildFilterString(true);
        // postObject['filtercolumn'] = filterKey;
        postObject['filterBuilder'] = filterKey;
        postObject['otherFilterBuilders'] = this.getOtherFilterBuildString();
        postObject['otherFilterConditions'] = this.getOtherFilterConditions();
      } else {
        filterKey = this.filterObj.FROMFILTERDIALOG.value;
        postObject['filterBuilder'] = filterKey;
        postObject['otherFilterBuilders'] = this.getOtherFilterBuildString();
        postObject['otherFilterConditions'] = this.getOtherFilterConditions();
        delete this.filterObj[FILTER_CONSTANTS.FROM_FILTER_DIALOG];
        this.cd.detectChanges();
      }

      // checking the default checkbox to apply filter
      this.checked = true;
    } else {
      // if apply filter is checked
      if (this.checked) {
        if (this.filterBuilder) {
          const filterPayload = this.buildFilterString(true);
          if (filterPayload && filterPayload.length > 0) {
            filterKey = filterPayload;
            postObject['filterBuilder'] = filterKey;
            postObject['otherFilterBuilders'] = this.getOtherFilterBuildString();
            postObject['otherFilterConditions'] = this.getOtherFilterConditions();
          }
        }
      }
    }
  }
  updateDataSet(group, data, callback) {

    if (sessionStorage.getItem(this.sessionKey) === JSON.stringify(this.reportQueryParam)) {
      return;
    }
    sessionStorage.setItem(this.sessionKey, JSON.stringify(this.reportQueryParam));

    const postObject = {};
    postObject['refreshCache'] = this.refreshCache;
    let summaryURL = '&summary=';
    let groupKey = '';
    let dataURL;
    let sortKey = '';
    let expandKey = '';
    const pageCount = (data['first'] / data['rows']) + 1;
    const matchMode = '';
    postObject['pagecount'] = pageCount;
    postObject['pagesize'] = data['rows'];

    this.setPostObjectFilter(postObject);

    postObject['bu_ids'] = this.visible_business_string;
    postObject['queryparam'] = this.reportQueryParam;

    if (data && data.multiSortMeta && data.multiSortMeta.length > 0) {
      // sortKey = '&sortby=';
      for (let i = 0; i < data.multiSortMeta.length; i++) {
        sortKey = sortKey + data.multiSortMeta[i]['field'] +
          (data.multiSortMeta[i]['order'] === 1 ? '' : '||DESC') + (i === data.multiSortMeta.length - 1 ? '' : '^');
      }
      postObject['sortby'] = sortKey
    }
    if (data.globalFilter && data.globalFilter.length > 0) {
      // globalFilter = '&globalfilter=' + data.globalFilter;
      postObject['globalfilter'] = data.globalFilter;
    }

    // tslint:disable-next-line:forin
    for (const key in this.summaryConfig) {
      summaryURL = summaryURL + key + '||' + this.summaryConfig[key]
        + (Object.keys(this.summaryConfig).indexOf(key) === Object.keys(this.summaryConfig).length - 1 ? '' : '^');
    }

    if (this.groupArray) {
      for (let i = 0; i < this.groupArray.length; i++) {
        groupKey = groupKey + this.groupArray[i] + (i === this.groupArray.length - 1 ? '' : '^');
        postObject['groupby'] = groupKey
      }
    }

    if (group && group.length > 0) {
      // expandKey = "&expand=";
      for (let i = 0; i < group.length; i++) {
        expandKey = expandKey + group[i]['label'] + '||' + group[i]['group'] + (i === group.length - 1 ? '' : '^');
      }
    }

    postObject['expandby'] = expandKey;

    // if (data.multiSortMeta && data.multiSortMeta.length > 0) {
    //   sortKey = '&sortby=';
    //   for (let i = 0; i < data.multiSortMeta.length; i++) {
    //     sortKey = sortKey + data.multiSortMeta[i]['field'] +
    //       (data.multiSortMeta[i]['order'] === 1 ? '' : ':DESC') + (i === data.multiSortMeta.length - 1 ? '' : ',');
    //   }
    // }

    dataURL = this.genericApiObject['search']
    // + groupURL + filterURL + sortURL + expandURL + '&pagesize=' + data['rows'] + '&pagecount=' + pageCount + summaryURL;
    const visibleColumns = [];
    if (this.columnArray) {
      for (let i = 1; i < this.columnArray.length; i++) {
        if (this.columnArray[i].display) {
          visibleColumns.push(this.columnArray[i].field);
        }
      }
    }
    postObject['columns'] = visibleColumns;

    // Update the filter on each column
    // this.updateFilterOnColumn(visibleColumns, this.filterBuilder);
    this.currentPostObject = postObject;
    this.dataService.getData(dataURL, postObject).subscribe((data2) => {
      this.refreshCache = false;
      callback(data2);
      sessionStorage.setItem(this.sessionKey, '0');
    }, (err) => {
      this.primeLoader = false;
      sessionStorage.setItem(this.sessionKey, '0');
      throw (err);
    })
  }

  updateFilterOnColumn(columnArray: any[], filterBuilder: FilterRoot) {
    if (!this.filterBuilder.filterRows) {
      return;
    }

    if (!columnArray) {
      return;
    }

    if (!this.dataTable) {
      return;
    }

    if (!this.indexes) {
      return;
    }

    const toBeRemovedDropDownFilter = [];
    // const columnsHeader1 = this.domHandler.find(this.el.nativeElement, '.filter-list .ui-column-filter');
    //  const columnsHeader2 = this.domHandler.find(this.el.nativeElement, '.filter-list select ');

    const fb = this.filterBuilder;
    this.indexes.forEach(index => {
      const element = index.el.nativeElement;
      const item = columnArray[index.index];
      fb.filterRows.forEach(filter => {
        if (filter.key === item.field) {
          if (element.tagName === 'P-DROPDOWN') {
            const dropdown = index.dropdown;
            dropdown.updateSelectedOption(filter.value);
            if (filter.value === '') {
              toBeRemovedDropDownFilter.push(filter);
              dropdown.updateSelectedOption(null);
              //  delete this.filters[field];
              // delete this.dataTable.filter[filter.key];
            }
          } if (element.tagName === 'P-CALENDAR') {
            // for calendar=
            const calendar = index.Calendar;
            if (filter.value) {
              const date = filter.value.replace(/\//g, '-');
              calendar.writeValue(date);
            } else {
              calendar.writeValue('');
            }
            // end of calendar
          } else {
            element.value = filter.value;
          }

          // the filter type
          if (filter.operand.value === 'isBlank' || filter.operand.value === 'isNotBlank') {

          } else {
            item.defaultFilter = filter.operand.value
          }
          ;
        }
      });
    });

    // FOR THE DROP DOWN SELECT, IF 'ALL' value is selected, the 'select' will be show up.
    // if (toBeRemovedDropDownFilter && toBeRemovedDropDownFilter.length > 0) {
    //   //   const kept = [];
    //   //   this.filterBuilder.filterRows.forEach(filter => {
    //   //     toBeRemovedDropDownFilter.forEach(removed => {
    //   //       if (filter.key === removed.key) {
    //   //       } else {
    //   //         kept.push(filter);
    //   //       }
    //   //     });
    //   //   });

    //   //   this.filterBuilder.filterRows = kept;
    // }
  }

  /*
    getComplexDateTimeList (field: string) {
    let f = field;
    let complexDateTimeList: Array<ComplexDateTimeObject > =[];
    if (this.filterBuilder.complexDateTimeList == null) {
      return complexDateTimeList;
    }
    this.filterBuilder.complexDateTimeList.forEach(element => {
      if (element.columnName === f) {
        complexDateTimeList.push(element);
      }
    });
    return complexDateTimeList;
  }
  */

  getComplexDateTimeList(field: string) {
    console.log(`${field} -> called at ${new Date()}`);
    return this.filterBuilder.complexDateTimeList == null ? [] : this.filterBuilder.complexDateTimeList;
  }

  emptyColumnsWhenClearFilter() {
    if (!this.filterBuilder.filterRows) {
      return;
    }

    if (!this.columnArray) {
      return;
    }

    if (!this.dataTable) {
      return;
    }

    if (!this.indexes) {
      return;
    }

    // Reset value
    this.indexes.forEach(index => {
      const element = index.el.nativeElement;
      const item = this.columnArray[index.index];
      if (element.tagName === 'P-DROPDOWN') {
        const dropdown = index.dropdown;
        dropdown.updateSelectedOption('');
      }
      if (element.tagName === 'P-CALENDAR') {
        const calendar = index.Calendar;
        calendar.writeValue('');
      } else {
        element.value = '';
      }
    });

    // Reset filter types
    this.indexes.forEach(index => {
      const item = this.columnArray[index.index];
      item.defaultFilter = 'contains';

      if (this.filterType[index.index]) {
        this.filterType[index.index] = item.defaultFilter;
      }
    });
  }

  onChangeMethod(event, index) {
    // tslint:disable-next-line:max-line-length
    if (event.relatedTarget != null && (event.relatedTarget.className.indexOf('saveLayoutButton') !== -1) && event.relatedTarget.tagName === 'BUTTON') {
      const promise = new Promise<boolean>((resolve, reject) => {
        return this.onChangeTask(index, promise);
      });

      promise.then(r => this.saveLayout());
    } else {
      this.onChangeTask(index, null);
    }

  }

  onChangeTask(index, resolve): Promise<boolean> | boolean {
    if (!this.filterBuilder.filterRows) {
      return;
    }

    if (!this.columnArray) {
      return;
    }

    if (!this.dataTable) {
      return;
    }

    if (!this.indexes) {
      return;
    }

    const filters = this.dataTable.filters;
    this.indexes.forEach(index2 => {
      const element = index2.el.nativeElement;
      const item = this.columnArray[index2.index];
      if (element.tagName === 'P-CALENDAR') {
        const calendar = index2.Calendar;

        if (calendar.value === '' || calendar.value === null) {
          // Remove the date time
          calendar.writeValue('');
          const f = filters[item.field];
          if (f != null) {
            delete filters[item.field];
            this.dataTable._filter();
          }
        } // end of if
      }
    });


    if (resolve != null) {
      resolve(true)
    } else {
      return true;
    }
  }

  addExpandedRowGroup(event) {
    this.primeLoader = true;
    this.groupSummary = {};
    this.expandedRowGroups = [];
    //  this.updateColumnWidth(null);
    this.cd.detectChanges();
    // If event label is last in the groupArray
    if (this.groupArray) {
      for (let i = 0; i < this.groupArray.length; i++) {
        if (i > event.level) {
          break;
        }
        let expandedRowObject = {};
        expandedRowObject = {
          group: event.row[this.groupArray[i]],
          label: this.groupArray[i]
        }
        this.expandedRowGroups.push(expandedRowObject);
      }
    }

    this.updateDataSet(this.expandedRowGroups, event.lazyLoadData, (data) => {
      this.totalRecords = data.totalCount;
      this.genericDataSet = data.data;
      this.summary = data.columnSummary || {};

      try {
        this.groupSummaryTable = data.extra.groupSummary;
      } catch {
      }

      // tslint:disable-next-line:forin
      // for (const key in this.summaryConfig) {
      //   this.groupSummary[key] = {
      //     type: this.summaryConfig[key],
      //     value: event.row.metadata[0]
      //   }
      // }
      // this.primeLoader = false;
      // this.cd.detectChanges();
      // this.fixColumnWidthOnCurrentLayout(this.getDefaultScreenWidth());
      this.primeLoader = false;

    });

  }

  subDragStart(target) {
    this.swapStart = target;
  }

  subDrop(event, index) {
    this.isDragActive = -2;
    const baseIndix = index;
    index = index === -1 ? 0 : index;
    //  this.filterObj = {};
    // this.selectedValue = [];
    this.primeLoader = true;
    this.display = false;
    setTimeout(() => {
      this.primeLoader = false;
      const startId = this.groupArray.indexOf(this.swapStart);
      const endId = index < startId && baseIndix > -1 ? index + 1 : index;
      const item = this.groupArray.splice(startId === -1 ? 0 : startId, 1)[0];
      this.groupArray.splice(endId, 0, item);
      this.multiSortMeta = this.constructMultiSortMetaArray(this.groupArray);
      this.display = true;
      this.cd.detectChanges();
    }, 1000)
    this.cd.detectChanges();
  }

  changeGroup(colName) {
    this.newReport = false;
    const index = this.groupArray.indexOf(colName);
    this.groupArray.splice(index, 1);
    this.groupMultiselect = [];
    this.multiSelectFilter = [];
    this.genericDataSet = [];
    this.expandedRowGroups = [];
    //  this.filterObj = {};
    //  this.selectedValue = [];
    this.primeLoader = true;
    this.display = false;
    this.multiSortMeta = this.constructMultiSortMetaArray(this.groupArray);

    // Two cases: (1) remove one but not the last column in group; (2) The last one.
    if (this.sortField) {
      this.multiSortMeta.push({ field: this.sortField, order: this.sortOrder });
    }

    if (this.sortObject.multisortmeta) {
      // const list: Array<any> = (<any>(this.sortObject)).multisortmeta;
      // list.forEach( item => {
      //   let found = false;
      //   for(let i=0; i< this.multiSortMeta.length; i++) {
      //       if(this.multiSortMeta[i].field === item.field) {
      //         found = true;
      //         break;
      //       }
      //   }
      //   if(!found) {
      //     this.multiSortMeta.push(item);
      //   }
      // });
    }

    if (this.groupArray.length === 0 && this.sortField && this.sortField !== '') {
      // (2) The last one, we need to keep the sort one for ungroup grid.
      this.multiSortMeta = [{ field: this.sortField, order: this.sortOrder }];
      this.sortObject.multisortmeta = this.multiSortMeta;
    }

    if (this.sortField === '') {
      // from regular gird to grouping but without sorting columns, then back to regular.
      this.sortObject.multisortmeta = [];
    }

    this.localDbService.setItemIntoLocalDb(this.dataService.getUrlParam(this.localDBKey.groupArray), this.groupArray, 'json');
    setTimeout(() => {
      this.display = true;
      this.cd.detectChanges();
      this.fixColumnWidthOnCurrentLayout(this.getDefaultScreenWidth());
      setTimeout(() => {
        this.copyColumnWidth();
      }, 3000);
    }, 1000);
    this.cd.detectChanges();
    this.newReport = true;
  }

  /**
   * Creates a new row object with blank values this can be passed to add-edit-form component
   * @returns void
   */
  addRowDialog(): void {
    if (this.genericApiObject.create) {
      if (this.genericApiObject.create === 'show') {
        this.onCreate.emit();
      } else {
        this.rowData = this.cloneRowData(this.schemaFields, true);
        this.displayDialogAdd = true;
      }
    }
  }

  /**
   * Updates the UI with the updated record in case of edit
   * @param  {} newRecord
   * @returns void
   */
  saveRecord(newRecord): void {
    // this.primeLoader = true;
    // window.localStorage.clear();
    // need to call again the record count api + current page records;
    if (newRecord.fireSaveEvent) {
      this.onSave.emit();
    } else {
      if (this.selectedItemIndex && this.selectedItemIndex > -1) {
        const genericDataSet = [...this.genericDataSet];
        genericDataSet[this.selectedItemIndex] = newRecord.record;
        this.genericDataSet = genericDataSet;
      }
      if (newRecord.action === 'create') {
        this.totalRecords = this.totalRecords + 1;
        this.refreshColumnSummary();
        if (this.customDataTable) {
          this.customDataTable.paginate();
        } else {
          this.customDataTable.paginate();
        }
      }
      this.rowData = {};
    }

  }

  refreshColumnSummary() {
    // this.getSummary(this.filterObj, this.gb.nativeElement.value, (data) => {
    //   this.summary = {};
    //   // tslint:disable-next-line:forin
    //   for (const key in this.summaryConfig) {
    //     this.summary[key] = {
    //       type: this.summaryConfig[key],
    //       value: data.data[0][key]
    //     }
    //   }
    //   this.primeLoader = false;
    // })
  }

  /**
   * Selects the row on which edit button was clicked
   * @param  {} entity
   * @returns void
   */
  editRowDialog(entity): void {
    if (this.genericApiObject.edit) {
      if (String(this.genericApiObject.edit) === 'show') {
        this.onEdit.emit(entity);
      } else {
        this.selectedItemIndex = NaN;
        this.rowData = this.cloneRowData(entity, false);
        this.selectedItemIndex = this.findSelectedRowIndex(entity);
        this.editDisplayDialog = true;
      }
    }
  }

  deleteRowDialog(entity) {
    if (this.genericApiObject.delete) {
      if (String(this.genericApiObject.delete) === 'show') {
        this.onDelete.emit(entity);
      } if ((entity.becomes_memberid === '0' || entity.becomes_memberid === '') && entity.status === 'New') {
        this.selectedItemIndex = NaN;
        this.rowData = this.cloneRowData(entity, false);
        this.confirmDisplayDialog = true;
        this.confirmDisplayDialogLabel = 'Are you sure you want to delete this row?  This history will also be deleted...';
        this.confirmDisplayDialogBtnLabel = 'Delete';
        this.formOptType = CONFIG.Constants.RECORD_DELETE;
        this.selectedItemIndex = this.findSelectedRowIndex(entity);
      } else {
        this.confirmDisplayDialog = false;
        this.PushWarnMessage('This applicant cannot be deleted as they have moved past the \"New\" status');
      }
    }
  }

  cloneRowData(c, isDefaultValue) {
    const data = {};
    for (const prop in c) {
      if (isDefaultValue) {
        data[prop] = '';
      } else {
        data[prop] = c[prop];
      }
    }
    return data;
  }

  saveEditedRow(row) {
    this.primeLoader = true;
    window.localStorage.clear();
    const genericDataSet = [...this.genericDataSet];
    genericDataSet[this.selectedItemIndex] = row;
    this.genericDataSet = genericDataSet;
  }

  deleteRow(index, row) {
    CONFIG.LOG(this.tableKeyField, 'table keyfield in deleterow');
    CONFIG.LOG(row[this.tableKeyField], 'value of table keyfield in deleterow');
    const dataURL = this.genericApiObject['delete'] + '/' + row[this.tableKeyField]
    this.dataService.delete_v2(dataURL, row).then(response => {
      // Record has been deleted
      if (response === 'success' || (response.data && response.data.toLowerCase().indexOf('success') > -1)) {
        if (!(response.data && response.data.toLowerCase().indexOf('success') > -1)) {
          this.genericDataSet = this.genericDataSet.filter((val, i) => i !== index);
          if (this.expandedRowGroups && this.expandedRowGroups.length) {
            this.expandedRowGroups.pop();
          }
          const tempFilterObj = Object.assign({}, this.filterObj);
          this.filterObj = tempFilterObj;
          this.refreshColumnSummary();
        }
        this.onSave.emit();
      } else { // Record could not be deleted
        this.store.dispatch(new fromMessage.PushErrorMessage(response.data || response));
      }
    },
     (err:any)=>{
      this.store.dispatch(new fromMessage.PushErrorMessage(err));  
     });
  }

  capitalizeLabel(txt) {
    return txt;
  }

  capitalizeTxt(txt) {
    return txt.replace(/[^a-zA-Z ]/g, ' ', ' $1').replace(/^./, function (str) {
      return str.toUpperCase();
    })
  }

  findSelectedRowIndex(entity): number {
    return this.genericDataSet.indexOf(entity);
  }

  clearFilter() {
    this.emptyColumnsWhenClearFilter();
    // this.multiSelectFilter = [];
    this.groupMultiselect = [];
    this.filterObj = {};
    this.filterBackup = {};
    this.selectedValue = [];
    this.multiSelectFilter = [];
    if (this.dateControlElem) {
      this.dateControlElem.forEach(element => {
        element.toDate = undefined;
        element.fromDate = undefined;
        element.checkedValues = [];
      });
    }
    this.checked = true;
    this.otherFilterConditions = [];
    this.otherFilterObjects = [];
    this.currentOtherFilterCondition_index = -1;
    this.filterBuilder = new FilterRoot();

    this.cd.detectChanges();
  }

  toggleFilter(value) {
    if (value === false) {
      this.filterBackup = Object.assign({}, this.filterObj);
      this.filterObj = {};
      this.selectedValue = [];
      this.multiSelectFilter = [];
    } else {
      this.filterObj = this.filterBackup;
      this.filterBackup = {};
      this.showValueInColumnFilter(this.filterBuilder.filterRows);
    }
    this.cd.detectChanges();
  }

  setDefaultFilter(column, index) {
    if (!this.filterType[index]) {
      if (column.defaultFilter) {
        this.filterType[index] = column.defaultFilter;
      } else {
        if (column.type === 'number' || column.type === 'currency' || column.type === 'percent' || column.type === 'date') {
          this.filterType[index] = 'equals';
        } else {
          this.filterType[index] = 'contains';
        }
      }
    }
  }

  resetFilter(filtersObj) {

    // set filtertype for columns when user selected none
    const filtersApplied = filtersObj.filters;
    let filteredCol;
    for (const field in filtersApplied) {
      if (filtersApplied[field].matchMode === '') {
        filteredCol = this.columnArray.filter((col) => {
          return col['field'] === field;
        });
        // tslint:disable-next-line:max-line-length
        if (filteredCol[0]['type'] === 'number' || filteredCol[0]['type'] === 'currency' || filteredCol[0]['type'] === 'percent' || filteredCol[0]['type'] === 'date') {
          this.filterType[this.findIndexInColumnArray(field)['index']] = 'equals';
        } else {
          this.filterType[this.findIndexInColumnArray(field)['index']] = 'contains';
        }
      }
    }

    if (this.filterBackup && (Object.keys(this.filterBackup) && Object.keys(this.filterBackup).length) !== 0
      && !this.checked && (Object.keys(this.filterObj) && Object.keys(this.filterObj).length !== 0)) {
      const filter = Object.assign({}, this.filterBackup);
      this.filterBackup = {};
      filter[Object.keys(this.filterObj)[0]] = this.filterObj[Object.keys(this.filterObj)[0]];
      this.filterObj = filter;
      this.checked = true;
    }
  }

  /**
   * To find out index of a field in column array
   * @param  {} fieldFilteredOn
   * @returns Object
   */
  findIndexInColumnArray(fieldFilteredOn): Object {
    const colObject = {};
    if (this.columnArray) {
      for (let i = 0; i < this.columnArray.length; i++) {
        if (this.columnArray[i].field === fieldFilteredOn) {
          colObject['index'] = i;
          colObject['column'] = this.columnArray[i];
          return colObject;
        }
      }
    }
  }

  checkFilter(index = null) {
    if (index === null) {
      if ((this.filterBuilder.filterRows && this.filterBuilder.filterRows.length > 0)
        || (this.filterBuilder.children && this.filterBuilder.children.length > 0)) {
        return true;
      } else {
        return false;
      }
    } else {
      if (index < 0) {
        return false;
      }
      const b = this.otherFilterObjects[index];
      if (b && ((b.filterRows && b.filterRows.length > 0)
        || (b.children && b.children.length > 0))) {
        return true;
      } else {
        return false;
      }
    }
  }

  get save_layout_visibile() {
    return (this.selectedLayout.memberId === this.memberId
      || (this.tokenService.currentUser && this.tokenService.currentUser.save_global_layout));
  }

  // LAYOUT HANDLING METHODS.
  get save_global_layout() {
    if (!this.layouts) {
      return false;
    }
    return !this.appLayout.has_global_layout && this.tokenService.currentUser && this.tokenService.currentUser.save_global_layout &&
      this.selectedLayout.memberId !== 0;
  }
  saveLayout() {
    if (this.selectedLayout && this.selectedLayout.name) {
      if (this.selectedLayout.memberId === 0) {
        this.confirmationService.confirm({
          message: 'Are you sure that you want to overwrite the current GLOBAL Layout?',
          header: 'Save Global Layout',
          accept: () => {
            this.confirmSaveLayout();
          }
        });
      } else {
        this.confirmSaveLayout();
      }
    } else {
      this.store.dispatch(new fromMessage.PushWarnMessage('Please enter a name for layout.'));
    }
    this.saveButtonClicked = false;
  }


  confirmSaveLayout() {
    const currentLayoutValues = this.getCurrentSessionValueForLayout(this.dtViewChild);
    currentLayoutValues.firstName = this.selectedLayout.fullName;
    currentLayoutValues.fullName = this.selectedLayout.fullName;
    currentLayoutValues.memberId = this.selectedLayout.memberId;
    // currentLayoutValues.ddlName = this.selectedLayout.memberId;
    this.primeLoader = true;
    if (this.selectedLayout.id && (this.memberId === this.selectedLayout.memberId || this.selectedLayout.memberId === 0)) {
      this.updateLayout(currentLayoutValues);
    } else {
      // when other user it will be saved for current user
      if (this.memberId !== this.selectedLayout.memberId) {
        currentLayoutValues.memberId = this.memberId;
      }
      this.addNewLayout(currentLayoutValues);
    }
  }

  private addNewLayout(currentLayoutValues: any, isCoppied = false) {
    this.dataService.addLayout(currentLayoutValues).then(resData => {
      // tslint:disable-next-line:radix
      this.selectedLayout = { ...currentLayoutValues, ...{ 'id': parseInt(resData), 'isDefault': 0 } };

      const savedMessage = isCoppied ? `Copy of ${this.selectedLayout.name} created successfully.` : 'Layout saved successfully.';
      this.store.dispatch(new fromMessage.PushSuccessMessage(savedMessage));
      let layouts = [...this.layouts];
      if (layouts[0] && layouts[0].isMemberDefault) {
        layouts = [];
      }
      layouts.push(this.selectedLayout);
      this.layouts = layouts;
      this.primeLoader = false;
      this.applyLayout();
      // this.fixColumnWidthOnCurrentLayout(this.getDefaultScreenWidth());
      this.cd.detectChanges();
    }, (error) => {
      this.PushErrorMessage(error);
      this.primeLoader = false;
    });
  };



  private updateLayout(currentLayoutValues: any) {
    currentLayoutValues.isDefault = this.selectedLayout.isDefault;
    this.dataService.updateLayout(currentLayoutValues).subscribe(resDada => {
      this.primeLoader = false;
      const currentIndex = this.layouts.findIndex(x => x.id === currentLayoutValues.id);
      this.layouts[currentIndex] = currentLayoutValues;
      this.selectedLayout = currentLayoutValues;
      this.layouts = this.layouts.slice();
      this.fixColumnWidthOnCurrentLayout(this.getDefaultScreenWidth());
      this.store.dispatch(new fromMessage.PushSuccessMessage('Layout updated successfully.'));
      this.appLayout.setSelectedLayout(this.selectedLayout.id);
    }, (error) => {
      this.PushErrorMessage(error);
      this.primeLoader = false;
    });
  }

  getCurrentColumnWidths() {
    const columnsHeader = this.domHandler.find(this.el.nativeElement, '#pData_table_1 th.ui-resizable-column');
    let visibleCount = 0;
    if (!columnsHeader) {
      return;
    }
    if (this.columnArray) {
      for (let i = 0; i < this.columnArray.length; i++) {
        if (this.columnArray[i].display && columnsHeader[visibleCount]) {
          this.columnArray[i].widthPercent = ((columnsHeader[visibleCount++].offsetWidth * 100) / window.innerWidth);
        }

      }
    }
  }

  getACopyOfCurrentColumnWidths() {
    const columnsHeader = this.domHandler.find(this.el.nativeElement, '#pData_table_1 th.ui-resizable-column');
    const columnsWidth = [];
    let visibleCount = 0;
    if (!columnsHeader) {
      return columnsWidth;
    }

    if (this.columnArray) {
      for (let i = 0; i < this.columnArray.length; i++) {
        if (this.columnArray[i].display && columnsHeader[visibleCount]) {
          const offsetWidth = columnsHeader[visibleCount++].offsetWidth;
          const widthPercent = ((offsetWidth * 100) / window.innerWidth);
          columnsWidth.push({ field: this.columnArray[i].field, widthPercent: widthPercent });
          // console.log(`${offsetWidth} -> ${this.columnArray[i].field}`);
        }

      }
    }

    return columnsWidth;
  }


  getCurrentSessionValueForLayout(dtViewChild) {
    this.getCurrentColumnWidths();
    this.paginationConfig.rows = (this.dtViewChild && this.dtViewChild.rfpgn && this.dtViewChild.rfpgn.rows) || this.paginationConfig.rows;

    // console.dir(this.filterObj);
    // console.dir(this.filterBuilder.complexDateTimeList);
    return {
      'name': this.selectedLayout.name,
      'layout': JSON.stringify({
        'active': true,
        'groupArray': this.groupArray,
        'columnArray': this.getSelectedColumns(dtViewChild.columns, this.columnArray),
        'currentPage': (this.dtViewChild && this.dtViewChild.rfpgn && this.dtViewChild.rfpgn.getPage()) || 0,
        'paginationRows': this.paginationConfig.rows,
        'filterObj': this.filterObj,
        'filterOperator': this.filterBuilder.filterOperator,
        'otherFilterObjects': this.otherFilterObjects,
        'otherFilterConditions': this.otherFilterConditions,
        'otherFilterNewCondtion': this.otherFilterNewCondtion,
        'multiSortMeta': this.multiSortMeta,
        'columnWidthList': [],
        'sortField': this.sortField ? this.sortField : '',
        'sortOrder': this.sortOrder,
        'apply_fixed_header': this.apply_fixed_header,
        'selectedBusinessUnitString': this.selectedLayout.memberId === 0 ? null : this.visible_business_string,
        'complexDateTimeList': this.filterBuilder.complexDateTimeList
      }),
      'id': this.selectedLayout.id,
      'gridId': this.gridId,
      'memberId': this.memberId,
      'isDefault': 0,
      'firstName': this.tokenService.currentUser.fullName,
      'fullName': this.tokenService.currentUser.fullName
    }
  };

  getSelectedColumns(columsFromDatatable: Array<Column>, reportSchema: any) {
    const savedColumns = [];

    (<Array<any>>reportSchema).forEach(element => {
      let found = false;
      if (columsFromDatatable) {
        for (let i = 0; i < columsFromDatatable.length; i++) {
          if (element.field === columsFromDatatable[i].field) {
            savedColumns.push(element);
            found = true;
            break;
          }
        }
      }

      if (!found) {
        element.display = false;
        savedColumns.push(element);
      }
    });

    return savedColumns;
  }


  openNewLayoutDialog() {
    this.confirmDisplayDialog = true;
    this.confirmDisplayDialogLabel = 'Are you sure you want to clear the current layout?';
    this.confirmDisplayDialogBtnLabel = 'Clear';
    this.formOptType = CONFIG.Constants.LAYOUT_NEW;
  }


  newLayout() {
    this.clearFilter();
    this.cleanLayout();
    const memberName = this.tokenService.currentUser.fullName;
    this.selectedLayout = this.layoutService.setLayoutDefault(this.gridId, this.memberId, memberName, this.reportSchema)[0];
    this.selectedLayout.name = '';
    this.applyLayout();
  };

  cleanLayout() {
    this.gb.nativeElement.value = '';
    this.selectedValue = [];
    this.groupArray = [];
    this.selectedLayout = {};
    this.filterObj = {};
    this.multiSortMeta = [];
    this.filterType = [];
  }


  copyLayout() {

    let copyLayout = { ...this.selectedLayout, ...{ name: this.selectedLayout.name + ' Copy', isDefault: 0, memberId: this.memberId } };
    if (!this.selectedLayout.id) {
      copyLayout = this.getCurrentSessionValueForLayout(this.dtViewChild);
      copyLayout.name = this.selectedLayout.name + ' Copy';
    }

    delete copyLayout.id;
    this.primeLoader = true;
    // when other user it will be saved for current user
    if (this.memberId !== this.selectedLayout.memberId) {
      copyLayout.memberId = this.memberId;
      copyLayout.owner = copyLayout.fullName = copyLayout.firstName = this['tokenService']['currentUser']['fullName'];
    }
    this.addNewLayout(copyLayout, true);
  };

  compareLayoutWithSchema(layout) {

    layout.columnArray.forEach(element => {
      const currentSchema = this.getColumnFromColumnArray(element.field);
      for (const key in currentSchema) {
        if (!(key in element) || this.no_copy_fields.indexOf(key) > -1) {
          element[key] = currentSchema[key];
        } else {
          if (currentSchema[key] && typeof currentSchema[key] === 'object' && currentSchema[key].constructor === Object) {
            for (const key1 in currentSchema[key]) {
              if (!(key1 in element[key])) {
                element[key][key1] = currentSchema[key][key1];
              }
            }
          }
        }
      }
    })
  }

  applyLayout() {
    if (this.selectedLayout.memberId !== this.memberId && this.selectedLayout.memberId !== 0) {
      if (!this.selectedLayout.name.toLowerCase().startsWith(this.selectedLayout.fullName.toLowerCase() + '\'s ')) {
        this.selectedLayout.name = this.selectedLayout.fullName + '\'s ' + this.selectedLayout.name;
      }
    }
    const currentLayoutValues = JSON.parse(this.selectedLayout.layout);
    this.compareLayoutWithSchema(currentLayoutValues);
    this.otherFilterConditions = currentLayoutValues.otherFilterConditions || [];
    this.otherFilterObjects = currentLayoutValues.otherFilterObjects || [];
    this.otherFilterNewCondtion = currentLayoutValues.otherFilterNewCondtion || 'And';
    this.groupArray = currentLayoutValues.groupArray;
    this.multiSortMeta = this.constructMultiSortMetaArray(this.groupArray);

    // Keep the saved layout and add the missing ones in.
    const backup = this.columnArray;
    this.columnArray = currentLayoutValues.columnArray;
    for (let i = 0; i < backup.length; i++) {
      let found = false;
      for (let j = 0; j < this.columnArray.length; j++) {
        if (backup[i].field === this.columnArray[j].field) {
          found = true;
          break;
        }
      }

      if (!found) {
        backup[i].display = false;
        this.columnArray.push(backup[i]);
      }
    }

    this.apply_fixed_header = !!currentLayoutValues.apply_fixed_header;

    if (this.passed_params_apply) {
      this.filterObj = this.passed_filterObject;
      this.setDefaultSelectedVisibleBusinessUnit(this.passed_business_unit_string);
      this.passed_params_apply = false;
    } else {
      this.filterObj = currentLayoutValues.filterObj;
      this.setDefaultSelectedVisibleBusinessUnit(currentLayoutValues.selectedBusinessUnitString);
    }
    this.columnArrayForColumnsSelection = this.getColumns();
    const tmpMultiSortMetaArr = currentLayoutValues.multiSortMeta;
    this.appLayout.setSelectedLayout(this.selectedLayout.id);

    this.filterBuilder.complexDateTimeList = currentLayoutValues.complexDateTimeList;
    this.filterBuilder.filterOperator = currentLayoutValues.filterOperator;

    // this.sortField = currentLayoutValues.sortField;
    // this.sortOrder = currentLayoutValues.sortOrder;

    if (this.multiSortMeta && this.multiSortMeta.length > 0) {
      this.multiSortMeta = this.utilityService.getUniqueArrayOrObjects(this.multiSortMeta, tmpMultiSortMetaArr, 'field');
    } else {
      this.multiSortMeta = tmpMultiSortMetaArr;
    }
    // this.dtViewChild = this.groupArray.length ? this.custom_dt : this.default_dt;
    setTimeout(() => {
      this.setDefaultLayout(currentLayoutValues);

      if (!this.initialiazedOnClumnWidth) {
        this.initialiazedOnClumnWidth = true;
        this.currentColumnWidths = this.getACopyOfCurrentColumnWidths();
        console.log(`Get a copy of columns settings from layout [ ${this.selectedLayout.name} ]`);
      }
    }, 1000);
  };

  getColumns(): any[] {
    const currentLayoutValues = JSON.parse(this.selectedLayout.layout);
    const result = [];
    this.baseCoulmnArray.forEach(e1 => {
      let found = false;
      currentLayoutValues.columnArray.forEach(e2 => {
        if (e1.field === e2.field) {
          result.push(e2);
          found = true;
        }
      });

      if (!found) {
        e1.display = false;
        result.push(e1);
      }
    });
    return result;
  }

  getDefaultScreenWidth() {
    let totalWidth = window.innerWidth;
    if (!this.selectedLayout.id) {
      const browserTableWidth = this.domHandler.find(this.el.nativeElement, '.myDefaultWidth');
      totalWidth = browserTableWidth[0].offsetWidth;
      if (this.firstLoad) {
        totalWidth -= 19;
        this.firstLoad = false;
      } else {
        totalWidth -= 2;
      }
    }
    return totalWidth;
  }

  setDefaultLayout(currentLayoutValues) {
    this.dtViewChild = this.groupArray && this.groupArray.length ? this.custom_dt : this.default_dt;
    if (this.dtViewChild) {

      this.fixColumnWidthOnCurrentLayout(this.getDefaultScreenWidth());
      const pageCount = currentLayoutValues.currentPage;
      this.paginationConfig.rows = currentLayoutValues.paginationRows || this.paginationConfig.rows;
      this.dtViewChild.rfpgn.rows = this.paginationConfig.rows;
      this.dtViewChild.rfpgn.changePage(pageCount);
      this.dtViewChild.setCurrentMultiSortMetaValues(this.multiSortMeta);
      this.showValueInColumnFilter(this.filterBuilder.filterRows);

    }
  };


  updateColumnWidth(event) {
    // this.applyLayout_WhenBringColumnsBack();
    this.fixColumnWidthOnCurrentLayout(this.getDefaultScreenWidth());
  }

  applyFixedHeaderChange(value: boolean) {
    this.apply_fixed_header = value;
    if (!this.apply_fixed_header) {
      this.fixed_header = false;
    }
  }


  @HostListener('window:scroll', ['$event'])
  onWindowScroll($event) {
    if (!this.apply_fixed_header || this.fixed_header_scroll <= 0) {
      return;
    }
    const number = window.pageYOffset || window.document.documentElement.scrollTop || window.document.body.scrollTop || 0;
    const diff = Math.abs(number - this.old_scroll_position);
    if (number >= this.fixed_header_scroll) {
      if (!this.fixed_header) {
        this.fixed_header = true;
        this.old_scroll_position = number;
        this.showFixedHeader(this.fixed_header);
      }
    } else if (diff > 40 && this.fixed_header) {
      this.fixed_header = false;
      this.old_scroll_position = number;
      this.showFixedHeader(this.fixed_header);
    }
  }


  showFixedHeader(show = true) {
    if (this.customDataTable) {
      this.customDataTable.resizableColumns = !show;
    }
    if (this.dataTable) {
      this.dataTable.resizableColumns = !show;
    }
  }

  copyWidthToNestedGroup() {
    const columnsHeader = this.domHandler.find(this.el.nativeElement, '#pData_table_1 th.ui-resizable-column');
    const nestedCols = this.domHandler.find(this.el.nativeElement, '.nested-table-data td');
    let totalWidth = 0;
    if (columnsHeader && nestedCols) {
      const m = nestedCols.length / columnsHeader.length;
      for (let i = 0; i < columnsHeader.length; i++) {
        if (nestedCols[i] && columnsHeader[i]) {
          totalWidth += parseInt(columnsHeader[i].style.width, 10);
          // nestedCols[i].width = columnsHeader[i].clientWidth + 'px';
          for (let j = 0; j < m; j++) {
            const n = j * columnsHeader.length + i;
            if (nestedCols[n]) {
              nestedCols[n].width = columnsHeader[i].clientWidth + 'px';
            }
          }
        }

      }
    }
  }

  copyColumnWidth(fromEvent = false) {
    const columnsHeader = this.domHandler.find(this.el.nativeElement, '#pData_table_1 th.ui-resizable-column');
    const columnsHeader_fixed = this.domHandler.find(this.el.nativeElement, '#pData_table_fixed th.ui-resizable-column');
    const columnsHeader_fixed_spans = this.domHandler.find(this.el.nativeElement,
      '#pData_table_fixed th.ui-resizable-column span.ui-column-title');
    let totalWidth = 0;
    if (columnsHeader && columnsHeader_fixed) {
      for (let i = 0; i < columnsHeader.length; i++) {
        if (columnsHeader_fixed[i] && columnsHeader[i]) {
          totalWidth += parseInt(columnsHeader[i].style.width, 10);
          columnsHeader_fixed[i].style.width = columnsHeader[i].clientWidth + 'px';
          if (columnsHeader_fixed_spans && columnsHeader_fixed_spans[i]) {
            columnsHeader_fixed_spans[i].style.width = (columnsHeader[i].clientWidth - 6) + 'px';
          }
        }
      }
    }
    let tableEle_fixed;
    let tableEle;
    try {
      if (columnsHeader_fixed[0]) {
        tableEle_fixed = columnsHeader_fixed[0].parentElement.parentElement.parentElement;
      }
      if (columnsHeader[0]) {
        tableEle = columnsHeader_fixed[0].parentElement.parentElement.parentElement;
      }
      if (tableEle_fixed && tableEle) {
        tableEle_fixed.style.width = tableEle.style.width;
      }
    } catch {
    }
    this.copyWidthToNestedGroup();

    if (fromEvent) {
      this.currentColumnWidths = this.getACopyOfCurrentColumnWidths();
    }
  }


  fixColumnWidthOnCurrentLayout(windowWidth) {
    let totalWidth = 0;
    let tableEle;
    this.cd.detectChanges();
    const columnsHeader = this.domHandler.find(this.el.nativeElement, '#pData_table_1 th.ui-resizable-column');
    try {
      tableEle = columnsHeader[0].parentElement.parentElement.parentElement;
    } catch {
    }

    let widthPercent = 1;
    const len = this.columnArray.length;
    let realLen = 0;
    if (this.columnArray) {
      for (let i = 0; i < this.columnArray.length; i++) {
        if (this.columnArray[i].display) {
          realLen++;
        }
      }
    }

    let width = '1em';
    if (realLen) {
      width = (50 / (realLen + 1)).toFixed(2);
      widthPercent = (100 / (realLen + 1));
    }

    let displayCount = 0;
    if (this.columnArray) {
      for (let i = 0; i < this.columnArray.length; i++) {

        const value = this.columnArray[i];
        if (value.display && columnsHeader[displayCount]) {
          if (value.widthPercent === 0) {
            value.widthPercent = widthPercent;
          }
          if (this.groupArray.includes(value.field)) {
            value.widthPercent = 0;
          }

          if (this.currentColumnWidths.length > 0 && !this.groupArray.includes(value.field)) {
            for (let j = 0; j < this.currentColumnWidths.length; j++) {
              if (this.currentColumnWidths[j].field === value.field && this.currentColumnWidths[j].widthPercent > 0) {
                value.widthPercent = this.currentColumnWidths[j].widthPercent;
              }
            }
          }

          const colWidth = (value.widthPercent * windowWidth) / 100;
          columnsHeader[displayCount].style.width = colWidth + 'px';
          this.columnArray[displayCount].width = columnsHeader[displayCount].style.width;
          displayCount++
          totalWidth += colWidth;
          // console.log(`colWidth = ${colWidth}, value.widthPercent = ${value.widthPercent }`);
        }
      }
    }

    if (tableEle && tableEle.style) {
      tableEle.style.width = totalWidth + 'px';
    }
    this.copyColumnWidth();
  }

  getCurrentDefaultLayout() {
    if (this.layouts) {
      for (let i = 0; i < this.layouts.length; i++) {
        if (this.layouts[i].isDefault) {
          return { 'index': i, 'layout': this.layouts[i] }
        }
      }
    }
  }

  resetSelectLayout() {
    this.isDefaultLayoutActivated = false
    if (this.layouts && this.layouts.length > 0) {
      for (let i = 0; i < this.layouts.length; i++) {
        delete JSON.parse(this.layouts[i].layout).active;
        delete this.layouts[i].isDefault;
      }
    }
  }

  formOperations(selectedItemIndex, rowData) {
    switch (this.formOptType) {
      case CONFIG.Constants.LAYOUT_DELETE: {
        // this.deleteLayout();
        break;
      }
      case CONFIG.Constants.LAYOUT_NEW: {
        this.newLayout();
        break;
      }
      case CONFIG.Constants.RECORD_DELETE: {
        this.deleteRow(selectedItemIndex, rowData);
        break;
      }
    }
  }


  deleteSubRow(event) {
    const dataURL = this.genericApiObject['delete'] + '/' + event[this.tableKeyField]
    this.dataService.delete_v2(dataURL, null).then(response => {
      this.totalRecords = this.totalRecords - 1;
      this.refreshColumnSummary();
    });
    this.cd.detectChanges();
  }

  editSubRow(event) {
    // this.dataService.editRecord(this.genericApiObject['edit'], event).then(response => {
    // });
    this.cd.detectChanges();
  }

  bulkEditSave() {

    this.selectionMode = this.savedSelectionMode;

    const dataURL = this.genericApiObject['bulkEdit'];
    if (this.editedRows) {
      this.dataService.bulkEdit_v2(this.editedRows, dataURL).then(response => {
        this.store.dispatch(new fromMessage.PushSuccessMessage('Data has been saved successfully.'));
      });
    } else {
      this.store.dispatch(new fromMessage.PushWarnMessage('No row was updated.'));
    }
    this.bulkEnable = false;
    this.editedRows = null;
    this.backupOfEditRow = null;
    this.switchRowToViewMode2();
    this.unHighlightUpdatedRow();
  }

  editComplete(event) {
    this.editedRows = this.editedRows || [];
    let exist = false;

    for (let i = 0; i < this.editedRows.length; i++) {
      if (this.editedRows[i][this.tableKeyField] === event[this.tableKeyField]) {
        this.editedRows[i] = event;
        exist = true;
        break;
      }
    }
    if (exist === false) {
      this.editedRows.push(event);
    }
  }

  highlightUpdatedRow(event, rowData, column) {
    if (!this.backupOfEditRow || this.backupOfEditRow.length === 0) {
      return;
    }

    const updatedRow = this.backupOfEditRow.filter((row) => {
      return row[this.tableKeyField] === rowData[this.tableKeyField];
    });

    if (updatedRow[0][column.field] !== rowData[column.field]) {
      event.target.parentElement.parentElement.style.background = 'yellow';
      event.target.parentElement.parentElement.style.color = 'black';
      this.highlightedColumns.push(event.target.parentElement.parentElement);
    }
  }

  unHighlightUpdatedRow() {
    this.highlightedColumns.forEach((col) => {
      col.removeAttribute('style', 'background');
      col.removeAttribute('style', 'color');
    })
  }

  /**
   * Enable/disbale save button according to validaitons while bulk editing
   * @param  {} event
   * @param  {} rowData
   * @param  {} column
   * @param  {} valueEntered
   * @param  {} rowIndex
   */
  inlineEditValidations(event, rowData, column, valueEntered, rowIndex) {
    if (column.type === 'boolean') { // No need for validations
      const tempEvent = {
        target: event
      }
      this.highlightUpdatedRow(tempEvent, rowData, column);
    } else {
      this.highlightUpdatedRow(event, rowData, column);
      let isCellValid = true;
      const validations = column.validations && column.validations.length ? JSON.parse(column.validations) : {};
      if (validations.required && valueEntered.trim() === '') {
        this.store.dispatch(new fromMessage.PushErrorMessage(`${column.label} is required.`));
        isCellValid = false;
      }

      if (validations.maxLength && valueEntered && (valueEntered.length > validations.maxLength)) {
        this.store.dispatch(new fromMessage.PushErrorMessage(`${column.label} should have atmost ${validations.maxLength} characters.`));
        isCellValid = false;
      }

      if (column.field && this.gridName === 'CustomerAsset' && column.field === 'model') {
        if (validateCharacters(['"'])(new FormControl(valueEntered, [])) != null) {
          isCellValid = false;
          this.store.dispatch(new fromMessage.PushErrorMessage(`${column.label} should not include these 2 characters
        ; '`));
        }
      } else if (column.type === 'string'
        || column.type === 'notes' || column.type === 'textbox'
        || column.type === 'number' || column.type === 'spinner') {
        if (validateCharacters([])(new FormControl(valueEntered, [])) != null) {
          isCellValid = false;
          this.store.dispatch(new fromMessage.PushErrorMessage(`${column.label} should not include these 3 characters
        ; ' "`));
        }
      }

      // if (column.type === 'date') {
      //   const data = rowData[column.field];
      //   // rowData[column.field] = valueEntered.getFullYear() + "-" + (valueEntered.getMonth() + 1) + "-" + valueEntered.getDate() ;
      // }

      if (!isCellValid) {
        this.invalidRowsColumns.set(rowIndex + '_' + column.field, true);
      } else {
        // tslint:disable-next-line:max-line-length
        this.invalidRowsColumns.delete(rowIndex + '_' + column.field); // delete an item from map, won't throw error even if doesn;t exist already
        this.editComplete(rowData); // add edited row
      }
      this.isInlineEditingValid = this.invalidRowsColumns.size === 0; // if size of map is zero, enable save button;
    }
  }


  inlineEditValidationsForCalendarOnBlur(event, rowData, column, valueEntered, rowIndex) {
    rowData[column.field + 'beforeUpdate'] = valueEntered;
    rowData[column.field + 'beforeUpdate_object'] = event;
    // try {
    //   // change 'td' style.
    //   event.target.parentElement.parentElement.parentElement.parentElement.style.background = 'yellow';
    //   event.target.parentElement.parentElement.parentElement.parentElement.color = 'black';
    //   event.target.parentElement.parentElement.parentElement.parentElement.children[1].children[2].children[0].style.color = 'black';
    //   this.highlightedColumns.push(event.target.parentElement.parentElement.parentElement.parentElement);
    // } catch {

    // }
  }

  inlineEditValidationsForCalendar(event, rowData, column, valueEntered, rowIndex) {
    // tslint:disable-next-line:max-line-length
    this.invalidRowsColumns.delete(rowIndex + '_' + column.field); // delete an item from map, won't throw error even if doesn;t exist already
    this.editComplete(rowData); // add edited row
    this.isInlineEditingValid = this.invalidRowsColumns.size === 0; // if size of map is zero, enable save button;

    try {
      const d1 = this.checkDate(valueEntered);
      const month1 = '' + (d1.getMonth() + 1);
      const day1 = '' + d1.getDate();
      const year1 = d1.getFullYear();

      const d2 = this.checkDate(rowData[column.field + 'beforeUpdate']);
      const month2 = '' + (d2.getMonth() + 1);
      const day2 = '' + d2.getDate();
      const year2 = d2.getFullYear();

      if (!(month1 === month2 && day1 === day2 && year1 === year2)) {
        // change 'td' style.
        const el = rowData[column.field + 'beforeUpdate_object'];
        el.target.parentElement.parentElement.parentElement.parentElement.style.background = 'yellow';
        el.target.parentElement.parentElement.parentElement.parentElement.color = 'black';
        el.target.parentElement.parentElement.parentElement.parentElement.children[1].children[2].children[0].style.color = 'black';
        this.highlightedColumns.push(el.target.parentElement.parentElement.parentElement.parentElement);
      }
    } catch {
    }
  }

  openFilterDialog(index = -1) {
    this.currentOtherFilterCondition_index = index;
    if (this.currentOtherFilterCondition_index === this.otherFilterConditions.length) {
      this.otherFilterConditions[index] = 'And';
    }
    if (this.currentOtherFilterCondition_index === -1) {
      this.lastSavedFilterRoot = JSON.parse(JSON.stringify(this.filterBuilder));
      this.currentFilterRoot = JSON.parse(JSON.stringify(this.filterBuilder));
    } else {
      this.currentFilterRoot = this.otherFilterObjects[this.currentOtherFilterCondition_index] || new FilterRoot();
      this.lastSavedFilterRoot = JSON.parse(JSON.stringify(this.currentFilterRoot));
    }
    if ((!this.currentFilterRoot) ||
      (
        this.currentFilterRoot && this.currentFilterRoot.children.length === 0 &&
        (!this.currentFilterRoot.complexDateTimeList
          || (this.currentFilterRoot.complexDateTimeList && this.currentFilterRoot.complexDateTimeList.length === 0)) &&
        (this.currentFilterRoot.filterRows && this.currentFilterRoot.filterRows.length === 0)
      )
    ) {
      this.currentFilterRoot.filterOperator = 'And';
    }
    this.displayFilter = true;

  }
  removeOtherFilter(index) {
    this.otherFilterConditions.splice(index, 1);
    this.otherFilterObjects.splice(index, 1);
    // if (this.otherFilterObjects.length === 0) {
    //   this.otherFilterCondition[this.currentOtherFilterCondition_index] = 'And';
    //   this.otherFilterObjects[this.currentOtherFilterCondition_index] = new FilterRoot();
    // }
  }

  removeFilterItem(key, index) {
    for (const i in this.filterBuilderObj) {
      if (i === key) {
        delete this.filterBuilderObj[key];
      }
    }
  }

  closeFilter() {
    this.filterClosable = true;
    this.displayFilter = false;
    this.filterBuilderObj = {};
    this.currentFilterRoot = this.lastSavedFilterRoot
    if (this.currentOtherFilterCondition_index === -1) {
      this.filterBuilder = this.currentFilterRoot;
    } else {
      if (this.lastSavedFilterRoot) {
        this.otherFilterObjects[this.currentOtherFilterCondition_index] = this.currentFilterRoot;
        if (!this.checkFilter(this.currentOtherFilterCondition_index)) {
          this.removeOtherFilter(this.currentOtherFilterCondition_index);
        }
      }
    }
  }

  saveFilter() {
    this.filterBackup = {};
    this.checked = true;
    this.displayFilter = false;
    // calculate child root depth
    this.calculateChildRootDepth(this.currentFilterRoot);

    // recreating filter builder
    if (this.childRootDepth === 0) {
      this.evaluateComplexDateTimeZeroLevel(this.currentFilterRoot);
    } else {
      this.recreateFilterBuilder(this.currentFilterRoot, (this.childRootDepth - 1))
    }



    this.currentFilterRoot.filterRows.forEach((row) => {
      row.rowCreationSource = FILTER_CONSTANTS.FROM_FILTER_DIALOG;
    });

    // current rows
    const list = [];
    this.currentFilterRoot.filterRows.forEach(element => {
      // if (element.operand.value === 'isBlank') {
      //   element.operand.value = 'Equals'
      // } else if (element.operand.value === 'isNotBlank') {
      //   element.operand.value = 'notEquals'
      // }

      list.push(element);
    });

    // tslint:disable-next-line:forin
    for (let i = 0; i < list.length; i++) {
      // create a filter builder object
      // tslint:disable-next-line:max-line-length
      this.createFilterBuilderObject(this.currentFilterRoot, list[i].key, list[i].value,
        list[i].operand.value, this.currentFilterRoot.filterOperator);

    }

    // Re calculate the complex objects based on filter row.
    this.reCalculateComplexDateTimeOjbects(this.currentFilterRoot);

    if (this.currentFilterRoot.complexDateTimeList && this.currentFilterRoot.complexDateTimeList.length > 0) {
      this.createComplexDateTree(this.currentFilterRoot.complexDateTimeList);
    }

    if (this.currentOtherFilterCondition_index === -1) {
      this.filterBuilder = this.currentFilterRoot;
      if (!this.checkFilter() && this.checkFilter(0)) {
        this.filterBuilder = this.otherFilterObjects[0];
        this.removeOtherFilter(0);
      }

      // based on the this.filterBuilder.filterRows to calculate the filterPayload
      const filterPayload = this.buildFilterString(true);
      if (filterPayload && filterPayload.length > 0) {
        const key = FILTER_CONSTANTS.FROM_FILTER_DIALOG;
        this.filterBuilderObj[key] = {
          value: filterPayload,
          matchMode: ''
        }
        this.filterObj = Object.assign({}, this.filterBuilderObj);
      } else {
        this.filterObj = {};
      }
      this.showValueInColumnFilter(this.filterBuilder.filterRows);
    } else {
      if (!this.otherFilterConditions[this.currentOtherFilterCondition_index]) {
        this.otherFilterConditions[this.currentOtherFilterCondition_index] = this.otherFilterNewCondtion;
        this.otherFilterNewCondtion = 'And';
      }
      this.otherFilterObjects[this.currentOtherFilterCondition_index] = this.currentFilterRoot;
      if (!this.checkFilter(this.currentOtherFilterCondition_index)) {
        this.removeOtherFilter(this.currentOtherFilterCondition_index);
      }
      this.loadDataWithFilters();
    }
  }

  private reCalculateComplexDateTimeOjbects(filterBuilder: FilterRoot) {
    //
    // We have the previous complex object list
    // We have the latest filterRows.
    //

    if (!filterBuilder.complexDateTimeList || filterBuilder.complexDateTimeList.length === 0) {
      return;
    }

    const complexDateTimeList: Array<ComplexDateTimeObject> = [];

    for (let i = 0; i < filterBuilder.complexDateTimeList.length; i++) {
      const element = filterBuilder.complexDateTimeList[i];
      const newItem = this.updateComplexDateTimeOjbect(element, filterBuilder.filterRows);
      if (!newItem) {
        // Remove the element: so not add it to the list again.
        console.dir(`Removing ${element}`);
      } else {
        // Remove-and-Insert: so add the new item to the list
        complexDateTimeList.push(newItem);
      }
    }

    // Update the list
    filterBuilder.complexDateTimeList = complexDateTimeList;
  }

  private updateComplexDateTimeOjbect(cdt: ComplexDateTimeObject, filterRows: Array<FilterRow>): ComplexDateTimeObject {
    if (!filterRows || filterRows.length === 0) {
      return null;
    }

    //
    // Check condition 1 if cdt has it.
    //
    if (cdt.hasOwnProperty('condition1') && cdt.condition1) {
      let stillInThere = false;
      for (let i = 0; i < filterRows.length; i++) {
        if (cdt.columnName === filterRows[i].key && cdt.condition1.value === filterRows[i].operand.value) {
          stillInThere = true;
          cdt.condition1.displayValue = filterRows[i].value;
          break;
        }
      }

      if (!stillInThere) {
        // The condition1 is removed
        cdt.condition1 = undefined;
      }
    }

    //
    // Check conditon 2 if cdt has it.
    //
    if (cdt.hasOwnProperty('condition2') && cdt.condition2) {
      let stillInThere = false;
      for (let i = 0; i < filterRows.length; i++) {
        if (cdt.columnName === filterRows[i].key && cdt.condition2.value === filterRows[i].operand.value) {
          stillInThere = true;
          cdt.condition2.displayValue = filterRows[i].value;
          break;
        }
      }

      if (!stillInThere) {
        // The condition1 is removed
        cdt.condition2 = undefined;
      }
    }

    //
    // Check c1 & c2
    //
    if (!cdt.condition1 && !cdt.condition2) {
      // Remove the content if no complex object existing.
      this.removeContentOnRelatedFilterColums(cdt);
      return null;
    }

    return cdt;
  }

  private removeContentOnRelatedFilterColums(cdt: ComplexDateTimeObject) {
    if (!cdt) {
      return;
    }

    this.indexes.forEach(index => {
      const item = this.columnArray[index.index];
      if (item.field === cdt.columnName) {
        index.Calendar.writeValue('');
      }
    });
  }

  private evaluateComplexDateTimeZeroLevel(root: FilterRoot) {
    let isDirty;
    const dateColumns = root.filterRows.filter(element => {
      return this.findIndexInColumnArray(element.key)['column']['type'] === 'date';
    });
    let complexDateColumn: any;
    if (dateColumns && dateColumns.length > 0) {
      complexDateColumn = dateColumns.filter(element => {
        return element.alias && element.alias.length > 0;
      });
    }

    let regularDateColumn: any;
    if (dateColumns && dateColumns.length > 0) {
      regularDateColumn = dateColumns.filter(element => {
        return !element.alias && element.type === 'calendar';
      });
    }

    if (regularDateColumn && regularDateColumn.length > 0) {
      let toPresent = false;
      let fromPresent = false;
      const dirtyDateControl = this.dateControlElem.filter(elem => {
        return regularDateColumn[0].key === elem.field;
      });
      regularDateColumn.forEach(element => {
        fromPresent = element.operand.value === 'greaterThanOrEqual' || fromPresent || false;
        toPresent = element.operand.value === 'lessThan' || toPresent || false;
      });
      if (!toPresent) {
        if (dirtyDateControl && dirtyDateControl.length > 0 && dirtyDateControl[0].toDate) {
          dirtyDateControl[0].toDate = undefined;
        }
      }
      if (!fromPresent) {
        if (dirtyDateControl && dirtyDateControl.length > 0 && dirtyDateControl[0].fromDate) {
          dirtyDateControl[0].fromDate = undefined;
        }
      }
    }
    if (complexDateColumn) {
      if (complexDateColumn.length === 1) {
        const dirtyDateControl = this.dateControlElem.filter(element => {
          return complexDateColumn[0].key === element.field;
        });
        const dirtyOption = complexDateColumn[0].alias.split('Is ')[1];
        if (dirtyDateControl[0].checkedValues.indexOf(dirtyOption) !== -1) {
          dirtyDateControl[0].checkedValues.splice(dirtyDateControl[0].checkedValues.indexOf(dirtyOption), 1);
          const checkedValues = [...dirtyDateControl[0].checkedValues];
          dirtyDateControl[0].checkedValues = checkedValues;
        }
      } else if (complexDateColumn.length === 2) {
        const changedOperands = complexDateColumn.filter(element => {
          return element.reservedOperand && element.reservedOperand.length > 0 && (element.operand.value !== element.reservedOperand);
        });
        isDirty = changedOperands && changedOperands.length > 0;
        if (isDirty) {
          const dirtyDateControl = this.dateControlElem.filter(element => {
            return changedOperands[0].key === element.field;
          });
          const dirtyOption = changedOperands[0].alias.split('Is ')[1];
          if (dirtyDateControl[0].checkedValues.indexOf(dirtyOption) !== -1) {
            dirtyDateControl[0].checkedValues.splice(dirtyDateControl[0].checkedValues.indexOf(dirtyOption), 1);
            const checkedValues = [...dirtyDateControl[0].checkedValues];
            dirtyDateControl[0].checkedValues = checkedValues;
          }
        }
      }
    }

  }

  showValueInColumnFilter(filters) {
    const colObj = {};
    const filterColumns = [];
    const changedIndexes = [];
    filters.forEach(element => {
      if (colObj[element.key]) {
        colObj[element.key] = colObj[element.key]['count'] + 1;
      } else {
        colObj[element.key] = {};
        colObj[element.key]['value'] = element.value;
        colObj[element.key]['count'] = 1;
        colObj[element.key]['matchMode'] = element.operand.value;
      }
    });
    if (Object.keys(colObj) && Object.keys(colObj).length) {
      // tslint:disable-next-line:forin
      for (const key in colObj) {
        const column = this.findIndexInColumnArray(key);
        const index = column['index'];
        changedIndexes.push(index);
        if (colObj[key]['count'] === 1) {
          if (column['column']['type'] === 'boolean') {
            if (colObj[key]['value'] === true) {
              this.selectedValue[index] = '1';
            } else if (colObj[key]['value'] === false) {
              this.selectedValue[index] = '0';
            }
          } else {
            this.selectedValue[index] = colObj[key]['value'];
            this.filterType[index] = colObj[key]['matchMode']
          }
        } else {
          this.selectedValue[index] = undefined;
        }
      }
    }
    const selectedValueClone = [...this.selectedValue];
    selectedValueClone.forEach((element, index) => {
      if (element && changedIndexes.indexOf(index) === -1) {
        this.selectedValue[index] = undefined;
      }
    });
  }

  booleanValueChange(boolean, field, entity) {
    const row = Object.assign({}, entity);
    this.capturePreviousData(row);
    if (boolean === true) {
      entity[field] = 'False';
    } else if (boolean === false) {
      entity[field] = 'True';
    }
    this.editComplete(entity);
  }

  findObjectByKw(objList, kw, value) {
    if (objList) {
      for (let i = 0; i < objList.length; i++) {
        if (objList[i][kw] === value) {
          return objList[i];
        }
      }
    }
  }

  removeWhiteSpaceFromArray(array) {
    const tmpArray = [];
    if (array) {
      for (let i = 0; i < array.length; i++) {
        if (array[i] !== '') {
          tmpArray.push(array[i].trim());
        }
      }
    }
    return tmpArray;
  }

  insertAndShift(arr, from, to) {
    const cutOut = arr.splice(from, 1)[0];
    arr.splice(to, 0, cutOut);
    return arr;
  }

  bulkEditCancel() {

    this.selectionMode = this.savedSelectionMode;

    if (this.backupOfEditRow && this.backupOfEditRow.length > 0) {
      if (this.genericDataSet) {
        for (let i = 0; i < this.genericDataSet.length; i++) {
          const obj = Object.assign({}, this.getPreviousData(this.genericDataSet[i]));
          if (Object.keys(obj).length !== 0) {
            const genericDataSet = [...this.genericDataSet];
            genericDataSet.splice(i, 1, obj);
            this.genericDataSet = genericDataSet;
          }
        }
      }
    }
    this.bulkEnable = false;
    this.backupOfEditRow = null;

    this.switchRowToViewMode2();
  }

  /**
   * Enable bulk editing on datatable and switch first row to bulk edit mode by default
   */
  bulkEditEnable() {

    this.savedSelectionMode = this.selectionMode;
    this.selectionMode = 'single';

    this.bulkEnable ? this.bulkEnable = false : this.bulkEnable = true;
    const tableBody = this.dtViewChild.el.nativeElement.getElementsByTagName('tbody')[0];
    const tempEventObj = {
      originalEvent: {
        currentTarget: {
          children: tableBody.children[0].children
        }
      }
    }

    // this.switchRowToViewMode();
    // this.OnRowSelect(tempEventObj);
  }

  getPreviousData(row) {
    if (this.backupOfEditRow) {
      for (let i = 0; i < this.backupOfEditRow.length; i++) {
        if (this.backupOfEditRow[i][this.tableKeyField] === row[this.tableKeyField]) {
          return this.backupOfEditRow[i];
        }
      }
    }
    return {};
  }

  capturePreviousData(row) {
    this.backupOfEditRow = this.backupOfEditRow || [];
    const obj = Object.assign({}, row);
    let exist = false;
    if (this.backupOfEditRow) {
      for (let i = 0; i < this.backupOfEditRow.length; i++) {
        if (this.backupOfEditRow[i][this.tableKeyField] === obj[this.tableKeyField]) {
          exist = true;
          break;
        }
      }
    }
    if (!exist) {
      this.backupOfEditRow.push(obj);
    }
  }

  getHeader(col) {
    if (this.reportSchema && this.reportSchema.length > 0) {
      for (let i = 0; i < this.reportSchema.length; i++) {
        if (this.reportSchema[i].name === col && this.reportSchema[i].display) {
          return this.reportSchema[i].header;
        }
      }
    }
  }

  loadReport() {
    if (!this.dataTable) {
      return;
    }
    const data = this.dataTable.createLazyLoadMetadata();
    data['queryparam'] = this.reportQueryParam;

    if ((this.expandedRowGroups && this.expandedRowGroups.length > 0) || (this.groupArray && this.groupArray.length > 0)) {
      // load group data
      this.lazyLoadGroup(data);
    } else {
      // load non-group data
      this.lazyLoadData(data);
    }
  }

  showReport(gridName) {
    this.primeLoader = true;
    this.lazy = false;
    this.newReport = false;
    this.cleanLayout();
    this.genericApiObject = {};
    this.genericDataSet = [];
    this.reportSchema = [];
    this.gridId = gridName;
    const reportURL = 'api/Page/' + gridName;
    this.localDBKey = {
      'groupArray': gridName + 'GROUP-ARRAY',
      'filterArray': gridName + 'FILTER-ARRAY',
      'layouts': gridName + 'LAYOUTS',
      'isBaseLayoutDefault': gridName + 'BASE-LAYOUT'
    }


    const memberName = this.tokenService.currentUser.fullName;
    if (!this.memberId) {
      return;
    }
    this.layoutService.loadGridSchemaByGridName(this.gridId, this.memberId, memberName).subscribe((memberGridData: any) => {
      this.reportTitle = memberGridData.gridSchema['name'];
      if (memberGridData.gridSchema['createEndPoint'].toString().toLowerCase() === 'true') {
        this.genericApiObject['create'] = `api/Page/${this.reportTitle}/Create`;
      } else {
        this.genericApiObject['create'] = memberGridData.gridSchema['createEndPoint'];
      }
      if (memberGridData.gridSchema['editEndPoint'].toString().toLowerCase() === 'true') {
        this.genericApiObject['edit'] = `api/Page/${this.reportTitle}/Edit`;
      } else {
        this.genericApiObject['edit'] = memberGridData.gridSchema['editEndPoint'];
      }
      if (memberGridData.gridSchema['bulkEditEndPoint'].toString().toLowerCase() === 'true') {
        this.genericApiObject['bulkEdit'] = `api/Page/${this.reportTitle}/BulkEdit`;
      } else {
        this.genericApiObject['bulkEdit'] = memberGridData.gridSchema['bulkEditEndPoint'];
      }
      if (memberGridData.gridSchema['deleteEndPoint'].toString().toLowerCase() === 'true') {
        this.genericApiObject['delete'] = `api/Page/${this.reportTitle}/Delete`;
      } else {
        this.genericApiObject['delete'] = memberGridData.gridSchema['deleteEndPoint'];
      }
      if (memberGridData.gridSchema['distinctEndPoint'].toString().toLowerCase() === 'true') {
        this.genericApiObject['distinct'] = `api/Page/${this.reportTitle}/GetDistinct`;
      } else {
        this.genericApiObject['distinct'] = memberGridData.gridSchema['distinctEndPoint'];
      }
      if (memberGridData.gridSchema['exportToExcelEndPoint'].toString().toLowerCase() === 'true') {
        this.genericApiObject['exportToExcel'] = `api/Page/${this.reportTitle}/ExportToExcel`;
      } else {
        this.genericApiObject['exportToExcel'] = memberGridData.gridSchema['exportToExcelEndPoint'];
      }
      if (memberGridData.gridSchema['exportToPdfEndPoint'].toString().toLowerCase() === 'true') {
        this.genericApiObject['exportToPdf'] = `api/Page/${this.reportTitle}/ExportToPDF`;
      } else {
        this.genericApiObject['exportToPdf'] = memberGridData.gridSchema['exportToPdfEndPoint'];
      }
      if (memberGridData.gridSchema['searchEndPoint'].toString().toLowerCase() === 'true') {
        this.genericApiObject['search'] = `api/Page/${this.reportTitle}/Search`;
      } else {
        this.genericApiObject['search'] = memberGridData.gridSchema['searchEndPoint'];
      }
      if (memberGridData.gridSchema['isSelectable'].toString().toLowerCase() === 'true') {
        this.genericApiObject['isSelectable'] = true;
      } else {
        this.genericApiObject['isSelectable'] = false;
      }

      this.genericApiObject['viewInvoiceUrl'] = memberGridData.gridSchema['viewInvoiceUrl'];
      this.genericApiObject['viewInvoiceUrlParams'] = memberGridData.gridSchema['viewInvoiceUrlParams'];
      this.hasSummary = memberGridData.gridSchema['hasSummaryRow'];
      this.tableEditable = (memberGridData.gridSchema['editable'].toString().toLowerCase() === 'true');
      this.reportSchema = memberGridData.gridSchema.columns;
      this.reportFor = memberGridData.gridSchema['reportFor'];
      if (this.reportSchema) {
        for (let i = 0; i < this.reportSchema.length; i++) {
          if (this.reportSchema[i].isKey === true) {
            this.tableKeyField = this.reportSchema[i].name;
            break;
          }
        }
      }
      this.layouts = memberGridData.memberLayouts.data;
      this.getInitialRecordCount();
    });

  }

  getRecords() {
    let dataURL;
    if (this.lazy === true) {
      this.createHashTable(this.reportSchema);
      this.newReport = true;
      this.updateToggleColumn(this.reportSchema);

    } else {
      if (sessionStorage.getItem(this.sessionKey) === JSON.stringify(this.reportQueryParam)) {
        return;
      }
      sessionStorage.setItem(this.sessionKey, JSON.stringify(this.reportQueryParam));
      dataURL = this.genericApiObject['search']
        + '?pagesize=' + this.totalRecords;
      this.dataService.getData(dataURL, []).subscribe((data) => {
        sessionStorage.setItem(this.sessionKey, '0');
        this.genericDataSet = Object.assign([], data.data);
        this.createHashTable(this.reportSchema);
        this.primeLoader = false;
        this.newReport = true;
        this.updateToggleColumn(this.reportSchema);

      },
        (err) => {
          sessionStorage.setItem(this.sessionKey, '0');
        }
      );
    }
  }

  getInitialRecordCount() {
    this.lazy = true;
    this.getRecords();
  }

  lazyLoadData(event: LazyLoadEvent) {
    if (this.need_query_param && (!this.reportQueryParam || (this.reportQueryParam && this.reportQueryParam.length === 0))) {
      return;
    }

    if (sessionStorage.getItem(this.sessionKey) === JSON.stringify(this.reportQueryParam)) {
      return;
    }

    sessionStorage.setItem(this.sessionKey, JSON.stringify(this.reportQueryParam));

    // console.log(`${this.selectedLayout.name}`);

    // works for datatable when grouping is not there
    this.paginatorFlag = true;
    this.primeLoader = true;
    const postObject = {};
    postObject['refreshCache'] = this.refreshCache;
    let filterKey = '';
    let sortKey = '';
    let dataURL;
    this.summary = {};
    // let globalFilter = ''
    const pageCount = (event['first'] / event['rows']) + 1;
    this.currentPageRowCount = event.rows;
    const matchMode = '';
    let fromFilterDialog = false;

    // from event of click->sort when we do this.
    this.sortField = event.sortField ? event.sortField : undefined;
    this.sortOrder = event.sortOrder ? event.sortOrder : undefined;

    const currentLayoutValues = JSON.parse(this.selectedLayout.layout);
    let defaultPageSize = event['rows'];
    if (!this.sortField) {
      // This event is triggered by ngInit.
      // Sort for single column that not put in to grup
      this.sortField = currentLayoutValues.sortField;
      this.sortOrder = currentLayoutValues.sortOrder;

      if (this.sortObject.field) {
        // We have something before, from grouping-grid back to regular one with sort stting.
        this.sortField = this.sortObject.field;
        this.sortOrder = this.sortObject.order;
      }

      if (this.sortField) {
        // when first time loading page, we put the sort in multisortmeta to generate the sort string.
        this.sortObject.multisortmeta = [{ field: this.sortField, order: this.sortOrder }];
        event.multiSortMeta = this.sortObject.multisortmeta;
      }

      if (!event.multiSortMeta || event.multiSortMeta.length === 0) {
        // if(this.sortField) {
        // when first time loading page, we put the sort in multisortmeta to generate the sort string.
        // this.sortObject.multisortmeta =[{field: this.sortField, order: this.sortOrder} ];
        // }
      }
      // this.filterBuilder.complexDateTimeList  = currentLayoutValues.complexDateTimeList;
    }

    if (!this.oneTime) {
      if (currentLayoutValues && currentLayoutValues.paginationRows) {
        defaultPageSize = currentLayoutValues.paginationRows;
      }

      this.oneTime = true;
    }

    if ((event.filters && (Object.keys(event.filters) && Object.keys(event.filters).length > 0))
      || (currentLayoutValues.complexDateTimeList && currentLayoutValues.complexDateTimeList.length > 0)) {

      // matchModes are not defined at back end currently.
      fromFilterDialog = (Object.keys(event.filters)[0] === FILTER_CONSTANTS.FROM_FILTER_DIALOG);

      if (!fromFilterDialog) {

        if (this.filterBuilder.level === 1) {
          // this.level1FilterRows = this.filterBuilder.filterRows;
          // this.filterBuilder.complexDateTimeList=[];
          const children = this.filterBuilder;
          const complexDateTimeList = this.filterBuilder.complexDateTimeList;
          this.filterBuilder = new FilterRoot();
          // this.filterBuilder.filterRows=[];
          this.filterBuilder.filterOperator = 'And';
          this.filterBuilder.children.push(children);
          this.filterBuilder.complexDateTimeList = complexDateTimeList;


        }
        if (!this.filterBuilder.level || this.filterBuilder.level === 0) {

          const filterDialogMadeFields = this.filterBuilder.filterRows.filter((row) => {
            return row.rowCreationSource === FILTER_CONSTANTS.FROM_FILTER_DIALOG;
          });

          this.filterBuilder.filterRows = [];
          if (filterDialogMadeFields && filterDialogMadeFields.length > 0) {
            this.filterBuilder.filterRows = this.filterBuilder.filterRows.concat(filterDialogMadeFields);
          }

        }

        // tslint:disable-next-line:forin
        for (const key in event.filters) {
          // create a filter builder object
          this.createFilterBuilderObject(this.filterBuilder, key, event.filters[key]['value'], event.filters[key]['matchMode']);
          const others = event.filters[key]['others'];
          if (others && others.length > 0) {
            // tslint:disable-next-line:forin
            for (const other of others) {
              this.createFilterBuilderObject(this.filterBuilder, key, other['value'], other['matchMode']);
            }
          }
        }

        if (this.filterBuilder.complexDateTimeList && this.filterBuilder.complexDateTimeList.length > 0) {
          // this.plugComplexDateTime(this.filterBuilder.complexDateTimeList);
          this.createComplexDateTree(this.filterBuilder.complexDateTimeList);
          // this.filterBuilder.complexDateTimeList = [];
        }


        filterKey = this.buildFilterString(true);
        // postObject['filtercolumn'] = filterKey;
        postObject['filterBuilder'] = filterKey;
        postObject['otherFilterBuilders'] = this.getOtherFilterBuildString();
        postObject['otherFilterConditions'] = this.getOtherFilterConditions();

      } else {
        filterKey = event.filters.FROMFILTERDIALOG.value;
        postObject['filterBuilder'] = filterKey;
        postObject['otherFilterBuilders'] = this.getOtherFilterBuildString();
        postObject['otherFilterConditions'] = this.getOtherFilterConditions();

        delete event.filters[FILTER_CONSTANTS.FROM_FILTER_DIALOG];

        //  we need filter object for non-complex objects
        this.filterObj = {};
        this.localfilters = {};
        const lf = this.localfilters;

        if (this.filterBuilder && this.filterBuilder.filterRows && this.filterBuilder.filterRows.length > 0) {

          // foreach
          this.filterBuilder.filterRows.forEach(el => {
            let foundInComplex = false;
            if (this.filterBuilder.complexDateTimeList) {
              for (let i = 0; i < this.filterBuilder.complexDateTimeList.length; i++) {
                if (this.filterBuilder.complexDateTimeList[i].columnName === el.key) {
                  foundInComplex = true;
                  break;
                }
              }
            }

            if (!foundInComplex) {
              const s: string = el.key;
              if (lf[s]) {
                const others = lf[s]['others'] || [];
                others.push({ value: el.value, matchMode: el.operand.value });
                lf[s] = Object.assign({}, lf[s], { ...lf[s], others: others })
              } else {
                lf[s] = { value: el.value, matchMode: el.operand.value };
              }
            }
          });
          // endof foreach

          this.filterObj = lf;
        }

        this.cd.detectChanges();
      }

      // checking the default checkbox to apply filter
      this.checked = true;

    } else {
      // if apply filter is checked
      if (this.checked) {
        if (this.filterBuilder) {
          const filterPayload = this.buildFilterString(true);
          if (filterPayload && filterPayload.length > 0) {
            filterKey = filterPayload;
            postObject['filterBuilder'] = filterKey;
            postObject['otherFilterBuilders'] = this.getOtherFilterBuildString();
            postObject['otherFilterConditions'] = this.getOtherFilterConditions();
          }
        } else {
          postObject['filterBuilder'] = null;
          postObject['otherFilterBuilders'] = null;
          postObject['otherFilterConditions'] = null;
        }
      }
    }


    // if(this.reportQueryParam && )
    postObject['bu_ids'] = this.visible_business_string;
    postObject['queryparam'] = this.reportQueryParam;

    // summary still working on old global filter logic (not applied on visible columns)
    // if (Object.keys(this.summaryConfig) && Object.keys(this.summaryConfig).length) {
    //   // this.getSummary(event.filters, event.globalFilter, (data) => {
    //   //   this.summary = {};
    //   //   // tslint:disable-next-line:forin
    //   //   for (const key in this.summaryConfig) {
    //   //     this.summary[key] = {
    //   //       type: this.summaryConfig[key],
    //   //       value: data.data[0][key]
    //   //     }
    //   //   }
    //   // })
    // }

    if (event.multiSortMeta && event.multiSortMeta.length > 0) {
      // sortKey = '&sortby=';

      for (let i = 0; i < event.multiSortMeta.length; i++) {
        if (i === 0) {
          sortKey = sortKey + event.multiSortMeta[i]['field'] +
            (event.multiSortMeta[i]['order'] === 1 ? '' : '||DESC');
        } else {
          sortKey = sortKey + '^' + event.multiSortMeta[i]['field'] +
            (event.multiSortMeta[i]['order'] === 1 ? '' : '||DESC');
        }

        // let localsort;

        // if (i === event.multiSortMeta.length - 1) {
        //   localsort = '^'
        //   // apply non-group sort
        //   if ((this.sortField) && (this.sortField !== '')) {
        //     let found = false;
        //     event.multiSortMeta.forEach(element => {
        //       if (element.field === this.sortField) {
        //         found = true;
        //       }
        //     });

        //     if (!found) {
        //       localsort = this.sortField;
        //       if (this.sortOrder === -1) {
        //         localsort = localsort + '||DESC^';
        //       }
        //     }
        //   }

        //   sortKey = sortKey + localsort;

        // } // end of applying non-group sort.

      }
      postObject['sortby'] = sortKey
      // console.log(sortKey);
    } else {
      // only local filter.
      if ((this.sortField) && (this.sortField !== '')) {
        let localsort = this.sortField;
        if (this.sortOrder === -1) {
          localsort = localsort + '||DESC^';
        } else {
          localsort = localsort + '^';
        }

        postObject['sortby'] = localsort;
      }
    }

    if (event.globalFilter && event.globalFilter.length > 0) {
      // globalFilter = '&globalfilter=' + event.globalFilter;
      postObject['globalfilter'] = event.globalFilter;
    }

    dataURL = this.genericApiObject['search'];

    this.setPostObjectColumns(postObject);

    postObject['pagecount'] = pageCount;
    postObject['pagesize'] = defaultPageSize; // event['rows'];
    this.cd.detectChanges();
    // console.dir(postObject);
    postObject['refreshCache'] = this.refreshCache;

    this.dataService.getData(dataURL, postObject).subscribe((data) => {
      sessionStorage.setItem(this.sessionKey, '0');
      this.totalRecords = data['totalCount'];
      this.genericDataSet = data.data;
      this.refreshCache = false;
      this.summary = data.columnSummary || {};
      if (!this.showEditColumn && this.columnArray && this.columnOptions) {
        this.columnArray = this.columnArray.filter(x => !x.type || x.type !== 'edit');
        this.columnOptions = this.columnOptions.filter(x => !x.value || !x.value.type || x.value.type !== 'edit');
      }
      this.primeLoader = false;

      // when search result coming
      this.onSeachFinish.emit(data);
      this.dataTable.previousRowIndex = -1;
    },
      (error) => {
        sessionStorage.setItem(this.sessionKey, '0');
        this.genericDataSet = [];
        this.primeLoader = false;
        this.paginatorFlag = false;
        throw (error);
      });
  }

  getOtherFilterConditions(): string[] {
    const str = [];
    for (let i = 0; i < this.otherFilterConditions.length; i++) {
      if (this.checkFilter(i)) {
        str.push(this.otherFilterConditions[i]);
      }
    }
    return str;
  }

  getOtherFilterBuildString(): string[] {
    const str = [];
    for (let i = 0; i < this.otherFilterObjects.length; i++) {
      if (this.checkFilter(i)) {
        str.push(this.buildFilterString(true, i));
      }
    }
    return str;
  }

  setPostObjectColumns(postObject) {
    const visibleColumns = [];
    if (this.columnArray) {
      for (let i = 1; i < this.columnArray.length; i++) {
        if (this.columnArray[i].display) {
          visibleColumns.push(this.columnArray[i].field);
        }
      }

      // Update the filter on each column
      if (this.checked) {
        this.updateFilterOnColumn(this.columnArray, this.filterBuilder);
      }
    }

    postObject['columns'] = visibleColumns;

  }

  lazyLoadGroup(event: LazyLoadEvent) {
    if (this.need_query_param && (!this.reportQueryParam || (this.reportQueryParam && this.reportQueryParam.length === 0))) {
      return;
    }

    // works for datatable when grouping is there
    this.paginatorFlag = true;
    this.primeLoader = true;
    const postObject = {};
    postObject['refreshCache'] = this.refreshCache;
    let filterKey = '';
    let sortKey = '';
    let dataURL;
    this.summary = {};
    // let globalFilter = ''
    const pageCount = (event['first'] / event['rows']) + 1;
    this.currentPageRowCount = event.rows;
    const matchMode = '';
    let fromFilterDialog = false;

    // from event of click->sort when we do this.
    this.sortField = event.sortField ? event.sortField : undefined;
    this.sortOrder = event.sortOrder ? event.sortOrder : undefined;

    const currentLayoutValues = JSON.parse(this.selectedLayout.layout);
    let defaultPageSize = event['rows'];
    if (!this.sortField) {
      // This event is triggered by ngInit.
      // Sort for single column that not put in to grup
      this.sortField = currentLayoutValues.sortField;
      this.sortOrder = currentLayoutValues.sortOrder;

      // First time load this page, we need to get a copy of this.
      // this.sortObject.field = this.sortField;
      // this.sortObject.order = this.sortOrder;

      this.sortObject.multisortmeta = event.multiSortMeta;
      if (this.sortObject.field) {
        this.sortField = this.sortObject.field;
        this.sortOrder = this.sortObject.order;
      }
      // this.filterBuilder.complexDateTimeList  = currentLayoutValues.complexDateTimeList;
    }

    if (!this.oneTimeForGroup) {
      if (currentLayoutValues && currentLayoutValues.paginationRows) {
        defaultPageSize = currentLayoutValues.paginationRows;
      }

      this.oneTimeForGroup = true;
    }

    if ((event.filters && (Object.keys(event.filters) && Object.keys(event.filters).length > 0))
      || (currentLayoutValues.complexDateTimeList && currentLayoutValues.complexDateTimeList.length > 0)) {

      // matchModes are not defined at back end currently.
      fromFilterDialog = (Object.keys(event.filters)[0] === FILTER_CONSTANTS.FROM_FILTER_DIALOG);

      if (!fromFilterDialog) {

        if (this.filterBuilder.level === 1) {
          // this.level1FilterRows = this.filterBuilder.filterRows;
          // this.filterBuilder.complexDateTimeList=[];
          const children = this.filterBuilder;
          const complexDateTimeList = this.filterBuilder.complexDateTimeList;
          this.filterBuilder = new FilterRoot();
          // this.filterBuilder.filterRows=[];
          this.filterBuilder.filterOperator = 'And';
          this.filterBuilder.children.push(children);
          this.filterBuilder.complexDateTimeList = complexDateTimeList;


        }
        if (!this.filterBuilder.level || this.filterBuilder.level === 0) {

          const filterDialogMadeFields = this.filterBuilder.filterRows.filter((row) => {
            return row.rowCreationSource === FILTER_CONSTANTS.FROM_FILTER_DIALOG;
          });

          this.filterBuilder.filterRows = [];
          if (filterDialogMadeFields && filterDialogMadeFields.length > 0) {
            this.filterBuilder.filterRows = this.filterBuilder.filterRows.concat(filterDialogMadeFields);
          }

        }

        // tslint:disable-next-line:forin
        for (const key in event.filters) {
          // create a filter builder object
          this.createFilterBuilderObject(this.filterBuilder, key, event.filters[key]['value'], event.filters[key]['matchMode']);
          const others = event.filters[key]['others'];
          if (others && others.length > 0) {
            // tslint:disable-next-line:forin
            for (const other of others) {
              this.createFilterBuilderObject(this.filterBuilder, key, other['value'], other['matchMode']);
            }
          }
        }

        if (this.filterBuilder.complexDateTimeList && this.filterBuilder.complexDateTimeList.length > 0) {
          // this.plugComplexDateTime(this.filterBuilder.complexDateTimeList);
          this.createComplexDateTree(this.filterBuilder.complexDateTimeList);
          // this.filterBuilder.complexDateTimeList = [];
        }


        filterKey = this.buildFilterString(true);
        // postObject['filtercolumn'] = filterKey;
        postObject['filterBuilder'] = filterKey;
        postObject['otherFilterBuilders'] = this.getOtherFilterBuildString();
        postObject['otherFilterConditions'] = this.getOtherFilterConditions();
      } else {
        filterKey = event.filters.FROMFILTERDIALOG.value;
        postObject['filterBuilder'] = filterKey;
        postObject['otherFilterBuilders'] = this.getOtherFilterBuildString();
        postObject['otherFilterConditions'] = this.getOtherFilterConditions();
        delete event.filters[FILTER_CONSTANTS.FROM_FILTER_DIALOG];

        // we need filter object for non-complex objects
        this.filterObj = {};
        this.localfilters = {};
        const lf = this.localfilters;

        if (this.filterBuilder && this.filterBuilder.filterRows && this.filterBuilder.filterRows.length > 0) {

          // foreach
          this.filterBuilder.filterRows.forEach(el => {
            let foundInComplex = false;
            if (this.filterBuilder.complexDateTimeList) {
              for (let i = 0; i < this.filterBuilder.complexDateTimeList.length; i++) {
                if (this.filterBuilder.complexDateTimeList[i].columnName === el.key) {
                  foundInComplex = true;
                  break;
                }
              }
            }

            if (!foundInComplex) {
              const s: string = el.key;
              if (lf[s]) {
                const others = lf[s]['others'] || [];
                others.push({ value: el.value, matchMode: el.operand.value });
                lf[s] = Object.assign({}, lf[s], { ...lf[s], others: others })
              } else {
                lf[s] = { value: el.value, matchMode: el.operand.value };
              }
            }
          });
          // endof foreach

          this.filterObj = lf;
        }

        this.cd.detectChanges();
      }

      // checking the default checkbox to apply filter
      this.checked = true;

    } else {
      // if apply filter is checked
      if (this.checked) {
        if (this.filterBuilder) {
          const filterPayload = this.buildFilterString(true);
          if (filterPayload && filterPayload.length > 0) {
            filterKey = filterPayload;
            postObject['filterBuilder'] = filterKey;
            postObject['otherFilterBuilders'] = this.getOtherFilterBuildString();
            postObject['otherFilterConditions'] = this.getOtherFilterConditions();

            // we need filter object for non-complex objects
            this.filterObj = {};
            this.localfilters = {};
            const lf = this.localfilters;

            if (this.filterBuilder && this.filterBuilder.filterRows && this.filterBuilder.filterRows.length > 0) {

              // foreach
              this.filterBuilder.filterRows.forEach(el => {
                let foundInComplex = false;
                if (this.filterBuilder.complexDateTimeList) {
                  for (let i = 0; i < this.filterBuilder.complexDateTimeList.length; i++) {
                    if (this.filterBuilder.complexDateTimeList[i].columnName === el.key) {
                      foundInComplex = true;
                      break;
                    }
                  }
                }

                if (!foundInComplex) {
                  const s: string = el.key;
                  if (lf[s]) {
                    const others = lf[s]['others'] || [];
                    others.push({ value: el.value, matchMode: el.operand.value });
                    lf[s] = Object.assign({}, lf[s], { ...lf[s], others: others })
                  } else {
                    lf[s] = { value: el.value, matchMode: el.operand.value };
                  }
                }
              });
              // endof foreach

              this.filterObj = lf;
            }

          }
        }
      }
    }


    // if(this.reportQueryParam && )
    postObject['bu_ids'] = this.visible_business_string;
    postObject['queryparam'] = this.reportQueryParam;

    // // summary still working on old global filter logic (not applied on visible columns)
    // if (Object.keys(this.summaryConfig) && Object.keys(this.summaryConfig).length) {
    //   this.getSummary(event.filters, event.globalFilter, (data) => {
    //     this.summary = {};
    //     // tslint:disable-next-line:forin
    //     for (const key in this.summaryConfig) {
    //       this.summary[key] = {
    //         type: this.summaryConfig[key],
    //         value: data.data[0][key]
    //       }
    //     }
    //   })
    // }


    if (event.multiSortMeta && event.multiSortMeta.length > 0) {
      // sortKey = '&sortby=';
      for (let i = 0; i < event.multiSortMeta.length; i++) {
        if (i === 0) {
          sortKey = sortKey + event.multiSortMeta[i]['field'] +
            (event.multiSortMeta[i]['order'] === 1 ? '' : '||DESC');
        } else {
          sortKey = sortKey + '^' + event.multiSortMeta[i]['field'] +
            (event.multiSortMeta[i]['order'] === 1 ? '' : '||DESC');
        }

        // if (i === event.multiSortMeta.length - 1) {
        //   localsort = '^'
        //   // apply non-group sort
        //   if ((this.sortField) && (this.sortField !== '')) {
        //     let found = false;
        //     event.multiSortMeta.forEach(element => {
        //       if (element.field === this.sortField) {
        //         found = true;
        //       }
        //     });

        //     if (!found) {
        //       localsort = this.sortField;
        //       if (this.sortOrder === -1) {
        //         localsort = localsort + '||DESC^';
        //       }
        //     }
        //   }

        //   sortKey = sortKey + localsort;

        // } // end of applying non-group sort.

      }
      postObject['sortby'] = sortKey
    } else {
      // only local filter.
      if ((this.sortField) && (this.sortField !== '')) {
        let localsort = this.sortField;
        if (this.sortOrder === -1) {
          localsort = localsort + '||DESC^';
        } else {
          localsort = localsort + '^';
        }

        postObject['sortby'] = localsort;
      }
    }

    if (event.globalFilter && event.globalFilter.length > 0) {
      // globalFilter = '&globalfilter=' + event.globalFilter;
      postObject['globalfilter'] = event.globalFilter;
    }

    dataURL = this.genericApiObject['search'];

    this.setPostObjectColumns(postObject);

    postObject['pagecount'] = pageCount;
    postObject['pagesize'] = defaultPageSize; // event['rows'];

    // if (currentLayoutValues && currentLayoutValues.paginationRows) {
    //   postObject['pagesize'] = currentLayoutValues.paginationRows;
    // }

    //////////////////////////////////////////////////////////////////////////////
    // aBove copy from lazyLoad
    //////////////////////////////////////////////////////////////////////////////

    this.primeLoader = true;
    this.updateDataSet(this.expandedRowGroups, event, (data) => {
      this.totalRecords = data.totalCount;
      this.genericDataSet = data.data;
      this.summary = data.columnSummary || {};
      try {
        this.groupSummaryTable = data.extra.groupSummary;
      } catch {
      }
      // this.getSummary(event.filters, event.globalFilter, (data2) => {
      //   // this.summary = {};
      //   // tslint:disable-next-line:forin
      //   for (const key in this.summaryConfig) {
      //     this.summary[key] = {
      //       type: this.summaryConfig[key],
      //       value: data2.data[0][key]
      //     }
      //   }
      //   this.primeLoader = false;
      //   this.cd.detectChanges();
      // })
      this.primeLoader = false;

      // when search result coming
      this.onSeachFinish.emit(data);
      this.dataTable.previousRowIndex = -1;
    });
  }


  convertStringToLowerCase(str) {
    return str ? str.toString().toLowerCase() : str;
  }

  updateOnColReorder(data) {
    if (this.fixed_header) {
      return;
    }
    const columnArray = [...this.columnArray];
    const dragTragetIndex = this.utilityService.getIndexFromArrayOfObjects(this.columnArray, 'field', data.columns[data.dropIndex].field);
    // tslint:disable-next-line:max-line-length
    let dropTragetIndex = 0;
    if (data.dropIndex === 0) {
    } else {
      dropTragetIndex = this.utilityService.getIndexFromArrayOfObjects(this.columnArray, 'field', data.columns[data.dropIndex - 1].field);
    }

    if (dropTragetIndex > dragTragetIndex) {
      this.columnArray.splice(dropTragetIndex, 0, this.columnArray.splice(dragTragetIndex, 1)[0]);

    } else {
      this.columnArray.splice(dropTragetIndex + 1, 0, this.columnArray.splice(dragTragetIndex, 1)[0]);

    }
    // this.clearFilter();
    this.setSelectedValueArrayOnReorder(columnArray);
    // this.fixColumnWidthOnCurrentLayout(this.getDefaultScreenWidth());
    this.cd.detectChanges();
  }

  setSelectedValueArrayOnReorder(columnArray) {
    const selectedValue = [...this.selectedValue]
    selectedValue.forEach((element, index) => {
      const fieldAtOldIndex = columnArray[index].field;
      const newIndex = this.findIndexInColumnArray(fieldAtOldIndex)['index'];
      this.selectedValue[newIndex] = element;
      if (this.selectedValue[index] && index !== newIndex && this.selectedValue[index] === selectedValue[index]) {
        this.selectedValue[index] = undefined;
      }
    })
  }

  fixColumnWidths() {
    setTimeout(() => {
      const columns = this.domHandler.find(this.el.nativeElement, 'span.ui-resizable-cell-data');
      if (columns) {
        for (let i = 0; i < columns.length; i++) {
          columns[i].style.width = columns[i].offsetWidth + 'px';
        }
      }
    }, 300);
  }

  updateFilterValue(field, event, fieldType, index) {
    // tslint:disable-next-line:max-line-length
    const isEnteredValueValid: boolean = event.target ? this.validateFilterQuery(event.target.value, fieldType) : this.validateFilterQuery(event.value, fieldType);
    if (event.target && isEnteredValueValid) { // In case of phone format the value to 905-975-7272 format
      this.filterKeyValueHash[field] = event.target.value;
    } else if (event.value && isEnteredValueValid) {
      this.filterKeyValueHash[field] = event.value;
    } else { // Empty filtered query when isEnteredValueValid is false
      event.target ? event.target.value = '' : event.value = '';
    }

    if (event.keyCode === 13) { // Only on Enter
      if (event.target.value === null || event.target.value === '') {
        const list = [];
        this.filterBuilder.filterRows.forEach(element => {
          if (element.key === field) {
            // delete this.filterBuilderObj[field];
          } else {
            list.push(element);
          }
        });

        this.filterBuilder.filterRows = list;
        this.updateFilterOnColumn(this.columnArray, this.filterBuilder);
      }
    }

    if (event.keyCode === '13') { // Only on Enter
      if (event.target.value && event.target.value.length) {
        this.multiSelectFilter[index] = [];
      }
      if (fieldType === 'phone') { // Format the valid entered filtered query to desired format (905)-975-7272
        // tslint:disable-next-line:max-line-length
        event.target ? event.target.value = event.target.value.replace(/\(/g, '').replace(/\)/g, '').replace(/\./g, '-') : event.value = event.value.replace(/\(/g, '').replace(/\)/g, '').replace(/\./g, '-');
      } else if (fieldType === 'currency') { // Remove all . except first
        const enteredCurrencyValue = event.target.value ? event.target.value.split(',') : (event.value ? event.value.split(',') : []);
        const correctedCurrencyArray = [];
        if (enteredCurrencyValue && enteredCurrencyValue.length) {
          enteredCurrencyValue.forEach((item) => {
            const noDecimal = item.indexOf('.') ? item.split('.').join('') : item;
            let finalItem = item;
            if (item.indexOf('.') > -1) {
              finalItem = noDecimal.slice(0, item.indexOf('.')) + '.' + noDecimal.slice(item.indexOf('.'));
            }
            correctedCurrencyArray.push(finalItem);
          });
        }
        event.target.value ? event.target.value = correctedCurrencyArray.join(',') : event.value = correctedCurrencyArray.join(',');
      }
    }
    if (!this.checkFilter() && this.checkFilter(0)) {
      this.filterBuilder = this.otherFilterObjects[0];
      this.removeOtherFilter(0);
    }
  }

  keyDownPress() {
    this.saveButtonClicked = true;
  }

  onBlurMethod(event, field, fieldType, index, filterMatchMode) {
    // if (event.target.value === null || event.target.value === '') {
    //   return;
    // }

    // Add this saveLayoutButton as a placeholder css class to identify the save buttons from Jordan.
    // tslint:disable-next-line:max-line-length
    if (this.saveButtonClicked) {
      this.saveButtonClicked = false;
      const promise = new Promise<boolean>((resolve, reject) => {
        return this.onBlurTask(event, field, fieldType, index, filterMatchMode, resolve);
      });

      promise.then(r => this.saveLayout());
    } else {
      this.onBlurTask(event, field, fieldType, index, filterMatchMode, null);
    }
  }

  onBlurTask(event, field, fieldType, index, filterMatchMode, resolve): Promise<boolean> | boolean {
    const saved = event.keyCode;
    event.keyCode = 13;
    // (keyup)="updateFilterValue(col.field, $event, column.type, indx);
    // dt.filter($event,col.field,col.filterMatchMode, column.type);"
    if (event.target.value === null || event.target.value === '') {
      // this.updateFilterValue(field, event, fieldType, index);
    }
    this.updateFilterValue(field, event, fieldType, index);
    this.dataTable.filter(event, field, filterMatchMode, fieldType);

    if (resolve != null) {
      resolve(true)
    } else {
      return true;
    }
  }

  /**
   * Validate value entered manually in header filters
   * @param  {string} value
   * @param  {string} type
   * @returns boolean
   */
  validateFilterQuery(value: string, type: string): boolean {
    let regex; // value contains only characters specified
    if (type === 'number') { // Allow numbers 0-9 only
      regex = /^[0-9]+$/;
      return regex.test(value);
    } else if (type === 'string' || type === 'notes') { // Do not allow ` ' [ | characters as they fail in SQL injection
      // regex = /[`'[|]/;
      regex = /[]/;
      return !regex.test(value);
    } else if (type === 'phone') { // Allow - () . , and numbers only
      regex = /[-().0-9,]+$/;
      return regex.test(value);
    } else if (type === 'currency' || type === 'percent') { // Allow numbers 0-9 and . only
      // regex = /(?:\+|\-|\$)?\d{1,}(?:\,?\d{3})*(?:\.\d+)?%?/; // Allow floating point numbers only
      regex = /^[0-9.]+$/;
      return regex.test(value);
    } else {
      return true;
    }
  }

  setFilterValueIntoCurrentEvent(col, event, index) {
    col.filterMatchMode = event.value;
    event.value = this.filterKeyValueHash[col.field];
    if (!event.value && this.multiSelectFilter[index] && this.multiSelectFilter[index].length > 0) {
      event.value = this.multiSelectFilter[index];
    }

    // When setting isBlank or IsNotBlank, triggger the update.
    if (this.filterBuilder != null && this.filterBuilder.filterRows != null) {
      if (col.filterMatchMode === 'isBlank' || col.filterMatchMode === 'isNotBlank') {
        // tslint:disable-next-line:no-shadowed-variable
        const event = {};
        event['value'] = '';
        this.customDataTable.filter(event, col.field, col.filterMatchMode, null);
        return;
      }
    }

    // When changing from 'isBlank'/'isNotBlank' to others, remove the filter.
    let indexOfElement = -1;
    for (let i = 0; i < this.filterBuilder.filterRows.length; i++) {
      const item = this.filterBuilder.filterRows[i];
      if (item.key === col.field && (item.operand.value === 'isBlank' || item.operand.value === 'isNotBlank')) {
        indexOfElement = i;
        break;
      }
    }

    if (indexOfElement >= 0) {
      this.filterBuilder.filterRows.splice(indexOfElement, 1);
      // tslint:disable-next-line:no-shadowed-variable
      const event = {};
      event['value'] = '';
      this.filterType[index] = col.filterMatchMode;
      this.customDataTable.filter(event, col.field, col.filterMatchMode, null);
      return;
    }

    if (this.filterBuilder != null && this.filterBuilder.filterRows != null) {
      this.filterBuilder.filterRows.forEach(element => {
        if (element.key === col.field) {
          event['datevalue'] = element.value;
        }
      });
    }
  }

  isBlankOrIsNotBlank(field) {
    const columns = this.filterBuilder.filterRows.filter(x => x.key === field);
    if (columns && columns.length === 1) {
      if (columns[0].operand.value === 'isBlank' || columns[0].operand.value === 'isNotBlank') {
        return true;
      }
    }

    return false;
  }

  getFilterString(key) {
    const filteredCol = this.columnArray.filter((col) => {
      return col['field'] === key;
    });
    // tslint:disable-next-line:max-line-length
    if (filteredCol[0]['type'] === 'number' || filteredCol[0]['type'] === 'currency' || filteredCol[0]['type'] === 'percent' || filteredCol[0]['type'] === 'boolean') {
      return 'equals'
    } else {
      return 'contains'
    }
  }

  applyBoldFontOnColumn(column) {

    if (column.Style) {
      const jsonObject = column.Style;
      // var jsonObject : any = JSON.parse(value)

      if (jsonObject['font-weight']) {
        return jsonObject['font-weight'].indexOf('bold') > -1;
      } else {
        return false;
      }
    } else {
      return false;
    }
  }


  /**
   * To save a field of notes type when edited inline
   * @param  {} rowData
   */
  saveInlineEditedNote(rowData) {
    this.editComplete(rowData);
  }

  getColumnHeaderForFilterDialog(column) {
    if (this.columnArray) {
      for (let i = 0; i < this.columnArray.length; i++) {
        if (this.columnArray[i].field === column) {
          return this.columnArray[i].label;
        }
      }
    }
  }

  setRowClass(rowData, rowIndex) {
    const classToBeApplied = '';
    if (!rowData) { return ''; }
    if (rowData['contact_login_enabled']) {
      return 'text-bold';
    } if (rowData['workorder_schedule_date']) {
      return 'back_green';
    } if (rowData['hold'] === true && (this.gridName === 'MasterWorkOrderV2' || this.gridName === 'MasterWorkOrder')) {
      return 'onHold';
    } if (rowData['sc'] === true && (this.gridName === 'MasterWorkOrderV2' || this.gridName === 'MasterWorkOrder')) {
      return 'serviceCall';
    } else {
      return ''
    }
  }

  // Remove group from 'expandedRowGroups' when top group is collapsed
  onRowGroupCollapse(event) {

    this.expandedRowGroups = [];
    this.primeLoader = true;
    // this.expandedRowGroups.splice(0,1);
    this.updateDataSet(this.expandedRowGroups, event.lazyLoadData, (data) => {
      this.totalRecords = data.totalCount;
      this.genericDataSet = data.data;
      this.summary = data.columnSummary || {};
      // tslint:disable-next-line:forin
      // for (const key in this.summaryConfig) {
      //   this.groupSummary[key] = {
      //     type: this.summaryConfig[key],
      //     value: event.row.metadata[0]
      //   }
      // }
      // this.primeLoader = false;
      // this.cd.detectChanges();
      try {
        this.groupSummaryTable = data.extra.groupSummary;
      } catch {
      }
      this.primeLoader = false;
    });

  }

  // Remove group from 'expandedRowGroups' when nested group is collapsed
  onNestedGroupCollapse(event) {
    this.expandedRowGroups.splice([event.level], 1);
    this.primeLoader = true;
    this.updateDataSet(this.expandedRowGroups, event.lazyLoadData, (data) => {
      this.totalRecords = data.totalCount;
      this.genericDataSet = data.data;
      this.summary = data.columnSummary || {};
      try {
        this.groupSummaryTable = data.extra.groupSummary;
      } catch {
      }
      // tslint:disable-next-line:forin
      // for (const key in this.summaryConfig) {
      //   this.groupSummary[key] = {
      //     type: this.summaryConfig[key],
      //     value: event.row.metadata[0]
      //   }
      // }
      // this.primeLoader = false;
      // this.cd.detectChanges();
      // this.primeLoader = false;
      this.primeLoader = false;
    });
    //  this.cd.detectChanges();
  }

  onNestedGroupExpanded(event) {
    this.primeLoader = true;
    this.groupSummary = {};
    this.expandedRowGroups = [];
    this.cd.detectChanges();
    // If event label is last in the groupArray
    if (this.groupArray) {
      for (let i = 0; i < this.groupArray.length; i++) {
        if (i > event.level) {
          break;
        }
        let expandedRowObject = {};
        expandedRowObject = {
          group: event.row[this.groupArray[i]],
          label: this.groupArray[i]
        }
        this.expandedRowGroups.push(expandedRowObject);
      }
    }

    this.updateDataSet(this.expandedRowGroups, event.lazyLoadData, (data) => {
      this.totalRecords = data.totalCount;
      this.genericDataSet = data.data;
      this.summary = data.columnSummary || {};

      try {
        this.groupSummaryTable = data.extra.groupSummary;
      } catch {
      }

      // tslint:disable-next-line:forin
      for (const key in this.summaryConfig) {
        this.groupSummary[key] = {
          type: this.summaryConfig[key],
          value: event.row.metadata[0]
        }
      }
      this.primeLoader = false;
      this.cd.detectChanges();
      this.copyWidthToNestedGroup();
    });
    // this.cd.detectChanges();
    // this.expandedRowGroups.splice([event.level],1);
  }


  filterRecordsBetweenDates(event) {
    //
    // this.filterBuilder.complexDateTimeList = event.complexDatetimeObjects;
    if (!this.filterBuilder.complexDateTimeList) {
      this.filterBuilder.complexDateTimeList = [];
    }

    if (event.complexDatetimeObjects && event.complexDatetimeObjects.length !== 0) {
      const columnName = event.complexDatetimeObjects[0].columnName;
      // Remove existing ones with the same columnName.
      const newArray = this.filterBuilder.complexDateTimeList.filter(x => x.columnName !== columnName);
      event.complexDatetimeObjects.forEach(element => {
        newArray.push(element);
      });

      this.filterBuilder.complexDateTimeList = newArray;
    } else {
      //
      // Now the event has nothing: No 'From' and 'To'
      //

      // Get the related field
      const field = event.field;

      // Remove the related complex object from the list
      const newArray = this.filterBuilder.complexDateTimeList.filter(x => x.columnName !== field);
      event.complexDatetimeObjects.forEach(element => {
        newArray.push(element);
      });
      this.filterBuilder.complexDateTimeList = newArray;

      // Remove the content for that calender in the head column
      this.indexes.forEach(index => {
        const item = this.columnArray[index.index];
        if (item.field === field) {
          // Get the related index object by index & reset
          index.Calendar.writeValue('');
        }
      });
    }
    //
    const currentLayoutValues = JSON.parse(this.selectedLayout.layout);
    currentLayoutValues.complexDateTimeList = this.filterBuilder.complexDateTimeList;

    if (this.filterBuilder.complexDateTimeList && this.filterBuilder.complexDateTimeList.length === 0) {
      this.replaceAndPlugRootMostChildren(this.filterBuilder, event.field, new FilterRoot());
    }


    this.customDataTable.filter(event, event.field, 'complexDateTime', 'date');
  }

  getHyperlinkUrl(rowObject, column, urlKey, urlParamKey, mobileUrlKey = 'mobileHyperlinkUrl') {
    //  const reportFor = '';
    const finalUrl = '';
    const paramsArray: string[] = column[urlParamKey] && column[urlParamKey].length ? column[urlParamKey].split(',') : [];

    if (column[urlKey] && column[urlKey].length) {
      const hyperlink_mobile = String(column[mobileUrlKey]);
      let hyperlink = String(column[urlKey]);
      if (this.cs.isSm && hyperlink_mobile) {
        hyperlink = hyperlink_mobile;
      }
      let original_hyperlink = hyperlink;
      let newtab = false;
      let is_command = false;
      CONFIG.LOG(hyperlink, 'hyper link in datatable');
      // run command
      if (hyperlink && hyperlink.startsWith('[C]')) {
        is_command = true;
        hyperlink = hyperlink.substr(3);
        original_hyperlink = hyperlink.replace(/\{/g, '').replace(/\}/g, '');
      }
      // open new tab
      if (hyperlink && hyperlink.startsWith('[B]')) {
        newtab = true;
        hyperlink = hyperlink.substr(3);
      }
      if (paramsArray && paramsArray.length) {
        paramsArray.forEach((param) => {
          const searchParam = '{{' + param + '}}';
          hyperlink = hyperlink.replace(searchParam, rowObject[param]);
        })
      }
      if (!is_command) {
        if (hyperlink.includes('.aspx')) {
          this.winRef.boingNesi1(hyperlink, this.reportFor, null, newtab);
        } else {
          this.winRef.boing(hyperlink, this.reportFor, 1600, 960, newtab);
        }
      } else {
        // base on config file pass params to cell click event.
        // look at VendorHomSearch vendor_name column's setting.
        const e = { field: column.field, data: {} };
        const params = original_hyperlink.split(',');
        const values = hyperlink.split(',');
        params.forEach((x, i, array) => {
          e.data[x] = values[i];
        });
        this.oncellclick.emit(e);
      }
    } else if (!(column.hyperlinkUrl && column.hyperlinkUrl.length)) {
      const urlToOpen = rowObject[column.field].replace('www.', 'http://');
      const b_win = window.open(urlToOpen, null, 'height=1035,width=800');
      b_win.focus();
    }
  }

  setMultiSelectFilter(field, matchMode) {
    const colObject = this.findIndexInColumnArray(field);
    const index = colObject['index'];
    const column = colObject['column'];
    if (column.type !== 'date') {
      if (this.multiSelectFilter[index] && this.filterObj[column.field]) {
        if (!Array.isArray(this.filterObj[column.field].value)) {
          if (this.multiSelectFilter[index].length) {
            this.selectedValue[index] = undefined;
            this.filterKeyValueHash[column.field] = undefined;
            const event = {};

            if (this.multiSelectFilter[index] && this.multiSelectFilter[index].length > 1) {
              event['value'] = this.multiSelectFilter[index];
              this.filterType[index] = 'in';
            } else if ((this.multiSelectFilter[index] && this.multiSelectFilter[index].length === 1)) {
              event['value'] = this.multiSelectFilter[index][0];
              this.filterKeyValueHash[column.field] = this.multiSelectFilter[index][0];
              this.filterType[index] = 'equals';
            }
            this.customDataTable.filter(event, column.field, matchMode, null);
          }
        } else {
          // if (this.multiSelectFilter[index].join('') !== this.filterObj[column.field].value.join('')) {
          const event = {};
          this.filterType[index] = 'equals';

          if (this.multiSelectFilter[index] && this.multiSelectFilter[index].length > 1) {
            event['value'] = this.multiSelectFilter[index];
            this.filterType[index] = 'in';
          } else if (this.multiSelectFilter[index] && this.multiSelectFilter[index].length === 1) {
            event['value'] = this.multiSelectFilter[index][0];
            this.filterKeyValueHash[column.field] = this.multiSelectFilter[index][0];
          } else {
            event['value'] = [];
            this.selectedValue[index] = undefined;
            this.filterKeyValueHash[column.field] = undefined;
            this.filterObj[column.field].value = undefined;
          }
          this.customDataTable.filter(event, field, matchMode, null);
          // }
        }
      } else if (this.multiSelectFilter[index] && !this.filterObj[column.field]) {
        const event = {};
        if (this.multiSelectFilter[index].length > 1) {
          event['value'] = this.multiSelectFilter[index];
          this.filterType[index] = 'in';
        } else if (this.multiSelectFilter[index].length === 1) {
          event['value'] = this.multiSelectFilter[index][0];
          this.filterKeyValueHash[column.field] = this.multiSelectFilter[index][0];
          this.filterType[index] = 'equals';
        }
        this.customDataTable.filter(event, column.field, matchMode, null);
      }
    }
  }

  get showEditColumn(): boolean {
    const list = this.genericDataSet.filter(x => x['edit'] && Number(x['edit']) === 1)
    return list.length > 0;
    // return true;
  }

  setSelectedValue(event, field) {
    const index = this.findIndexInColumnArray(field)['index'];
    this.multiSelectFilter[index] = event.value;
  }

  // tslint:disable-next-line:member-ordering
  private _newGridObject: MemberLayout = new MemberLayout();

  onPopulateGridEvent(event) {
    this.sortObject = { order: 1, field: null, multisortmeta: [] };
    this.dataTable.sortObject = { order: 1, field: null, multisortmeta: [] };

    this.initialiazedOnClumnWidth = false;
    this.currentColumnWidths = [];

    this.overlayPanel.hide();
    if (event.deletedIndex && event.deletedIndex !== 0) {
      if (event.layout.id && (event.layout.id !== this.selectedLayout.id)) {
        this.selectedLayout = event.layout;
        this.applyLayout();
      }
    } else {
      if (event.layout.id && (event.layout.id === this.selectedLayout.id)) {
        // this.store.dispatch(new fromMessage.PushInfoMessage('Layout already selected'));
      } else {
        this.clearFilter();
        this.selectedLayout = event.layout;
        setTimeout(() => {
          this.applyLayout();
        }, 1000);
      }
    }
  }

  /**
   * set default page
   */

  showsetDefaultPage(): void {
    CONFIG.LOG(this.pageId, 'setting default page to');
    this.processing = true;
    if (this.pageId) {

      this.bs.saveDefaultPage(this.pageId).subscribe((response) => {
        this.store.dispatch(new fromMessage.PushSuccessMessage('Default page set to ' + response));
        this.processing = false;
      }, (err) => {
        this.store.dispatch(new fromMessage.PushErrorMessage('Error while setting default page'));
        this.processing = false;
      }, () => {
        this.processing = false;
      });
    }
  }

  createDateFilterString(filterObject: any, key: String) {
    const filterStringInitial = filterObject.value.split('`');
    if (!filterStringInitial) {
      return;
    }
    const filterStringFinal = [];
    filterStringInitial.forEach((element, index) => {
      // tslint:disable-next-line:max-line-length
      const value = element.replace('^', ' AND ').replace(`||${filterObject.matchMode}`, `${(index === filterStringInitial.length - 1) ? '' : ' Or '}`).replace(`${key}||`, ` [${this.getColumnFromColumnArray(key).label}] Is `);
      filterStringFinal.push(value);
    });
    return filterStringFinal.join().replace(/,/g, '')
  }

  createFilterString(key: string, matchMode: string, value: any): string {
    const column = this.getColumnFromColumnArray(key);
    const columnType = column.type;
    let matchModeArray: Object[];
    const matchModeKey = matchMode ? matchMode : this.getFilterString(key);
    let matchModeLabel: string;
    let valueString: any;
    if (columnType === 'percent') {
      if (Array.isArray(value)) {
        value = value.map(element => {
          return element = this.percentPipe.transform(Number(element), column.format.format);
        });
        valueString = value.join(',')
      } else {
        valueString = this.percentPipe.transform(value, column.format.format);
      }
    } else if (columnType === 'currency') {
      if (Array.isArray(value)) {
        value = value.map(element => {
          return element = this.currencyPipe.transform(Number(element), column.format.format, 'symbol-narrow', column.format.format);
        });
        valueString = value.join(',')
      } else {
        valueString = this.currencyPipe.transform(value, column.format.currency, 'symbol-narrow', column.format.format);
      }
    } else if (columnType === 'date') {
      if (Array.isArray(value)) {
        value = value.map(element => {
          return element = this.datePipe.transform(element, column.format.format);
        });
        valueString = value.join(',')
      } else {
        valueString = this.datePipe.transform(value, column.format.format)
      }
    } else {
      valueString = columnType !== 'boolean' ? `'${value}'` : (value === '0' ? 'Unchecked' : (value === '1' ? 'Checked' : 'All'));
    }
    if (columnType === 'number' || columnType === 'date' || columnType === 'currency' || columnType === 'percent') {
      matchModeArray = this.filterMatchModeNumber;
    } else {
      matchModeArray = this.filterMatchModeString;
    }
    if (matchModeArray) {
      for (let i = 0; i < matchModeArray.length; i++) {
        if (matchModeArray[i]['value'] === matchModeKey) {
          matchModeLabel = matchModeArray[i]['label'];
          break;
        }
      }
    }
    return `[${this.getColumnHeaderForFilterDialog(key)}]` + ' ' + matchModeLabel + ' ' + valueString;
  }



  /**
   * Switch row to bulk edit mode when row is selected && bulk editing is enabled && row is selected
   * @param  {} event
   */
  OnRowSelect(event) {
    if (this.unselectedRow) {
      this.switchRowToViewMode();
    }
    this.unselectedRow = event.originalEvent.currentTarget.children;
    if (this.bulkEnable) {
      const templateColumns = event.originalEvent.currentTarget.children;
      const selectedRowClasses = 'ui-datatable-even ui-widget-content ng-star-inserted ui-state-highlight';
      const colEditModeClasses = 'ui-editable-column ng-star-inserted ui-cell-editing';
      templateColumns[0].parentElement.setAttribute('class', selectedRowClasses);
      if (templateColumns) {
        for (let i = 0; i < templateColumns.length; i++) {
          if (templateColumns[i].className && templateColumns[i].className.indexOf('ui-editable-column') > -1) {
            templateColumns[i].setAttribute('class', colEditModeClasses);
          }
        }
      }
    }
  }

  /**
   * Switch row to view mode from bulk editing mode when row is unSelected
   * @param  {} event=this.unselectedRow
   */
  switchRowToViewMode() {
    const tableColumns = this.unselectedRow
    const colViewModeClasses = 'ui-editable-column ng-star-inserted';
    const unSelectedRowClasses = 'ui-datatable-even ui-widget-content ng-star-inserted';
    if (tableColumns && tableColumns.length) {
      tableColumns[0].parentElement.setAttribute('class', unSelectedRowClasses);
      for (let i = 0; i < tableColumns.length; i++) {
        if (tableColumns[i].className && tableColumns[i].className.indexOf('ui-cell-editing') > -1) {
          tableColumns[i].setAttribute('class', colViewModeClasses);
        }
      }
    }
  }

  switchRowToViewMode2() {
    const tableColumns = this.unselectedRow
    const colViewModeClasses = 'ui-editable-column ng-star-inserted';
    if (tableColumns && tableColumns.length) {
      for (let i = 0; i < tableColumns.length; i++) {
        if (tableColumns[i].className && tableColumns[i].className.indexOf('ui-cell-editing') > -1) {
          tableColumns[i].setAttribute('class', colViewModeClasses);
        }
      }
    }
  }

  showInvoiceButton(rowData) {
    if (this.gridId === 'MasterWorkOrder' && rowData.status && rowData.status.toLowerCase().indexOf('invoiced') > -1) {
      return true;
    } else if (this.gridId === 'MasterOutstandingInvoicesGrid') {
      return true;
    } else {
      return false;
    }
  }

  showDeleteButton(rowData) {
      return this.gridId === 'ApplicantHomeGrid' && rowData.status == "New";
  }

  inComplexDateTimeList(filterBuilder: FilterRoot, key: string, value: any, matchMode: any, operator?: string) {
    const yes = true;

    if (filterBuilder.complexDateTimeList) {
      for (let i = 0; i < filterBuilder.complexDateTimeList.length; i++) {
        if (filterBuilder.complexDateTimeList[i].columnName === key) {
          return false;
        }
      }
    }

    return yes;
  }

  createFilterBuilderObject(filterBuilder: FilterRoot, key: string, value: any, matchMode: any, operator?: string) {

    if (matchMode !== 'complexDateTime' && this.inComplexDateTimeList(filterBuilder, key, value, matchMode, operator)) {

      if (!filterBuilder) {
        filterBuilder = new FilterRoot();
      }

      if (!operator && !filterBuilder.filterOperator) {
        filterBuilder.filterOperator = 'And';
      }
      const filterRow: FilterRow = new FilterRow();

      const column = this.getColumnInformationByKey(key);
      filterRow.type = this.getColumnTypeByColumn(column);

      filterRow.column = this.getColumnHeaderForFilterDialog(key);
      filterRow.key = key;
      filterRow.reservedKey = key;

      filterRow.operand = { label: '', value: '' };
      // filterRow.operand.label = (matchMode) ? matchMode : this.getFilterString(key);
      filterRow.operand.value = (matchMode) ? matchMode : this.getFilterString(key);
      let filterMatchmodeArray: Array<any>;
      if (filterRow.type === 'number' || filterRow.type === 'percent' || filterRow.type === 'currency') {
        filterMatchmodeArray = this.filterMatchModeNumber;
      } else if (filterRow.type === 'date') {
        filterMatchmodeArray = this.filterMatchModeDate;
      } else {
        filterMatchmodeArray = this.filterMatchModeString;
      }

      const filterMatchMode = filterMatchmodeArray.filter((mode) => {
        return mode.value === filterRow.operand.value;
      })
      if (filterMatchMode && filterMatchMode.length > 0) {
        filterRow.operand.label = filterMatchMode[0]['label'];
      }

      filterRow.placeholder = this.getPlaceholderByColumnType(column);
      if (Array.isArray(value)) {
        let newValue = value.join(',');

        // if (filterRow.type === 'string' || filterRow.type === 'dropdown') {
        newValue = newValue.split(',').map(function (str) {
          return '\'' + str + '\'';
        }).join(',')
        // }
        if (matchMode === 'Is none of' || matchMode === 'not in' || matchMode === 'notEquals') {
          filterRow.operand.label = 'Is none of';
          filterRow.operand.value = 'not in';
        } else {
          filterRow.operand.label = 'Is any of';
          filterRow.operand.value = 'in';
        }

        filterRow.value = newValue;
      } else {
        filterRow.value = value;

        if (filterRow.operand.value === 'in') {
          filterRow.operand.label = 'Equals';
          filterRow.operand.value = 'in';
        }
      }


      filterRow.rowCreationSource = FILTER_CONSTANTS.FROM_FILTER_GRID;

      if (filterRow.type === 'boolean' && filterRow.value === '') {
        // not added for dropdown list.
      } else {
        let found = false;
        if (filterBuilder.filterRows) {
          for (let i = 0; i < filterBuilder.filterRows.length; i++) {
            if (filterBuilder.filterRows[i].key === filterRow.key && filterBuilder.filterRows[i].value === filterRow.value) {
              found = true;
              // filterBuilder.filterRows[i] = null;

              filterBuilder.filterRows.splice(i, 1)
              break;
            }
          }
        }


        filterBuilder.filterRows.push(filterRow);

      } // try to add it in.
    }

  }


  IsFilterRowMultple(field: string) {
    const o = this.filterBuilder.filterRows.filter(x => x.key === field);
    if (!o) { return false; }
    const c = o.length;
    if (c === 0) {
      return false;
    } else if (c === 1) {
      return o[0].operand.label.indexOf('of') > -1;
    } else {
      return true;
    }
  }

  filterTooltip(field: string) {
    const o = this.filterBuilder.filterRows.filter(x => x.key === field);
    if (!o) { return null; }
    const c = o.length;
    if (c === 0) {
      return null;
    } else if (c === 1) {
      return '[' + o[0].column + '] ' + o[0].operand.label + ' "' + o[0].value + '"';
    } else {
      let tip = '';
      o.forEach((x, i, array) => {
        tip += '[' + x.column + '] ' + x.operand.label + ' "' + x.value + '" ';
        if (i < array.length - 1) {
          tip += this.filterBuilder.filterOperator + ' ';
        }
      });
      return tip;
    }
  }

  buildFilterString(createPayload?: boolean, index = -1) {

    let displayFilterString = '';
    this.filterString = '';
    let b = this.filterBuilder;
    if (index > -1) {
      b = this.otherFilterObjects[index];
    }
    displayFilterString = this.buildRecursiveString('', b, createPayload);
    // console.log(displayFilterString);
    return displayFilterString;
  }

  // tslint:disable-next-line:member-ordering
  private newFilterString = '';
  buildRecursiveString(fs: string, root: FilterRoot, createPayload?: boolean) {

    let rowFilterString = '';
    if (root.children && root.children.length > 0) {

      root.children.forEach((childRoot, index) => {
        this.filterString = this.buildRecursiveString(this.filterString, childRoot, createPayload);
        if (this.filterString !== '') {

          if (root.children.length > 1 && index === 0) {
            this.filterString = '(' + this.filterString;
          }
          if (index !== (root.children.length - 1)) {
            const rootFilterOperator = this.evaluateRootOperator(root);
            this.filterString += ') ' + rootFilterOperator + ' (';
            // this.filterString += ') ' + root.filterOperator + ' (';
          } else if (root.children.length > 1 && index === (root.children.length - 1)) {
            this.filterString = this.filterString + ')';

            if (root.filterOperator && root.filterOperator.toLowerCase().indexOf('not') > -1) {
              this.filterString = 'Not(' + this.filterString + ')';
            } else {
              this.filterString = '(' + this.filterString + ')';
            }
          }
        }

      });
    }
    root.filterRows.forEach((row, index) => {
      const column = createPayload ? '|' + row.key : '[' + row.column + ']';
      const operand = createPayload ? this.getOperandValueByColumnType(row) : row.operand.label;
      const value = row.value;
      if (row.useAliasing) {
        if (createPayload) {
          rowFilterString += column + operand;
        } else {
          rowFilterString += column + ' ' + operand;
        }
      } else {
        if (row.type === 'date' || row.type === 'dropdownDateTime') {
          if (createPayload) {
            rowFilterString += column + '^' + operand + '^' + value + '^';
          } else {
            rowFilterString += column + ' ' + operand + ' ' + value;
          }

        } else {

          if (createPayload) {
            rowFilterString += column + '^' + operand + '^' + value + '^';
          } else {
            // tslint:disable-next-line:max-line-length
            rowFilterString += column + ' ' + operand + ' \'' + (row.type === 'boolean' ? (value === '1' ? 'Checked' : (value === '' ? '' : 'Unchecked')) : value) + '\'';
          }

        }
      }

      if (index !== (root.filterRows && root.filterRows.length - 1)) {
        const rootFilterOperator = this.evaluateRootOperator(root);
        rowFilterString += ' ' + rootFilterOperator + ' ';
      } else {
        if (root.level === 0) {
          rowFilterString = '(' + rowFilterString + ')';
        } else {
          if (root.filterOperator && root.filterOperator.toLowerCase().indexOf('not') > -1) {
            rowFilterString = 'Not(' + rowFilterString + ')';
          } else {
            rowFilterString = '(' + rowFilterString + ')';
          }
        }


      }
    });

    if (root.children && root.children.length > 0 && root.filterRows.length > 0) {
      const rootFilterOperator = this.evaluateRootOperator(root);
      rowFilterString = ' ' + rootFilterOperator + ' ' + rowFilterString;
      // rowFilterString = ' ' + root.filterOperator + ' ' + rowFilterString;
    }


    this.filterString += rowFilterString;
    if (root.parentFilterRows && root.parentFilterRows.length > 0) {
      this.filterString = '(' + this.filterString + ')';
    } else if (root.level === 0) {
      if (root.filterOperator && root.filterOperator.toLowerCase().indexOf('not') > -1) {
        this.filterString = 'Not(' + this.filterString + ')';
      }
    }

    return this.filterString;

  }

  evaluateRootOperator(root: FilterRoot) {
    let rootOperator: string = root.filterOperator;
    if (root.filterOperator && root.filterOperator.toLowerCase() === 'not and') {
      rootOperator = 'And';
    }
    if (root.filterOperator && root.filterOperator.toLowerCase() === 'not or') {
      rootOperator = 'Or';
    }

    return rootOperator;

  }

  isChildrenFilterRowExists(childrenRoot: Array<FilterRoot>) {
    let exists = false;
    if (childrenRoot.length) {
      for (let index = 0; index < childrenRoot.length; index++) {
        const childRoot = childrenRoot[index];
        if (childRoot.filterRows && childRoot.filterRows.length > 0) {
          exists = true;
          break;
        }
      }
    }
    return exists;

  }
  getColumnInformationByKey(key: string) {
    return this.columnArray.filter((c) => {
      return c.field === key;
    })
  }

  getColumnTypeByColumn(column: any[]) {
    let colType = '';
    if (column && column.length > 0) {
      colType = column[0]['type']
    }
    return colType;
  }

  getPlaceholderByColumnType(column: any[]) {

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

  private getOperandValueByColumnType(row: FilterRow) {
    if (row.operand.value.toLowerCase() === 'not in') {
      return 'not in';
    }
    if ((row.operand.label.toLowerCase() === 'equals' || row.operand.label.toLowerCase() === 'is any of')
      && row.operand.value.toLowerCase() === 'in') {
      return 'in';
    }

    let matchedFilterValue = [];

    if (row.type === 'string' || row.type === 'notes' || row.type === 'phone') {
      matchedFilterValue = this.filterMatchModeString.filter((filterString) => {
        return filterString.label.toLowerCase() === row.operand.label.toLowerCase()
      })
    } else if (row.type === 'boolean') {
      matchedFilterValue = this.filterMatchModeBooelan.filter((filterString) => {
        return filterString.label.toLowerCase() === row.operand.label.toLowerCase()
      })
    } else if (row.type === 'date' || row.type === 'complexDateTime' || row.type === 'dropdownDateTime') {
      if (!row.useAliasing) {
        matchedFilterValue = this.filterMatchModeDate.filter((filterString) => {
          return (filterString.label.toLowerCase() === row.operand.label.toLowerCase())
        });
      } else {
        return this.getDateStringByAlias(row);
      }
    } else {
      matchedFilterValue = this.filterMatchModeNumber.filter((mode) => {
        return mode.label.toLowerCase() === row.operand.label.toLowerCase()
      })
    }

    if (matchedFilterValue && matchedFilterValue.length > 0) {
      return matchedFilterValue[0]['value'];
    } else {
      return 'startsWith';
    }
  }

  getDateStringByAlias(row: FilterRow) {
    let aliasQuery = '';
    if (row.operand.label.indexOf('Yesterday') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMEYESTERDAY + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETODAY;
    } else if (row.operand.label.indexOf('Today') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETODAY + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETOMORROW;
    } else if (row.operand.label.indexOf('Tomorrow') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETOMORROW + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMEDAYAFTERTOMORROW;
    } else if (row.operand.label.indexOf('Last Week') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMELASTWEEK + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETHISWEEK;
    } else if (row.operand.label.indexOf('This Week') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETHISWEEK + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMENEXTWEEK;
    } else if (row.operand.label.indexOf('Next Week') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMENEXTWEEK + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETWOWEEKSAWAY;
    } else if (row.operand.label.indexOf('Next Year') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMENEXTYEAR + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_NEXTYEAR;
    } else if (row.operand.label.indexOf('This Year') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETHISYEAR + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMENEXTYEAR;
    } else if (row.operand.label.indexOf('Last Year') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LASTYEAR + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETHISYEAR;
    } else if (row.operand.label.indexOf('Next Month') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATETHANOREQUAL_LOCALDATETIMENEXTMONTH + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_NEXTMONTH;
    } else if (row.operand.label.indexOf('This Month') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETHISMONTH + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMENEXTMONTH;
    } else if (row.operand.label.indexOf('Last Month') > -1) {
      // tslint:disable-next-line:max-line-length
      aliasQuery = COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LASTMONTH + ' And |' + row.key + COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETHISMONTH;
    }


    return aliasQuery;
  }

  plugComplexDateTime(complexDatetimeObjects: Array<ComplexDateTimeObject>) {

    const root: FilterRoot = new FilterRoot();

    if (complexDatetimeObjects && complexDatetimeObjects.length === 1) {

      root.filterOperator = 'And';
      root.children = [];
      root.filterRows = this.filterBuilder.filterRows.filter((row) => {
        return row.type !== 'complexDateTime';
      })
      root.level = 0;
      // root.isDirty=false;

      this.convertAndAppendComplexDateTimeToFilterRow(root, complexDatetimeObjects[0]);

    } else {
      // check if already complex date is set at root level
      root.filterRows = this.filterBuilder.filterRows.filter((row) => {
        return row.type !== 'complexDateTime';
      })

      // if root filter rows already exists
      if (this.filterBuilder.filterRows && this.filterBuilder.filterRows.length > 0) {
        root.filterOperator = 'And';
        root.level = 0;
        root.filterRows = this.filterBuilder.filterRows;

        const childRoot: FilterRoot = new FilterRoot();
        childRoot.filterOperator = 'Or';
        childRoot.parentFilterRows = root.filterRows;
        childRoot.level = 1;

        complexDatetimeObjects.forEach((complexDateTime) => {
          const nextChildRoot: FilterRoot = new FilterRoot();
          nextChildRoot.filterOperator = 'And';
          nextChildRoot.level = 2;

          this.convertAndAppendComplexDateTimeToFilterRow(nextChildRoot, complexDateTime);
          nextChildRoot.parentFilterRows = childRoot.filterRows;
          childRoot.children.push(nextChildRoot);


        });

        if (this.filterBuilder.children.length > 0) {
          root.children = this.filterBuilder.children;
        }
        root.children.push(childRoot);

      } else { // if root rows does not exist
        root.filterOperator = 'Or';
        root.level = 1;
        // root.isDirty=false;
        complexDatetimeObjects.forEach((complexDateTime) => {
          const childRoot: FilterRoot = new FilterRoot();
          childRoot.filterOperator = 'And';
          childRoot.parentFilterRows = root.filterRows;
          childRoot.level = 2;
          this.convertAndAppendComplexDateTimeToFilterRow(childRoot, complexDateTime);

          root.children.push(childRoot);

        });

        if (this.filterBuilder.children.length > 0) {
          this.filterBuilder.children.push(root);
        }
      }
      root.complexDateTimeList = complexDatetimeObjects;
    }
    if (this.filterBuilder.children.length === 0) {
      this.filterBuilder = root;
    }
  }


  private groupByColumns(complexDatetimeObjects: Array<ComplexDateTimeObject>) {
    const listOfList = new Array<Array<ComplexDateTimeObject>>();
    if (!complexDatetimeObjects) {
      return [];
    }

    complexDatetimeObjects.forEach(element => {
      const list = new Array<ComplexDateTimeObject>();
      list.push(element);

      // found the list
      const found = listOfList.filter(item => (item.filter(c => c.columnName === element.columnName) ?
        item.filter(c => c.columnName === element.columnName).length : 0) > 0);
      // found.push(element);
      //

      listOfList.push(list);
    });

    return listOfList;
  }

  private createComplexDateTree(complexDatetimeObjects: Array<ComplexDateTimeObject>) {

    const listOfList = this.groupByColumns(complexDatetimeObjects);

    listOfList.forEach(element => {
      this.createComplexDateTreeForEachField(element);
    });

    const i = 1;
  }

  private createComplexDateTreeForEachField(complexDatetimeObjects: Array<ComplexDateTimeObject>) {
    const root: FilterRoot = new FilterRoot();

    if (complexDatetimeObjects && complexDatetimeObjects.length === 1) {
      root.children = [];
      root.level = 0;
      root.alias = complexDatetimeObjects[0].alias;
      root.isComplexDateTime = false;
      this.convertAndAppendComplexDateTimeToFilterRow(root, complexDatetimeObjects[0]);

      this.plugComplexDateTree(root, complexDatetimeObjects);
    } else {
      const childRoot: FilterRoot = new FilterRoot();
      childRoot.filterOperator = 'Or';
      childRoot.level = 1;
      childRoot.isComplexDateTime = true;
      complexDatetimeObjects.forEach((complexDateTime) => {
        const nextChildRoot: FilterRoot = new FilterRoot();
        nextChildRoot.columnName = complexDateTime.columnName;
        nextChildRoot.alias = complexDateTime.alias;
        nextChildRoot.filterOperator = 'And';
        nextChildRoot.level = 2;
        nextChildRoot.isComplexDateTime = true;
        this.convertAndAppendComplexDateTimeToFilterRow(nextChildRoot, complexDateTime);
        nextChildRoot.parentFilterRows = childRoot.filterRows;
        childRoot.children.push(nextChildRoot);

      });

      root.children.push(childRoot);
      this.plugComplexDateTree(childRoot, complexDatetimeObjects);
    }


  }


  private plugComplexDateTree(childRoot: FilterRoot, complexDateTime: Array<ComplexDateTimeObject>) {

    if (childRoot.level === 0) {
      this.replaceAndPlugRootMostChildren(this.filterBuilder, complexDateTime[0].columnName, childRoot);
      this.filterBuilder.children = this.getChildrenExceptComplexDate();
    } else if (childRoot.level === 1) {
      // if already have rows at level 0 then append as child
      if (this.filterBuilder.filterRows && this.filterBuilder.filterRows.length > 0) {
        this.replaceAndPlugRootMostChildren(this.filterBuilder, complexDateTime[0].columnName, childRoot);
        this.filterBuilder.children = this.getChildrenExceptComplexDate();
        this.filterBuilder.children.push(childRoot);
      } else {
        // if no rows at level 0 then make childRoot as main root
        this.filterBuilder = childRoot;
      }


    }

  }


  replaceAndPlugRootMostChildren(root: FilterRoot, key: string, childRoot: FilterRoot) {
    if (this.filterBuilder.filterRows && this.filterBuilder.filterRows.length === 0) {
      root.filterOperator = 'And';
      root.children = [];
      root.level = 0;
    } else {
      root.filterOperator = this.filterBuilder.filterOperator;
      root.children = this.filterBuilder.children;
      root.level = 0;
    }

    // find and replace keys at level 0
    root.filterRows = this.filterBuilder.filterRows.filter((row) => {
      return row.key !== key;
    });

    root.filterRows = root.filterRows.concat(childRoot.filterRows);
    this.filterBuilder.filterRows = root.filterRows;
  }

  getChildrenExceptComplexDate() {
    const children = this.filterBuilder.children.filter((child) => {
      return !child.isComplexDateTime;
    });
    return children;
  }

  convertAndAppendComplexDateTimeToFilterRow(root: FilterRoot, complexDateTime: ComplexDateTimeObject) {
    const row1: FilterRow = new FilterRow();

    if (complexDateTime.condition1) {
      row1.column = this.getColumnHeaderForFilterDialog(complexDateTime.columnName);
      row1.key = complexDateTime.columnName;
      row1.operand = { label: '', value: '' };
      row1.operand.label = complexDateTime.condition1.label;
      row1.operand.value = complexDateTime.condition1.value;
      row1.value = complexDateTime.condition1.displayValue;
      row1.type = complexDateTime.condition1.type;
      row1.placeholder = '<enter a value>';
      row1.reservedKey = row1.key;
      row1.alias = complexDateTime.alias;

      root.filterRows.push(row1);
    }

    if (complexDateTime.condition2) {
      const row2: FilterRow = new FilterRow();
      row2.column = this.getColumnHeaderForFilterDialog(complexDateTime.columnName);
      row2.key = complexDateTime.columnName;
      row2.operand = { label: '', value: '' };
      row2.operand.label = complexDateTime.condition2.label;
      row2.operand.value = complexDateTime.condition2.value;
      row2.value = complexDateTime.condition2.displayValue;
      row2.type = complexDateTime.condition2.type;
      row2.placeholder = '<enter a value>';
      row2.reservedKey = row2.key;
      row2.alias = complexDateTime.alias;

      root.filterRows.push(row2);
    }
  }


  private calculateChildRootDepth(root: FilterRoot) {

    if (root.children && root.children.length > 0) {
      root.children.forEach((child) => {
        this.calculateChildRootDepth(child);
      })
    } else {
      this.childRootDepth = (!root.level) ? 0 : root.level;
    }
  }

  private recreateFilterBuilder(root: FilterRoot, level: number) {
    const parentMostLevel = (!this.filterBuilder.level) ? 0 : this.filterBuilder.level;
    const rootLevel = (!root.level) ? 0 : root.level;

    if (level >= parentMostLevel) {

      if (rootLevel === level) {
        this.applyFilterBuilderCreateConditions(root);
      } else {
        root.children.forEach((child) => {
          this.recreateFilterBuilder(child, level);
        });
        this.recreateFilterBuilder(this.filterBuilder, level - 1)
      }
      // this.recreateFilterBuilder(this.filterBuilder, level - 1)
    }

  }

  private applyFilterBuilderCreateConditions(root: FilterRoot) {

    // evaluate about the complexdate whether a dirty one or not
    this.evaluateComplexDateTimeChildren(root);


    const newChildren: Array<FilterRoot> = [];
    let indicesToRemove = [];

    // having same operator
    let conditionOutcome = root.children.filter((child, index) => {
      if (child.children && child.children.length === 0 && child.filterOperator === root.filterOperator) {
        indicesToRemove.push(index);
        return child;
      }

    });

    // if same operator condition met
    if (conditionOutcome && conditionOutcome.length > 0) {
      // transfer all fiter rows to parent
      conditionOutcome.forEach((childCondition) => {
        if (childCondition.filterRows && childCondition.filterRows.length > 0) {
          root.filterRows = root.filterRows.concat(childCondition.filterRows);
        }
      });

      // remove the children as there is no need of those children
      this.deleteChildrenByIndices(root, indicesToRemove);
    }

    indicesToRemove = [];
    // having only one child filter row
    conditionOutcome = root.children.filter((child, index) => {
      if (child.children && child.children.length === 0 && child.filterRows.length === 1) {
        indicesToRemove.push(index);
        return child;
      }
    });

    if (conditionOutcome && conditionOutcome.length > 0) {
      // transfer all fiter rows to parent
      conditionOutcome.forEach((childCondition) => {
        root.filterRows = root.filterRows.concat(childCondition.filterRows);
      });

      // remove the children as there is no need of those children
      this.deleteChildrenByIndices(root, indicesToRemove);
    }

    // having more than one child filter row
    conditionOutcome = root.children.filter((child) => {
      return (child.children && child.children.length === 0 && child.filterRows.length > 1)
    });

    indicesToRemove = [];
    if (conditionOutcome && conditionOutcome.length > 0) {

      // if only one children found which is having more than 1 filter rows
      if (conditionOutcome.length === 1) {
        // if root filter rows does not exist then merge in root filter rows
        if (root.filterRows && root.filterRows.length === 0) {
          root.filterRows = root.filterRows.concat(conditionOutcome[0].filterRows);
          indicesToRemove.push(0);

          this.deleteChildrenByIndices(root, indicesToRemove);
        } else if (root.filterRows.length > 0) {
          newChildren.push(conditionOutcome[0]);
        }

      } else {
        conditionOutcome.forEach((child) => {
          newChildren.push(child);
        })
      }
    }

    if (newChildren && newChildren.length > 0) {
      root.children = newChildren;
    }



  }

  private evaluateComplexDateTimeChildren(root: FilterRoot) {
    if (root.isComplexDateTime && root.children) {

      for (let index = 0; index < root.children.length; index++) {
        const child = root.children[index];
        // check if child has other than date column
        const changedComplexDate = child.filterRows.filter((row) => {
          return row.key !== row.reservedKey;
        });

        // if other than dates type are found then our root is dirty and we will not consider as complexdate time
        if (changedComplexDate && changedComplexDate.length > 0) {
          const dirtyDateControl = this.dateControlElem.filter(element => {
            return child.columnName === element.field;
          });
          dirtyDateControl[0].toDate = undefined;
          dirtyDateControl[0].fromDate = undefined;
          dirtyDateControl[0].checkedValues = [];
          root.isComplexDateTime = false;
          root.isDirty = true;
          const columnIndex = this.findIndexInColumnArray(child.columnName)['index'];
          // trigger event to filter panel to remove all selection
          // todo
          break;
        }

        // check if only operand has changed then child is dirty but root is not
        const changedOperands = child.filterRows.filter((row) => {
          return row.reservedOperand && row.reservedOperand.length > 0 && (row.operand.value !== row.reservedOperand);
        });

        child.isDirty = (changedOperands && changedOperands.length > 0);
        // check if child has only 1 filter row rather than 2
        child.isDirty = child.isDirty || (child.filterRows && child.filterRows.length !== 2)
        if (child.isDirty) {
          // let columnIndex = this.findIndexInColumnArray(child.columnName)['index'];
          const dirtyOption = child.alias.split('Is ')[1];
          const dirtyDateControl = this.dateControlElem.filter(element => {
            return child.columnName === element.field;
          });
          if (dirtyDateControl[0].checkedValues.indexOf(dirtyOption) !== -1) {

            dirtyDateControl[0].checkedValues.splice(dirtyDateControl[0].checkedValues.indexOf(dirtyOption), 1);
            const checkedValues = [...dirtyDateControl[0].checkedValues];
            dirtyDateControl[0].checkedValues = checkedValues;
          }
          // trigger event to filter panel to unselect this child such as y'day, today
        }

      }

    }
  }


  saveGlobalLayout() {
    let copyLayout = { ...this.selectedLayout };
    copyLayout = this.getCurrentSessionValueForLayout(this.dtViewChild);
    copyLayout.name = 'Global Layout';
    copyLayout.memberId = 0;
    delete copyLayout.id;
    this.primeLoader = true;
    copyLayout.owner = copyLayout.fullName = copyLayout.firstName = 'Admin';
    this.addNewLayout(copyLayout, true);
  }


  private deleteChildrenByIndices(root: FilterRoot, indicesToRemove: Array<number>) {
    if (indicesToRemove && indicesToRemove.length > 0) {
      for (let i = indicesToRemove.length - 1; i >= 0; i--) {
        root.children.splice(indicesToRemove[i], 1);
      }
    }
  }

  checkActionType() {
    if (!this.filterClosable) {
      this.filterBuilder = this.lastSavedFilterRoot;
    }
    this.filterClosable = false;
  }
  checkDate(d: any) {
    if (d === null || d === '' || d.toString() === '1901-01-01' || d.toString() === '1901-01-01 00:00:00'
      || d.toString() === '0001-01-01' || d.toString() === '0001-01-01 00:00:00') {
      return null;
    } else {
      try {
        // Supposed the format is "yyyy-mm-dd"
        // check this https://stackoverflow.com/questions/7556591/javascript-date-object-always-one-day-off
        const d2 = d.replace(/-/g, '\/');
        const d1 = new Date(d2);
        return d1;
      } catch {
        // if not follow "yyyy-mm-dd"
        return new Date(d);
      }
    }
  }

  refresh_cache() {
    this.onRefresh.emit();
  }

  public after_onRefresh() {
    this.refreshCache = true;
    this.loadReport();
  }

  defaultCallback(rowData: any, rowIndex: number, columnName) {
    return '';
  }

  private changeFilterOperator(filterOperator: any) {
    const otherThanOperatorSelection: string = filterOperator['label'];
    if (this.otherFilterConditions[this.currentOtherFilterCondition_index]) {
      if (this.otherFilterConditions[this.currentOtherFilterCondition_index] !== otherThanOperatorSelection) {
        this.otherFilterConditions[this.currentOtherFilterCondition_index] = otherThanOperatorSelection;
        this.loadDataWithFilters();
      }
    } else {
      this.otherFilterNewCondtion = otherThanOperatorSelection;
    }
    this.filterOperatorOverlay.hide();
  }


  public loadDataWithFilters() {
    this.toggleFilter(this.checked);
  }

  public selectItem(chosenItem: any, value: any) {
    // console.dir(chosenItem);
    // console.log(value);

    if (value === true) {
      // added it in
      this._selectedItems.push(chosenItem);
    } else {
      // remove it out.
      this._selectedItems = this._selectedItems.filter(item => item !== chosenItem);
    }

    // console.dir(this._selectedItems );
  }

  public RemoveSelectedItems() {
    this._selectedItems = [];
  }

  public getSelectedItems() {
    return this._selectedItems;
  }

  public onPageHandler(event) {
    this.currentPageRowFromOnPageEvent = event.rows;
    // console.dir(this.currentPageRowFromOnPageEvent);
  }

  public onPageSortHandler(event) {
    this.sortObject = event;
    this.sortField = event.field;
    this.sortOrder = event.order;
    // console.dir(this.sortObject);
  }

  table_scroll(e) {
    this.fixed_header_scroll_x = e.currentTarget.scrollLeft;
    this.update_fixed_header_scroll_x();
  }

  update_fixed_header_scroll_x() {
    const headers = this.domHandler.find(this.el.nativeElement, '.fixed_header');
    const init = 260 - (!this.ms.menuHide ? 0 : 250);

    let length = 0;
    try {
      length = headers.length;
    } catch {
    }

    if (headers && length > 0) {
      try {
        headers.forEach(x => {
          x.style.left = init - this.fixed_header_scroll_x + 'px';
        });
      } catch {
        console.dir(headers);
      }
    }
  }

}




export class MemberLayout {
  id: number;
  fullName: string;
  firstName: string;
  memberId: number;
  name: string;
  layout: string;
  isDefault: number;
  gridId: string;
  owner: string;
  isMemberDefault: boolean;
}
