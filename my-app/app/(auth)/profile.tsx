import { SafeAreaView, ScrollView, View, Text, Image, Dimensions } from "react-native";
import CustomButton from "@/components/custom-button";
import images from "@/constants/images";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { jwtDecode } from "jwt-decode";
import { useRouter } from "expo-router";
import { useEffect, useState } from "react";
import {IUserFromToken} from "@/types/account/IUserFromToken";
import {BASE_URL} from "@/constants/Urls";

const Profile = () => {

    const [user, setUser] = useState<IUserFromToken | null>(null);
    const router = useRouter();

    useEffect(() => {
        const loadUser = async () => {
            try {
                const token = await AsyncStorage.getItem("token");
                if (!token) {
                    router.replace("/(auth)/sign-in");
                    return;
                }

                const decoded = jwtDecode<IUserFromToken>(token);
                setUser(decoded);
            } catch (error) {
                console.error("Error loading user:", error);
            }
        };

        loadUser();
    }, []);

    const logout = async () => {
        await AsyncStorage.removeItem("token");
        router.replace("/(auth)/sign-in");
    };

    if (!user) {
        return (
            <SafeAreaView className="bg-primary h-full flex justify-center items-center">
                <Text className="text-white text-lg">Завантаження профілю...</Text>
            </SafeAreaView>
        );
    }

    return (
        <SafeAreaView className="bg-primary h-full">
            <ScrollView>
                <View
                    className="w-full items-center px-4 py-10"
                    style={{
                        minHeight: Dimensions.get("window").height - 100,
                    }}
                >
                    {/* Фото профілю */}
                    <View className="w-[200px] h-[200px] rounded-full overflow-hidden mb-6 border-4 border-slate-400">
                        <Image
                            source={
                                user.image
                                    ? { uri: `${BASE_URL}/images/${user.image}` }
                                    : images.noimage
                            }
                            className="w-full h-full object-cover"
                            resizeMode="cover"
                        />
                    </View>

                    {/* Ім'я користувача */}
                    <Text className="text-2xl font-bold text-white mb-2">
                        {user.firstName} {user.lastName}
                    </Text>

                    {/* Email */}
                    <Text className="text-base text-gray-300 mb-6">{user.email}</Text>

                    {/* Інформаційні поля */}
                    <View className="bg-slate-800 rounded-2xl p-5 w-full">
                        <Text className="text-white text-lg font-semibold mb-3">
                            Основна інформація
                        </Text>

                        <View className="mb-3">
                            <Text className="text-gray-400 text-sm">Ім'я</Text>
                            <Text className="text-white text-base">{user.firstName}</Text>
                        </View>

                        <View className="mb-3">
                            <Text className="text-gray-400 text-sm">Прізвище</Text>
                            <Text className="text-white text-base">{user.lastName}</Text>
                        </View>

                        <View className="mb-3">
                            <Text className="text-gray-400 text-sm">Електронна пошта</Text>
                            <Text className="text-white text-base">{user.email}</Text>
                        </View>
                    </View>

                    {/* Кнопки */}
                    <View className="w-full mt-8">
                        <CustomButton
                            title="Редагувати профіль"
                            handlePress={() => console.log("Edit profile")}
                            containerStyles="w-full bg-slate-500 rounded-xl mb-3"
                        />
                        <CustomButton
                            title="Вийти"
                            handlePress={logout}
                            containerStyles="w-full bg-red-700 rounded-xl"
                        />
                    </View>
                </View>
            </ScrollView>
        </SafeAreaView>
    );
};

export default Profile;
