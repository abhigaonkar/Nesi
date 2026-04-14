import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { PickListItem } from '../../../../models/picklist/picklistItem';
import { LabelValueInt } from '../../../../models/Shared/labelValueString';
import { CONFIG, downloadcsv } from '../../../../configuration';
import { CoreService } from '../../../../services/shared/core.service';
import * as fromRoot from '../../../../reducers';
import { ActivatedRoute } from '@angular/router';
import { LabelValueString } from 'app/models/Shared/labelValueInt';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { WindowRef } from 'app/services/shared/windowRef';
import { MenuItem } from 'primeng/primeng';
import { MessageBase } from 'app/core/messageBaseComponent';
import * as fromMessage from '../../../../actions/layout/growlMessage';
import { Store } from '@ngrx/store';
import { DataExtra } from 'app/models/core/dataExtra';
import { LoaderService } from 'app/core/loader/loader.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-main',
  templateUrl: './picklist-main.component.html',
  styleUrls: ['./picklist-main.component.css']
})
export class PicklistMainComponent extends MessageBase implements OnInit {
  @Output() priceChange = new EventEmitter();
  @Output() priceUpdated = new EventEmitter();
  @Output() sectionsChanged = new EventEmitter();
  @Output() quoted_price_updated = new EventEmitter();

  @Input() edit_disabled = false;
  @Input() set quoted_price(value: number) {
    if (value && this.model) {
      this.model.quote_price = value;
    }
  };

  @Input() quote_id: number;
  @Input() revision: number;
  buid: number;
  notIncludeSectionTotal = 0;
  model: any;
  private sub: any;
  draggingIndex = -1;
  timeout: any;

  items: PickListItem[];
  sections: LabelValueInt[];
  sources: LabelValueInt[];
  sectionFilters: LabelValueInt[];

  selectedFilterSection = [];
  selectedSection = 0;
  selectedSource = 0;
  public sorts: any[];
  commandButtons: MenuItem[];

  selectSort = 'line_number';
  filterQuery = '';
  sortDesc = false;
  is_select_all = false;
  delete_count = 0;
  editSectionsDisplay = false;
  copyMoveDisplay = false;
  moveCopyType = '';

  draggedRow: any;
  submitting = false;

  copy_count = 0;
  enable_reorder = true;

  constructor(
    public cs: CoreService,
    private cf: ConfirmationService,
    private route: ActivatedRoute,
    private win: WindowRef,
    protected store: Store<fromRoot.State>,
    private ls: LoaderService,

  ) {
    super(store);
  }

  ngOnInit() {
    if (!this.quote_id) {
      this.sub = this.route.params.subscribe(params => {
        this.quote_id = +params['quote_id']; // (+) converts string 'id' to a number
        this.revision = +params['revision'];
        this.init();
      });
    } else {
      this.init();
    }
    this.sorts = [
      { label: 'Default', value: 'line_number' },
      { label: 'Section', value: 'section_name' },
      { label: 'Part No', value: 'part_no' },
      { label: 'Description', value: 'label' },
      { label: 'Qty', value: 'qty' },
      { label: 'Cost', value: 'cost' },
      { label: 'Average Cost', value: 'avg_cost' },
      { label: 'Recommended Cost', value: 'recommended_cost' },
      { label: 'Date Updated', value: 'date_updated' },
      { label: 'Extd', value: 'extd' },
      { label: '~Ext\'d', value: 'extd2' },
      { label: 'Margin', value: 'margin' },
      { label: 'Include', value: 'is_checked' },
    ];
    this.commandButtons = [
      {
        label: 'Actions', icon: 'fa-list',
        items: [
          { label: 'Copy', icon: 'fa-copy', command: () => { this.copySelected() } },
          { label: 'Move', icon: 'fa-arrow-right', command: () => { this.moveSelected() } },
          { label: 'Delete', icon: 'fa-times', command: () => { this.deleteSelected() } },
          { label: 'Include', icon: 'fa-check-square', command: () => { this.includeSelected() } },
          { label: 'Exclude', icon: 'fa-square', command: () => { this.excludeSelected() } },
          { label: '~Ext\'d=Ext\'d', icon: 'fa-copy', command: () => { this.copyExtdValue() } },
        ]
      }
    ];
  }

  get menuItemDisabled(): boolean {
    return this.edit_disabled || !this.items || this.items.length === 0;
  }


  check_reorder() {
    if (this.enable_reorder) {
      this.selectSort = 'line_number';
      this.sortDesc = false;
      this.doItems(false);
    }
  }

  saveSelected() {
  }

  copySelected() {
    if (this.selectItems && this.selectItems.length > 0) {
      this.moveCopyType = 'Copy';
      this.copyMoveDisplay = true;
    } else {
      this.PushWarnMessage('No worksheet line item is selected.');
    }
  }



  moveSelected() {
    if (this.selectItems && this.selectItems.length > 0) {
      this.moveCopyType = 'Move';
      this.copyMoveDisplay = true;
    } else {
      this.PushWarnMessage('No worksheet line item is selected.');
    }
  }


  includeSelected() {
    if (this.selectItems && this.selectItems.length > 0) {
      this.selectItems.forEach(x => {
        if (!x.is_checked) {
          this.check_changed(x, true);
        }
      });
    }
  }

  excludeSelected() {
    if (this.selectItems && this.selectItems.length > 0) {
      this.selectItems.forEach(x => {
        if (x.is_checked) {
          this.check_changed(x, false);
        }
      });
    }
  }

  copyExtdValue() {
    if (this.selectItems && this.selectItems.length > 0) {
      this.copy_count = 0;
      this.selectItems.forEach(x => {
        if (x.is_checked) {
          x.extd2 = x.extd;
          this.copy_count++;
          this.field_Changed('extd2', x);
        }
      });
    }
  }

  field_Changed(field: string, item: any) {
    if (field === 'extd2' && !item.extd2) {
      item.extd2 = 0;
    }
    CONFIG.LOG(item, 'item submit to edit');
    const url = CONFIG.apiURL.page.shared.pickList.quoteLineUpdate + field;
    this.cs.patchObject<any>(url, item)
      .subscribe(
        (res) => {
          if (this.CheckResponseMessage(res.data)) {
            this.copy_count--;
            if (this.copy_count === 0) {
              this.reCalculate();
            }
          }
        }
      );
  }


  check_changed(item: any, value) {
    this.cs.postString(CONFIG.apiURL.page.shared.pickList.LineIsChecked + this.quote_id.toString() + '/' + this.revision,
      {
        id: item.id,
        value: value,
      }).subscribe(
        (res) => {
          item.is_checked = value;
          this.reCalculate();
        }
      );
  }

  moveCopySuccessed(event: any) {
    this.copyMoveDisplay = false;
    this.init();
  }


  deleteSelected() {
    if (this.selectItems && this.selectItems.length > 0) {
      this.cf.confirm({
        message: 'Do your really want to delete all selected items?',
        accept: () => {
          this.delete_count = 0;
          this.selectItems.forEach(x => {
            this.delete_count++;
            this.delete(x.id);
          });
          this.init();
        }
      });
    } else {
      this.PushWarnMessage('No worksheet line item is selected.')
    }
  }

  init(event = null) {
    this.cs.getObject<any>(CONFIG.apiURL.page.shared.pickList.quote + this.quote_id.toString() + '/' + this.revision.toString())
      .subscribe(
        (res) => {
          this.is_select_all = false;
          CONFIG.LOG(this.model, 'init in picklist-main');
          this.notIncludeSectionTotal = res.notIncludeSectionTotal;
          this.model = res;
          this.model.items.forEach(x => {
            if (!this.model.member_can_see_cost) {
              x.display_cost = 0;
            } else {
              x.display_cost = x.cost;
            }
            if (!this.model.member_can_see_sell) {
              x.display_sell = 0;
              x.display_sell = x.sell;
            }
          });
          this.sections = this.model.section;
          this.sources = this.model.sources;
          this.sectionFilters = this.sections.slice(1);
          this.buid = this.model.businessUnitId;
          if (!event) {
            this.doItems();
            this.priceUpdated.emit();
          } else {
            this.selectedSection = 0;
            this.sectionChanged(event);
          }
        }
      );
  }


  public updateSectionFromValue(value: any[]) {
    this.sections = value;
    this.sectionFilters = this.sections.slice(1);
  }

  sectionChanged(event: any) {
    // CONFIG.LOG(event.value, 'sectionChanged in picklist-main');
    // if (this.selectedFilterSection.length !== 0) {
    this.selectedFilterSection = [];
    if (this.selectedSection > 0) {
      this.selectedFilterSection.push(this.selectedSection);
    }
    if (event && event.not_update_price) {
      this.doItems(false);
    } else {
      this.doItems();
    }
    this.selectedSource = 1;
    // }
  }

  calculateMargin(item): number {
    if (!item.extd2 || item.item <= 0) {
      return 0;
    }
    const m = (item.extd2 - (item.cost * item.qty)) / item.extd2;
    return m;
  }
  calculateMargin_o(item): number {
    if (!item.extd || item.item <= 0) {
      return 0;
    }
    const m = (item.extd - (item.cost * item.qty)) / item.extd;
    return m;
  }
  doItems(eventEmit: boolean = true) {
    this.items = this.model.items;

    if (!this.items) {
      this.items = [];
    }

    this.items.forEach(x => x.margin = this.calculateMargin(x));

    CONFIG.LOG(this.selectedFilterSection, 'selectedFilterSection in filter items in picklist main');
    if (this.selectedFilterSection && this.selectedFilterSection.length !== 0) {
      this.items = this.items.filter(x =>
        this.selectedFilterSection.indexOf(x.section_id) > -1
      );
    }
    CONFIG.LOG(this.filterQuery, 'filterQuery in filter items in picklist main');
    if (this.filterQuery) {
      const q = this.filterQuery.toLowerCase();
      this.items = this.items.filter(x => String(x.label).toLowerCase().indexOf(q) > -1 || String(x.part_no).indexOf(q) > -1);
    }

    CONFIG.LOG(this.selectSort, 'selectSort in sort changed in picklist main');
    this.items = this.sortItems(this.items);
    this.items.forEach((x, index) => x.index = index);
    this.reCalculate(eventEmit);
  }


  sortItems(o_items: any[]): any[] {
    if (this.selectSort) {
      if (this.selectSort === 'label') {
        o_items.sort((a, b) => {
          const sa = String(a[this.selectSort]).toLowerCase().trim();
          const sb = String(b[this.selectSort]).toLowerCase().trim();
          if (sa < sb) {
            return -1;
          } else if (sa > sb) {
            return 1;
          } else {
            return 0;
          }
        });
      } else if (this.selectSort === 'section_name') {
        const items_number = o_items.filter(x => x.section_name.indexOf('.') > -1);
        const items_text = o_items.filter(x => x.section_name.indexOf('.') === -1)
          .sort((a, b) => {
            const sa = String(a[this.selectSort]).toLowerCase().trim();
            const sb = String(b[this.selectSort]).toLowerCase().trim();
            if (sa < sb) {
              return -1;
            } else if (sa > sb) {
              return 1;
            } else {
              return 0;
            }
          });
        const items_number_1 = items_number.filter(x => !x.section_name.startsWith('N'))
          .sort((a, b) => {
            const sa = String(a[this.selectSort]).toLowerCase().trim();
            const sb = String(b[this.selectSort]).toLowerCase().trim();
            const na = Number(sa.substr(0, sa.indexOf('.')));
            const nb = Number(sb.substr(0, sb.indexOf('.')));
            if (na < nb) {
              return -1;
            } else if (na > nb) {
              return 1;
            } else {
              return 0;
            }
          });
        const items_number_2 = items_number.filter(x => x.section_name.startsWith('N'))
          .sort((a, b) => {
            const sa = String(a[this.selectSort]).toLowerCase().trim();
            const sb = String(b[this.selectSort]).toLowerCase().trim();
            const na = Number(sa.substr(1, sa.indexOf('.')));
            const nb = Number(sb.substr(1, sb.indexOf('.')));
            if (na < nb) {
              return -1;
            } else if (na > nb) {
              return 1;
            } else {
              return 0;
            }
          });
        o_items = [...items_number_1, ...items_number_2, ...items_text];
      } else {
        o_items.sort((a, b) => {
          if (a[this.selectSort] && b[this.selectSort]) {
            return (a[this.selectSort] - b[this.selectSort]);
          } else {
            return 1;
          }
        }
        );
      }
    }
    if (this.sortDesc) {
      o_items = o_items.reverse();
    }
    return o_items;
  }
  sourceChanged(event: any) {
    //  CONFIG.LOG(this.selectedSource, 'sourceChanged in picklist-main');
  }

  cancel_search() {
    this.selectedSource = 0;
    this.ls.hide();
    setTimeout(() => {
      this.selectedSource = 1;
    }, 50);
  }


  section_deleted(event) {
    this.selectedFilterSection = this.selectedFilterSection.filter(x => x !== event);
  }

  filterItems(event: any) {
    this.doItems(false);
  }

  sortChanged(event: any) {
    this.enable_reorder = this.draggable;
    this.doItems(false);
  }
  descChanged() {
    this.sortDesc = !this.sortDesc;
    this.enable_reorder = this.draggable;
    this.doItems(false);
  }

  checkAll() {
    this.items.forEach(x => x.is_selected = this.is_select_all);
    // this.reCalculate();
  }

  get selectItems(): any[] {
    return this.items.filter(x => x.is_selected);
  }


  quote_price_updated(event) {
    if (this.model) {
      this.model.quote_price = event;
    }
    this.quoted_price_updated.emit(event);
  }

  reCalculate(eventEmit: boolean = true) {
    this.items.forEach(x => x.margin = this.calculateMargin(x));

    this.model.totalLabor = this.laborSellSum;
    this.model.totalTM = this.extdSum;
    this.model.totalQuote = this.extd2Sum;
    this.model.totalMaterial = this.materialSellSum;
    this.model.totalCost = this.costSum;
    this.model.totalLaborCost = this.laborCostSum;
    this.model.totalMaterialCost = this.materialCostSum;
    this.model.totalHours = this.laborHours;
    this.model.totalNotInclude = this.noteIncludeSum + this.notIncludeSectionTotal;
    this.model.discount = 0;

    if (eventEmit) {
      const event = {
        totalTM: this.model.totalTM,
        totalQuote: this.model.totalQuote,
        totalCost: this.model.totalCost,
      }
      this.priceChange.emit(event);
      this.priceUpdated.emit();
    }
  }
  get laborHours(): number {
    let sum = 0;
    this.model.items.forEach(x => sum += x.is_checked && x.part_no >= 990000 && x.part_no < 2000000 ? x.qty : 0);
    return sum;
  }
  get laborCostSum(): number {
    let sum = 0;
    this.model.items.forEach(x => sum += x.is_checked && x.part_no >= 990000 && x.part_no < 2000000 ? x.cost * x.qty : 0);
    return sum;
  }

  get laborSellSum(): number {
    let sum = 0;
    this.model.items.forEach(x => sum += x.is_checked && x.part_no >= 990000 && x.part_no < 2000000 ? x.extd2 : 0);
    return sum;
  }

  get materialSellSum(): number {
    return this.extd2Sum - this.laborSellSum;
  }

  get materialCostSum(): number {
    return this.costSum - this.laborCostSum;
  }

  get current_qtySum(): number {
    let sum = 0;
    this.items.forEach(x => sum += x.is_checked ? x.qty : 0);
    return sum;
  }
  get costSum(): number {
    let sum = 0;
    this.model.items.forEach(x => sum += x.is_checked ? x.cost * x.qty : 0);
    return sum;
  }

  get extdSum(): number {
    let sum = 0;
    this.model.items.forEach(x => sum += x.is_checked ? x.extd : 0);
    return sum;
  }

  get extd2Sum(): number {
    let sum = 0;
    this.model.items.forEach(x => sum += x.is_checked ? x.extd2 : 0);
    return sum;
  }


  get margin() {
    const m = (this.extd2Sum - this.costSum) / this.extd2Sum;
    return m;

  }

  get current_costSum(): number {
    let sum = 0;
    this.items.forEach(x => sum += x.is_checked ? x.cost * x.qty : 0);
    return sum;
  }

  get current_extdSum(): number {
    let sum = 0;
    this.items.forEach(x => sum += x.is_checked ? x.extd : 0);
    return sum;
  }

  get current_extd2Sum(): number {
    let sum = 0;
    this.items.forEach(x => sum += x.is_checked ? x.extd2 : 0);
    return sum;
  }


  get current_margin() {
    if (this.current_extd2Sum !== 0) {
      return (this.current_extd2Sum - this.current_costSum) / this.current_extd2Sum;
    } else {
      return 0;
    }
  }

  get noteIncludeSum() {
    let sum = 0;
    this.model.items.forEach(x => sum += x.is_checked ? 0 : x.extd);
    return sum;
  }
  get current_noteIncludeSum() {
    let sum = 0;
    this.items.forEach(x => sum += x.is_checked ? 0 : x.extd);
    return sum;
  }

  get current_aveSum(): number {
    let sum = 0;
    this.items.forEach(x => sum += x.is_checked ? x.avg_cost : 0);
    return sum;
  }

  get current_recSum(): number {
    let sum = 0;
    this.items.forEach(x => sum += x.is_checked ? x.recommended_cost : 0);
    return sum;
  }

  



  delete(id: number) {
    CONFIG.LOG(id, 'id in delete picklist item');
    this.cs.deleteString(CONFIG.apiURL.page.shared.pickList.quote
      + this.quote_id.toString() + '/' + this.revision.toString() + '/' + id.toString())
      .subscribe(
        (res: string) => {
          this.items = this.items.filter(x => x.id !== id);
          this.delete_count--;
          if (this.delete_count === 0) {
            this.init();
          }
        }
      )

  }


  printWorkSheet() {
    const url = CONFIG.Nesi1URL.printQuoteWorksheet
      .replace('@quote_id', String(this.quote_id))
      .replace('@revision', String(this.revision))
      .replace('@T', String(Math.round(this.extdSum * 100) / 100))
      .replace('@Q', String(Math.round(this.extd2Sum * 100) / 100));
    this.win.boingNesi1(url, 'print_' + this.model.id);
  }

  excelWorkSheet() {
    this.items.forEach(x => x.margin = this.calculateMargin(x));
    this.items.forEach(x => x.margin_o = this.calculateMargin_o(x));

    const exportItems = this.items.map(x => {
      return {
        quote_id: x.quote_id,
        revision: x.revision,
        section_name: x.section_name.replace(/,/gi, ' '),
        part_no: x.part_no,
        description: x.label.replace(/,/gi, ' '),
        qty: this.round(x.qty, 2),
        cost: this.round(x.display_cost, 2),
        avg_cost: x.avg_cost,
        recommended_cost: x.recommended_cost,
        date_updated: x.date_updated,
        extd: this.round(x.extd, 2),
        margin_extd: this.round(x.margin_o, 3),
        extd2: this.round(x.extd2, 2),
        margin_extd2: this.round(x.margin, 3),
        discount: this.round(x.discount, 2),
        notes: x.notes || '',
        is_included: x.is_checked,
        line_number: x.index
      }
    });

    downloadcsv(this.sortItems(exportItems), 'worksheet_' + this.quote_id.toString() + this.revision.toString());
  }

  applyDicsount(event) {
    const postData = { data: event.quoteDiscount };
    this.selectedSource = -1;
    this.submitting = true;
    this.cs.postDataExtra(CONFIG.apiURL.page.shared.pickList.applyDiccount
      + this.quote_id.toString() + '/' + this.revision.toString(), postData)
      .subscribe(
        (res) => {
          this.submitting = false;
          if (this.CheckResponseMessage(res.data)) {
            this.model.items = res.extra.items;
            this.model.quoteDiscount = res.extra.quoteDiscount;
            this.model.discountAmount = res.extra.discountAmount;
            this.doItems();
          }
        }
      );
  }

  // row_highlight(row, index) {
  //   if (!this.draggedRow || !row) {
  //     return false;
  //   }
  //   const o= this.draggedRow.id == row.id || this.draggingIndex == index;
  //   CONFIG.LOG(row.id, 'row id in highlight');
  //   CONFIG.LOG(o, 'is highlight');
  //   return o;
  // }

  public openSection(event: any) {
    CONFIG.LOG(event.section_id, 'open worksheet section id in picklistmain');
    this.selectedSection = event.section_id;
    this.selectedFilterSection = [event.section_id];
    this.doItems();
  }

  public updateSection(event: any) {
    this.init(event);
  }


  editSections() {
    this.editSectionsDisplay = true;
  }

  loadSections(event) {
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.shared.pickList.quoteSectionList + this.quote_id + '/' + this.revision)
      .subscribe(
        (res) => {
          this.sections = res;
          this.sectionFilters = this.sections.slice(1);
        }
      );
  }

  get draggable() {
    return this.selectSort === 'line_number' && !this.sortDesc && this.enable_reorder;
  }
  dragStart(event, item: any) {
    this.draggingIndex = -1;

    if (this.draggable) {
      this.draggedRow = item;
    } else {
      this.draggedRow = null;
    }
  }
  dragEnd(event) {
    this.draggedRow = null;
    setTimeout(() => {

    }, 3000);
  }

  drop(event, pos) {
    CONFIG.LOG(pos, 'drop position in worksheet list');
    if (this.draggedRow) {
      const draggedIndex = this.draggedRow.index;
      this.draggingIndex = this.draggedRow.index;
      CONFIG.LOG(draggedIndex, 'draggedIndex position in worksheet list');
      this.draggedRow = null;
      if (draggedIndex === pos) {
        return;
      }
      const newDiv = [];
      const oldDiv = this.items;
      if (this.items.length > 1) {
        this.submitting = true;
        for (let i = 0; i < oldDiv.length; i++) {
          if (pos === i) {
            newDiv.push(oldDiv[draggedIndex]);
            newDiv.push(oldDiv[i]);
          } else if (draggedIndex === i) {

          } else {
            newDiv.push(oldDiv[i]);
          }
        }
        if (pos === oldDiv.length) {
          newDiv.push(oldDiv[draggedIndex]);
        }
        this.items = newDiv;
        this.postReOrder();
      }
    }
  }

  postReOrder() {
    this.submitting = true;
    const orders = [];
    let index = 0;
    this.items.forEach(x => {
      orders.push({
        id: x.id,
        order: index + 1,
      });
      index++;
    });
    CONFIG.LOG(orders, 'order index post reorder worksheet list');
    this.cs.postObject<DataExtra>(CONFIG.apiURL.page.shared.pickList.reOrderQuoteWorkSheet
      + this.quote_id + '/' + this.revision, orders)
      .subscribe(
        (res: DataExtra) => {
          this.model.items = res.extra;
          this.model.items.forEach(x => {
            if (!this.model.member_can_see_cost) {
              x.display_cost = 0;
            } else {
              x.display_cost = x.cost;
            }
            if (!this.model.member_can_see_sell) {
              x.display_sell = 0;
              x.display_sell = x.sell;
            }
          });
          CONFIG.LOG(this.model.items.length, 'length of items');
          this.doItems(false);
          this.submitting = false;
          this.timeout = window.setTimeout(() => {
            this.timeout = null;
            this.draggingIndex = -1
          }, 3000);
        }
      );
  }

  findIndex(row: any) {
    let index = -1;
    for (let i = 0; i < this.items.length; i++) {
      if (this.items[i].id === row.id) {
        index = i;
        break;
      }
    }
    return index;
  }
}
