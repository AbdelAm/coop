import {StatusModel} from './status-model';
import {UserModel} from './user-model';

export class RequestModel {
  id!: number;
  type!: string;
  userId!: string;
  user!: UserModel;
  message!: string;
  status!: StatusModel;
  dateRequest!: Date;
}
