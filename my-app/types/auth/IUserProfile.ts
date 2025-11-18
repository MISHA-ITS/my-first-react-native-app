export interface IUserProfile {
    firstName: string;
    lastName: string;
    image: string | null;
    email: string;
    dateRegister: string;
    roles: string[];
}