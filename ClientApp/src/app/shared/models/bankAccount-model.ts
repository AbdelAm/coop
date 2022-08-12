import {TransactionModel} from './transaction-model';
import {UserModel} from './user-model';

export class BankAccountModel {
  id!: number;
  accountNumber!: string;
  balance!: number;
  dateCreated!: Date;
  userId!: string;
  user!: UserModel;
  transactions!: Array<TransactionModel>;
}
