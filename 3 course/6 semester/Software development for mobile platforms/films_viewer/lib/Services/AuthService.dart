// auth_service.dart
import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:films_viewer/Services/ProfileService.dart';
import 'package:firebase_auth/firebase_auth.dart';

import '../Models/UserProfile.dart';

class AuthService {
  final FirebaseAuth _auth = FirebaseAuth.instance;
  final FirebaseFirestore _firestore = FirebaseFirestore.instance;

  // Текущий пользователь
  User? get currentUser => _auth.currentUser;

  // Поток состояния авторизации
  Stream<User?> get authStateChanges => _auth.authStateChanges();

  // Вход по email/password
  Future<User?> signIn(String email, String password) async {
    try {
      final credential = await _auth.signInWithEmailAndPassword(
        email: email,
        password: password,
      );

      return credential.user;
    } on FirebaseAuthException catch (e) {
      throw _handleAuthError(e);
    }
  }

  // Регистрация с отправкой подтверждения
  Future<User?> signUp(String email, String password) async {
    try {
      final credential = await _auth.createUserWithEmailAndPassword(
        email: email,
        password: password,
      );

      var user = UserProfile.empty(credential.user!.uid, email = credential.user!.email!);
      await ProfileService().createInitialProfile(user);

      await credential.user!.sendEmailVerification();
      return credential.user;
    } on FirebaseAuthException catch (e) {
      throw _handleAuthError(e);
    }
  }

  // Проверка подтверждения email
  Future<void> checkEmailVerification() async {
    await _auth.currentUser?.reload();
  }

  // Повторная отправка подтверждения
  Future<void> resendVerificationEmail() async {
    await _auth.currentUser?.sendEmailVerification();
  }

  // Удаление пользователя с повторной аутентификацией
  Future<void> deleteUser(String password) async {
    final user = _auth.currentUser;

    if (user != null) {
      try {
        final email = user.email;
        if (email == null) {
          throw Exception('Email пользователя не найден');
        }

        final credential = EmailAuthProvider.credential(email: email, password: password);
        await user.reauthenticateWithCredential(credential);

        await _firestore.collection('users').doc(user.uid).delete();

        final movies = await _firestore.collection('movies').where('favoritedBy', arrayContains: user.uid).get();
        for (var movie in movies.docs) {
          final favorites = List<String>.from(movie['favoritedBy']);
          favorites.remove(user.uid);
          await movie.reference.update({'favoritedBy': favorites});
        }

        await user.delete();
        await signOut();
      } on FirebaseAuthException catch (e) {
        throw Exception(_handleAuthError(e));
      } catch (e) {
        throw Exception('Ошибка при удалении пользователя: ${e.toString()}');
      }
    }
  }

  // Выход
  Future<void> signOut() async => await _auth.signOut();

  // Обработка ошибок
  String _handleAuthError(FirebaseAuthException e) {
    switch (e.code) {
      case 'requires-recent-login':
        return 'Требуется повторный вход';
      case 'unverified-email':
        return 'Email не подтвержден';
      case 'invalid-email':
        return 'Некорректный email';
      case 'user-disabled':
        return 'Пользователь заблокирован';
      case 'user-not-found':
        return 'Пользователь не найден';
      case 'wrong-password':
        return 'Неверный пароль';
      case 'email-already-in-use':
        return 'Email уже зарегистрирован';
      case 'weak-password':
        return 'Слишком слабый пароль';
      case 'invalid-credential':
        return 'Проверьте логин или пароль';
      default:
        return 'Ошибка аутентификации: ${e.message}';
    }
  }
}