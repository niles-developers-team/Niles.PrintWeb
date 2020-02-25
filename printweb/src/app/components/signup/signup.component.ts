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
  form: FormGroup;

  loading: boolean;

  constructor(private _service: UserService,
    private _snackbar: MatSnackBar,
    private _router: Router) { }

  ngOnInit(): void {
    this.loading = false;
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.form = new FormGroup({
      userName: new FormControl('', [
        Validators.required,
        Validators.minLength(5)
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(6)
      ]),
      email: new FormControl('', [
        Validators.required,
        Validators.email
      ]),
      firstName: new FormControl('', [
        Validators.required
      ]),
      lastName: new FormControl('', [
        Validators.required
      ])
    });
  }

  submit(): void {
    this.loading = true;
    this.form.disable();

    const user: IUser = this.form.value;

    this._service.create(user)
      .subscribe(() => this._router.navigate(['/']),
        (error) => this._snackbar.open(error.message, 'Close', { duration: 3000 })
      )
      .add(() => {
        this.form.enable();
        this.loading = false;
      });
  };


  /* Handle form errors in Angular 8 */
  public errorHandling(control: string, error: string) {
    return this.form.controls[control].hasError(error, '');
  }
}
