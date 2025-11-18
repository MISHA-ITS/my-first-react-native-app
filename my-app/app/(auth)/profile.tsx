import { SafeAreaView, ScrollView, View, Text, Image, Dimensions } from "react-native";
import CustomButton from "@/components/custom-button";
import images from "@/constants/images";
import { useRouter } from "expo-router";
import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "@/store";
import { logout } from "@/store/authSlice";
import { BASE_URL } from "@/constants/Urls";
import { IUserProfile } from "@/types/auth/IUserProfile";
import { getSecureStore } from "@/utils/secureStore";

const Profile = () => {
    const { user } = useAppSelector((state) => state.auth);
    const dispatch = useAppDispatch();
    const router = useRouter();
    const [allUsers, setAllUsers] = useState<IUserProfile[]>([]);
    const [loadingUsers, setLoadingUsers] = useState(false);

    useEffect(() => {
        if (!user) {
            router.replace("/(auth)/sign-in");
        }
    }, [user]);

    useEffect(() => {
        const fetchUsers = async () => {
            if (!user || !user.roles.includes("Admin")) return;

            try {
                setLoadingUsers(true);
                const token = await getSecureStore("token");
                if (!token) return;

                const response = await fetch(`${BASE_URL}/api/Account/usersList`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                if (!response.ok) {
                    console.log("Failed to fetch users:", response.status);
                    setAllUsers([]);
                    return;
                }

                const data: IUserProfile[] = await response.json();
                console.log("Fetched users:", data);
                setAllUsers(data);
            } catch (error) {
                console.log("Error fetching users:", error);
            } finally {
                setLoadingUsers(false);
            }
        };

        fetchUsers();
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

    const imageUri = user.image ? `${BASE_URL}/images/200_${user.image}` : null;

    return (
        <SafeAreaView className="bg-primary h-full">
            <ScrollView>
                <View
                    className="w-full items-center px-4 py-10"
                    style={{ minHeight: Dimensions.get("window").height - 100 }}
                >
                    {/* Профіль користувача */}
                    <View className="w-[200px] h-[200px] rounded-full overflow-hidden mt-10 mb-6 border-4 border-slate-400 bg-white">
                        <Image
                            source={imageUri ? { uri: imageUri } : images.noimage}
                            className="w-full h-full"
                            resizeMode="cover"
                        />
                    </View>

                    <Text className="text-2xl font-bold text-black mb-6">
                        {user.firstName} {user.lastName}
                    </Text>

                    <View className="bg-slate-800 rounded-2xl p-5 w-full">
                        <Text className="text-white text-lg font-semibold mb-3">Основна інформація</Text>
                        <View className="mb-3">
                            <Text className="text-white text-base">Електронна пошта: {user.email}</Text>
                            <Text className="text-gray-400 text-sm">👤 Ролі: {user.roles?.join(", ")}</Text>
                        </View>
                    </View>

                    <View className="w-full mt-8">
                        <CustomButton
                            title="Вийти"
                            handlePress={handleLogout}
                            containerStyles="w-full bg-red-700 rounded-xl"
                        />
                    </View>

                    {/* Горизонтальна риска */}
                    {user.roles.includes("Admin") && <View className="border-t border-gray-400 w-full my-6" />}

                    {/* Список користувачів для адміна */}
                    {user.roles.includes("Admin") && (
                        <View className="w-full">
                            <Text className="text-black text-xl font-bold mb-4">Список користувачів</Text>

                            {loadingUsers && <Text>Завантаження користувачів...</Text>}

                            {!loadingUsers && allUsers.length === 0 && <Text>Користувачів не знайдено</Text>}

                            {!loadingUsers &&
                                allUsers.map((u) => {
                                    const userImageUri = u.image ? `${BASE_URL}/images/50_${u.image}` : null;
                                    return (
                                        <View
                                            key={u.email}
                                            className="flex-row items-center bg-slate-200 rounded-xl p-3 mb-3"
                                        >
                                            <Image
                                                source={userImageUri ? { uri: userImageUri } : images.noimage}
                                                className="w-12 h-12 rounded-full mr-3"
                                                resizeMode="cover"
                                            />
                                            <View>
                                                <Text className="text-black font-semibold">
                                                    {u.firstName} {u.lastName}
                                                </Text>
                                                <Text className="text-gray-700 text-sm">{u.email}</Text>
                                                <Text className="text-gray-500 text-xs">
                                                    Ролі: {u.roles?.join(", ")}
                                                </Text>
                                            </View>
                                        </View>
                                    );
                                })}
                        </View>
                    )}
                </View>
            </ScrollView>
        </SafeAreaView>
    );
};

export default Profile;
