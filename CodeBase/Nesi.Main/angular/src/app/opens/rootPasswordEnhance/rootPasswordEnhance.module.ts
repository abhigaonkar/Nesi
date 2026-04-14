import { NgModule } from '@angular/core';
import { ReleaseSystemModule } from '../../pages/releaseSystem/release-system.module';
import { RootPasswordEnhanceRouterModule } from './rootPasswordEnhance.routing.module';


@NgModule({
    imports: [
        RootPasswordEnhanceRouterModule,
        ReleaseSystemModule
    ],
    declarations: [
    ],
    providers: [
    ]
})
export class RootPasswordEnhanceModule { }
