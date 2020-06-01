import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserService } from '../services/user.service';
import { Roles } from '../models/user.model';


@Injectable({ providedIn: 'root' })
export class AuthenticateGuard implements CanActivate {
    constructor(
        private router: Router,
        private userService: UserService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.userService.currentUserValue;
        const currentUrl = state.url;

        const isAuthorizeRoute = currentUrl === '/signin' || currentUrl === '/signup';

        if (currentUrl === '/') {
            switch (currentUser.role) {
                case Roles.Admin:
                    this.router.navigateByUrl('/admin');
                    return true;

                case Roles.Tenant:
                    this.router.navigateByUrl('/tenant');
                    return true;
            }
        }

        if (currentUser && isAuthorizeRoute) {
            // logged in so redirect to main page
            this.router.navigateByUrl('/');
            return false;
        }

        if (currentUser && !isAuthorizeRoute)
            // logged in so return true
            return true;

        if (!currentUser && isAuthorizeRoute)
            return true;

        // not logged in so redirect to login page with the return url
        this.router.navigateByUrl('/signin');
        return false;
    }
}