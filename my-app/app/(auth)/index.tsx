import {Dimensions, SafeAreaView, ScrollView, Text, View, Image, TouchableOpacity} from "react-native";
import images from "@/constants/images";
import { useEffect, useState } from "react";
import { BASE_URL } from "@/constants/Urls";
import { getSecureStore } from "@/utils/secureStore";
import { INoteCategory } from "@/types/note/INoteCategory";
import CustomButton from "@/components/custom-button";
import {router} from "expo-router";

const Index = () => {
    const [categories, setCategories] = useState<INoteCategory[]>([]);
    const [loading, setLoading] = useState(false);

    // Завантаження категорій
    useEffect(() => {
        const fetchCategories = async () => {
            try {
                setLoading(true);

                const token = await getSecureStore("token");
                if (!token) return;

                const response = await fetch(`${BASE_URL}/api/NoteCategories/List`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                if (!response.ok) {
                    console.log("Failed to fetch categories", response.status);
                    return;
                }

                const data: INoteCategory[] = await response.json();
                console.log("Categories:", data);

                setCategories(data);
            } catch (error) {
                console.log("Error fetching categories:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchCategories();
    }, []);

    return (
        <>
            <SafeAreaView className="bg-primary h-full">
                <ScrollView>
                    <View className="w-full gap-2 flex justify-center items-center h-full px-4 my-6"
                          style={{
                              minHeight: Dimensions.get('window').height - 100,
                          }}>

                        {/* Заголовок */}
                        <View className="w-[200px] h-[200px] overflow-hidden mt-10 mb-6">
                            <Image
                                source={{ uri: "https://static.tildacdn.pro/tild3236-3261-4261-b930-653466663134/_.png" }}
                                className="w-full h-full"
                                resizeMode="cover"
                            />
                        </View>

                        <Text className="mt-2 text-2xl font-pbold font-bold text-secondary">My React Native</Text>
                        <Text className="mt-2 text-4xl font-pbold font-bold text-secondary">- A - P - P -</Text>

                        {/* Горизонтальна риска */}
                        <View className="border-t border-gray-400 w-full my-6" />

                        {/* Список категорій */}
                        <View className="w-full">
                            <Text className="text-black text-xl font-bold mb-4">Ваші категорії</Text>

                            {loading && <Text>Завантаження...</Text>}

                            {!loading && categories.length === 0 && (
                                <Text>Категорій поки немає</Text>
                            )}

                            {!loading && categories.map(cat => {
                                const imageUri = cat.image
                                    ? `${BASE_URL}/images/100_${cat.image}`
                                    : null;

                                return (
                                    <TouchableOpacity key={cat.id} className="flex-row items-center bg-slate-200 rounded-xl p-3 mb-3">
                                        <Image
                                            source={imageUri ? { uri: imageUri } : images.noimage}
                                            className="w-14 h-14 rounded-lg mr-3"
                                            resizeMode="cover"
                                        />
                                        <Text className="text-black text-lg font-semibold">{cat.name}</Text>
                                    </TouchableOpacity>
                                );
                            })}

                            <CustomButton title="Додати категорію" handlePress={() => {
                                router.push("/category/create")
                            }} containerStyles="mt-4 w-full bg-slate-700 rounded-xl"/>

                        </View>
                    </View>
                </ScrollView>
            </SafeAreaView>
        </>
    );
}

export default Index;
