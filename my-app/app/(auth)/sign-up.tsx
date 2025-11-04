import React, {useState} from "react";
import {
    View,
    Text,
    Pressable,
    ScrollView,
    SafeAreaView,
    TouchableOpacity,
    Image,
} from "react-native";
import { useRouter } from "expo-router";
import {IUserCreate} from "@/models/account";
import images from "@/constants/images";
import FormField from "@/components/form-fields";
import CustomButton from "@/components/custom-button";
import {pickImage} from "@/utils/pickimage";
import { showMessage } from "react-native-flash-message";
import {pickUserFile} from "@/utils/pickUserFile";
import {getFileFromUriAsync} from "@/utils/getFileFromUriAsync";
import {IRegisterRequest} from "@/types/account/IRegisterRequest";
import {useRegisterMutation} from "@/services/apiAccount";

const userInitState : IUserCreate = {
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    imageUrl: "",
};

const SignUp = () => {
    //Метод register - для реєстрації
    //boolean isLoading - Для відслідковування запиту
    //error - об'єкт, який містить помилки
    const [register, {isLoading, error: registerError}] = useRegisterMutation();
    console.log("Register", isLoading, registerError);
    //Зберігає дані користувача
    const [user, setUser] = useState<IUserCreate>(userInitState);
    //Зберігає помилки
    const [errors, setErrors] = useState<string[]>([]);
    const [confirmPassword, setConfirmPassword] = useState<string>('');

    const router = useRouter();

    const validationChange = (isValid: boolean, fieldKey: string) => {
        if (isValid && errors.includes(fieldKey)) {
            setErrors(errors.filter(x => x !== fieldKey))
        } else if (!isValid && !errors.includes(fieldKey)) {
            setErrors(state => [...state, fieldKey])
        }
    };

    const submit = async () => {
        if (errors.length !== 0) {
            console.error(errors);
            showMessage({
                message: "Правильно заповніть всі поля",
                type: "info",
            });
            return;
        }
        if(user.imageUrl) {
            const fileImage =
                await getFileFromUriAsync(user.imageUrl);
            console.log("Submit form-- file",  fileImage);
            try {
                const model : IRegisterRequest = {...user, imageFile: fileImage};
                await register(model);
                router.replace("/(auth)");

                // const url = "https://spr311.itstep.click/api/account/register";
                // console.log("Submit form-- model",  model);
                // // const url = "http://10.0.2.2:5165/api/account/register";
                // const formData = serialize(model)
                // console.log("Submit form-- url",  url);
                // await axios.post(url, formData,
                //     {
                //         headers: {
                //             'Content-Type': 'multipart/form-data'
                //         }
                //     });
            }
            catch(ex) {
                console.log("Submit form-- error", ex);
            }
        }
        console.log("Submit form", user);
    }

    const handlePickUserFile = async () => {
        const fileData = await pickUserFile();
        if (fileData) {
            setUser({
                ...user,
                firstName: fileData.firstName || "",
                lastName: fileData.lastName || "",
                email: fileData.email || "",
                password: fileData.password || "",
                imageUrl: fileData.imageUrl || "",
            });
            setConfirmPassword(fileData.password || "");
        }
    };

    return (
        <SafeAreaView className="bg-primary h-full">
            <ScrollView
                contentContainerStyle={{ flexGrow: 1 }}
                keyboardShouldPersistTaps="handled"
                className="bg-gray-50 dark:bg-gray-900 px-6 py-10"
            >
                <View className="w-full gap-2 flex items-center h-full px-4 my-6" style={{ minHeight: 400 }}>
                    <Text className="text-3xl font-bold text-center text-blue-600 mb-8">
                        Створити акаунт
                    </Text>

                    <TouchableOpacity
                        className=' mb-5 self-center mx-2 w-[200px] h-[200px] rounded-full overflow-hidden '
                        onPress={async () => setUser({ ...user, imageUrl: await pickImage() })}
                    >
                        <Image source={user.imageUrl ? { uri: user.imageUrl } : images.noimage} className=" object-cover w-full h-full" />
                    </TouchableOpacity>

                    <FormField
                        placeholder="Вкажіть прізвище"
                        title="Прізвище"
                        value={user.lastName}
                        handleChangeText={(e) => setUser({ ...user, lastName: e })}
                        onValidationChange={validationChange}
                        rules={[
                            {
                                rule: 'required',
                                message: "Прізвище є обов'язковим"
                            },
                            {
                                rule: 'min',
                                value: 2,
                                message: 'Прізвище має містити мінімум 2 символи'
                            },
                            {
                                rule: 'max',
                                value: 40,
                                message: 'Прізвище має містити максимум 40 символів'
                            }
                        ]}
                    />

                    <FormField
                        placeholder="Вкажіть ваше ім'я"
                        title="Ім'я"
                        value={user.firstName}
                        handleChangeText={(e) => setUser({ ...user, firstName: e })}
                        onValidationChange={validationChange}
                        rules={[
                            {
                                rule: 'required',
                                message: 'Ім\'я є обов\'язковим'
                            },
                            {
                                rule: 'min',
                                value: 2,
                                message: 'Ім\'я має містити мінімум 2 символи '
                            },
                            {
                                rule: 'max',
                                value: 40,
                                message: 'Ім\'я має містити максимум 40 символів '
                            }
                        ]}
                    />

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

                    <FormField
                        placeholder="Повторити пароль"
                        title="Повторити пароль"
                        value={confirmPassword}
                        handleChangeText={(e) => setConfirmPassword(e)}
                        onValidationChange={validationChange}
                        rules={[
                            {
                                rule: 'required',
                                message: 'Вкажіть пароль'
                            },
                            {
                                rule: 'equals',
                                value: user.password,
                                message: 'Паролі не співпадають'
                            },
                        ]}
                    />

                    {/* Завантажити дані з файлу */}
                    <Pressable onPress={handlePickUserFile}>
                        <Text className="text-blue-600 font-semibold mt-5">Завантажити дані з файлу</Text>
                    </Pressable>

                    <CustomButton
                        title="Зареєструватися"
                        handlePress={submit}
                        containerStyles="mt-4 w-full bg-slate-500 rounded-xl"
                    />

                    {/* Перехід на Sign In */}
                    <Pressable onPress={() => router.push("/sign-in")}>
                        <Text className="text-center text-gray-600 dark:text-gray-300">
                            Уже маєте акаунт?{" "}
                            <Text className="text-blue-600 font-semibold">Увійти</Text>
                        </Text>
                    </Pressable>

                    <CustomButton
                        title="Повернутися на головну"
                        handlePress={() => router.replace("/")}
                        containerStyles="mt-4 w-full border-2 border-black-600 bg-white-500 rounded-xl"
                    />
                </View>
            </ScrollView>
        </SafeAreaView>
    );
};

export default SignUp;
