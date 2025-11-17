import { SafeAreaView, ScrollView, View, Text, Image, Dimensions } from "react-native";
import CustomButton from "@/components/custom-button";
import images from "@/constants/images";
import { useRouter } from "expo-router";
import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "@/store";
import { logout, loadUserFromStorage } from "@/store/authSlice";
import { BASE_URL } from "@/constants/Urls";

const Profile = () => {
    const { user } = useAppSelector((state) => state.auth);
    const dispatch = useAppDispatch();
    const router = useRouter();

    useEffect(() => {
        // Якщо користувача немає в стані — повертаємо на логін
        if (!user) {
            router.replace("/(auth)/sign-in");
        }
    }, [user]);

    const handleLogout = () => {
        dispatch(logout());
        router.replace("/(auth)/sign-in");
    };

    if (!user) {
        return (
            <SafeAreaView className="bg-primary h-full flex justify-center items-center">
                <Text className="text-blue-950 text-lg">Завантаження профілю...</Text>
            </SafeAreaView>
        );
    }

    const imageUri = user.image ? `${BASE_URL}/Images/${user.image}` : null;

    console.log("imageUri:", imageUri);

    return (
        <SafeAreaView className="bg-primary h-full">
            <ScrollView>
                <View
                    className="w-full items-center px-4 py-10"
                    style={{
                        minHeight: Dimensions.get("window").height - 100,
                    }}
                >
                    <View className="w-[200px] h-[200px] rounded-full overflow-hidden mt-10 mb-6 border-4 border-slate-400 bg-white">
                        <Image
                            source={imageUri ? { uri: imageUri } : images.noimage}
                            className="w-full h-full"
                            resizeMode="cover"
                        />
                    </View>

                    <Text className="text-2xl font-bold text-black mb-6">{user.firstName} {user.lastName}</Text>

                    <View className="bg-slate-800 rounded-2xl p-5 w-full">
                        <Text className="text-white text-lg font-semibold mb-3">Основна інформація</Text>
                        <View className="mb-3">
                            <Text className="text-white text-base">Електронна пошта: {user.email}</Text>
                            <Text className="text-gray-400 text-sm">Ролі: {user.roles?.join(", ")}</Text>
                        </View>
                    </View>

                    <View className="w-full mt-8">
                        <CustomButton
                            title="Вийти"
                            handlePress={handleLogout}
                            containerStyles="w-full bg-red-700 rounded-xl"
                        />
                    </View>
                </View>
            </ScrollView>
        </SafeAreaView>
    );
};

export default Profile;
