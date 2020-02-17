import { Component } from '@angular/core';
import { IUser } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
    selector: 'me',
    templateUrl: './me.component.html',
    styleUrls: ['../../../app.component.scss']
})
export class MeComponent {
    currentUser: IUser;

    constructor(private readonly _userService: UserService)
    {
        this.currentUser = this._userService.currentUserValue;
    }
}