import { Component } from '@angular/core';
import { MenuItem } from './models/menuItem.model';
import { RouteService } from './services/route.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'printweb';
  public readonly routes: MenuItem[];
  public readonly dashRoutes: MenuItem;

  constructor(private readonly _routeService: RouteService) {
    const routes = this._routeService.enabledMenuItems;
    this.routes = routes?.slice(1);
    this.dashRoutes = routes[0];
  }
}
