export class TokenModel {
    cif: string;
    name: string;
    isAdmin: boolean;
    bankAccount: string;
    token: string;
    validTo: Date;

    constructor(result: TokenModel)
    {
        this.cif = result.cif;
        this.name = result.name;
        this.isAdmin = result.isAdmin;
        this.bankAccount = result.bankAccount;
        this.token = result.token;
        this.validTo = result.validTo;
    }
}