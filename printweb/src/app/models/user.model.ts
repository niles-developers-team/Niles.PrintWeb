import { NumberValueAccessor } from '@angular/forms';

export interface IUser {
    id?: number;
    firstName: string;
    lastName: string;
    password: string;
    email: string;
    role?: Roles;
    userName: string;
    code?: string;
}

export interface IUserAuthenticated extends IUser {
    code?: string;
    token?: string;
}

export interface IUserGetOptions {
    id?: number;
    role?: Roles;
    ids?: number[];
    onlyConfirmed?: boolean;
    search?: string;
    userName?: string;
    email?: string;
}

export interface IUserAuthorizeOptions {
    userNameOrEmail: string;
    password: string;
    rememberMe: boolean;
}

export enum Roles {
    Admin = 0,
    Tenant = 1,
    Client = 2
}