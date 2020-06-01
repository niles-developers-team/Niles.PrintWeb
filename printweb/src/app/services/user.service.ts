import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { IUser, IUserAuthenticated, IUserGetOptions, IUserAuthorizeOptions } from '../models/user.model';
import { isNull } from 'util';

@Injectable({ providedIn: 'root' })
export class UserService {
    private readonly _apiUrl = 'api/user';
    private _currentUserSubject: BehaviorSubject<IUserAuthenticated>;
    public currentUser: Observable<IUserAuthenticated>;

    private _httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(
        private httpClient: HttpClient
    ) {
        this._currentUserSubject = new BehaviorSubject<IUserAuthenticated>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this._currentUserSubject.asObservable();
    }

    public get currentUserBehavior(): BehaviorSubject<IUserAuthenticated> {
        return this._currentUserSubject;
    }

    public get currentUserValue(): IUserAuthenticated {
        return this._currentUserSubject.value;
    }

    public getUserShortening(user: IUser): string {
        let shortening: string = '';

        if(user)
        {
            shortening = `${user.firstName[0]}.${(user.lastName)[0]}.`;
        }

        return shortening;
    }

    public get(options: IUserGetOptions): Observable<IUser[]> {
        let params = new HttpParams();
        if (options.id)
            params = params.append('id', options.id.toString());
        if (options.ids)
            params = params.set('ids', options.ids.toString());
        if (!isNaN(options.role) && !isNull(options.role))
            params = params.set('role', options.role.toString());
        if (options.onlyConfirmed !== null)
            params = params.set('onlyConfirmed', options.onlyConfirmed.toString());
        if (options.search)
            params = params.set('search', options.search);
        if (options.userName)
            params = params.set('username', options.userName);
        if (options.email)
            params = params.set('email', options.userName);

        return this.httpClient.get<IUser[]>(this._apiUrl, { params });
    }

    public signin(options: IUserAuthorizeOptions): Observable<IUserAuthenticated> {
        return this.httpClient.post<IUser>(`${this._apiUrl}/sign-in`, options, this._httpOptions)
            .pipe(map(data => {
                localStorage.setItem('currentUser', JSON.stringify(data));
                this._currentUserSubject.next(data);
                return data;
            }));
    }

    public signout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this._currentUserSubject.next(null);
    }

    public confirm(code: string) {
        return this.httpClient.post(`${this._apiUrl}/confirm`, code, this._httpOptions);
    }

    public create(user: IUserAuthenticated): Observable<IUser> {
        return this.httpClient.post<IUserAuthenticated>(this._apiUrl, user, this._httpOptions);
    }

    public update(user: IUserAuthenticated): Observable<IUser> {
        return this.httpClient.put<IUser>(this._apiUrl, user, this._httpOptions);
    }

    public delete(id: number): Observable<any> {
        const params = new HttpParams().set('ids', JSON.stringify(id));
        return this.httpClient.delete(this._apiUrl, { params: params });
    }

    public changePassword(user: IUser): Observable<any> {
        const url = `${this._apiUrl}/change-password`;
        return this.httpClient.patch<IUser>(url, user, this._httpOptions);
    }

    public validateUser(options: { userName: string, email: string }) {
        const params = new HttpParams();

        params.set('userName', options.userName);
        params.set('email', options.email);

        return this.httpClient.get<string>(`${this._apiUrl}/validate`), { params };
    }
}