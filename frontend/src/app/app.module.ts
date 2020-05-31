import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginModule } from './login/login.module';
import { NavbarComponent } from './navbar/navbar.component';
import { GenericsModule } from './generics/generics.module';
import { RequestInterceptorConfig } from './services/request.interceptor';
import { ActivateRouteGuard } from './services/auth.guard';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LoginModule,
    GenericsModule,
    HttpClientModule,
  ],
  providers: [
    RequestInterceptorConfig,
    ActivateRouteGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
