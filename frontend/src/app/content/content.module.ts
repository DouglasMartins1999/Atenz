import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile/profile.component';
import { GenericsModule } from '../generics/generics.module';

@NgModule({
  declarations: [ProfileComponent],
  imports: [
    CommonModule,
    GenericsModule
  ]
})
export class ContentModule { }
