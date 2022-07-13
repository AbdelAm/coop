import { TransactionModel } from "./transaction-model";
import { UserModel } from "./user-model";


export class AccountModel {
    id!: number;
    accountNumber!: string;
    balance!: number;
    dateCreated!: Date;
    ownerId!: string;
    owner!: UserModel;
    transactions!: Array<TransactionModel>;
}
