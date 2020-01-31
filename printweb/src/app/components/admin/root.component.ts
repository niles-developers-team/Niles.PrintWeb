import { Component } from '@angular/core';
import { ApplicationRoute } from 'src/app/models/applicationRoute.model';
import { adminRoutes } from 'src/app/sharedConstants/adminRoutes';

@Component({
    selector: 'admin',
    templateUrl: './root.component.html',
    styleUrls: ['../../app.component.css']
})
export class AdminComponent {
    public routes: ApplicationRoute[] = adminRoutes;

    constructor() {}

}