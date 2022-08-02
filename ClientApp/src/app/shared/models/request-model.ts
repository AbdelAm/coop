import { StatusModel } from './status-model'
import { UserModel } from './user-model';

export class RequestModel {
    id!: number;
    type!: string;
    userId!: number;
    message!: string;
    status!: StatusModel;
}
