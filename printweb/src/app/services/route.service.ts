import { Injectable } from '@angular/core';
import { UserService } from './user.service';
import { MenuItem } from '../models/menuItem.model';
import { IUser, Roles } from '../models/user.model';
import { adminRoutes } from '../sharedConstants/adminRoutes';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RouteService {
    private _currentUser: BehaviorSubject<IUser>;

    constructor(private readonly _userService: UserService) {
        this._currentUser = this._userService.currentUserBehavior;
    }

    public get enabledMenuItems(): MenuItem[] {
        const user = this._currentUser.value
        const userRole = user !== null ? user.role : null;

        switch(userRole)
        {
            case Roles.Admin:
                return adminRoutes.map(o => {
                    const item: MenuItem = { path: o.path, text: o.text, icon: o.icon };
                    return item;
                });

            default: return [];
        }
    }
}