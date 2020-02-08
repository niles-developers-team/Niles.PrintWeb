import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { ErrorStateMatcher } from '@angular/material/core';
import { FormControl, FormGroupDirective, NgForm, Validators, FormGroup } from '@angular/forms';
import { IUser } from 'src/app/models/user.model';

@Component({
  selector: 'sign-up',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css', '../../app.component.css']
})
export class SignUpComponent {
  userForm: FormGroup;
  user: IUser;

  ngOnInit(): void {
    this.user = {
      firstName: '',
      email: '',
      lastName: '',
      password: '',
      username: ''
    };
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.userForm = new FormGroup({
      username: new FormControl(this.user.username, [
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
  }

  /* Handle form errors in Angular 8 */
  public errorHandling(control: string, error: string) {
    return this.userForm.controls[control].hasError(error, '');
  }
}
