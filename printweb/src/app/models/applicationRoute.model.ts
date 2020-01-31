export interface ApplicationRoute {
    path: string;
    component: any;
    canActivate: any[];
    text: string;
    icon?: any;
    outlet?: string;
}