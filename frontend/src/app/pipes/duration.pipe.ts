import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({ name: 'timeduration' })
export class DurationPipe implements PipeTransform {
    transform(value: string): string {
        const time = value?.split(":") || ['0'];
        return parseInt(time[0]) === 0 ? time?.slice(1).join(":") : time?.join(":")
    } 
}

@Pipe({ name: 'durationconcat' })
export class DurationConcatPipe implements PipeTransform {
    transform(value: string[]): string {
        const date = moment("2000-01-01");
		const dur = date.clone()

		for(let v of value){
			dur.add(v);
        }

		return `${dur.diff(date, "hours")}:${dur.format("mm:ss")}`
    }
}