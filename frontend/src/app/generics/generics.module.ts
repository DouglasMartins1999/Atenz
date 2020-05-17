import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeaderComponent } from './header/header.component';
import { CourseCardComponent } from './course-card/course-card.component';
import { BookCardComponent } from './book-card/book-card.component';
import { LessonCardComponent } from './lesson-card/lesson-card.component';

@NgModule({
	declarations: [
		HeaderComponent, 
		CourseCardComponent, 
		BookCardComponent, 
		LessonCardComponent
	],
	imports: [
		CommonModule
	],
	exports: [
		HeaderComponent, 
		CourseCardComponent,
		BookCardComponent,
		LessonCardComponent
	]
})
export class GenericsModule { }
