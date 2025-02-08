import 'package:cloud_firestore/cloud_firestore.dart';
import '../Models/UserProfile.dart';

class ProfileService {
  final FirebaseFirestore _firestore = FirebaseFirestore.instance;

  // Получение профиля пользователя
  Stream<UserProfile> getProfileStream(String userId) {
    return _firestore.collection('users').doc(userId).snapshots().map((snapshot) {
      if (snapshot.exists) {
        return UserProfile.fromFirestore(snapshot.data()!, snapshot.id);
      } else {
        return UserProfile.empty(userId);
      }
    });
  }

  // Обновление профиля
  Future<void> updateProfile(UserProfile profile) async {
    await _firestore
        .collection('users')
        .doc(profile.id)
        .update(profile.toFirestore());
  }

  // Создание профиля при первом входе
  Future<void> createInitialProfile(UserProfile profile) async {
    final doc = await _firestore.collection('users').doc(profile.id).get();
    if (!doc.exists) {
      var dictionary = profile.toFirestore();
      dictionary["emailVerified"] = false;
      dictionary["createdAt"] = FieldValue.serverTimestamp();
      await _firestore.collection('users')
          .doc(profile.id)
          .set(dictionary);
    }
  }
}