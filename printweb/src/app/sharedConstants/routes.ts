import { AdminComponent } from '../components/admin/root.component';
import { SignUpComponent } from '../components/signup/signup.component';
import { AuthenticateGuard } from '../guards/authenticate.guard';
import { SignInComponent } from '../components/signin/signin.component';
import { adminRoutes } from './adminRoutes';
import { Routes, Route } from '@angular/router';
import { AuthorizeGuard } from '../guards/authorize.guard';

const appRoutes: Routes = adminRoutes.map(o => {
    const route: Route = { path: o.path, component: o.component, canActivate: o.canActivate, outlet: o.outlet, data: { breadcrumb: o.text } };
    return route;
});

export const routes: Routes = [
    { path: 'admin', redirectTo: 'admin/dash', pathMatch: 'full' },
    { path: 'admin', component: AdminComponent, canActivate: [AuthenticateGuard, AuthorizeGuard], children: appRoutes, data: { breadcrumb: 'Admin', parent: true } },
    { path: 'signup', component: SignUpComponent, canActivate: [AuthenticateGuard] },
    { path: 'signin', component: SignInComponent, canActivate: [AuthenticateGuard] }
]