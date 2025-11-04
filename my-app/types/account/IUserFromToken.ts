export interface IUserFromToken {
    email: string;
    firstName: string;
    lastName: string;
    image?: string;
    exp?: number;
    iat?: number;
}