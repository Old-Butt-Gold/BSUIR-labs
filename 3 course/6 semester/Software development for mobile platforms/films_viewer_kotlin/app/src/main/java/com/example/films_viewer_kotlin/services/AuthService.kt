package com.example.films_viewer_kotlin.services

import com.example.films_viewer_kotlin.models.UserProfile
import com.google.android.gms.tasks.Task
import com.google.android.gms.tasks.Tasks
import com.google.firebase.auth.EmailAuthProvider
import com.google.firebase.auth.FirebaseAuth
import com.google.firebase.auth.FirebaseAuthException
import com.google.firebase.firestore.FirebaseFirestore
import kotlinx.coroutines.tasks.await
import okhttp3.internal.wait

class AuthService {

    private val auth: FirebaseAuth = FirebaseAuth.getInstance()
    private val firestore: FirebaseFirestore = FirebaseFirestore.getInstance()

    val currentUser get() = auth.currentUser

    suspend fun signIn(email: String, password: String): com.google.firebase.auth.FirebaseUser? {
        try {
            val result = auth.signInWithEmailAndPassword(email, password).await()
            // Обновляем поле emailVerified в документе пользователя
            return result.user
        } catch (e: FirebaseAuthException) {
            throw Exception(handleAuthError(e))
        }
    }

    suspend fun signUp(email: String, password: String): com.google.firebase.auth.FirebaseUser? {
        try {
            val result = auth.createUserWithEmailAndPassword(email, password).await()

            val userProfile = UserProfile.empty(result.user!!.uid, result.user!!.email ?: "")
            ProfileService().createInitialProfile(userProfile).await()

            result.user!!.sendEmailVerification().await()
            return result.user
        } catch (e: FirebaseAuthException) {
            throw Exception(handleAuthError(e))
        }
    }

    suspend fun checkEmailVerification() {
        auth.currentUser?.reload()?.await()
    }

    suspend fun resendVerificationEmail() {
        auth.currentUser?.sendEmailVerification()?.await()
    }

    suspend fun deleteUser(password: String) {
        val user = auth.currentUser
        if (user != null) {
            try {
                val email = user.email ?: throw Exception("Email пользователя не найден")

                val credential = EmailAuthProvider.getCredential(email, password)
                user.reauthenticate(credential).await()

                firestore.collection("users").document(user.uid).delete().await()

                val moviesSnapshot = firestore.collection("movies")
                    .whereArrayContains("favoritedBy", user.uid)
                    .get().await()

                for (doc in moviesSnapshot.documents) {
                    val favorites = (doc.get("favoritedBy") as? List<*>)?.toMutableList() ?: mutableListOf()
                    favorites.remove(user.uid)
                    doc.reference.update("favoritedBy", favorites).await()
                }

                user.delete().await()

                signOut();
            } catch (e: FirebaseAuthException) {
                throw Exception(handleAuthError(e))
            } catch (e: Exception) {
                throw Exception("Ошибка при удалении пользователя: ${e.message}")
            }
        }
    }

    suspend fun signOut() {
        auth.signOut()
    }

    private fun handleAuthError(e: FirebaseAuthException): String {
        return when (e.errorCode) {
            "ERROR_REQUIRES_RECENT_LOGIN" -> "Требуется повторный вход"
            "ERROR_UNVERIFIED_EMAIL" -> "Email не подтвержден"
            "ERROR_INVALID_EMAIL" -> "Некорректный email"
            "ERROR_USER_DISABLED" -> "Пользователь заблокирован"
            "ERROR_USER_NOT_FOUND" -> "Пользователь не найден"
            "ERROR_WRONG_PASSWORD" -> "Неверный пароль"
            "ERROR_EMAIL_ALREADY_IN_USE" -> "Email уже зарегистрирован"
            "ERROR_WEAK_PASSWORD" -> "Слишком слабый пароль"
            "ERROR_INVALID_CREDENTIAL" -> "Проверьте логин или пароль"
            else -> "Ошибка аутентификации: ${e.message}"
        }
    }
}
