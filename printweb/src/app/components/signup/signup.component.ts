import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';

@Component({
    selector: 'sign-up',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.css', '../../app.component.css']
})
export class SignUpComponent {
    loading: boolean;
    userName: string;
    password: string;
    rememberMe: boolean;

    constructor(private _service: UserService) { }

    signIn() {
        this._service.signin({
            userName: this.userName,
            password: this.password,
            remeberMe: this.rememberMe
        }).subscribe(result => {

        });
    }
}