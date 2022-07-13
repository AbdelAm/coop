
export class RegisterModel {
    name!: string;
    socialNumber!: number;
    cifNumber!: number;
    email!: string;
    password!: string;
    dateCreated!: Date;
    isAdmin!: boolean;
    isConfirmed: boolean = false;
    isValid: boolean = false;
}