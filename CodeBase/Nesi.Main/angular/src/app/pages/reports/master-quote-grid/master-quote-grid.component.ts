import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '../../../components/nesi-datatable/components/datatable/datatable.component';
import { CONFIG } from 'app/configuration';
import { TokenService } from 'app/services/authentication/tokenService';
import { CoreService } from 'app/services/shared/core.service';
import { LabelValueInt } from 'app/models/Shared/labelValueString';

@Component({
  selector: 'nesi-master-quote-grid',
  templateUrl: './master-quote-grid.component.html',
  styleUrls: ['./master-quote-grid.component.css']
})
export class MasterQuoteGridComponent implements OnInit {

  @ViewChild(DatatableComponent) dt: DatatableComponent;
  rate = 'N/A';

  notesConfig = {
    call: this.updateNote,
    that: this
  };

  spinnerConfig = {
    min: 0,
    max:100,
    step: 30
  };

  public updateNote(data, note, that) {
    // console.dir(data);
    let quote = data;
    quote.note = note;
    that.cs.postDataExtra(that.linkForNote, quote).subscribe(
      (res) => {
        quote.notes = res.extra;
      }
    );

    // setTimeout(() => {
    //   let d = data;
    //   console.dir(d);
    //   d.notes = d.notes + newNote;
    //   console.log(d.notes );
    // }, 1000);
  }

  public displayEdit = false;
  public entity: any;
  public linkForEdit = CONFIG.apiURL.page.reports.masterQuoteGrid.edit;
  public linkForNote = CONFIG.apiURL.page.reports.masterQuoteGrid.addNote;
  public linkForUpdateChance = CONFIG.apiURL.page.reports.masterQuoteGrid.updateChance;
  public reasonList = [];

  constructor(
    public ts: TokenService,
    public cs: CoreService,
  ) {
  }

  ngOnInit() {
    this.cs.getList<LabelValueInt>(CONFIG.apiURL.page.reports.masterQuoteGrid.chance)
      .subscribe(
        (res) => {
          this.reasonList = res;
        }
      );
  }

  loadDetail() {
    this.dt.after_onRefresh();
  }

  public masterQuoteGridReportCallback(rowData: any, rowIndex: number) {
    const pct_chance = rowData['pct_chance'];
    if (pct_chance >= 75) {
      return 'masterQuoteGreen';
    } else if (pct_chance >= 50) {
      return 'masterQuoteYellow1';
    } else if (pct_chance >= 25) {
      return 'masterQuoteYellow2';
    }

    return ' ';
  }

  seachFinish(data: any) {
    this.rate = data.extra.caculationsOnAllColumns;
  }


  masterQuoteGridReportColumnCallback(rowData: any, rowIndex: number, columnName) {
    if (columnName !== 'notes') {
      return;
    }

    const value = rowData[columnName];
    if (!value || value.length < 1) {
      return;
    }

    return 'masterQuoteNotes'
  }

  // Open editor
  openEdit(event) {
    this.entity = event;
    this.displayEdit = true;
  }

  // Close editor
  exitFromEditor() {
    this.entity = null;
    this.displayEdit = false;
  }

  dropDownForChanceReason(event) {
    // console.dir(event);
    this.cs.postDataExtra(this.linkForUpdateChance, event.data).subscribe(
      (res) => {
      }
    );
  }

}
