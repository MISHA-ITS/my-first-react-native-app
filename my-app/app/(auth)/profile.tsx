import { SafeAreaView, ScrollView, View, Text, Image, Dimensions } from "react-native";
import CustomButton from "@/components/custom-button";
import images from "@/constants/images";

const mockUser = {
    firstName: "Михайло",
    lastName: "Тесленко",
    email: "misha@example.com",
    imageUrl: "https://thispersondoesnotexist.com", // випадкове зображення користувача
};

const Profile = () => {
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
                            source={mockUser.imageUrl ? { uri: mockUser.imageUrl } : images.noimage}
                            className="w-full h-full object-cover"
                        />
                    </View>

                    {/* Ім'я користувача */}
                    <Text className="text-2xl font-bold text-white mb-2">
                        {mockUser.firstName} {mockUser.lastName}
                    </Text>

                    {/* Email */}
                    <Text className="text-base text-gray-300 mb-6">{mockUser.email}</Text>

                    {/* Інформаційні поля */}
                    <View className="bg-slate-800 rounded-2xl p-5 w-full">
                        <Text className="text-white text-lg font-semibold mb-3">
                            Основна інформація
                        </Text>

                        <View className="mb-3">
                            <Text className="text-gray-400 text-sm">Ім'я</Text>
                            <Text className="text-white text-base">{mockUser.firstName}</Text>
                        </View>

                        <View className="mb-3">
                            <Text className="text-gray-400 text-sm">Прізвище</Text>
                            <Text className="text-white text-base">{mockUser.lastName}</Text>
                        </View>

                        <View className="mb-3">
                            <Text className="text-gray-400 text-sm">Електронна пошта</Text>
                            <Text className="text-white text-base">{mockUser.email}</Text>
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
                            handlePress={() => console.log("Logout")}
                            containerStyles="w-full bg-red-700 rounded-xl"
                        />
                    </View>
                </View>
            </ScrollView>
        </SafeAreaView>
    );
};

export default Profile;
