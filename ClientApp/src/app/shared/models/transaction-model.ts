import { BankAccountModel } from "./bankAccount-model";

export class TransactionModel {
    id!: number;
    amount!: number;
    dateCreated!: Date;
    senderAccountId!: number;
    receiverAccountId: number;
    senderAccount!: BankAccountModel;
    receiverAccount!: BankAccountModel;
    isValid: boolean = false;
}