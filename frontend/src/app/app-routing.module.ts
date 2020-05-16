import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProfileComponent } from './content/profile/profile.component';
import { LoginComponent } from './login/login/login.component';
import { ContentModule } from './content/content.module';
import { LessonComponent } from './content/lesson/lesson.component';

const routes: Routes = [
  {
    path: "",
    component: ProfileComponent
  },
  {
    path: "profile",
    component: ProfileComponent
  },
  {
    path: "course",
    component: LessonComponent
  },
  {
    path: "login",
    component: LoginComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
