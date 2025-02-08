package com.example.films_viewer_kotlin.services

import com.example.films_viewer_kotlin.models.Movie
import com.google.firebase.firestore.FieldValue
import com.google.firebase.firestore.FirebaseFirestore
import kotlinx.coroutines.channels.awaitClose
import kotlinx.coroutines.flow.callbackFlow
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.tasks.await

class MovieService {

    private val firestore = FirebaseFirestore.getInstance()

    fun getMoviesStream(): Flow<List<Movie>> = callbackFlow {
        val collectionRef = firestore.collection("movies")
        val listenerRegistration = collectionRef.addSnapshotListener { snapshot, error ->
            if (error != null) {
                close(error)
                return@addSnapshotListener
            }
            val movies = snapshot?.documents?.map { doc ->
                Movie.fromFirestore(doc.data ?: emptyMap(), doc.id)
            } ?: emptyList()
            trySend(movies)
        }
        awaitClose { listenerRegistration.remove() }
    }

    fun getFavoriteMovies(userId: String): Flow<List<Movie>> = callbackFlow {
        val query = firestore.collection("movies")
            .whereArrayContains("favoritedBy", userId)
        val listenerRegistration = query.addSnapshotListener { snapshot, error ->
            if (error != null) {
                close(error)
                return@addSnapshotListener
            }
            val movies = snapshot?.documents?.map { doc ->
                Movie.fromFirestore(doc.data ?: emptyMap(), doc.id)
            } ?: emptyList()
            trySend(movies)
        }
        awaitClose { listenerRegistration.remove() }
    }

    suspend fun toggleFavorite(movieId: String, userId: String) {
        val docRef = firestore.collection("movies").document(movieId)
        val snapshot = docRef.get().await()
        if (!snapshot.exists()) return

        val favoritedBy = snapshot.get("favoritedBy") as? List<*> ?: emptyList<Any>()
        val isFavorite = favoritedBy.contains(userId)

        if (isFavorite) {
            docRef.update("favoritedBy", FieldValue.arrayRemove(userId)).await()
        } else {
            docRef.update("favoritedBy", FieldValue.arrayUnion(userId)).await()
        }
    }

}
