import { Component } from '@angular/core';
import { IUser } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { ChangePasswordDialog } from '../changePassword/changePassword.dialog';

@Component({
  selector: 'me',
  templateUrl: './me.component.html',
  styleUrls: ['../../app.component.scss']
})
export class MeComponent {
  user: IUser;
  form: FormGroup;
  editMode: boolean;
  loading: boolean;

  constructor(private readonly _userService: UserService,
    private _snackbar: MatSnackBar,
    private readonly _router: Router,
    private dialog: MatDialog) {
    this.user = this._userService.currentUserValue;
  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    this.form = new FormGroup({
      userName: new FormControl(this.user.userName, [
        Validators.required,
        Validators.minLength(5)
      ]),
      email: new FormControl(this.user.email, [
        Validators.required,
        Validators.email
      ]),
      firstName: new FormControl(this.user.firstName, [
        Validators.required
      ]),
      lastName: new FormControl(this.user.lastName, [
        Validators.required
      ])
    });
  }

  /* Handle toggle user account info edit mode */
  public onEditModeEnable() {
    this.editMode = true;
  }

  public onError(control: string, error: string ) {
    return this.form.controls[control].hasError(error, '');
  }

  public setFormControlValue(control: string, value: any) {
    this.form.controls[control].setValue(value);
  }

  /* Handle cancel user account info editing */
  public onCancel() {
    this.editMode = false;
    this.setFormControlValue('userName', this.user.userName);
    this.setFormControlValue('email', this.user.email);
    this.setFormControlValue('firstName', this.user.firstName);
    this.setFormControlValue('lastName', this.user.lastName);
  }

  public onSignOutClick() {
    this._userService.signout();
    this._router.navigateByUrl('/signin');
  }

  public onUpdateClick() {
    this.loading = true;
    this.form.disable();

    const currentUser: IUser = this._userService.currentUserValue;
    const formControls = this.form.controls;
    currentUser.email = formControls['email'].value;
    currentUser.firstName = formControls['firstName'].value;
    currentUser.lastName = formControls['lastName'].value;
    currentUser.userName = formControls['userName'].value;

    this._userService.update(currentUser)
      .subscribe(() => true,
        (error) => this._snackbar.open(error.message, 'Close', { duration: 3000 })
      )
      .add(() => {
        this.form.enable();
        this.editMode = false;
        this.loading = false;
      });
  }

  public onChangePasswordClick() {
    const dialogRef = this.dialog.open(ChangePasswordDialog);

    dialogRef.afterClosed().subscribe(result => {
      if (!result)
        return;
      const currentUser: IUser = this._userService.currentUserValue;
      currentUser.password = result;

      this._userService.changePassword(currentUser)
        .subscribe(() => this._snackbar.open('You successfully changed your password.', 'Close', { duration: 3000 }),
          (error) => this._snackbar.open(error.message, 'Close', { duration: 3000 })
        )
    });
  }
}