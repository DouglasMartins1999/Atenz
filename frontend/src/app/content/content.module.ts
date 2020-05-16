import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile/profile.component';
import { GenericsModule } from '../generics/generics.module';
import { WatchingComponent } from './profile/watching/watching.component';

@NgModule({
  declarations: [ProfileComponent, WatchingComponent],
  imports: [
    CommonModule,
    GenericsModule
  ]
})
export class ContentModule { }
