import {
    View,
    Text,
    Dimensions,
    SafeAreaView,
    KeyboardAvoidingView,
    Platform,
    TouchableOpacity,
    ScrollView,
} from "react-native";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { router } from "expo-router";

export default function HomeScreen() {
    const onPressSignIn = () => {
        router.replace("/sign-in");
        console.log("sign in");
    };

    const onPressSignUp = () => {
        router.replace("/sign-up");
        console.log("sign up");
    };

    return (
        <SafeAreaProvider>
            <SafeAreaView className="flex-1">
                <KeyboardAvoidingView
                    behavior={Platform.OS === "ios" ? "padding" : "height"}
                    className="flex-1"
                >
                    <ScrollView
                        contentContainerStyle={{ flexGrow: 1, paddingHorizontal: 20 }}
                        keyboardShouldPersistTaps="handled"
                    >
                        <View
                            className="w-full flex justify-center items-center my-6"
                            style={{
                                minHeight: Dimensions.get("window").height - 100,
                            }}
                        >
                            <Text className="text-3xl font-bold mb-6 text-black">
                                Додати категорію
                            </Text>

                            <TouchableOpacity
                                className="bg-blue-700 rounded-lg px-5 py-2.5 mb-4 dark:bg-blue-600"
                                style={{ width: 120 }}
                                onPress={onPressSignIn}
                            >
                                <Text className="text-center text-white font-bold text-lg">Вхід</Text>
                            </TouchableOpacity>

                            <TouchableOpacity
                                className="bg-green-700 rounded-lg px-5 py-2.5 dark:bg-green-600"
                                style={{ width: 120 }}
                                onPress={onPressSignUp}
                            >
                                <Text className="text-center text-white font-bold text-lg">Реєстрація</Text>
                            </TouchableOpacity>
                        </View>
                    </ScrollView>
                </KeyboardAvoidingView>
            </SafeAreaView>
        </SafeAreaProvider>
    );
}
