import { StatusModel } from './status-model'

export class RequestModel {
    id!: number;
    type!: string;
    message!: string;
    status!: StatusModel;
}
