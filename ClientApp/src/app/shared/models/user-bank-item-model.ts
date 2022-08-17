import { BankAccountModel } from "./bankAccount-model";
import { UserItemModel } from "./user-item-model";

export class UserBankItemModel extends UserItemModel {
  bankAccount!: BankAccountModel;
}
