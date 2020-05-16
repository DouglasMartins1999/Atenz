import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeaderComponent } from './header/header.component';
import { CourseCardComponent } from './course-card/course-card.component';
import { BookCardComponent } from './book-card/book-card.component';

@NgModule({
	declarations: [
		HeaderComponent, 
		CourseCardComponent, 
		BookCardComponent
	],
	imports: [
		CommonModule
	],
	exports: [
		HeaderComponent, 
		CourseCardComponent,
		BookCardComponent
	]
})
export class GenericsModule { }
