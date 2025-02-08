package com.example.films_viewer_kotlin.screens.movies

import android.os.Bundle
import android.os.Parcel
import android.os.Parcelable
import android.widget.Toast
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowDropDown
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.platform.LocalContext
import androidx.core.os.bundleOf
import androidx.navigation.NavController
import com.example.films_viewer_kotlin.Routes
import com.example.films_viewer_kotlin.models.Movie
import com.example.films_viewer_kotlin.services.MovieService
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch

@Composable
fun MoviesScreen(
    userId: String,
    movieService: MovieService,
    navController: NavController
) {
    if (userId.isBlank()) {
        return
    }

    val context = LocalContext.current
    val scope = rememberCoroutineScope()

    // Состояния для списка фильмов и фильтров
    var allMovies by remember { mutableStateOf<List<Movie>>(emptyList()) }
    var filteredMovies by remember { mutableStateOf<List<Movie>>(emptyList()) }
    var genres by remember { mutableStateOf<List<String>>(emptyList()) }
    var searchQuery by remember { mutableStateOf("") }
    var selectedGenre by remember { mutableStateOf<String?>(null) }

    // Подписываемся на поток фильмов
    LaunchedEffect(movieService) {
        movieService.getMoviesStream().collectLatest { movies ->
            allMovies = movies
            // Фильтрация списка
            filteredMovies = movies.filter { movie ->
                movie.title.lowercase().contains(searchQuery.lowercase()) &&
                        (selectedGenre == null || movie.genres.contains(selectedGenre))
            }
            // Вычисляем список жанров
            genres = movies.flatMap { it.genres }.toSet().toList().sorted()
        }
    }

    Column(modifier = Modifier.fillMaxSize()) {
        // Строка поиска и выбор жанра
        Row(modifier = Modifier
            .fillMaxWidth()
            .padding(horizontal = 16.dp, vertical = 4.dp)) {
            OutlinedTextField(
                value = searchQuery,
                onValueChange = { query ->
                    searchQuery = query
                    filteredMovies = allMovies.filter { movie ->
                        movie.title.lowercase().contains(query.lowercase()) &&
                                (selectedGenre == null || movie.genres.contains(selectedGenre))
                    }
                },
                label = { Text("Поиск") },
                leadingIcon = { Icon(Icons.Default.Search, contentDescription = "Поиск") },
                modifier = Modifier.weight(1f)
            )
            Spacer(modifier = Modifier.width(12.dp))
            GenreDropdown(
                genres = listOf(null) + genres,
                selectedGenre = selectedGenre,
                onGenreSelected = { genre ->
                    selectedGenre = genre
                    filteredMovies = allMovies.filter { movie ->
                        movie.title.lowercase().contains(searchQuery.lowercase()) &&
                                (selectedGenre == null || movie.genres.contains(selectedGenre))
                    }
                }
            )
        }
        // Список фильмов
        LazyColumn(modifier = Modifier.fillMaxSize()) {
            items(filteredMovies) { movie ->
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

@Composable
fun GenreDropdown(
    genres: List<String?>,
    selectedGenre: String?,
    onGenreSelected: (String?) -> Unit
) {
    var expanded by remember { mutableStateOf(false) }
    Box {
        OutlinedTextField(
            value = selectedGenre ?: "Все жанры",
            onValueChange = {},
            readOnly = true,
            singleLine = true,
            maxLines = 1,
            label = { Text("Жанры") },
            trailingIcon = {
                IconButton(onClick = { expanded = true }) {
                    Icon(Icons.Default.ArrowDropDown, contentDescription = null)
                }
            },
            modifier = Modifier.width(155.dp)
        )
        DropdownMenu(
            expanded = expanded,
            onDismissRequest = { expanded = false }
        ) {
            genres.forEach { genre ->
                DropdownMenuItem(
                    text = { Text(text = genre ?: "Все жанры") },
                    onClick = {
                        onGenreSelected(genre)
                        expanded = false
                    }
                )
            }
        }
    }
}
