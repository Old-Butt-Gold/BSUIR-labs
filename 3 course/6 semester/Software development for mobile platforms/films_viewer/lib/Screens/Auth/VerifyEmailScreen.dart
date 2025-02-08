import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:flutter/material.dart';
import '../../Routes.dart';
import '../../Services/AuthService.dart';

class VerifyEmailScreen extends StatefulWidget {
  const VerifyEmailScreen({super.key});

  @override
  _VerifyEmailScreenState createState() => _VerifyEmailScreenState();
}

class _VerifyEmailScreenState extends State<VerifyEmailScreen> {
  final AuthService _auth = AuthService();
  final FirebaseFirestore _firebase = FirebaseFirestore.instance;

  bool _isLoading = false;

  Future<void> _checkEmailVerification() async {
    setState(() => _isLoading = true);
    try {
      await _auth.checkEmailVerification();
      final user = _auth.currentUser;
      if (user?.emailVerified ?? false) {
        await _firebase.collection("users").doc(user!.uid).update({
          "emailVerified": true,
        });

        Navigator.pushReplacementNamed(context, Routes.home);
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Email пока не подтвержден.')),
        );
      }
    } finally {
      setState(() => _isLoading = false);
    }
  }

  Future<void> _resendVerification() async {
    setState(() => _isLoading = true);
    try {
      await _auth.resendVerificationEmail();
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Письмо отправлено повторно')),
      );
    } finally {
      setState(() => _isLoading = false);
    }
  }

  Future<void> _signOut() async {
    await _auth.signOut();
    Navigator.pushReplacementNamed(context, Routes.authWrapperScreen);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Подтвердите Email')),
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(20.0),
          child: SingleChildScrollView(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                const Icon(
                  Icons.email,
                  size: 100,
                  color: Colors.blue,
                ),
                const SizedBox(height: 20),
                const Text(
                  'Проверьте вашу почту и подтвердите email.',
                  textAlign: TextAlign.center,
                  style: TextStyle(fontSize: 18),
                ),
                const SizedBox(height: 20),
                _isLoading
                    ? const CircularProgressIndicator()
                    : Column(
                  children: [
                    ElevatedButton.icon(
                      onPressed: _checkEmailVerification,
                      icon: const Icon(Icons.check),
                      label: const Text('Проверить подтверждение'),
                      style: ElevatedButton.styleFrom(
                        padding: const EdgeInsets.symmetric(
                          horizontal: 24.0,
                          vertical: 12.0,
                        ),
                      ),
                    ),
                    const SizedBox(height: 10),
                    ElevatedButton.icon(
                      onPressed: _resendVerification,
                      icon: const Icon(Icons.refresh),
                      label: const Text('Отправить повторно'),
                      style: ElevatedButton.styleFrom(
                        padding: const EdgeInsets.symmetric(
                          horizontal: 24.0,
                          vertical: 12.0,
                        ),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 20),
                TextButton(
                  onPressed: _signOut,
                  child: const Text('Назад'),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}