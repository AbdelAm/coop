import { AccountModel } from "./account-model";


export class UserModel {
    id!: string;
    nom!: string;
    socialNumber!: number;
    cifNumber!: number;
    email!: string;
    password!: string;
    accounts!: Array<AccountModel>;
    dateCreated!: Date;
    isAdmin!: boolean;
    isConfirmed: boolean = false;
    isValid: boolean = false;
}