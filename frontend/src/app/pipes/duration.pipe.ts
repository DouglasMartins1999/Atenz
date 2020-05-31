import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'timeduration' })
export class DurationPipe implements PipeTransform {
    transform(value: string): string {
        const time = value.split(":");
        return parseInt(time[0]) === 0 ? time.slice(1).join(":") : time.join(":")
    } 
}