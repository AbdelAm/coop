import { StatusModel } from './status-model'

export class RequestModel {
    id!: number;
    type!: string;
    userId!: number;
    message!: string;
    status!: StatusModel;
}
