import {IImageFile} from "@/types/IImageFile";

export interface IRegisterRequest {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
    imageFile: IImageFile | null;
}