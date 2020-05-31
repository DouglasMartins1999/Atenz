import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'padstart' })
export class PaddingStartPipe implements PipeTransform {
    transform(value: string, chars: number, replace: string = "0"): string {
        return value.toString().padStart(chars, replace);
    } 
}