import { BankAccountModel } from "./bankAccount-model";


export class UserItemModel {
    cif!: string;
    name!: string;
    email!: string;
    socialNumber!: number;
    isConfirmed: boolean = false;
    status!: string;
}