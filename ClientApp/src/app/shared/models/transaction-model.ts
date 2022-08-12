import {BankAccountModel} from './bankAccount-model';

export class TransactionModel {
  id!: number;
  amount!: number;
  dateTransaction!: Date;
  senderBankAccountId!: number;
  receiverBankAccountId: number;
  senderBankAccount!: BankAccountModel;
  receiverBankAccount!: BankAccountModel;
  status: number;
  motif: string;
}
