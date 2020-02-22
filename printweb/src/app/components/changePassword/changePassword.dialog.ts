import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
    selector: 'changePassword',
    templateUrl: './changePassword.dialog.html',
    styleUrls: ['../../app.component.scss']
})
export class ChangePasswordDialog {
    form: FormGroup;
    
    constructor(public dialogRef: MatDialogRef<ChangePasswordDialog>) { }

    ngOnInit(): void {
      this.form = new FormGroup({
        newPassword: new FormControl('', [
            Validators.required,
            Validators.minLength(6)
        ])
      });
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    onUpdateClick(): void {
        const newPassword = this.form.controls['newPassword'].value;
        this.dialogRef.close(newPassword);
    }
}