export class TokenModel {
    userId!: string;
    username!: string;
    roles!: Array<string>;
    token!: string;
    validTo!: Date;
}