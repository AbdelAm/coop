import { BankAccountModel } from "./bankAccount-model";

export class TransactionModel {
    id!: number;
    amount!: number;
    dateTransaction!: Date;
    senderAccountId!: number;
    receiverAccountId: number;
    senderAccount!: BankAccountModel;
    receiverAccount!: BankAccountModel; 
    status: string;
}
