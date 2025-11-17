import {deleteSecureStore, getSecureStore, saveSecureStore} from "@/utils/secureStore";
import {IUser} from "@/types/auth/IUser";
import {jwtDecode} from "jwt-decode";
import {IAuthState} from "@/types/auth/IAuthState";
import {createAsyncThunk, createSlice, PayloadAction} from "@reduxjs/toolkit";

export const getUserFromToken = (token: string | null): IUser | null  => {
    try {
        if (!token) return null;
        const decodedToken = jwtDecode<IUser>(token);
        let roles: string[] = [];

        if(typeof decodedToken.roles === "string") {
            roles = [decodedToken.roles];
        } else if(Array.isArray(decodedToken.roles)){
            roles = decodedToken.roles;
        }

        return {
            email: decodedToken.email,
            firstName: decodedToken.firstName,
            lastName: decodedToken.lastName,
            image: decodedToken.image,
            roles: roles
        };
    }
    catch (e) {
        console.log("Invalid token", e);

        return null;
    }
}

// ✅ Завантажує користувача при запуску програми
export const loadUserFromStorage = createAsyncThunk(
    "auth/loadUserFromStorage",
    async () => {
        const token = await getSecureStore("token");
        if (!token) return null;
        return getUserFromToken(token);
    }
);

const token = getSecureStore('token');

const initUser = token ? getUserFromToken(token) : null;

//Дані які будуть зберігатися про авторизованого користувача
const initState: IAuthState = {
    user: initUser,
}

const authSlice = createSlice ({
    name: 'auth',
    initialState: initState,
    reducers: {
        login: (state, action: PayloadAction<string>) => {
            const token = action.payload;
            const user = getUserFromToken(token);
            if(user) {
                saveSecureStore("token", token); //це щоб при перезапуску додатку користувач не вилітав
                state.user = user;
            }
        },
        logout: (state) => {
            deleteSecureStore("token");
            state.user = null;
        },
    }
});

export const {login, logout} = authSlice.actions;

export default authSlice.reducer;