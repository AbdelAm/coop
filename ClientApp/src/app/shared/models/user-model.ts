import { BankAccountModel } from "./bankAccount-model";


export class UserModel {
    id!: string;
    nom!: string;
    socialNumber!: number;
    cifNumber!: number;
    email!: string;
    password!: string;
    accounts!: Array<BankAccountModel>;
    dateCreated!: Date;
    isAdmin!: boolean;
    isConfirmed: boolean = false;
    status!: string;
}