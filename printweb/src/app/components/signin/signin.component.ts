import { Component } from "@angular/core";
import { UserService } from 'src/app/services/user.service';
import { FormControl, Validators, FormGroup } from '@angular/forms';

@Component({
    selector: 'sign-in',
    templateUrl: './signin.component.html',
    styleUrls: ['./signin.component.css', '../../app.component.css']
})
export class SignInComponent {
    userForm: FormGroup;

    loading: boolean;
    userNameOrEmail: string;
    password: string;
    rememberMe: boolean;

    constructor(private _service: UserService) { }

    ngOnInit(): void {
        //Called after the constructor, initializing input properties, and the first call to ngOnChanges.

        this.loading = false;

        this.userNameOrEmail = '';
        this.password = '';
        this.rememberMe = false;

        this.userForm = new FormGroup({
            usernameOrEmail: new FormControl(this.userNameOrEmail, [
                Validators.required
            ]),
            password: new FormControl(this.password, [
                Validators.required
            ]),
            rememberMe: new FormControl(this.rememberMe)
        });
    }

    submit(): void {
        this._service.signin({
            userName: this.userNameOrEmail,
            password: this.password,
            remeberMe: this.rememberMe
        }).subscribe(() => {

        });
    }

    /* Handle form errors in Angular 8 */
    public errorHandling(control: string, error: string): boolean {
        return this.userForm.controls[control].hasError(error, '');
    }
}