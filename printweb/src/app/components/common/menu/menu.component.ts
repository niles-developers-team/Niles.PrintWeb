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
    @Input() public items: MenuItem[] = [];

    constructor(private readonly _userServce: UserService,
        private readonly _router: Router
    ) {

    }

    public collapsed: boolean = true;

    public onLogoClick() {
        this.collapsed = !this.collapsed;
    }

    public signOut() {
        this._userServce.signout();
        this._router.navigateByUrl('/signin');
    }
}