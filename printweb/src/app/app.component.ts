import { Component } from '@angular/core';
import { MenuItem } from './models/menuItem.model';
import { RouteService } from './services/route.service';
import { BehaviorSubject } from 'rxjs';
import { IUser } from './models/user.model';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'printweb';
  public routes: MenuItem[];
  public currentUser: BehaviorSubject<IUser>;

  constructor(
    private readonly _routeService: RouteService,
    private readonly _userService: UserService) {
    this.currentUser = _userService.currentUserBehavior;
  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.  
    this.currentUser.subscribe(user => {
      this.routes = this._routeService.enabledMenuItems;
    });
  }
}
