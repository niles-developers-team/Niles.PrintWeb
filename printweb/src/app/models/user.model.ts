export interface IUser {
    id?: number;
    firstName: string;
    lastName: string;
    password: string;
    email: string;
    username: string;
}

export interface IUserAuthenticated extends IUser {
    code?: string;
    token?: string;
}