import { Injectable } from "@angular/core";
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserService } from '../services/user.service';
import { Roles } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthorizeGuard implements CanActivate {
    private readonly _adminRelativePath = 'admin';
    private readonly _tenantRelativePath = 'tenant';

    constructor(
        private router: Router,
        private userService: UserService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.userService.currentUserValue;
        const currentUrl = state.url;

        const adminPath = currentUrl.includes(this._adminRelativePath);
        const tenantPath = currentUrl.includes(this._tenantRelativePath);

        if (adminPath && currentUser.role != Roles.Admin) {
            this.router.navigateByUrl('/forbidden');
            return false;
        }

        if (tenantPath && (currentUser.role != Roles.Admin && currentUser.role != Roles.Tenant)) {
            this.router.navigateByUrl('/forbidden');
            return false;
        }
        return true;
    }
}