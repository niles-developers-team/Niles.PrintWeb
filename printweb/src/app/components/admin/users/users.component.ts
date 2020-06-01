import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { IUser } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialog } from '../confirm/confirm.dialog';

@Component({
    selector: 'users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss', '../../../app.component.scss']
})
export class UsersComponent {
    displayedColumns: string[] = ['username', 'edit', 'email', 'firstName', 'lastName', 'clear'];
    users: MatTableDataSource<IUser>;
    allOrOnlyConfirmed: boolean;
    loading: boolean;

    constructor(private readonly _userService: UserService,
        private dialog: MatDialog) {
        this.users = new MatTableDataSource<IUser>();
        this.allOrOnlyConfirmed = true;
        this.users.filterPredicate = (users, filter) => {
            const usersStr = users.userName + users.email + users.firstName + users.lastName;
            return usersStr.indexOf(filter) != -1;
        }
    }

    ngOnInit(): void {
        //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
        this.getUsers();
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.users.filter = filterValue.trim().toLowerCase();
    }

    public onDeleteClick(id: number): void {
        const dialogRef = this.dialog.open(ConfirmDialog, {
            data: "Are you sure you want to delete user?"
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result)
                this._userService.delete(id);
        })
    }



    public onAllUsersClick(): void {
        this.allOrOnlyConfirmed = true;
        this.getUsers();
    }

    public onOnlyConfirmedClick(): void {
        this.allOrOnlyConfirmed = false;
        this.getUsers();
    }

    private getUsers(): void {
        this.loading = true;
        this._userService.get({
            onlyConfirmed: !this.allOrOnlyConfirmed
        }).subscribe((users) => {
            this.users.data = users;
            this.loading = false;
        });
    }
}
