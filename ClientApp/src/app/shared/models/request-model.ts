import { Time } from '@angular/common';
import { StatusModel } from './status-model'
import { UserModel } from './user-model';

export class RequestModel {
    id!: number;
    type!: string;
    userId!: string;
    message!: string;
    status!: StatusModel;
    dateRequest!: Date;
}
