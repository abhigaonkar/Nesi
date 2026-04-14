import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { CONFIG } from '../../../configuration';
import { CoreService } from '../../../services/shared/core.service';

@Component({
  selector: 'nesi-workorder-main-with-file',
  templateUrl: './workorder-main-with-file.component.html',
  styleUrls: ['./workorder-main-with-file.component.css']
})
export class WorkorderMainWithFileComponent implements OnInit {

  public pdf_path: string;

  public position: any;
  public init_position: any;

  public sub_postion = new Subject();

  constructor(
    private cs: CoreService,
  ) { }

  ngOnInit() {
    this.get_position();
    this.sub_postion.subscribe(
      pos => this.save_position(pos)
    );
  }

  resize(e) {
    this.sub_postion.next(e);
  }

  save_position(pos) {
    this.cs.saveProfileValueByPropertyName('workorder_split_position', pos)
      .subscribe(res => {
        CONFIG.LOG(pos, 'saved position position value');
      });
  }
  get_position() {
    this.cs.getProfileValueByPropertyName('workorder_split_position')
      .subscribe(res => {
        if (res) {
          this.init_position = res;
          this.position = this.init_position;
          CONFIG.LOG(this.init_position, 'get init position position value');
        }
      });
  }

  get_pdf_path(e) {
    this.pdf_path = e.data;
  }
}
