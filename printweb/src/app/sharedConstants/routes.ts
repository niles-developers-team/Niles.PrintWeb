import { AdminComponent } from '../components/admin/root.component';
import { SignUpComponent } from '../components/signup/signup.component';
import { AuthorizeGuard } from '../guards/authorize.guard';
import { SignInComponent } from '../components/signin/signin.component';
import { adminRoutes } from './adminRoutes';
import { Routes, Route } from '@angular/router';

const appRoutes: Routes = adminRoutes.map(o => {
    const route: Route = { path: o.path, component: o.component, canActivate: o.canActivate, outlet: o.outlet, data: { breadcrumb: o.text } };
    return route;
});

export const routes: Routes = [
    { path: 'admin', redirectTo: 'admin/dash', pathMatch: 'full' },
    { path: 'admin', component: AdminComponent, children: appRoutes, data: { breadcrumb: 'Admin', parent: true } },
    { path: 'signup', component: SignUpComponent, canActivate: [AuthorizeGuard] },
    { path: 'signin', component: SignInComponent, canActivate: [AuthorizeGuard] }
]