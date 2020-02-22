import { ApplicationRoute } from '../models/applicationRoute.model';
import { MeComponent } from '../components/me/me.component';
import { AuthorizeGuard } from '../guards/authorize.guard';

export const commonRoutes: ApplicationRoute[] = [
    { path: 'me', icon: null, text: null, component: MeComponent, canActivate: [AuthorizeGuard] }
];