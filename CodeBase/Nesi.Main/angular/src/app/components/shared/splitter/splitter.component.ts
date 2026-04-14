import { Component, EventEmitter, Output, ViewChild, ElementRef } from '@angular/core';
import { SplitBehaviourDirective, Position } from './splitBehaviour.directive';

/**
 * A horizontal, draggable divider element between resizable content areas within a split container.
 * This component and its creation is managed by a split container.
 **/
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'splitter',
  template: `
        <div #splitter class="splitter"
            (mousedown)="onMouseDown($event)"
            (document:mouseup)="onMouseUp($event)"
            (document:mousemove)="onMouseMove($event)"></div>
    `,
  // tslint:disable-next-line:use-host-property-decorator
  host: { 'style': 'position:relative' },
  styles: [`
        .splitter {
            flex: 0 0 auto;
            width: 10px;
            height:102%;
            cursor: col-resize;

            background-image:url('/assets/images/split-horizontal.svg');
            background-position:50% 46%;
            background-repeat:no-repeat;
            margin-top: -12px;

            /* Needed for height:100% without having an explicit height given to the parent */
            position:absolute;
        }
    `]
})
export class SplitterComponent {
  public startX: number;

  public startWidth: number;

  public dragging: boolean;

  @ViewChild('splitter')
  public element: ElementRef;

  public _splitBehaviour: SplitBehaviourDirective;

  @Output()
  public positionChanged: EventEmitter<Position> = new EventEmitter();
  @Output()
  public positionDone: EventEmitter<Position> = new EventEmitter();

  constructor() {
    this.dragging = false;
  }

  public set splitBehaviour(value: SplitBehaviourDirective) {
    this._splitBehaviour = value;
  }

  public get splitBehaviour() {
    return this._splitBehaviour;
  }

  public onMouseDown(e: MouseEvent): void {
    this.dragging = true;
    this.startX = e.clientX;
    this.startWidth = this.splitBehaviour.getElementWidth();
  }

  public onMouseUp(e: MouseEvent): void {
    if (this.dragging) {
      this.positionDone.emit(new Position(this.startWidth + e.clientX - this.startX, e.pageY));
    }
    this.dragging = false;
  }

  public onMouseMove(e: MouseEvent): void {
    if (this.dragging) {
      this.positionChanged.emit(new Position(this.startWidth + e.clientX - this.startX, e.pageY));
    }
  }

  public onMouseLeave(e: MouseEvent): void {
    this.dragging = false;
  }
}
