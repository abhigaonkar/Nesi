import { Component, OnInit } from '@angular/core';
import { CoreService } from '../../../services/shared/core.service';
import { saveAs } from 'file-saver';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-passwordEnhance',
  templateUrl: './passwordEnhance.component.html',
  styleUrls: ['./passwordEnhance.component.css']
})
export class PasswordEnhanceComponent implements OnInit {

  updateSQL: string;
  rollbackSQL: string;
  submitting = false;

  constructor(
    private cs: CoreService,
  ) { }

  ngOnInit() {
  }

  getScript() {
    this.submitting = true;
    this.cs.getObject('api/Page/PasswordEnhance')
      .subscribe((res: any) => {
        this.updateSQL = res.data;
        this.rollbackSQL = res.data2;
      });
  }

  save_Update_File() {
    const file = new File([this.updateSQL], 'updateSQL.txt', { type: 'text/plain;charset=utf-8' });
    saveAs(file);
  }

  save_Rollback_File() {
    const file = new File([this.rollbackSQL], 'rollBackSQL.txt', { type: 'text/plain;charset=utf-8' });
    saveAs(file);
  }
}
