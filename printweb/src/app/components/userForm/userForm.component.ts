import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IUser } from 'src/app/models/user.model';

@Component({
    selector: 'user-form',
    templateUrl: './userForm.component.html',
    styleUrls: ['../../app.component.scss']
})
export class UserFormComponent {
    @Input() public readonly form: FormGroup;
    @Input() readonly readonly: boolean;
    @Input() readonly disabled: boolean;
    @Input() onError: (control: string, errorName: string) => boolean;

    /* Handle form errors in Angular 8 */
    public errorHandling(control: string, errorName: string) {
        return this.onError(control, errorName);
    }
}