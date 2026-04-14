import { Component, OnInit, Input, Output, EventEmitter, Renderer } from '@angular/core';
import { DeviceService } from '../../../services/authentication/device';
import { CONFIG } from '../../../configuration';

@Component({
  selector: 'bar-hoverIcon',
  templateUrl: './hoverIcon.component.html',
  styleUrls: ['./hoverIcon.component.css']
})
export class HoverIconComponent implements OnInit {
  @Input()
  icon: string;
  @Input()
  badge: number;
  @Input()
  badgeColor = 'orange';

  @Output() click = new EventEmitter();
  @Output() iconmouseover = new EventEmitter();


  constructor(private renderer: Renderer,
    private device: DeviceService
  ) { }

  ngOnInit() {
    if (this.device.isMobile) {
      this.icon = this.icon.replace('fa-2x', 'fa-15x');
    }
  }


  get badgeClass() {
    if (this.badge < 10) {
      return this.badgeColor + '-icon-badge';
    } else if (this.badge < 100) {
      return this.badgeColor + '-icon-badge-sm';
    } else {
      return this.badgeColor + '-icon-badge-xs';
    }
  }

  show(event: any, op: any) {
    if (!this.device.isMobile) {
      if (op.el && op.el.nativeElement && op.el.nativeElement.children && op.el.nativeElement.children[0]) {
        op.show(event);
        op.el.nativeElement.children[0].style.top = '15px';
      }
    }
    CONFIG.LOG('mouseover', 'hovericon');
    this.iconmouseover.emit(event);
  }
  onClick(event: any) {
    this.click.emit(event);
  }
}
