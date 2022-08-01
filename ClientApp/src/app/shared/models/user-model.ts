import {BankAccountModel} from './bankAccount-model';

export class UserModel {
  cif!: number;
  name!: string;
  email!: string;
  socialNumber!: number;
  bankAccounts: Array<BankAccountModel>;
  isAdmin!: boolean;
  isConfirmed = false;
  status!: string;
}
