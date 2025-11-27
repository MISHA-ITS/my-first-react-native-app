import React, { useState } from "react";
import {
    View,
    Text,
    ScrollView,
    SafeAreaView,
    TouchableOpacity,
    Image,
} from "react-native";
import { useRouter } from "expo-router";
import FormField from "@/components/form-fields";
import CustomButton from "@/components/custom-button";
import images from "@/constants/images";
import { pickImage } from "@/utils/pickimage";
import { getSecureStore } from "@/utils/secureStore";
import { BASE_URL } from "@/constants/Urls";
import { showMessage } from "react-native-flash-message";
import { getFileFromUriAsync } from "@/utils/getFileFromUriAsync";

export default function CreateNoteCategory() {
    const router = useRouter();

    const [categoryName, setCategoryName] = useState("");
    const [imageUrl, setImageUrl] = useState("");
    const [errors, setErrors] = useState<string[]>([]);
    const [loading, setLoading] = useState(false);

    const validationChange = (isValid: boolean, fieldKey: string) => {
        if (isValid && errors.includes(fieldKey)) {
            setErrors(errors.filter(x => x !== fieldKey));
        } else if (!isValid && !errors.includes(fieldKey)) {
            setErrors(state => [...state, fieldKey]);
        }
    };

    const submit = async () => {
        if (errors.length !== 0) {
            showMessage({
                message: "Правильно заповніть всі поля",
                type: "danger",
            });
            return;
        }

        setLoading(true);

        try {
            const token = await getSecureStore("token");
            const fileImage = imageUrl
                ? await getFileFromUriAsync(imageUrl)
                : null;

            const formData = new FormData();
            formData.append("Name", categoryName);

            if (fileImage) {
                formData.append("Image", {
                    uri: fileImage.uri,
                    name: fileImage.name,
                    type: fileImage.type
                } as any);
            }

            const response = await fetch(`${BASE_URL}/api/NoteCategories/Create`, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                },
                body: formData,
            });

            if (!response.ok) {
                showMessage({ message: "Помилка створення", type: "danger" });
                return;
            }

            showMessage({ message: "Категорію створено!", type: "success" });

            router.replace("/"); // повернутись на головну

        } catch (ex) {
            console.log("error:", ex);
            showMessage({ message: "Помилка", type: "danger" });
        } finally {
            setLoading(false);
        }
    };

    return (
        <SafeAreaView className="bg-primary h-full">
            <ScrollView className="px-6 py-10">
                <Text className="text-3xl font-bold text-center text-blue-700 mb-6">
                    Нова категорія
                </Text>

                {/* Вибір зображення */}
                <TouchableOpacity
                    className="mb-5 self-center w-[150px] h-[150px] rounded-xl overflow-hidden bg-slate-200"
                    onPress={async () => {
                        const img = await pickImage();
                        setImageUrl(img ?? "");
                    }}
                >
                    <Image
                        source={imageUrl ? { uri: imageUrl } : images.noimage}
                        className="w-full h-full"
                        resizeMode="cover"
                    />
                </TouchableOpacity>

                <FormField
                    placeholder="Назва категорії"
                    title="Назва"
                    value={categoryName}
                    handleChangeText={setCategoryName}
                    rules={[
                        { rule: "required", message: "Назва є обов'язковою" },
                        { rule: "min", value: 2, message: "Мінімум 2 символи" },
                        { rule: "max", value: 50, message: "Максимум 50 символів" },
                    ]}
                    onValidationChange={validationChange}
                />

                <CustomButton
                    title="Зберегти"
                    handlePress={submit}
                    isLoading={loading}
                    containerStyles="mt-6 bg-green-600 rounded-xl"
                />

                <CustomButton
                    title="Назад"
                    handlePress={() => router.back()}
                    containerStyles="mt-2 bg-gray-400 rounded-xl"
                />
            </ScrollView>
        </SafeAreaView>
    );
}
