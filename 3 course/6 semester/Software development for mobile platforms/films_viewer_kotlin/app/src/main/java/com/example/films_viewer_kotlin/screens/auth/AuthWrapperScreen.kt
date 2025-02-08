package com.example.films_viewer_kotlin.screens.auth

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.navigation.NavController
import com.example.films_viewer_kotlin.Routes
import com.google.firebase.auth.FirebaseAuth


@Composable
fun AuthWrapperScreen(navController: NavController) {
    val auth = FirebaseAuth.getInstance()
    val currentUser = auth.currentUser

    LaunchedEffect(currentUser) {
        if (currentUser == null) {
            // Если пользователь не авторизован, направляем на экран авторизации
            navController.popBackStack()
            navController.navigate(Routes.AuthScreen.route)
        } else {
            if (!currentUser.isEmailVerified) {
                // Если email не подтвержден, направляем на экран подтверждения
                navController.popBackStack()
                navController.navigate(Routes.VerifyEmailScreen.route)
            } else {
                navController.popBackStack()
                // Если пользователь авторизован и email подтвержден, направляем на главный экран
                navController.navigate(Routes.Home.route)
            }
        }
    }

    Box(
        modifier = Modifier.fillMaxSize(),
        contentAlignment = Alignment.Center
    ) {
        CircularProgressIndicator(color = MaterialTheme.colorScheme.primary)
    }
    /*when {
        firebaseUser == null -> {
            navController.navigate("authScreen") {
            }
            // AuthScreen(navController)
        }
        firebaseUser != null && !firebaseUser!!.isEmailVerified -> {
            navController.navigate("verifyEmail") {
            }
            // VerifyEmailScreen(navController)
        }
        else -> {
            navController.navigate("home") {
            }
            // MainPage()
        }
    }*/
}
