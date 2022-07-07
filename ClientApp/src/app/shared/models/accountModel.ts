import { TransactionModel } from "./transactionModel";
import { UserModel } from "./userModel";


export class AccountModel {
    id!: number;
    accountNumber!: string;
    balance!: number;
    dateCreated!: Date;
    ownerId!: string;
    owner!: UserModel;
    transactions!: Array<TransactionModel>;
    isValid: boolean = false;
}