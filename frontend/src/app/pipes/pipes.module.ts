import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DurationPipe, DurationConcatPipe } from './duration.pipe';
import { PaddingStartPipe } from './padding.pipe';
import { FileSizePipe } from './filesize.pipe';
import { FileFormatPipe } from './format.pipe';

@NgModule({
	declarations: [
		DurationPipe,
		DurationConcatPipe,
		PaddingStartPipe,
		FileSizePipe,
		FileFormatPipe
	],
	imports: [
		CommonModule
	],
	exports: [
		DurationPipe,
		DurationConcatPipe,
		PaddingStartPipe,
		FileSizePipe,
		FileFormatPipe
	]
})
export class PipesModule { }