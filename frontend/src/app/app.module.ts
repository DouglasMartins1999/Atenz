import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginModule } from './login/login.module';
import { NavbarComponent } from './navbar/navbar.component';
import { GenericsModule } from './generics/generics.module';
import { RequestInterceptorConfig } from './services/request.interceptor';
import { ActivateRouteGuard } from './services/auth.guard';

import ptBr from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
registerLocaleData(ptBr);

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
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
  ],
  providers: [
    ActivateRouteGuard,
    RequestInterceptorConfig,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
