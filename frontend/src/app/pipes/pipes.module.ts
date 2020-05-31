import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DurationPipe } from './duration.pipe';
import { PaddingStartPipe } from './padding.pipe';
import { FileSizePipe } from './filesize.pipe';
import { FileFormatPipe } from './format.pipe';

@NgModule({
	declarations: [
		DurationPipe,
		PaddingStartPipe,
		FileSizePipe,
		FileFormatPipe
	],
	imports: [
		CommonModule
	],
	exports: [
		DurationPipe,
		PaddingStartPipe,
		FileSizePipe,
		FileFormatPipe
	]
})
export class PipesModule { }