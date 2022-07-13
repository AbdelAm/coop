import { AccountModel } from "./account-model";

export class TransactionModel {
    id!: number;
    amount!: number;
    dateTransaction!: Date;
    senderAccountId!: number;
    receiverAccountId: number;
    senderAccount!: AccountModel;
    receiverAccount!: AccountModel; 
    status: string;
}
