import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'fileformat' })
export class FileFormatPipe implements PipeTransform {
    transform(value: string): string {
        return value.split("?")[0].split(".").reverse()[0].toUpperCase() || null;
    } 
}