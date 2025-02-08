package com.example.films_viewer_kotlin

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.Crossfade
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.foundation.gestures.detectTapGestures
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.input.nestedscroll.NestedScrollConnection
import androidx.compose.ui.input.nestedscroll.NestedScrollSource
import androidx.compose.ui.input.nestedscroll.nestedScroll
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.unit.dp
import androidx.navigation.NavController
import com.example.films_viewer_kotlin.screens.favorites.FavoritesScreen
import com.example.films_viewer_kotlin.screens.movies.MoviesScreen
import com.example.films_viewer_kotlin.screens.profile.ProfileScreen
import com.example.films_viewer_kotlin.screens.settings.SettingsScreen
import com.example.films_viewer_kotlin.services.AuthService
import com.example.films_viewer_kotlin.services.MovieService

@Composable
fun MainPage(navController: NavController) {
    val authService = remember { AuthService() }
    val movieService = remember { MovieService() }

    // !!!!
    val userId = authService.currentUser?.uid ?: ""

    var currentIndex by remember { mutableIntStateOf(0) }
    var isBottomBarVisible by remember { mutableStateOf(true) }

    // Определяем данные для нижней навигации
    data class BottomNavItem(val label: String, val icon: ImageVector, val color: Color)

    val bottomNavItems = listOf(
        BottomNavItem("Фильмы", Icons.Default.Movie, Color(255, 152, 0)), // Orange
        BottomNavItem("Избранное", Icons.Default.Favorite, Color(255, 82, 82)),
        BottomNavItem("Профиль", Icons.Default.Person, Color(105, 240, 174)), // GreenAccent
        BottomNavItem("Настройки", Icons.Default.Settings, Color(68, 138, 255)) // BlueAccent
    )

    // Список экранов: MoviesScreen, FavoritesScreen, ProfileScreen, SettingsScreen.
    // Реализуем MoviesScreen и FavoritesScreen как заглушки.
    val screens = listOf<@Composable () -> Unit>(
        { MoviesScreen(userId = userId, movieService = movieService, navController) },
        { FavoritesScreen(userId = userId, movieService = movieService, navController) },
        { ProfileScreen(userId = userId, email = authService.currentUser?.email ?: "", navController = navController) },
        { SettingsScreen(navController = navController) }
    )

    val nestedScrollConnection = remember {
        object : NestedScrollConnection {
            override fun onPreScroll(available: Offset, source: NestedScrollSource): Offset {
                if (available.y < 0 && isBottomBarVisible) {
                    isBottomBarVisible = false
                } else if (available.y > 0 && !isBottomBarVisible) {
                    isBottomBarVisible = true
                }
                return Offset.Zero;
            }
        }
    }

    Scaffold(
        bottomBar = {
            AnimatedVisibility(visible = isBottomBarVisible, enter = fadeIn(), exit = fadeOut()) {
                NavigationBar(containerColor = Color.Transparent, tonalElevation = 0.dp) {
                    bottomNavItems.forEachIndexed { index, item ->
                        NavigationBarItem(
                            icon = { Icon(item.icon, contentDescription = item.label) },
                            label = { Text(item.label) },
                            selected = currentIndex == index,
                            onClick = { currentIndex = index },
                            alwaysShowLabel = true,
                            colors = NavigationBarItemDefaults.colors(
                                selectedIconColor = item.color,
                                selectedTextColor = item.color,
                                unselectedIconColor = item.color.copy(alpha = 0.4f),
                                unselectedTextColor = item.color.copy(alpha = 0.4f)
                            )
                        )
                    }
                }
            }
        }
    ) { paddingValues ->
        Box(
            modifier = Modifier
                .padding(paddingValues)
                .nestedScroll(nestedScrollConnection)
                .fillMaxSize()
                .pointerInput(Unit) {
                    detectTapGestures(
                        onTap = {
                            if (!isBottomBarVisible) {
                                isBottomBarVisible = true
                            }
                        }
                    )
                }
        ) {
            Crossfade(targetState = currentIndex) { index ->
                screens[index]()
            }
        }
    }
}

/*
@Composable
fun MoviesScreen(userId: String) {
    // Здесь можно использовать LazyColumn с заглушками
    val listState = rememberLazyListState()
    LazyColumn(state = listState, modifier = Modifier.fillMaxSize()) {
        items(20) { index ->
            Text(
                text = "Фильм $index",
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(16.dp)
            )
        }
    }
}

@Composable
fun FavoritesScreen(userId: String) {
    val listState = rememberLazyListState()
    LazyColumn(state = listState, modifier = Modifier.fillMaxSize()) {
        items(10) { index ->
            Text(
                text = "Избранное $index",
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(16.dp)
            )
        }
    }
}*/
