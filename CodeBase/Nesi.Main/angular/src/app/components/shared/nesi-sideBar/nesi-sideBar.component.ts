import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { WindowRef } from '../../../services/shared/windowRef';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nesi-sidebar',
  templateUrl: './nesi-sideBar.component.html',
  styleUrls: ['./nesi-sideBar.component.css']
})
export class NesiSideBarComponent implements OnInit {

  @Input() visible = false;
  @Input() header: string;
  @Input() larger = false;
  @Input() submitting = false;

  @Output() onShow = new EventEmitter;
  @Output() onHide = new EventEmitter;
  @Output() visibleChange = new EventEmitter;
  @Input() fullScreen: false;
  @Input() open_url: string;

  constructor(
    private win: WindowRef,
  ) { }

  ngOnInit() {
  }

  on_hide(event) {
    this.visible = false;
    this.visibleChange.emit(this.visible);
    this.onHide.emit(event)
  }

  get styleClass(): string {
    if (this.fullScreen) {
      return null;
    } else {
      if (this.larger) {
        return 'ui-sidebar-xlg';
      } else {
        return 'ui-sidebar-lg';
      }
    }
  }

  open_window() {
    this.win.boingNesi1(this.open_url, this.header);
    this.visible = false;
  }
}

