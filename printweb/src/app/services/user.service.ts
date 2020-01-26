import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { IUser, IUserAuthenticated } from '../models/user.model';

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

    public get currentUserValue(): IUserAuthenticated {
        return this._currentUserSubject.value;
    }

    public get(options: { id?: number, ids?: number[], onlyConfirmed: boolean, search?: string }): Observable<IUser[]> {
        const params = new HttpParams();
        params.set('id', options.id.toString());
        params.set('ids', options.ids.toString());
        params.set('onlyConfirmed', options.onlyConfirmed.toString());
        params.set('search', options.search);
        return this.httpClient.get<IUser[]>(this._apiUrl, { params });
    }

    public signin(options: { userName: string, password: string, remeberMe: boolean }): Observable<IUserAuthenticated> {
        return this.httpClient.post<IUser>(`${this._apiUrl}/sign-in`, options, this._httpOptions)
            .pipe(map(data => {
                localStorage.setItem('currentUser', JSON.stringify(data));
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
        const params = new HttpParams();
        params.set('id', id.toString());
        return this.httpClient.delete(this._apiUrl);
    }

    public validateUser(options: { userName: string, email: string }) {
        const params = new HttpParams();

        params.set('userName', options.userName);
        params.set('email', options.email);

        return this.httpClient.get<string>(`${this._apiUrl}/validate`), { params };
    }
}