import {createApi} from "@reduxjs/toolkit/query/react";
import {createBaseQuery} from "@/utils/createBaseQuery";
import {ILoginResponse} from "@/types/account/ILoginResponse";
import {IRegisterRequest} from "@/types/account/IRegisterRequest";
import {ILoginRequest} from "@/types/account/ILoginRequest";
import {serialize} from "object-to-formdata";

export const apiAccount = createApi({
    reducerPath: "apiAccount", // ключ у Redux store
    baseQuery: createBaseQuery("account/"), // базовий URL
    tagTypes: ["Account"], // тег для оновлення кешу
    // список запитів/мутацій
    endpoints: (builder) => ({
        //mutation - зміна,
        register: builder.mutation<ILoginResponse, IRegisterRequest>({
            query: (data)=> {
                const formData = serialize(data)
                return{
                    url: "register",
                    method: "POST",
                    body: formData
                }
            }
        }),
        login: builder.mutation<ILoginResponse, ILoginRequest>({
            query: (data)=> {
                return{
                    url: "login",
                    method: "POST",
                    body: data
                }
            }
        }),
        getProfile: builder.query<any, void>({
            query: () => ({
                url: "profile",
                method: "GET",
            }),
            providesTags: ["Account"],
        }),
    }),
});

export const {
    useRegisterMutation,
    useLoginMutation,
    useGetProfileQuery
} = apiAccount;