import { Component } from "@angular/core";
import { UserService } from 'src/app/services/user.service';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';


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

    constructor(private _service: UserService,
        private _snackbar: MatSnackBar,
        private _router: Router) { }

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
        this.loading = true;
        this.userForm.disable();
        this._service.signin({
            userName: this.userNameOrEmail,
            password: this.password,
            remeberMe: this.rememberMe
        })
            .subscribe(() => this._router.navigate(['/']),
                (error) => this._snackbar.open(error.message, 'Close', { duration: 3000 })
            )
            .add(() => {
                this.userForm.enable();
                this.loading = false;
            });
    }

    /* Handle form errors in Angular 8 */
    public errorHandling(control: string, error: string): boolean {
        return this.userForm.controls[control].hasError(error, '');
    }
}
