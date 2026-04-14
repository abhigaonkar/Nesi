import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TimerFormatPipe } from 'app/pipes/timerFormat';
import { GroupByPipe } from 'app/pipes/groupby.pipe';
import { SafeHtmlPipe } from 'app/pipes/safeHtml';
import { FileSizePipe } from 'app/pipes/fileSize';
import { OrderByPipe } from 'app/pipes/orderBy.pipe';
import { DisableControlDirective } from 'app/directives/disableControl';
import { PhoneFormatPipe } from 'app/pipes/phoneFormat.pipe';
import { StaleDatePipe } from './staleDate.pipe';

@NgModule({
  imports: [
    CommonModule,
  ],
  declarations: [
    TimerFormatPipe,
    OrderByPipe,
    GroupByPipe,
    SafeHtmlPipe,
    FileSizePipe,
    DisableControlDirective,
    PhoneFormatPipe,
    StaleDatePipe,
  ],
  exports: [
    TimerFormatPipe,
    OrderByPipe,
    GroupByPipe,
    SafeHtmlPipe,
    FileSizePipe,
    DisableControlDirective,
    PhoneFormatPipe,
    StaleDatePipe,
  ],
})
export class PipesModule { }
