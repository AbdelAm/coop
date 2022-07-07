import { AccountModel } from "./accountModel";

export class TransactionModel {
    id!: number;
    amount!: number;
    dateCreated!: Date;
    senderAccountId!: number;
    receiverAccountId: number;
    senderAccount!: AccountModel;
    receiverAccount!: AccountModel;
    isValid: boolean = false;
}