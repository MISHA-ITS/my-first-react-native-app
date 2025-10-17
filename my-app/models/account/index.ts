export interface IUserCreate {
    email: string;
    firstName: string;
    lastName: string;
    password: string;
    imageUrl: string | undefined;
}

export interface IUserLogin {
    email: string;
    password: string;
}