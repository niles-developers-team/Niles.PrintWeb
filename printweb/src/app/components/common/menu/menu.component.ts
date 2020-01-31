import { Component, Input } from "@angular/core";
import { ApplicationRoute } from 'src/app/models/applicationRoute.model';

@Component({
    selector: 'main-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss', '../../app.component.css']
})
export class MenuComponent {
    @Input() public items: ApplicationRoute[] = [];
}