package com.example.films_viewer_kotlin.services

import com.example.films_viewer_kotlin.models.UserProfile
import com.google.android.gms.tasks.Task
import com.google.android.gms.tasks.Tasks
import com.google.firebase.firestore.FieldValue
import com.google.firebase.firestore.FirebaseFirestore
import kotlinx.coroutines.channels.awaitClose
import kotlinx.coroutines.flow.callbackFlow
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.tasks.await

class ProfileService {

    private val firestore = FirebaseFirestore.getInstance()

    fun getProfileStream(userId: String): Flow<UserProfile> = callbackFlow {
        val docRef = firestore.collection("users").document(userId)
        val listenerRegistration = docRef.addSnapshotListener { snapshot, error ->
            if (error != null) {
                close(error)
                return@addSnapshotListener
            }
            if (snapshot != null && snapshot.exists()) {
                val data = snapshot.data
                if (data != null) {
                    val profile = UserProfile.fromFirestore(data, snapshot.id)
                    trySend(profile)
                }
            } else {
                trySend(UserProfile.empty(userId))
            }
        }
        awaitClose { listenerRegistration.remove() }
    }

    suspend fun updateProfile(profile: UserProfile) {
        firestore.collection("users")
            .document(profile.id)
            .update(profile.toFirestore())
            .await()
    }

    fun createInitialProfile(profile: UserProfile): Task<Void> {
        val docRef = firestore.collection("users").document(profile.id)
        return docRef.get().continueWithTask { task ->
            if (!task.result.exists()) {
                val dictionary = profile.toFirestore().toMutableMap()
                dictionary["emailVerified"] = false
                dictionary["createdAt"] = FieldValue.serverTimestamp()
                docRef.set(dictionary)
            } else {
                Tasks.forResult(null)
            }
        }
    }
}
