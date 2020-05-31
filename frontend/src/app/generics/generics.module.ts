import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeaderComponent } from './header/header.component';
import { CourseCardComponent } from './course-card/course-card.component';
import { BookCardComponent } from './book-card/book-card.component';
import { LessonCardComponent } from './lesson-card/lesson-card.component';
import { ModalComponent } from './modal/modal.component';
import { LoadingComponent } from './loading/loading.component';

@NgModule({
	declarations: [
		HeaderComponent, 
		CourseCardComponent, 
		BookCardComponent, 
		LessonCardComponent, 
		ModalComponent, 
		LoadingComponent
	],
	imports: [
		CommonModule
	],
	exports: [
		HeaderComponent, 
		CourseCardComponent,
		BookCardComponent,
		LessonCardComponent,
		ModalComponent,
		LoadingComponent
	]
})
export class GenericsModule { }
