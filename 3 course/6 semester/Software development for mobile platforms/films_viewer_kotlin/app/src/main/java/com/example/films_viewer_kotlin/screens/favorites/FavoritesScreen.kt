package com.example.films_viewer_kotlin.screens.favorites

import android.widget.Toast
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.material3.Text
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.platform.LocalContext
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.ui.Modifier
import androidx.compose.runtime.remember
import androidx.compose.runtime.mutableStateOf
import androidx.core.os.bundleOf
import androidx.navigation.NavController
import com.example.films_viewer_kotlin.Routes
import com.example.films_viewer_kotlin.models.Movie
import com.example.films_viewer_kotlin.services.MovieService
import com.example.films_viewer_kotlin.screens.movies.MovieCard
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch

@Composable
fun FavoritesScreen(
    userId: String,
    movieService: MovieService,
    navController: NavController
) {
    if (userId.isBlank()) {
        return
    }

    var movies by remember { mutableStateOf<List<Movie>>(emptyList()) }
    val scope = rememberCoroutineScope()
    val listState = rememberLazyListState()

    LaunchedEffect(movieService, userId) {
        movieService.getFavoriteMovies(userId).collectLatest { favMovies ->
            movies = favMovies
        }
    }

    if (movies.isEmpty()) {
        Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
            Text("Нет избранных фильмов")
        }
    } else {
        LazyColumn(state = listState, modifier = Modifier.fillMaxSize()) {
            itemsIndexed(movies) { index, movie ->
                MovieCard(
                    movie = movie,
                    userId = userId,
                    navController = navController,
                    movieService = movieService,
                    scope = scope
                )
            }
        }
    }
}
