@file:OptIn(ExperimentalMaterial3Api::class, ExperimentalPagerApi::class)

package com.example.films_viewer_kotlin.screens.movies

import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Favorite
import androidx.compose.material.icons.filled.FavoriteBorder
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import coil.compose.AsyncImage
import com.example.films_viewer_kotlin.models.Movie
import com.example.films_viewer_kotlin.services.MovieService
import com.google.accompanist.pager.ExperimentalPagerApi
import com.google.accompanist.pager.HorizontalPager
import com.google.accompanist.pager.rememberPagerState
import kotlinx.coroutines.launch

@Composable
fun MovieDetailScreen(
    movie: Movie,
    userId: String,
) {
    val context = LocalContext.current
    var currentIndex by remember { mutableIntStateOf(0) }
    var isFavorite by remember { mutableStateOf(movie.favoritedBy.contains(userId)) }
    val scope = rememberCoroutineScope()
    val pagerState = rememberPagerState(initialPage = movie.imageUrls.size * 500);
    val movieService = remember { MovieService() }

    Scaffold(
        topBar = {
            TopAppBar(
                title = {
                    Text(text = movie.title)
                },
                actions = {
                    IconButton(onClick = {
                        isFavorite = !isFavorite
                        scope.launch {
                            try {
                                movieService.toggleFavorite(movie.id, userId)
                            } catch (e: Exception) {
                                Toast.makeText(
                                    context,
                                    "Ошибка обновления избранного",
                                    Toast.LENGTH_SHORT
                                ).show()
                            }
                        }
                    }) {
                        Icon(
                            imageVector = if (isFavorite) Icons.Default.Favorite else Icons.Default.FavoriteBorder,
                            contentDescription = "Favorite",
                            tint = Color.Red
                        )
                    }
                }
            )
        }
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .padding(paddingValues)
                .verticalScroll(rememberScrollState())
                .padding(16.dp)
        ) {
            HorizontalPager(
                count = movie.imageUrls.size * 1000,
                state = pagerState,
                modifier = Modifier
                    .fillMaxWidth()
                    .height(500.dp)
            ) { page ->
                AsyncImage(
                    model = movie.imageUrls[page % movie.imageUrls.size],
                    contentDescription = "Movie Image",
                    contentScale = androidx.compose.ui.layout.ContentScale.Crop,
                    modifier = Modifier.fillMaxSize()
                )
            }
            Spacer(modifier = Modifier.height(8.dp))
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.Center
            ) {
                movie.imageUrls.forEachIndexed { index, _ ->
                    val selected = (pagerState.currentPage % movie.imageUrls.size) == index
                    val size = if (selected) 12.dp else 8.dp
                    Box(
                        modifier = Modifier
                            .padding(horizontal = 4.dp)
                            .size(size)
                            .background(
                                color = if (selected) Color.Red else Color.Gray,
                                shape = MaterialTheme.shapes.small
                            )
                    )
                }
            }
            Spacer(modifier = Modifier.height(8.dp))
            // Жанры в виде Chip'ов:
            Row(modifier = Modifier.fillMaxWidth(), horizontalArrangement = Arrangement.spacedBy(8.dp)) {
                movie.genres.forEach { genre ->
                    AssistChip(
                        onClick = {},
                        label = { Text(text = genre, color = Color.White) },
                        colors = AssistChipDefaults.assistChipColors(containerColor = Color.Red)
                    )
                }
            }
            Spacer(modifier = Modifier.height(20.dp))
            DetailRow(title = "Режиссер", value = movie.director)
            DetailRow(title = "Год выпуска", value = movie.year.toString())
            DetailRow(title = "Продолжительность", value = "${movie.duration} мин")
            Spacer(modifier = Modifier.height(20.dp))
            Text("Описание", style = MaterialTheme.typography.titleMedium)
            Spacer(modifier = Modifier.height(8.dp))
            Text(movie.description, style = MaterialTheme.typography.bodyMedium)
        }
    }
    }