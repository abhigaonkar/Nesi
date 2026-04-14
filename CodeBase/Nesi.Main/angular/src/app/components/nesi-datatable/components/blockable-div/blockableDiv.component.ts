import { ChangeDetectorRef, Input, Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { BlockableUI } from 'primeng/primeng';


@Component({
    // tslint:disable-next-line:component-selector
    selector: 'blockable-div',
    template: `
        <div [ngStyle]="style" [ngClass]="class" ><ng-content></ng-content></div>
    `
})
// tslint:disable-next-line:component-class-suffix
export class BlockableDiv implements BlockableUI {

    @Input() style: any;
    @Input() class: any;

    constructor(private el: ElementRef) {
    }

    getBlockableElement(): HTMLElement {
        return this.el.nativeElement.children[0];
    }

}
