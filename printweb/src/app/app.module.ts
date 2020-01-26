import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { FlexLayoutModule } from "@angular/flex-layout";
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { MaterialModule } from './material.module';
import { ApiUrlInterceptor } from './interceptors/url.iterceptor';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { MainComponent } from './components/main/main.component';
import { SignUpComponent } from './components/signup/signup.component';
import { SignInComponent } from './components/signin/signin.component';
import { AuthorizeGuard } from './guards/authorize.guard';

const appRoutes: Routes = [
  { path: '', component: MainComponent },
  { path: 'signup', component: SignUpComponent, canActivate: [AuthorizeGuard] },
  { path: 'signin', component: SignInComponent, canActivate: [AuthorizeGuard] }
];

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
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
    RouterModule.forRoot(appRoutes)
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ApiUrlInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
