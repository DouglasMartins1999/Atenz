import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProfileComponent } from './content/profile/profile.component';
import { LoginComponent } from './login/login/login.component';
import { ContentModule } from './content/content.module';
import { LessonComponent } from './content/lesson/lesson.component';
import { SearchComponent } from './content/search/search.component';
import { ActivateRouteGuard } from './services/auth.guard';

const routes: Routes = [
  {
    path: "",
    canActivate: [ActivateRouteGuard],
    component: ProfileComponent
  },
  {
    path: "search",
    canActivate: [ActivateRouteGuard],
    component: SearchComponent
  },
  {
    path: "profile",
    canActivate: [ActivateRouteGuard],
    component: ProfileComponent
  },
  {
    path: "course",
    canActivate: [ActivateRouteGuard],
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
