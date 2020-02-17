import { Injectable } from '@angular/core';
import { UserService } from './user.service';
import { MenuItem } from '../models/menuItem.model';
import { IUser, Roles } from '../models/user.model';
import { adminRoutes } from '../sharedConstants/adminRoutes';

@Injectable({ providedIn: 'root' })
export class RouteService {
    private readonly _currentUser: IUser;

    constructor(private readonly _userService: UserService) {
        this._currentUser = this._userService.currentUserValue; 
    }

    public get enabledMenuItems(): MenuItem[] {
        const userRole = this._currentUser !== null ? this._currentUser.role : null;

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