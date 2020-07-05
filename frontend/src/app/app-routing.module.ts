import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProfileComponent } from './content/profile/profile.component';
import { LoginComponent } from './login/login/login.component';
import { LessonComponent } from './content/lesson/lesson.component';
import { SearchComponent } from './content/search/search.component';
import { ActivateRouteGuard } from './services/auth.guard';
import { ContentModule } from './content/content.module';
import { BookComponent } from './content/book/book.component';

const routes: Routes = [
  {
    path: "login",
    component: LoginComponent
  },
  {
    path: "search",
    canActivate: [ActivateRouteGuard],
    component: SearchComponent
  },
  {
    path: "view",
    canActivate: [ActivateRouteGuard],
    component: LessonComponent
  },
  {
    path: "read",
    canActivate: [ActivateRouteGuard],
    component: BookComponent
  },
  {
    path: "",
    canActivate: [ActivateRouteGuard],
    component: ProfileComponent
  },
  {
    path: ":section",
    canActivate: [ActivateRouteGuard],
    component: ProfileComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
