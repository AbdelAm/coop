import { AccountModel } from "./accountModel";


export class UserModel {
    id!: string;
    nomComplet!: string;
    socialNumber!: number;
    email!: string;
    password!: string;
    accounts!: Array<AccountModel>;
    dateCreated!: Date;
    isConfirmed: boolean = false;
    isValid: boolean = false;
}