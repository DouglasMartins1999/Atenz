import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile/profile.component';
import { GenericsModule } from '../generics/generics.module';
import { WatchingComponent } from './profile/watching/watching.component';
import { LessonComponent } from './lesson/lesson.component';
import { SearchComponent } from './search/search.component';
import { PipesModule } from '../pipes/pipes.module';

@NgModule({
	declarations: [
		ProfileComponent, 
		WatchingComponent, 
		LessonComponent, 
		SearchComponent
	],
	imports: [
		CommonModule,
		GenericsModule,
		PipesModule
	]
})
export class ContentModule { }
