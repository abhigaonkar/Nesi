import {
  Component, ContentChildren,
  QueryList, ComponentFactoryResolver, ViewContainerRef,
  AfterContentInit, Output, Input, EventEmitter
} from '@angular/core';

import { SplitBehaviourDirective, SplitBehaviour, Position } from './splitBehaviour.directive';
import { SplitterComponent } from './splitter.component';

/**
 * Hosts resizable content areas divided by a draggable border (splitter).
 *
 * The split container defined flex attributes to allow the horizontal arrangement of child content areas.
 * On initialization, it will query all child elements in the light DOM annotated by the split-behaviour directive
 * and separate them by splitters.
 * As the splitBehaviour directive manages concrete content area resizing, dragging events of the splitter (positionChanged) are subscribed
 * and propagated to the directive.
 **/
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'split-container',
  template: `
        <div class="split-container">
            <ng-content></ng-content>
        </div>
    `,
  styles: [`
        .split-container {
            display: flex;
            flex-direction: row;
            flex-wrap: no-wrap;
            flex-grow: 1;
            height:100%;
        }
    `]
})
export class SplitContainerComponent implements AfterContentInit {
  // Workaround: We want to query all child elements hosting a SplitBehaviourDirective instance,
  // but we both need the respective ViewContainerRef (for splitter element creation)
  // as well as the respective Directive implementation instance.
  // There might be a better way to achive this...
  @ContentChildren(SplitBehaviourDirective, { read: ViewContainerRef })
  private panesVcr: QueryList<ViewContainerRef>;

  @ContentChildren(SplitBehaviourDirective)
  private panes: QueryList<SplitBehaviourDirective>;
  private fixedPane: any;

  @Output() positionChange = new EventEmitter();
  @Input() position: any;
  @Input() set init_position(value: any) {
    if (value) {
      if (this.fixedPane) {
        this.fixedPane.resize(value);
      }
    }
  }
  @Output() resize = new EventEmitter();

  constructor(private resolver: ComponentFactoryResolver) {
  }

  public ngAfterContentInit(): void {
    const splitterFactory = this.resolver.resolveComponentFactory(SplitterComponent);

    const paneDirectives = this.panes.toArray();
    this.panesVcr.map((vcr, idx) => {
      if (paneDirectives[idx].behaviour === SplitBehaviour[SplitBehaviour.fixed]) {
        const splitter = vcr.createComponent(splitterFactory);
        splitter.instance.splitBehaviour = paneDirectives[idx];
        this.fixedPane = paneDirectives[idx];
        if (this.position) {
          paneDirectives[idx].resize(this.position);
        }
        splitter.instance.positionChanged.subscribe((pos: Position) => {
          paneDirectives[idx].resize(pos);
          this.position = pos;
          this.positionChange.emit(this.position);
        });
        splitter.instance.positionDone.subscribe((pos: Position) => {
          paneDirectives[idx].resize(pos);
          this.position = pos;
          this.positionChange.emit(this.position);
          this.resize.emit(pos);
        });
      }
    });
  }
}
