import { Injectable } from "@angular/core";
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserService } from '../services/user.service';

@Injectable({ providedIn: 'root' })
export class AdminGuard implements CanActivate {
    constructor(
        private router: Router,
        private userService: UserService
    ) { }

    //FIXME: Needs to implement this method
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return true;
    }
}