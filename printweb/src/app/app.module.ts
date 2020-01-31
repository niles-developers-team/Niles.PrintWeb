import { NgModule } from '@angular/core';
import { Routes, RouterModule, Route } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { FlexLayoutModule } from "@angular/flex-layout";
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { MaterialModule } from './material.module';
import { ApiUrlInterceptor } from './interceptors/url.iterceptor';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { SignUpComponent } from './components/signup/signup.component';
import { SignInComponent } from './components/signin/signin.component';
import { MenuComponent } from './components/common/menu/menu.component';
import { AdminComponent } from './components/admin/root.component';
import { AdminDashComponent } from './components/admin/dash/adminDash.component';
import { UsersComponent } from './components/admin/users/users.component';
import { BreadcrumbsComponent } from './components/common/breadcrumbs/breadcrumbs.component';
import { routes } from './sharedConstants/routes';


@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    AdminDashComponent,
    BreadcrumbsComponent,
    UsersComponent,
    MenuComponent,
    SignUpComponent,
    SignInComponent
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    RouterModule.forRoot(routes)
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ApiUrlInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
