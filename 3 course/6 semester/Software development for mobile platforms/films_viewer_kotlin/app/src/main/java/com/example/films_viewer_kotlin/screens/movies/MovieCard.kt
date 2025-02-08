package com.example.films_viewer_kotlin.screens.movies

import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Text
import androidx.compose.material3.MaterialTheme
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Favorite
import androidx.compose.material.icons.filled.FavoriteBorder
import androidx.compose.runtime.Composable
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import androidx.navigation.NavController
import coil.compose.AsyncImage
import com.example.films_viewer_kotlin.Routes
import com.example.films_viewer_kotlin.models.Movie
import com.example.films_viewer_kotlin.repository.DataHolder
import com.example.films_viewer_kotlin.services.MovieService
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.launch

@Composable
fun MovieCard(
    movie: Movie,
    userId: String,
    navController: NavController,
    movieService: MovieService,
    scope: CoroutineScope
) {
    val context = LocalContext.current
    val isFavorite = movie.favoritedBy.contains(userId)
    Card(
        shape = RoundedCornerShape(12.dp),
        elevation = CardDefaults.cardElevation(6.dp),
        modifier = Modifier
            .fillMaxWidth()
            .padding(16.dp)
            .clickable {
                navController.navigate(Routes.MovieDetail.route) {
                    DataHolder.movie = movie;
                    DataHolder.userId = userId;
                }
            }
    ) {
        Row(modifier = Modifier.padding(16.dp), verticalAlignment = Alignment.Top) {
            AsyncImage(
                model = movie.posterUrl,
                contentDescription = movie.title,
                contentScale = androidx.compose.ui.layout.ContentScale.Crop,
                modifier = Modifier
                    .size(width = 120.dp, height = 180.dp)
                    .clip(RoundedCornerShape(8.dp))
            )
            Spacer(modifier = Modifier.width(16.dp))
            Column(modifier = Modifier.weight(1f)) {
                Text(text = movie.title, style = MaterialTheme.typography.titleMedium)
                Spacer(modifier = Modifier.height(8.dp))
                Text(
                    text = movie.genres.joinToString(", "),
                    style = MaterialTheme.typography.bodyMedium.copy(color = Color.Gray)
                )
                Spacer(modifier = Modifier.height(8.dp))
                Text(
                    text = "${movie.year} • ${movie.duration} мин",
                    style = MaterialTheme.typography.bodySmall.copy(color = Color.Gray)
                )
            }
            IconButton(onClick = { scope.launch {
                try {
                    movieService.toggleFavorite(movie.id, userId)
                } catch (e: Exception) {
                    Toast.makeText(context, "Ошибка обновления избранного " + e.message, Toast.LENGTH_LONG).show()
                }
            } }) {
                Icon(
                    imageVector = if (isFavorite) Icons.Default.Favorite else Icons.Default.FavoriteBorder,
                    contentDescription = "Favorite",
                    tint = Color.Red
                )
            }
        }
    }
}
