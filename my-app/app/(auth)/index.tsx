import {Dimensions, SafeAreaView, ScrollView, Text, View, Image} from "react-native";
import images from "@/constants/images";

const SignIn = () => {
    return (
        <>
            <SafeAreaView className="bg-primary h-full">
                <ScrollView>
                    <View className="w-full gap-2 flex justify-center items-center h-full px-4 my-6"
                          style={{
                              minHeight: Dimensions.get('window').height - 100,
                          }}>
                        <View className="w-[200px] h-[200px] overflow-hidden mt-10 mb-6">
                            <Image
                                source={{ uri: "https://static.tildacdn.pro/tild3236-3261-4261-b930-653466663134/_.png" }}
                                className="w-full h-full"
                                resizeMode="cover"
                            />
                        </View>

                        <View className="flex flex-row items-center justify-center">
                            <Text className="mt-2 text-2xl font-pbold font-bold text-secondary">My React Native</Text>
                        </View>
                        <Text className="mt-2 text-4xl font-pbold font-bold text-secondary">- A - P - P -</Text>
                    </View>
                </ScrollView>
            </SafeAreaView>
        </>
    );
}

export default SignIn;