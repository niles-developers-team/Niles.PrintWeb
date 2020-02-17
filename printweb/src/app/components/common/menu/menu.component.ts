import { Component, Input } from "@angular/core";
import { MenuItem } from 'src/app/models/menuItem.model';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';

@Component({
    selector: 'main-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss', '../../../app.component.scss']
})
export class MenuComponent {
    @Input() public readonly items: MenuItem[] = [];
    @Input() public readonly dashItem: MenuItem;
    public currentUserShortening: string;

    constructor(
        private readonly _userServce: UserService,
        private readonly _router: Router
    ) { 
        this.currentUserShortening = this._userServce.currentUserShortening;
    }
}