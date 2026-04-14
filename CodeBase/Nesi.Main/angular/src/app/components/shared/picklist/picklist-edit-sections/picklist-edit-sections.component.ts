
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TokenService } from '../../../../services/authentication/tokenService';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../reducers';
import { CoreService } from '../../../../services/shared/core.service';
import { CONFIG } from '../../../../configuration';
import { MessageBase } from 'app/core/messageBaseComponent';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';

@Component({
  selector: 'nesi-picklist-edit-sections',
  templateUrl: './picklist-edit-sections.component.html',
  styleUrls: ['./picklist-edit-sections.component.css']
})
export class PicklistEditSectionsComponent extends MessageBase implements OnInit {
  @Input() quote_id: string;
  @Input() revision: string;
  @Output() changed = new EventEmitter();
  @Output() deleted = new EventEmitter();

  sectionName: string;
  sectionList: any[];
  loading = false;
  selectedSection: any;

  constructor(
    private ts: TokenService,
    protected store: Store<fromRoot.State>,
    public cs: CoreService,
    private cf: ConfirmationService,
  ) {
    super(store);
  }

  ngOnInit() {
    this.loadSections();
  }

  get url() {
    return CONFIG.apiURL.page.shared.pickList.editSections + this.quote_id + '/' + this.revision;
  }

  loadSections(fireEvent = false) {
    this.cs.getList<any>(this.url)
      .subscribe(
      (res) => {
        this.sectionList = res;
        if (fireEvent) {
          this.changed.emit(
            this.sectionList.map(x => { return { label: x.section, value: x.id } })
          );
        }
      }
      );
  }

  addSection() {
    if (!this.sectionName) {
      this.PushWarnMessage('Section name is required.');
      return;
    }
    this.cs.postString(this.url, { data: this.sectionName })
      .subscribe(
      (res) => {
        if (this.PushShortResponseMessage(res)) {
          this.sectionName = '';
          this.loadSections(true);
        }
      }
      );
  }

  deleteSection(id: number) {
    if (!id) {
      return;
    }
    this.cf.confirm({
      message: 'Do your really want to delete the section?',
      accept: () => {
        this.cs.deleteString(this.url + '/' + id.toString())
          .subscribe(
          (res) => {
            if (this.PushShortResponseMessage(res)) {
              this.selectedSection=null;
              this.deleted.emit(id);
              this.loadSections(true);
            }
          }
          );
      }
    })
  }

  updateSection(id: number, name: string) {
    if (!id) {
      return;
    }
    if (!name) {
      this.PushWarnMessage('Section Name is required.');
      return;
    }
    this.cs.patchString(this.url, { label: name, value: id })
      .subscribe(
      (res) => {
        if (this.PushShortResponseMessage(res)) {
          this.selectedSection=null;
          this.loadSections(true);

        }
      }
      );
  }
}
