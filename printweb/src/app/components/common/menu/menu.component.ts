import { Component, Input } from "@angular/core";
import { MenuItem } from 'src/app/models/menuItem.model';

@Component({
    selector: 'main-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss', '../../../app.component.scss']
})
export class MenuComponent {
    @Input() public items: MenuItem[] = [];

    public collapsed: boolean = true;

    public onLogoClick() {
        this.collapsed = !this.collapsed;
    }
}