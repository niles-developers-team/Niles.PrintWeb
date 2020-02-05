import { ApplicationRoute } from "../models/applicationRoute.model";
import { AuthenticateGuard } from '../guards/authenticate.guard';
import { AuthorizeGuard } from '../guards/authorize.guard';
import { UsersComponent } from '../components/admin/users/users.component';
import { AdminDashComponent } from '../components/admin/dash/adminDash.component';

export const adminRoutes: ApplicationRoute[] = [
    { path: 'dash', icon: 'dashboard', text: 'Dashboard', component: AdminDashComponent, canActivate: [AuthorizeGuard] },
    { path: 'users', icon: 'face', text: 'Users', component: UsersComponent, canActivate: [AuthorizeGuard] }
];