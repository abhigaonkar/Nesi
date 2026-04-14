import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { SplitBehaviourDirective } from './splitBehaviour.directive';
import { SplitContainerComponent } from './splitContainer.component';
import { SplitterComponent } from './splitter.component';

@NgModule({
    declarations: [
        SplitBehaviourDirective,
        SplitterComponent,
        SplitContainerComponent
    ],
    entryComponents: [
        SplitterComponent
    ],
    exports: [SplitContainerComponent,
        SplitterComponent,
        SplitBehaviourDirective,
    ]
})
export class SplitterModule {

}
