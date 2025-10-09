import React from "react";
import {
    View,
    Text,
    TextInput,
    Pressable,
    ScrollView,
    Alert,
} from "react-native";
import { useRouter } from "expo-router";
import { Formik } from "formik";
import * as Yup from "yup";

// 🔹 Схема валідації через Yup
const SignUpSchema = Yup.object().shape({
    firstName: Yup.string()
        .min(2, "Мінімум 2 символи")
        .required("Введіть ім'я"),
    lastName: Yup.string()
        .min(2, "Мінімум 2 символи")
        .required("Введіть прізвище"),
    email: Yup.string()
        .email("Невірний формат email")
        .required("Введіть email"),
    password: Yup.string()
        .min(6, "Мінімум 6 символів")
        .required("Введіть пароль"),
    repeatPassword: Yup.string()
        .oneOf([Yup.ref("password"), undefined], "Паролі не співпадають")
        .required("Підтвердіть пароль"),
});

const SignUpScreen = () => {
    const router = useRouter();

    const handleSignUp = async (values: any, { setSubmitting, resetForm }: any) => {
        try {
            // 🔹 Імітація запиту (для прикладу)
            await new Promise((resolve) => setTimeout(resolve, 1000));

            Alert.alert("Реєстрація успішна", "Тепер ви можете увійти у свій акаунт!");
            const { firstName, lastName, email, password } = values;
            console.log(`users first name: ${firstName}`)
            console.log(`users last name: ${lastName}`)
            console.log(`users email: ${email}`)
            console.log(`users password: ${password}`)
            resetForm();
            router.replace("/sign-in");
        } catch (error: any) {
            Alert.alert("Помилка", error.message || "Щось пішло не так");
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <ScrollView
            contentContainerStyle={{ flexGrow: 1 }}
            keyboardShouldPersistTaps="handled"
            className="bg-gray-50 dark:bg-gray-900 px-6 py-10"
        >
            <View className="flex-1 justify-center">
                <Text className="text-3xl font-bold text-center text-blue-600 mb-8">
                    Створити акаунт
                </Text>

                <Formik
                    initialValues={{
                        firstName: "",
                        lastName: "",
                        email: "",
                        password: "",
                        repeatPassword: "",
                    }}
                    validationSchema={SignUpSchema}
                    onSubmit={handleSignUp}
                >
                    {({
                          handleChange,
                          handleBlur,
                          handleSubmit,
                          values,
                          errors,
                          touched,
                          isSubmitting,
                      }) => (
                        <>
                            {/* Ім'я */}
                            <View className="mb-4">
                                <Text className="text-gray-700 dark:text-gray-200 mb-2">
                                    Ім&apos;я
                                </Text>
                                <TextInput
                                    className={`border rounded-xl px-4 py-3 bg-white dark:bg-gray-800 ${
                                        touched.firstName && errors.firstName
                                            ? "border-red-500"
                                            : "border-gray-300 dark:border-gray-700"
                                    }`}
                                    placeholder="Введіть ім'я"
                                    value={values.firstName}
                                    onChangeText={handleChange("firstName")}
                                    onBlur={handleBlur("firstName")}
                                />
                                {touched.firstName && errors.firstName && (
                                    <Text className="text-red-500 text-sm mt-1">
                                        {errors.firstName}
                                    </Text>
                                )}
                            </View>

                            {/* Прізвище */}
                            <View className="mb-4">
                                <Text className="text-gray-700 dark:text-gray-200 mb-2">
                                    Прізвище
                                </Text>
                                <TextInput
                                    className={`border rounded-xl px-4 py-3 bg-white dark:bg-gray-800 ${
                                        touched.lastName && errors.lastName
                                            ? "border-red-500"
                                            : "border-gray-300 dark:border-gray-700"
                                    }`}
                                    placeholder="Введіть прізвище"
                                    value={values.lastName}
                                    onChangeText={handleChange("lastName")}
                                    onBlur={handleBlur("lastName")}
                                />
                                {touched.lastName && errors.lastName && (
                                    <Text className="text-red-500 text-sm mt-1">
                                        {errors.lastName}
                                    </Text>
                                )}
                            </View>

                            {/* Email */}
                            <View className="mb-4">
                                <Text className="text-gray-700 dark:text-gray-200 mb-2">
                                    Email
                                </Text>
                                <TextInput
                                    className={`border rounded-xl px-4 py-3 bg-white dark:bg-gray-800 ${
                                        touched.email && errors.email
                                            ? "border-red-500"
                                            : "border-gray-300 dark:border-gray-700"
                                    }`}
                                    placeholder="Введіть email"
                                    keyboardType="email-address"
                                    autoCapitalize="none"
                                    value={values.email}
                                    onChangeText={handleChange("email")}
                                    onBlur={handleBlur("email")}
                                />
                                {touched.email && errors.email && (
                                    <Text className="text-red-500 text-sm mt-1">
                                        {errors.email}
                                    </Text>
                                )}
                            </View>

                            {/* Пароль */}
                            <View className="mb-4">
                                <Text className="text-gray-700 dark:text-gray-200 mb-2">
                                    Пароль
                                </Text>
                                <TextInput
                                    className={`border rounded-xl px-4 py-3 bg-white dark:bg-gray-800 ${
                                        touched.password && errors.password
                                            ? "border-red-500"
                                            : "border-gray-300 dark:border-gray-700"
                                    }`}
                                    placeholder="Введіть пароль"
                                    secureTextEntry
                                    value={values.password}
                                    onChangeText={handleChange("password")}
                                    onBlur={handleBlur("password")}
                                />
                                {touched.password && errors.password && (
                                    <Text className="text-red-500 text-sm mt-1">
                                        {errors.password}
                                    </Text>
                                )}
                            </View>

                            {/* Повтор пароля */}
                            <View className="mb-6">
                                <Text className="text-gray-700 dark:text-gray-200 mb-2">
                                    Повторіть пароль
                                </Text>
                                <TextInput
                                    className={`border rounded-xl px-4 py-3 bg-white dark:bg-gray-800 ${
                                        touched.repeatPassword && errors.repeatPassword
                                            ? "border-red-500"
                                            : "border-gray-300 dark:border-gray-700"
                                    }`}
                                    placeholder="Повторіть пароль"
                                    secureTextEntry
                                    value={values.repeatPassword}
                                    onChangeText={handleChange("repeatPassword")}
                                    onBlur={handleBlur("repeatPassword")}
                                />
                                {touched.repeatPassword && errors.repeatPassword && (
                                    <Text className="text-red-500 text-sm mt-1">
                                        {errors.repeatPassword}
                                    </Text>
                                )}
                            </View>

                            {/* Кнопка реєстрації */}
                            <Pressable
                                onPress={handleSubmit as any}
                                disabled={isSubmitting}
                                className={`${
                                    isSubmitting ? "bg-blue-400" : "bg-blue-600 active:bg-blue-700"
                                } py-3 rounded-2xl mb-4`}
                            >
                                <Text className="text-white text-center text-base font-semibold">
                                    {isSubmitting ? "Реєстрація..." : "Зареєструватися"}
                                </Text>
                            </Pressable>

                            {/* Перехід на Sign In */}
                            <Pressable onPress={() => router.push("/sign-in")}>
                                <Text className="text-center text-gray-600 dark:text-gray-300">
                                    Уже маєте акаунт?{" "}
                                    <Text className="text-blue-600 font-semibold">Увійти</Text>
                                </Text>
                            </Pressable>

                            <Pressable
                                onPress={() => router.replace("/")}
                                className="bg-gray-700 px-6 py-3 rounded-2xl active:bg-gray-950 mt-10"
                            >
                                <Text className="text-center text-white text-base font-semibold">
                                    Повернутися на головну
                                </Text>
                            </Pressable>
                        </>
                    )}
                </Formik>
            </View>
        </ScrollView>
    );
};

export default SignUpScreen;
