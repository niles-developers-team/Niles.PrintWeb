import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { FormControl, FormGroupDirective, NgForm, Validators, FormGroup } from '@angular/forms';
import { IUser } from 'src/app/models/user.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'sign-up',
  templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.css', '../../app.component.scss']
})
export class SignUpComponent {
  userForm: FormGroup;
  user: IUser;

  loading: boolean;

  constructor(private _service: UserService,
    private _snackbar: MatSnackBar,
    private _router: Router) { }

  ngOnInit(): void {
    this.loading = false;
    this.user = {
      firstName: '',
      email: '',
      lastName: '',
      password: '',
      userName: ''
    };
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.userForm = new FormGroup({
      username: new FormControl(this.user.userName, [
        Validators.required,
        Validators.minLength(5)
      ]),
      password: new FormControl(this.user.password, [
        Validators.required,
        Validators.minLength(6)
      ]),
      email: new FormControl(this.user.email, [
        Validators.required,
        Validators.email
      ]),
      firstname: new FormControl(this.user.firstName, [
        Validators.required
      ]),
      lastname: new FormControl(this.user.lastName, [
        Validators.required
      ])
    });
  }

  submit(): void {
    this.loading = true;
    this.userForm.disable();
    this._service.create(this.user)
      .subscribe(() => this._router.navigate(['/']),
        (error) => this._snackbar.open(error.message, 'Close', { duration: 3000 })
      )
      .add(() => {
        this.userForm.enable();
        this.loading = false;
      });
  };


  /* Handle form errors in Angular 8 */
  public errorHandling(control: string, error: string) {
    return this.userForm.controls[control].hasError(error, '');
  }
}
