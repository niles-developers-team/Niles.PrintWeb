import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
    selector: 'user-account-info',
    templateUrl: './userAccountInfo.component.html',
    styleUrls: ['../../app.component.scss']
})
export class UserAccountInfoComponent {
    @Input() public readonly form: FormGroup;
    @Input() readonly readonly: boolean = false;
    @Input() readonly disabled: boolean = false;
    @Input() onError: (control: string, errorName: string) => boolean;

    public get isNewUser(): boolean { return Boolean(this.form.controls['password']); }

    /* Handle form errors in Angular 8 */
    public errorHandling(control: string, errorName: string) {
        return this.onError(control, errorName);
    }
}