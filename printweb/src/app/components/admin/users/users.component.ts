import { Component } from '@angular/core';
import { IUser } from 'src/app/models/user.model';


const ELEMENT_DATA: IUser[] = [
    { username: 'A', email: '', firstName: '', lastName: '', password: null},
    { username: 'B', email: '', firstName: 'He', lastName: '', password: null},
    { username: 'C', email: '', firstName: 'Li', lastName: '', password: null},
    { username: 'D', email: '', firstName: 'Be', lastName: '', password: null},
    { username: 'E', email: '', firstName: 'B', lastName: '', password: null},
    { username: 'F', email: '', firstName: 'C', lastName: '', password: null},
    { username: 'G', email: '', firstName: 'N', lastName: '', password: null},
    { username: 'H', email: '', firstName: 'O', lastName: '', password: null},
    { username: 'I', email: '', firstName: 'F', lastName: '', password: null},
    { username: 'J', email: '', firstName: 'Ne', lastName: '', password: null},
];

/**
 * @title Basic use of `<table mat-table>`
 */
@Component({
    selector: 'users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.css', '../../../app.component.scss']
})
export class UsersComponent {
    displayedColumns: string[] = ['username', 'edit', 'email', 'firstName', 'lastName'];
    users = ELEMENT_DATA;
}