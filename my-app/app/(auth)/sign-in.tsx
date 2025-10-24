import { View, Text, Pressable} from "react-native";
import React, { useState } from "react";
import { useRouter } from "expo-router";
import CustomButton from "@/components/custom-button";
import {IUserLogin} from "@/models/account";
import FormField from "@/components/form-fields";
import axios from "axios";

const userInitState : IUserLogin = {
    email: "",
    password: "",
};

const API_URL = "http://10.0.2.2:5175/api/account";

const SignIn = () => {
    const router = useRouter();

    //Зберігає дані користувача
    const [user, setUser] = useState<IUserLogin>(userInitState);
    //Зберігає помилки
    const [errors, setErrors] = useState<string[]>([]);

    const [loading, setLoading] = useState(false);


    const validationChange = (isValid: boolean, fieldKey: string) => {
        if (isValid && errors.includes(fieldKey)) {
            setErrors(errors.filter(x => x !== fieldKey))
        } else if (!isValid && !errors.includes(fieldKey)) {
            setErrors(state => [...state, fieldKey])
        }
    };

    const submit = async () => {
        console.log("Submit form", user)

        setLoading(true);

        try {
            const response = await axios.post(`${API_URL}/Login`, user, {
                headers: { "Content-Type": "application/json" },
                timeout: 5000, // 5 секунд на очікування відповіді
            });

            const data = response.data;
            console.log("Login success:", data);

            // ✅ Збереження токена (наприклад, якщо використовуєш expo-secure-store)
            // await SecureStore.setItemAsync("token", data.token);

            console.log("Успіх", "Вхід виконано успішно!");
            router.replace("/"); // переходь на головну чи профіль
        } catch (error: any) {
            console.error("Login error:", error);

            if (axios.isAxiosError(error)) {
                const message =
                    error.response?.data?.message ||
                    "Невірний email або пароль, або сервер недоступний";
                console.log("Помилка входу", message);
            } else {
                console.log("Помилка", "Невідома помилка");
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <View className="flex-1 justify-center bg-gray-50 dark:bg-gray-900 px-6">
            <Text className="text-3xl font-bold text-center text-blue-600 mb-8">
                Увійти до акаунта
            </Text>

            <FormField
                placeholder="Enter your email"
                title="Email"
                value={user.email}
                handleChangeText={(e) => setUser({ ...user, email: e })}
                keyboardType="email-address"
                rules={[
                    {
                        rule: 'required',
                        message: 'Email є обов\'язковим'
                    },
                    {
                        rule: 'regexp',
                        value: '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$',
                        message: 'Invalid email address'
                    },
                ]}
                onValidationChange={validationChange}
            />

            <FormField
                placeholder="Вкажіть пароль"
                title="Пароль"
                value={user.password}
                handleChangeText={(e) => setUser({ ...user, password: e })}
                onValidationChange={validationChange}
                rules={[
                    {
                        rule: 'required',
                        message: 'Пароль є обов\'язковим'
                    },
                    {
                        rule: 'regexp',
                        value: '[0-9]',
                        message: 'Пароль має містити цифри'
                    },
                    {
                        rule: 'regexp',
                        value: '[!@#$%^&*(),.?":{}|<>]',
                        message: 'Пароль має містити спец символи '
                    },
                    {
                        rule: 'min',
                        value: 6,
                        message: 'Пароль має містити мін 6 символів'
                    },
                    {
                        rule: 'max',
                        value: 40,
                        message: 'Максимальна довжина паролю 40 символів'
                    }
                ]}
            />

            <CustomButton
                title="Увійти"
                handlePress={submit}
                containerStyles="mt-10 w-full bg-blue-700 rounded-xl" />

            <Pressable onPress={() => router.push("/sign-up")}>
                <Text className="text-center text-gray-600 dark:text-gray-300">
                    Немає акаунта?{" "}
                    <Text className="text-blue-600 font-semibold">Зареєструватися</Text>
                </Text>
            </Pressable>

            <CustomButton
                title="Повернутися на головну"
                handlePress={() => router.replace("/")}
                containerStyles="mt-4 w-full border-2 border-black-600 bg-white-500 rounded-xl"
            />
        </View>
    );
};

export default SignIn;
