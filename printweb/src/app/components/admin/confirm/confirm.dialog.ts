import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';

@Component({
    selector: 'confirm',
    templateUrl: './confirm.dialog.html',
    styleUrls: ['../../../app.component.scss']
})
export class ConfirmDialog {


    constructor(public dialogRef: MatDialogRef<ConfirmDialog>,
    @Inject(MAT_DIALOG_DATA) public data: string) { }
}