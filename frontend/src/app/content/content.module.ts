import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile/profile.component';
import { GenericsModule } from '../generics/generics.module';
import { WatchingComponent } from './profile/watching/watching.component';
import { LessonComponent } from './lesson/lesson.component';
import { SearchComponent } from './search/search.component';
import { PipesModule } from '../pipes/pipes.module';
import { PlyrModule } from 'ngx-plyr';
import { BookComponent } from './book/book.component';

@NgModule({
	declarations: [
		ProfileComponent, 
		WatchingComponent, 
		LessonComponent, 
		SearchComponent, 
		BookComponent
	],
	imports: [
		CommonModule,
		GenericsModule,
		PipesModule,
		PlyrModule
	]
})
export class ContentModule { }
