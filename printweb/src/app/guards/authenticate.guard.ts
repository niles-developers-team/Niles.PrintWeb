import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserService } from '../services/user.service';


@Injectable({ providedIn: 'root' })
export class AuthenticateGuard implements CanActivate {
    constructor(
        private router: Router,
        private userService: UserService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.userService.currentUserValue;

        const isAuthorizeRoute = route.routeConfig.path === 'signin' || route.routeConfig.path === 'signup';

        if (currentUser && isAuthorizeRoute) {
            this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
            return false;
        }

        if (currentUser && !isAuthorizeRoute)
            // logged in so return true
            return true;

        if (!currentUser && isAuthorizeRoute)
            return true;

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/signin'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}