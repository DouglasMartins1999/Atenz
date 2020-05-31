import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DurationPipe } from './duration.pipe';
import { PaddingStartPipe } from './padding.pipe';

@NgModule({
	declarations: [
		DurationPipe,
		PaddingStartPipe
	],
	imports: [
		CommonModule
	],
	exports: [
		DurationPipe,
		PaddingStartPipe
	]
})
export class PipesModule { }