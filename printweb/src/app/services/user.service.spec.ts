import { UserService } from './user.service';
import { TestBed } from '@angular/core/testing';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { IUser, IUserAuthenticated, Roles } from '../models/user.model';
import { ApiUrlInterceptor } from '../interceptors/url.iterceptor';
import { JwtInterceptor } from '../interceptors/jwt.interceptor';

describe('User service', () => {
    let service: UserService;
    const newUser: IUser = {
        email: 'test@test.test',
        firstName: 'test',
        lastName: 'test',
        password: 'testuser',
        userName: 'testuser',
        role: Roles.Client
    };

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientModule],
            providers: [
                UserService,
                { provide: HTTP_INTERCEPTORS, useClass: ApiUrlInterceptor, multi: true },
                { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
            ]
        });

        service = TestBed.get(UserService);
    });

    it('#sign-in return user with token', (done: DoneFn) => {
        service.signin({ userName: 'admin', password: 'admin1', remeberMe: true })
            .subscribe((user: IUserAuthenticated) => {
                expect(user).not.toBeNull('#sign-in result is null');
                expect(user.token).not.toBeNull('#sign-in lost token');
                done();
            })
    });

    it('#get return users collection', (done: DoneFn) => {
        service.get({ onlyConfirmed: true }).subscribe(
            (users: IUser[]) => {
                expect(users.length).toBeGreaterThan(0);
                done();
            }
        )
    });

    it('#create successfully create user', (done: DoneFn) => {
        service.create(newUser).subscribe(
            (user: IUser) => {
                expect(user).not.toBeNull('#create not create user');
                expect(user).not.toBeUndefined('#create not create user');
                expect(user.id).not.toBeUndefined('#create not create user');
                newUser.id = user.id;
                done();
            }
        )
    });

    it('#update successfully update user', (done: DoneFn) => {
        newUser.userName = 'newtestuser';
        service.update(newUser).subscribe(
            (user: IUser) => {
                expect(user).not.toBeNull('#create not create user');
                expect(user).not.toBeUndefined('#create not create user');
                expect(user.id).not.toBeUndefined('#create not create user');
                done();
            }
        )
    })

    it('#delete successfully delete user', (done: DoneFn) => {
        newUser.userName = 'newtestuser';
        service.delete(newUser.id).subscribe(() => {
            expect().nothing();
            done();
        });
    })

    it('#sign-out succussfully', (done: DoneFn) => {
        service.signout();
        expect().nothing();
        done();
    });
});