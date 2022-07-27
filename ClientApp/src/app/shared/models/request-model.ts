import { StatusModel } from './status-model'

export class RequestModel {
    Id!: number;
    Type!: string;
    Message!: string;
    status!: StatusModel;
}
