import { Component } from "@angular/core";
import { UserService } from 'src/app/services/user.service';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { IUserAuthorizeOptions } from 'src/app/models/user.model';


@Component({
    selector: 'sign-in',
    templateUrl: './signin.component.html',
    styleUrls: ['./signin.component.css', '../../app.component.scss']
})
export class SignInComponent {
    userForm: FormGroup;

    loading: boolean;

    constructor(private _service: UserService,
        private _snackbar: MatSnackBar,
        private _router: Router) { }

    ngOnInit(): void {
        //Called after the constructor, initializing input properties, and the first call to ngOnChanges.

        this.loading = false;

        this.userForm = new FormGroup({
            userNameOrEmail: new FormControl('', Validators.required),
            password: new FormControl('', Validators.required),
            rememberMe: new FormControl(false)
        });
    }

    submit(): void {
        this.loading = true;
        this.userForm.disable();

        const options: IUserAuthorizeOptions = this.userForm.value;

        this._service.signin(options)
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
