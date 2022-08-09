export class TokenModel {
    cif: string;
    name: string;
    isAdmin: boolean;
    token: string;
    validTo: Date;

    constructor(result: TokenModel)
    {
        this.cif = result.cif;
        this.name = result.name;
        this.isAdmin = result.isAdmin;
        this.token = result.token;
        this.validTo = result.validTo;
    }
}