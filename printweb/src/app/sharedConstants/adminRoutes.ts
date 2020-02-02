import { ApplicationRoute } from "../models/applicationRoute.model";
import { AuthorizeGuard } from '../guards/authorize.guard';
import { AdminGuard } from '../guards/admin.guard';
import { UsersComponent } from '../components/admin/users/users.component';
import { AdminDashComponent } from '../components/admin/dash/adminDash.component';

//FIXME: Implement Authorize guard
export const adminRoutes: ApplicationRoute[] = [
    { path: 'dash', icon: 'dashboard', text: 'Dashboard', component: AdminDashComponent, canActivate: [AdminGuard] },
    { path: 'users', icon: 'face', text: 'Users', component: UsersComponent, canActivate: [AdminGuard] }
];