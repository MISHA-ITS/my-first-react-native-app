import * as DocumentPicker from "expo-document-picker";

export const pickUserFile = async (): Promise<any | null> => {
    try {
        const result = await DocumentPicker.getDocumentAsync({
            type: "application/json",
            copyToCacheDirectory: true,
        });

        if (result.canceled) return null;

        const fileUri = result.assets[0].uri;

        const response = await fetch(fileUri);
        const fileText = await response.text();
        const userData = JSON.parse(fileText);

        return userData;
    } catch (error) {
        console.error("Помилка при зчитуванні файлу:", error);
        return null;
    }
};