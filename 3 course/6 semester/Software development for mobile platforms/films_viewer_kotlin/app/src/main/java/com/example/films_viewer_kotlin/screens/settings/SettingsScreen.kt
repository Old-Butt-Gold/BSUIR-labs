package com.example.films_viewer_kotlin.screens.settings

import android.widget.Toast
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import androidx.navigation.NavController
import com.example.films_viewer_kotlin.Routes
import com.example.films_viewer_kotlin.services.AuthService
import kotlinx.coroutines.launch

@Composable
fun SettingsScreen(navController: NavController) {
    val authService = remember { AuthService() }
    var showDeleteDialog by remember { mutableStateOf(false) }
    var showCreateDialog by remember { mutableStateOf(false) }

    val scope = rememberCoroutineScope()
    val context = LocalContext.current

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Button(
            onClick = { showDeleteDialog = true },
            colors = ButtonDefaults.buttonColors(
                containerColor = Color(29, 27, 32),
                contentColor = Color(207, 193, 235)
            )
        ) {
            Text("Удалить аккаунт")
        }
        Spacer(modifier = Modifier.height(20.dp))
        Button(
            onClick = { showCreateDialog = true },
            colors = ButtonDefaults.buttonColors(
                containerColor = Color(29, 27, 32),
                contentColor = Color(207, 193, 235)
            )
        ) {
            Text("Создать нового пользователя")
        }
    }

    if (showDeleteDialog) {
        DeleteUserDialog(
            onDismiss = { showDeleteDialog = false },
            onDelete = { password ->
                scope.launch {
                    try {
                        authService.deleteUser(password)
                        navController.popBackStack()
                        navController.navigate(Routes.AuthWrapperScreen.route)
                    } catch (e: Exception) {
                        Toast.makeText(context, "Ошибка: ${e.message}", Toast.LENGTH_LONG).show()
                    }
                }
            }
        )
    }

    if (showCreateDialog) {
        CreateUserDialog(
            onDismiss = { showCreateDialog = false },
            onCreate = { email, password ->
                scope.launch {
                    try {
                        authService.signUp(email, password)
                        Toast.makeText(
                            context,
                            "Пользователь создан. Проверьте почту для подтверждения.",
                            Toast.LENGTH_LONG
                        ).show()
                    } catch (e: Exception) {
                        Toast.makeText(
                            context,
                            "Ошибка при создании пользователя: ${e.message}",
                            Toast.LENGTH_LONG
                        ).show()
                    }
                }
            }
        )
    }
}

@Composable
fun DeleteUserDialog(onDismiss: () -> Unit, onDelete: (String) -> Unit) {
    var password by remember { mutableStateOf("") }
    AlertDialog(
        onDismissRequest = onDismiss,
        title = { Text("Удаление аккаунта") },
        text = {
            Column {
                Text("Введите ваш пароль для подтверждения удаления аккаунта.")
                Spacer(modifier = Modifier.height(8.dp))
                OutlinedTextField(
                    value = password,
                    onValueChange = { password = it },
                    label = { Text("Пароль") },
                    visualTransformation = PasswordVisualTransformation(),
                    singleLine = true
                )
            }
        },
        confirmButton = {
            Button(onClick = { onDelete(password); onDismiss() }) {
                Text("Удалить")
            }
        },
        dismissButton = {
            TextButton(onClick = onDismiss) {
                Text("Отмена")
            }
        }
    )
}

@Composable
fun CreateUserDialog(onDismiss: () -> Unit, onCreate: (String, String) -> Unit) {
    var email by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    AlertDialog(
        onDismissRequest = onDismiss,
        title = { Text("Создать нового пользователя") },
        text = {
            Column {
                OutlinedTextField(
                    value = email,
                    onValueChange = { email = it },
                    label = { Text("Email") },
                    singleLine = true
                )
                Spacer(modifier = Modifier.height(8.dp))
                OutlinedTextField(
                    value = password,
                    onValueChange = { password = it },
                    label = { Text("Пароль") },
                    visualTransformation = PasswordVisualTransformation(),
                    singleLine = true
                )
            }
        },
        confirmButton = {
            Button(onClick = { onCreate(email, password); onDismiss() }) {
                Text("Создать")
            }
        },
        dismissButton = {
            TextButton(onClick = onDismiss) {
                Text("Отмена")
            }
        }
    )
}
