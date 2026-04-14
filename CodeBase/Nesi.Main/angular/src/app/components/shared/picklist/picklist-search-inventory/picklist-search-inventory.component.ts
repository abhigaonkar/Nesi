import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { TokenService } from 'app/services/authentication/tokenService';
import { Store } from '@ngrx/store';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';
import { PickListSearchbase } from 'app/components/shared/picklist/_base/picklist-search-base';
import * as fromRoot from '../../../../reducers';
import { LabelValueInt } from 'app/models/Shared/labelValueString';
import { extend } from 'webdriver-js-extender';
import { ViewChild } from '@angular/core/src/metadata/di';
// tslint:disable-next-line:max-line-length

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-picklist-search-inventory',
  templateUrl: './picklist-search-inventory.component.html',
  styleUrls: ['./picklist-search-inventory.component.css']
})
export class PicklistSearchInventoryComponent extends PickListSearchbase implements OnInit {
  @Output() part9595 = new EventEmitter();
  @Output() cancelSearch = new EventEmitter();
  @Input() can_see_cost = true;
  placeholder = 'Search Inventory';
  sourceName = 'Inventory Search';
  searchResult: any[];
  allResult: any[];
  uniqueSearchResult: any[];
  match_whole_word = true;


  gbFilter: string;
  searching = false;
  selectedItems: any[];
  searchFrom: LabelValueInt[] = [];
  selectedSearchFrom = 0;
  atts: any[];
  tags: any[];
  vals: any[];

  filters: any[] = [];

  selectItem: number;

  selectedVal: any;

  attributes: any[];
  countVals: any[];


  extendData: any[] = [];
  query_master_id_list: number[] = [];
  imageLoadingCount = 0;
  imageBuffer: Map<number, any>;
  getFullDataWhenSearch = true;
  has_result = false;
  fromChanged = false;

  searching_masterId: string;
  constructor(
    protected fb: FormBuilder,
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService

  ) {
    super(fb, store, cs);
    this.searchUrl = CONFIG.apiURL.page.shared.pickList.InventorySearchHistory;
    this.getListUrl = CONFIG.apiURL.page.shared.pickList.QuoteSearchInventory + '$quote_id/$revision';
    this.isDropdown = false;
    super.Init(CONFIG.apiURL.page.shared.pickList.addQuoteMaterials + '$quote_id/$revision');
    // this.searchFrom.push({ label: 'Master Id', value: 6 });
    this.searchFrom.push({ label: 'Description/PartNo', value: 0 });
    this.searchFrom.push({ label: 'Most Commonly Used', value: 1 });
    this.searchFrom.push({ label: 'Only Stocked', value: 2 });
    this.searchFrom.push({ label: 'Recent WOs', value: 3 });
    this.searchFrom.push({ label: 'Recent Quotes', value: 4 });
    this.searchFrom.push({ label: 'Vendor Part No', value: 5 });
    //  this.imageBuffer = new Map<number, any>();
  }

  ngOnInit() {
    super.ngOnInit();
    this.getSettings();
  }

  // extendResult() {
  //   // let row = this.allResult.filter(x => x.master_id === data.master_id && !x.hasValue);
  //   // row = { ...row, ...data, hasvalue: true };
  //   const newR = [];
  //   for (let i = 0; i < this.allResult.length; i++) {
  //     for (let j = 0; j < this.extendData.length; j++) {
  //       if (this.allResult[i].master_id == this.extendData[j].master_id) {
  //         newR.push({ ...this.allResult[i], ...this.extendData[j], hasValue: true });
  //       }
  //     }
  //   }
  //   this.allResult = newR;
  //   this.getUniqueResult();
  // }

  hasValue(id: number) {
    const r = this.uniqueSearchResult.filter(x => x.master_id === id);
    if (!r || !r[0] || !r[0].hasValue) {
      return id;
    } else {
      return null;
    }
  }

  select(event: any) {
    CONFIG.LOG(this.text, 'query string in get list in search inventory');
    if (event) {
      this.text = event.value;
    }
    this.searchClick();
  }

  keyUpSearch(event: any) {
    if (event.which === 13) {
      CONFIG.LOG(event.which, 'key up in autocomplete');
      this.searchClick();
    }
  }

  searchClick() {
    CONFIG.LOG(this.text, 'input text string in get list in search inventory');
    if (!this.text) { return; }
    if (this.text === '55556' ||this.text === '55558') {
      this.text = '';
      this.part9595.emit();
      return;
    }
    this.dialogDisplay = true;
    this.searchResult = null;
    if (this.text.label) {
      this.getList(this.text.label);
    } else {
      this.getList(this.text);
    }
  }

  getInventoryDetails(obj: any[]) {
    if (!obj || obj.length === 0) {
      this.searching = false;
      return;
    }
    this.cs.postObject<any>(CONFIG.apiURL.page.shared.pickList.quoteInventories
      + this.quote_id + '/' + this.revision, obj)
      .subscribe(
        (res) => {
          this.searching = false;
          res.forEach(x => {
            x.qty = ''
          });
          this.extendData = this.extendData.concat(res);
          this.getUniqueResult();
        }
      );
  }

  // getInventoryDetail(master_id: number) {
  //   if (this.extendData.findIndex(x => x.master_id == master_id) > -1) {
  //     return;
  //   }
  //   this.cs.getObject<any>(CONFIG.apiURL.page.shared.pickList.quoteInventory
  //     + this.quote_id + '/' + this.revision + '/' + master_id + '/0/0')
  //     .subscribe(
  //     (res) => {
  //       this.extendData.push(res);
  //       this.getUniqueResult();
  //       //  this.extendResult();
  //     }
  //     );
  // }

  sourceChanged(event: any) {
    this.has_result = false;
    this.fromChanged = true;
    // this.text = null;
    if (this.selectedSearchFrom === 3 || this.selectedSearchFrom === 4) {
      this.isDropdown = true;
      if (this.selectedSearchFrom === 3) {
        this.getAllUrl = CONFIG.apiURL.page.shared.pickList.recentWos;
        this.placeholder = 'Select Work Order';
      } else {
        this.getAllUrl = CONFIG.apiURL.page.shared.pickList.recentQuotes;
        this.placeholder = 'Select Quote';
      }
      this.getAllUrl += '$quote_id/$revision';
      this.getSearch();
    } else if (this.selectedSearchFrom === 6) {

    } else {
      this.placeholder = 'Search Inventory';
      this.isDropdown = false;
      this.searchClick();
    }
  }

  clearSearch() {
    this.allResult = [];
    this.atts = [];
    this.vals = [];
    this.tags = [];
    this.uniqueSearchResult = [];
    this.countVals = [];
    this.selectedVal = null;
    this.filters = [];
    this.attributes = [];
  }

  getList(query: string) {

    CONFIG.LOG(query, 'query string in get list in search inventory');
    // if (query==='9595' && this.selectedSearchFrom === 0) {
    //   this.has_result = false;
    //   this.searching_masterId = query;
    //   this.fromChanged = false;
    // }
    const url = this.replaceURL(this.getListUrl);
    this.searching = true;
    this.has_result = false;
    this.clearSearch();
    // tslint:disable-next-line:max-line-length
    this.cs.postObject<any>(url, {
      data: query,
      type: this.selectedSearchFrom,
      getfull: this.getFullDataWhenSearch,
      matchwhole: this.match_whole_word
    })
      .subscribe(
        res => {
          if (res) {
            //  CONFIG.LOG(res, 'get list in search inventory');
            this.allResult = res.data;
            this.atts = res.atts;
            this.vals = res.vals;
            this.tags = res.tags;
            this.getUniqueResult();
          }
          this.has_result = true;
          this.searching = false;
        }
      );
  }

  tableValueChange(event: any) {
    // CONFIG.LOG(event, 'table value');
    this.imageLoadingCount = 0;
    this.query_master_id_list = [];
  }

  loadedImage(event: any) {
    if (this.getFullDataWhenSearch) { return; }
    const master_id = event.master_id;
    const data = event.data;

    this.imageLoadingCount--;

    if (this.extendData.findIndex(x => String(x.master_id) === String(master_id)) < 0
      && this.query_master_id_list.indexOf(Number(master_id)) < 0
    ) {
      this.query_master_id_list.push(master_id)
    }
    if (this.imageLoadingCount === 0) {
      CONFIG.LOG(this.query_master_id_list, 'post to webapi get master deatils');
      this.getInventoryDetails(this.query_master_id_list)
    }
  }

  loadingImage(event: any) {
    if (this.getFullDataWhenSearch) { return; }
    const master_id = event.data;
    this.searching = true;
    this.imageLoadingCount++;
  }

  resetFilter(val: any) {
    const newFilters = [];
    for (let i = 0; i < this.filters.length; i++) {
      if (this.filters[i].id === val.id) {
        this.filters = newFilters;
        if (i > 0) {
          this.selectedVal = this.filters[i - 1];
        } else {
          this.selectedVal = null;
          this.filters = [];
        }
        this.getUniqueResult();
        return;
      }
      newFilters.push(this.filters[i]);
    }
  }

  filterVal(val: any) {
    this.selectedVal = val;
    if (this.selectedVal) {
      this.filters.push(this.selectedVal);
    }
    this.getUniqueResult();
  }

  getSearchresult() {
    this.searchResult = this.allResult;
    if (this.gbFilter) {
      const filters = this.gbFilter.trim().split(' ');
      for (const filter of filters) {
        if (filter.trim()) {
          CONFIG.LOG(filter, 'filter value in search result fitler');
          this.searchResult = this.searchResult.filter(x => String(x.description).toLowerCase().indexOf(filter.toLowerCase().trim()) > -1);
        }
      }
    }
    for (let i = 0; i < this.filters.length; i++) {
      this.searchResult = this.searchResult.filter(x => this.filters[i].master_ids.indexOf(x.master_id) > -1);
    }
    this.searchResult.forEach(x => {
      if (!x.extd) {
        x.extd = x.sell;
      }
      if (!x.extd2) {
        x.extd2 = x.extd;
      }
      x.origin_cost = x.cost;
      x.display_cost = this.can_see_cost ? x.cost : 0;
      x.margin = this.getMargin(x);
    })
  }

  isInFilter(id: number) {
    for (let i = 0; i < this.filters.length; i++) {
      if (this.filters[i].id === id) {
        return true;
      }
    }
    return false;
  }



  getCountVals() {
    this.getSearchresult();
    this.countVals = [];
    for (let i = 0; i < this.vals.length; i++) {
      const items = this.searchResult.filter(x => x.attribute_value_id === this.vals[i].id);
      if (items.length > 0 && !this.isInFilter(this.vals[i].id)) {
        const groupItems = this.group(items, 'master_id');
        this.countVals.push({
          ...this.vals[i],
          header: `${this.vals[i].v} (${groupItems.length} part${groupItems.length > 1 ? 's' : ''})`,
          count: groupItems.length,
          items: items,
          master_ids: groupItems.map(x => x.master_id)
        });
      }
    }
  }

  getAttributes() {
    this.getCountVals();
    this.attributes = [];
    for (let i = 0; i < this.atts.length; i++) {
      const sub = this.countVals.filter(x => x.aid === this.atts[i].id)
      if (sub.length > 0) {
        this.attributes.push({
          header: `${this.atts[i].a} (${sub.length} option${sub.length > 1 ? 's' : ''})`,
          id: this.atts[i].id,
          count: sub.length,
          vals: [...sub]
        });
      }
    }
  }

  getUniqueResult() {
    this.getAttributes();
    let ary: any[];
    if (this.getFullDataWhenSearch) {
      ary = this.searchResult;
    } else {
      ary = this.searchResult.map(x => {
        const ext = this.extendData.find(y => y.master_id === x.master_id);
        return {
          master_id: x.master_id,
          description: x.description,
          onhand: x.onhand,
          ...ext
        };
      });
    }
    this.uniqueSearchResult = this.group(ary, 'master_id');
    this.selectedItems = this.uniqueSearchResult.filter(x => x.qty && x.qty !== 0);
  }


  group(array: any[], keyField: string): any[] {
    const keys: any[] = array.map(x => x[keyField]);
    const uniqueKey: any[] = [];
    const unique: any[] = [];
    for (let i = 0; i < array.length; i++) {
      if (uniqueKey.indexOf(keys[i]) === -1) {
        uniqueKey.push(keys[i]);
        unique.push(array[i]);
      }
    }
    return unique;
  }

  qtyChanged(item: any) {
    CONFIG.LOG(item, 'item value in qty change in search inventory table');
    this.selectedItems = this.uniqueSearchResult.filter(x => x.qty && x.qty !== 0);
    if (item.qty > 0) {
      if (!item.is_exclude) {
        this.cs.getObject<any>(this.getCostURL(item.master_id, item.qty))
          .subscribe(
            (res) => {
              this.fillItem(item, res);
            });
      } else {
        this.fillCost2(item);
      }
    }
  }

  fillItem(item, res) {
    if (res.cost > 0) {
      item.cost = res.cost;
      item.sell = res.sell;
      item.extd = res.extd;
      item.extd2 = res.extd2;
      item.origin_cost = res.origin_cost;
      item.display_cost = this.can_see_cost ? item.cost : 0;
      item.margin = this.getMargin(item);
    } else {
      item.extd = item.qty * item.sell;
      item.extd2 = item.extd;
    }
  }

  fillCost2(item) {
    let qty = item.qty;
    const cost = item.cost;
    if (!cost) {
      return
    }
    if (!qty) {
      qty = 1;
      //  item.qty = 1;
    }
    this.cs.getObject<any>(this.getCost2URL(qty, cost))
      .subscribe(
        (res) => {
          this.fillItem(item, res);
        }
      );
  }

  getCost2URL(qty: number, cost: number) {
    return CONFIG.apiURL.page.shared.pickList.costHandler
      + this.quote_id + '/' + this.revision + '/' + qty.toString().replace('.', '_') + '/' + cost.toString().replace('.', '_');
  }

  AddToQuote() {
    this.submitting = true;
    setTimeout(() => {
      this.submitedValue = this.selectedItems.map(x => {
        return {
          master_id: x.master_id,
          description: x.description,
          qty: x.qty,
          is_checked: true,
          cost: x.cost,
          sell: x.sell,
          extd: x.extd,
          extd2: x.extd2,
          section_id: this.section_id,
          discount: 0
        }
      });
      this.postUrl = this.replaceURL(this.postUrl);
      this.submitting = true;
      this.formPost();
    }, 500);

  }

  applyRowStyle(row: any): string {
    //  CONFIG.LOG(row, 'apply row style');
    return row.onhand > 0 ? 'bold' : '';
  }

  getMargin(item: any) {
    if (!item.extd2 || item.extd2 <= 0) {
      return 0;
    }
    const m = (item.extd2 - (item.cost * (item.qty > 0 ? item.qty : 1))) / item.extd2;
    return m;
  }

  cancel_Search() {
    this.searching = false;
    this.cancelSearch.emit();
  }


  match_whole_word_change() {
    this.cs.postString(CONFIG.apiURL.page.shared.pickList.settings_matchWholeWord, { data: this.match_whole_word ? '1' : '0' })
      .subscribe(
        (res) => {
          if (!this.CheckResponseMessage(res)) {
            this.PushWarnMessage(res);
          }
        }
      );
  }

  getSettings() {
    this.cs.getString(CONFIG.apiURL.page.shared.pickList.settings_matchWholeWord)
      .subscribe(
        (res) => {
          this.match_whole_word = res === '1';
        }
      );
  }

}
