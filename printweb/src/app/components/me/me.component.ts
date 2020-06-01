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
  userForm: FormGroup;
  editMode: boolean;
  loading: boolean;

  constructor(private readonly _userService: UserService,
    private _snackbar: MatSnackBar,
    private readonly _router: Router,
    private dialog: MatDialog) {
    this.user = this._userService.currentUserValue;
  }

  ngOnInit(): void {
    this.userForm = new FormGroup({
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

  /* Handle form errors in Angular 8 */
  public errorHandling(control: string, error: string) {
    return this.userForm.controls[control].hasError(error, '');
  }

  public setFormControlValue(control: string, value: any) {
    this.userForm.controls[control].setValue(value);
  }

  /* Handle toggle user account info edit mode */
  public onEditModeEnable() {
    this.editMode = true;
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
    this.userForm.disable();

    const currentUser: IUser = this._userService.currentUserValue;
    const formControls = this.userForm.controls;
    currentUser.email = formControls['email'].value;
    currentUser.firstName = formControls['firstName'].value;
    currentUser.lastName = formControls['lastName'].value;
    currentUser.userName = formControls['userName'].value;

    this._userService.update(currentUser)
      .subscribe(() => true,
        (error) => this._snackbar.open(error.message, 'Close', { duration: 3000 })
      )
      .add(() => {
        this.userForm.enable();
        this.editMode = false;
        this.loading = false;
      });
  }

  public onChangePasswordClick() {
    const dialogRef = this.dialog.open(ChangePasswordDialog);

    dialogRef.afterClosed().subscribe(result => {
      if(!result)
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