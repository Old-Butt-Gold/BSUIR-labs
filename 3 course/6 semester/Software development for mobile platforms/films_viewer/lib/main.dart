import 'package:films_viewer/Screens/Auth/AuthScreen.dart';
import 'package:films_viewer/Screens/Auth/VerifyEmailScreen.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:flutter/material.dart';
import 'Routes.dart';
import 'Screens/Auth/AuthWrapperScreen.dart';
import 'MainPage.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp();

  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      initialRoute: Routes.authWrapperScreen,
      routes: {
        Routes.authWrapperScreen: (context) => const AuthWrapperScreen(),
        Routes.home: (context) => const MainPage(),
        Routes.authScreen: (context) => const AuthScreen(),
        Routes.verifyEmailScreen: (context) => const VerifyEmailScreen(),
      },
      title: 'Фильмы',
      themeMode: ThemeMode.system,
      theme: ThemeData(
          colorScheme: ColorScheme.fromSeed(
            seedColor: Colors.deepPurple,
            brightness: Brightness.dark,
            secondary: Colors.orangeAccent,
          ),
      ),

    );
  }
}