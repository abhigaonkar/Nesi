import { Pipe, PipeTransform } from '@angular/core';
import { TimerBase } from '../components/shared/Bases/TimerBase';

@Pipe({ name: 'timerFormat' })
export class TimerFormatPipe extends TimerBase implements PipeTransform {

  transform(second: number = 0): string {
    return this.getTimeString(second);
  }
}
