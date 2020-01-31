import { Component } from '@angular/core';
import { ApplicationRoute } from 'src/app/models/applicationRoute.model';
import { adminRoutes } from 'src/app/sharedConstants/adminRoutes';
import { MenuItem } from 'src/app/models/menuItem.model';

@Component({
    selector: 'admin',
    templateUrl: './root.component.html',
    styleUrls: ['../../app.component.scss']
})
export class AdminComponent {
    public routes: MenuItem[] = adminRoutes.map(o => {
        const item: MenuItem = { path: o.path, text: o.text, icon: o.icon };
        return item;
    });

    constructor() { }

}