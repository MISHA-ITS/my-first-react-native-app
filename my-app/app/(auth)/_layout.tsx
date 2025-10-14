import { useColorScheme } from '@/hooks/use-color-scheme';
import {Tabs} from "expo-router";
import {Colors} from "@/constants/theme";
import {HapticTab} from "@/components/haptic-tab";
import {IconSymbol} from "@/components/ui/icon-symbol";
import React from "react";
import Entypo from '@expo/vector-icons/Entypo';
import MaterialIcons from '@expo/vector-icons/MaterialIcons';

const AuthLayout = () => {

    const colorScheme = useColorScheme();

    return (
        <Tabs
            screenOptions={{
                tabBarActiveTintColor: Colors[colorScheme ?? 'light'].tint,
                headerShown: false,
                tabBarButton: HapticTab,
            }}>
            <Tabs.Screen
                name="index"
                options={{
                    title: 'Головна',
                    tabBarIcon: ({ color }) => <Entypo name="home" size={28} color={color} />,
                }}
            />
            <Tabs.Screen
                name="sign-in"
                options={{
                    title: 'Вхід',
                    tabBarIcon: ({ color }) => <Entypo name="login" size={28} color={color} />,
                }}
            />
            <Tabs.Screen
                name="sign-up"
                options={{
                    title: 'Реєстрація',
                    tabBarIcon: ({ color }) => <MaterialIcons name="app-registration" size={28} color={color} />,
                }}
            />
        </Tabs>
    );
}

export default AuthLayout;