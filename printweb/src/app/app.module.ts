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
import { AuthorizeGuard } from './guards/authorize.guard';
import { MenuComponent } from './components/common/menu/menu.component';
import { AdminComponent } from './components/admin/root.component';
import { adminRoutes } from './sharedConstants/adminRoutes';
import { AdminDashComponent } from './components/admin/dash/adminDash.component';
import { UsersComponent } from './components/admin/users/users.component';

const adminChildren: Routes = adminRoutes.map(o => {
  const route: Route = { path: o.path, component: o.component, canActivate: o.canActivate };
  return route;
});

const appRoutes: Routes = [
  { path: 'admin', redirectTo: 'dash'},
  { path: 'admin', component: AdminComponent, children: adminChildren },
  { path: 'signup', component: SignUpComponent, canActivate: [AuthorizeGuard] },
  { path: 'signin', component: SignInComponent, canActivate: [AuthorizeGuard] }
];

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    AdminDashComponent,
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
    RouterModule.forRoot(appRoutes)
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ApiUrlInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
