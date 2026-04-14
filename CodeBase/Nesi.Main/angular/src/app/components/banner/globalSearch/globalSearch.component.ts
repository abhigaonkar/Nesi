import { Component, OnInit, ViewEncapsulation, EventEmitter, AfterViewInit, ElementRef, Renderer } from '@angular/core';
import { SearchItem } from '../../../models/banner/searchItem';
import { CoreService } from '../../../services/shared/core.service';
import { CONFIG } from '../../../configuration';
import { WindowRef } from '../../../services/shared/windowRef';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-globalSearch',
  templateUrl: './globalSearch.component.html',
  styleUrls: ['./globalSearch.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class GlobalSearchComponent implements OnInit, AfterViewInit {

  selectedItem: SearchItem;
  items: SearchItem[];
  query: string;
  constructor(
    public cs: CoreService,
    private win: WindowRef,
    private elem: ElementRef,
    private renderer: Renderer,
  ) { }

  ngAfterViewInit(): void {
    // const inputs = document.getElementsByClassName('ui-autocomplete-input');
    // for (let i = 0; i < inputs.length; i++) {
    //   if (inputs[i]['id'] === 'idSearchText') {
    //     this.renderer.listen(inputs[i], 'onchange', (e) => {
    //       CONFIG.LOG('changed', 'chaged value on input');

    //       const v = String(inputs[i]['value']);
    //       CONFIG.LOG(v, 'value on iput');
    //       if (!v) { return; }
    //       inputs[i].setAttribute('value', v.replace(/[^0-9]/g, ''));
    //     }
    //     );
    //   }
    // }

  }


  ngOnInit() {

  }

  filter(event: any) {

    // if (event.query) {
    this.query = event.query;
    // }
    if (!this.query || this.query.length < 3) { return; }
    CONFIG.LOG(this.query, 'query in filter global search');
    this.cs.postList<SearchItem>(CONFIG.apiURL.layout.idSearch, { data: this.query })
      .subscribe(
      (res: SearchItem[]) => { this.items = res }
      );
  }

  selectItem(value: SearchItem) {
    if (value.id > 0) {
      CONFIG.LOG(value.url, 'url in global search');
      if (value.url.startsWith('/#/')) {
        this.win.boing(value.url, 'open_' + value.id.toString(), 1600, 960);
      } else {
        window.open(this.win.generateNesi1Url(value.url), '_blank');
      }
    }
    // this.query = value.search;
    // const inputs = document.getElementsByClassName('ui-autocomplete-input');
    // for (let i = 0; i < inputs.length; i++) {
    //   if (inputs[i]['id'] === 'idSearchText') {
    //     inputs[i]['value'] = '';
    //   }
    // }

  }

  showId(value) {
    //  CONFIG.LOG(this.query, 'query in showid global search');
    if (!value) { return ''; }
    const r = String(value).replace(this.query, '<i style="color:red;font-weight: bold">' + this.query + '</i>');
    //  CONFIG.LOG(r, 'result in showid global search');
    return '#' + r;

  }

}
