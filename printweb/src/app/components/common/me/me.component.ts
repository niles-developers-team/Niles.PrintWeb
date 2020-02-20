import { Component } from '@angular/core';
import { IUser } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
    selector: 'me',
    templateUrl: './me.component.html',
    styleUrls: ['../../../app.component.scss']
})
export class MeComponent {
    user: IUser;
    userForm: FormGroup;
    editMode: boolean;

    constructor(private readonly _userService: UserService) {
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
}