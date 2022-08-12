import {UserItemModel} from './user-item-model';

export class UserInfoModel {
  cif!: string;
  name!: string;
  socialNumber!: string;
  phone!: string;

  setInfo(user: UserItemModel) {
    this.cif = user.cif;
    this.name = user.name;
    this.socialNumber = user.socialNumber;
    this.phone = user.phone;
  }
}
