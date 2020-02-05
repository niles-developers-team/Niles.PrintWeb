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

        if (!isAuthorizeRoute) {
            if (currentUser) {
                // logged in so return true
                return true;
            }

            this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
            return false;
        }

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/signin'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}