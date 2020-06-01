import { Routes, Route } from '@angular/router';

import { SignUpComponent } from '../components/signup/signup.component';
import { AuthenticateGuard } from '../guards/authenticate.guard';
import { SignInComponent } from '../components/signin/signin.component';
import { ForbiddenComponent } from '../components/common/forbidden/forbidden';

import { adminRoutes } from './adminRoutes';
import { commonRoutes } from './commonRoutes';

function compoundRoutes(): Routes {
    //transform application common routes
    const cmnRoutes: Route[] = commonRoutes.map(o => {
        const route: Route = {
            path: o.path,
            component: o.component,
            canActivate: o.canActivate,
            outlet: o.outlet,
            data: { breadcrumb: o.text }
        };
        return route;
    });

    //transform application admin routes
    const admRoutes: Routes = adminRoutes.map(o => {
        const route: Route = {
            path: o.path,
            component: o.component,
            canActivate: o.canActivate,
            outlet: o.outlet,
            data: { breadcrumb: o.text }
        };
        return route;
    });

    const authRoutes = [
        { path: 'signup', component: SignUpComponent, canActivate: [AuthenticateGuard] },
        { path: 'signin', component: SignInComponent, canActivate: [AuthenticateGuard] }
    ];

    const systemRoutes = [
        { path: 'forbidden', component: ForbiddenComponent }
    ];

    const routes: Route[] = [];

    routes.push(...cmnRoutes);
    routes.push(...admRoutes);
    routes.push(...authRoutes);
    routes.push(...systemRoutes);

    return routes;
};

export const routes: Routes = compoundRoutes();