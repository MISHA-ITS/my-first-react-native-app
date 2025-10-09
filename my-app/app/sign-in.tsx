import { View, Text, TextInput, Pressable, Alert } from "react-native";
import { useState } from "react";
import { useRouter } from "expo-router";

const SignInScreen = () => {
    const router = useRouter();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    const handleSignIn = async () => {
        if (!email || !password) {
            Alert.alert("Помилка", "Будь ласка, заповніть усі поля.");
            return;
        }

        if (!emailRegex.test(email)) {
            Alert.alert("Помилка", "Невірний формат email.");
            return;
        }

        if (password.length < 6) {
            Alert.alert("Помилка", "Пароль має містити щонайменше 6 символів.");
            return;
        }

        try {
            setIsSubmitting(true);

            // 🔹 Імітація запиту до сервера
            await new Promise((resolve) => setTimeout(resolve, 1000));

            Alert.alert("Успішний вхід", `Вітаємо, ${email}!`);
            console.log(`User email: ${email}`)
            console.log(`User password: ${password}`)
            router.replace("/"); // Повернення на головну після успішного входу
        } catch (error: any) {
            Alert.alert("Помилка входу", error.message || "Щось пішло не так");
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <View className="flex-1 justify-center bg-gray-50 dark:bg-gray-900 px-6">
            <Text className="text-3xl font-bold text-center text-blue-600 mb-8">
                Увійти до акаунта
            </Text>

            <View className="mb-4">
                <Text className="text-gray-700 dark:text-gray-200 mb-2">Email</Text>
                <TextInput
                    className="border border-gray-300 dark:border-gray-700 rounded-xl px-4 py-3 bg-white dark:bg-gray-800 text-gray-900 dark:text-gray-100"
                    placeholder="Введіть email"
                    value={email}
                    onChangeText={setEmail}
                    autoCapitalize="none"
                    keyboardType="email-address"
                />
            </View>

            <View className="mb-6">
                <Text className="text-gray-700 dark:text-gray-200 mb-2">Пароль</Text>
                <TextInput
                    className="border border-gray-300 dark:border-gray-700 rounded-xl px-4 py-3 bg-white dark:bg-gray-800 text-gray-900 dark:text-gray-100"
                    placeholder="Введіть пароль"
                    value={password}
                    onChangeText={setPassword}
                    secureTextEntry
                />
            </View>

            <Pressable
                onPress={handleSignIn}
                disabled={isSubmitting}
                className={`${
                    isSubmitting ? "bg-blue-400" : "bg-blue-600 active:bg-blue-700"
                } py-3 rounded-2xl mb-4`}
            >
                <Text className="text-white text-center text-base font-semibold">
                    {isSubmitting ? "Вхід..." : "Увійти"}
                </Text>
            </Pressable>

            <Pressable onPress={() => router.push("/sign-up")}>
                <Text className="text-center text-gray-600 dark:text-gray-300">
                    Немає акаунта?{" "}
                    <Text className="text-blue-600 font-semibold">Зареєструватися</Text>
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
        </View>
    );
};

export default SignInScreen;
