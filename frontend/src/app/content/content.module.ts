import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile/profile.component';
import { GenericsModule } from '../generics/generics.module';
import { WatchingComponent } from './profile/watching/watching.component';
import { LessonComponent } from './lesson/lesson.component';

@NgModule({
	declarations: [
		ProfileComponent, 
		WatchingComponent, LessonComponent
	],
	imports: [
		CommonModule,
		GenericsModule
	]
})
export class ContentModule { }
