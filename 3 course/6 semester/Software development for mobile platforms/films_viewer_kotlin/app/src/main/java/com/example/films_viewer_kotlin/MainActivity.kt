package com.example.films_viewer_kotlin

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.runtime.Composable
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.navigation.toRoute
import com.example.films_viewer_kotlin.repository.DataHolder
import com.example.films_viewer_kotlin.screens.auth.AuthScreen
import com.example.films_viewer_kotlin.screens.auth.AuthWrapperScreen
import com.example.films_viewer_kotlin.screens.auth.VerifyEmailScreen
import com.example.films_viewer_kotlin.screens.movies.MovieDetailScreen
import com.example.films_viewer_kotlin.ui.theme.FilmsViewerKotlinTheme
import com.google.firebase.FirebaseApp

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        FirebaseApp.initializeApp(this);
        enableEdgeToEdge()
        setContent() { // trailing lambda
            FilmsViewerKotlinTheme() {
                FilmViewerApp();
            }
        }
    }
}

@Composable
fun FilmViewerApp() {
    val navController = rememberNavController()

    NavHost(navController = navController, startDestination = Routes.AuthWrapperScreen.route) {
        composable(Routes.AuthWrapperScreen.route) { AuthWrapperScreen(navController) }
        composable(Routes.AuthScreen.route) { AuthScreen(navController) }
        composable(Routes.VerifyEmailScreen.route) { VerifyEmailScreen(navController) }
        composable(Routes.Home.route) { MainPage(navController) }
        composable(Routes.MovieDetail.route) { MovieDetailScreen(DataHolder.movie, DataHolder.userId) }
    }
}

// или использовать такой атрибут когда нужно paddingValues
// @SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
/*
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun MainPage() {
    Scaffold(
        topBar = {
            TopAppBar(title = { Text("Главная страница") })
        }
    ) { paddingValues ->
        Text(
            text = "Добро пожаловать!",
            style = MaterialTheme.typography.headlineMedium,
            modifier = Modifier.padding(paddingValues).padding(16.dp)
        )
    }
}*/
