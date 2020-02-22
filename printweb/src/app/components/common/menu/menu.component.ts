import { Component, Input, ChangeDetectionStrategy } from "@angular/core";
import { MenuItem } from 'src/app/models/menuItem.model';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { IUser } from 'src/app/models/user.model';

@Component({
    selector: 'main-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss', '../../../app.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class MenuComponent {
    @Input() public readonly items: MenuItem[] = [];
    public userAuthenticated: boolean;
    public currentUserShortening: string;
    private readonly _currentUser: BehaviorSubject<IUser>;

    constructor(
        private readonly _userService: UserService,
        private readonly _router: Router
    ) { 
        this._currentUser = _userService.currentUserBehavior;
    }

    ngOnInit(): void {
        //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    this._currentUser.subscribe(user => {
        this.currentUserShortening = this._userService.getUserShortening(this._currentUser.value);
        this.userAuthenticated = Boolean(this._currentUser.value);
      });        
    }
}