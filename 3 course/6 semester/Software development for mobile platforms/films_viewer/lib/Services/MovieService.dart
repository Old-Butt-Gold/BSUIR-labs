import 'package:cloud_firestore/cloud_firestore.dart';
import '../Models/Movie.dart';

class MovieService {
  final FirebaseFirestore _firestore = FirebaseFirestore.instance;

  // Получение списка всех фильмов
  Stream<List<Movie>> getMoviesStream() {
    return _firestore.collection('movies').snapshots().map((snapshot) {
      return snapshot.docs.map((doc) {
        return Movie.fromFirestore(doc.data(), doc.id);
      }).toList();
    });
  }

  // Получение избранных фильмов для конкретного пользователя
  Stream<List<Movie>> getFavoriteMovies(String userId) {
    return _firestore
        .collection('movies')
        .where('favoritedBy', arrayContains: userId) // Фильтруем по пользователю
        .snapshots()
        .map((snapshot) {
      return snapshot.docs.map((doc) {
        return Movie.fromFirestore(doc.data(), doc.id);
      }).toList();
    });
  }

  // Добавление фильма в избранное
  Future<void> toggleFavorite(String movieId, String userId) async {
    final docRef = _firestore.collection('movies').doc(movieId);
    final doc = await docRef.get();

    if (!doc.exists) return;

    final isFavorite = (doc['favoritedBy'] as List).contains(userId);

    if (isFavorite) {
      await docRef.update({
        'favoritedBy': FieldValue.arrayRemove([userId])
      });
    } else {
      await docRef.update({
        'favoritedBy': FieldValue.arrayUnion([userId])
      });
    }
  }

  // Добавление нового фильма (для админ-панели)
  Future<void> addMovie(Movie movie) async {
    final docRef = await _firestore.collection('movies').add(movie.toFirestore());
    // final generatedId = docRef.id;

    // Обновляем документ, чтобы сохранить сгенерированный id в объекте
    // await docRef.update({'id': generatedId});
  }

}