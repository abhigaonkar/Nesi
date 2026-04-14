import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CoreService } from 'app/services/shared/core.service';
import { CONFIG } from 'app/configuration';

@Component({
  selector: 'nesi-inventory-image',
  templateUrl: './inventory-image.component.html',
  styleUrls: ['./inventory-image.component.css']
})
export class InventoryImageComponent implements OnInit {
  @Input() master_id: number;
  image: any;
  tooltipX: string;
  tooltipY: string;
  @Input() width: string;
  @Input() height: string;
  @Output() startLoad = new EventEmitter();
  @Output() endLoad = new EventEmitter();
  @Input() imagesBuffer: Map<number, any>;

  constructor(
    private cs: CoreService,
  ) { }

  ngOnInit() {
    this.getImage();
  }

  getImage() {
    if (this.master_id) {
      // CONFIG.LOG(this.imagesBuffer, 'images buffer in iventory image');
      // CONFIG.LOG(this.imagesBuffer.get(this.master_id), 'get image from images buffer in iventory image');
      // if (this.imagesBuffer && !this.imagesBuffer.get(this.master_id)) {
        this.startLoad.emit({ data: this.master_id });
        this.cs.getString(CONFIG.apiURL.page.shared.pickList.InventoryPicture + this.master_id)
          .subscribe(
          (res) => {
            this.image = 'data:image/png;base64,' + res;
            this.endLoad.emit({ master_id: this.master_id, data: this.image });
          }
          );
      // } else {
      //   this.image = this.imagesBuffer.get(this.master_id);
      // }
    }
  }
  moveTip(event: any) {
    this.tooltipX = (event.clientX + 20) + 'px';
    this.tooltipY = (event.clientY + 20) + 'px';
  }
}
