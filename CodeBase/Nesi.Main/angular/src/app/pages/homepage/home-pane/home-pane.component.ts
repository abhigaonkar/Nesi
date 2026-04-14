import { Component, OnInit, Input } from '@angular/core';
import { CONFIG } from 'app/configuration';
import { CoreService } from 'app/services/shared/core.service';

@Component({
  selector: 'nesi-home-pane',
  templateUrl: './home-pane.component.html',
  styleUrls: ['./home-pane.component.css']
})
export class HomePaneComponent implements OnInit {
  @Input() title: string;
  @Input() level_1: number;
  @Input() level_2: number;
  @Input() redAge: number;
  @Input() name: string;
  @Input() status_name: string = "Status";

  items: any;
  loading = false;

  sortField = '';
  desc = false;
  constructor(
    public cs: CoreService,
  ) { }

  get url(): string {
    return CONFIG.apiURL.page.homepage.base + this.name;
  }

  sort(name) {
    if (this.sortField === name) {
      this.desc = !this.desc;
    } else {
      this.sortField = name;
      this.desc = false;
    }
  }

  ngOnInit() {
    this.cs.getList<any>(this.url)
      .subscribe(
      (res) => {
        this.items = res;
        this.loading = false;
      }
      );
  }

}
