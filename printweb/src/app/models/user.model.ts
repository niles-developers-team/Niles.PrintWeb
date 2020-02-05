export interface IUser {
    id?: number;
    firstName: string;
    lastName: string;
    password: string;
    email: string;
    role: Roles;
    code?: string;
}

export interface IUserAuthenticated extends IUser {
    token?: string;
}

export enum Roles {
    Admin = 0,
    Tenant = 1,
    Client = 2
}