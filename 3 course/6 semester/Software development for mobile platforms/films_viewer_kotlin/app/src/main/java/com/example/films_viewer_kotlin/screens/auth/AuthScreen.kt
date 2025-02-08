@file:OptIn(ExperimentalMaterial3Api::class)

package com.example.films_viewer_kotlin.screens.auth

import androidx.compose.ui.Alignment

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Email
import androidx.compose.material.icons.filled.Security
import androidx.compose.material.icons.filled.Visibility
import androidx.compose.material.icons.filled.VisibilityOff
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.*
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavController
import com.example.films_viewer_kotlin.Routes
import com.example.films_viewer_kotlin.services.AuthService
import com.google.firebase.firestore.FirebaseFirestore
import kotlinx.coroutines.launch
import kotlinx.coroutines.tasks.await

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun AuthScreen(navController: NavController) {
    val scope = rememberCoroutineScope()
    var email by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var isLoading by remember { mutableStateOf(false) }
    var isLogin by remember { mutableStateOf(true) }
    var showPassword by remember { mutableStateOf(false) }

    var emailError by remember { mutableStateOf<String?>(null) }
    var passwordError by remember { mutableStateOf<String?>(null) }

    val authService = remember { AuthService() }
    val snackbarHostState = remember { SnackbarHostState() }

    fun validate(): Boolean {
        var valid = true
        if (!email.contains("@")) {
            emailError = "Введите корректный email"
            valid = false
        } else {
            emailError = null
        }
        if (password.length < 6) {
            passwordError = "Пароль должен быть не менее 6 символов"
            valid = false
        } else {
            passwordError = null
        }
        return valid
    }

    Scaffold(
        topBar = {
            TopAppBar(title = { Text(if (isLogin) "Вход" else "Регистрация") })
        },
        snackbarHost = { SnackbarHost(snackbarHostState) }
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingValues)
                .padding(20.dp),
            verticalArrangement = Arrangement.Center
        ) {
            OutlinedTextField(
                value = email,
                onValueChange = {
                    email = it
                    if (emailError != null) emailError = null // Сбрасываем ошибку при вводе
                },
                label = { Text("Email") },
                leadingIcon = { Icon(Icons.Filled.Email, contentDescription = null) },
                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Email),
                modifier = Modifier.fillMaxWidth(),
                isError = emailError != null
            )
            if (emailError != null) {
                Text(
                    text = emailError ?: "",
                    color = MaterialTheme.colorScheme.error,
                    style = MaterialTheme.typography.bodySmall,
                    modifier = Modifier.padding(start = 16.dp)
                )
            }
            Spacer(modifier = Modifier.height(16.dp))
            OutlinedTextField(
                value = password,
                onValueChange = {
                    password = it
                    if (passwordError != null) passwordError = null
                },
                label = { Text("Пароль") },
                leadingIcon = { Icon(Icons.Filled.Security, contentDescription = null) },
                trailingIcon = {
                    IconButton(onClick = { showPassword = !showPassword }) {
                        Icon(
                            imageVector = if (showPassword) Icons.Filled.VisibilityOff else Icons.Filled.Visibility,
                            contentDescription = null
                        )
                    }
                },
                visualTransformation = if (showPassword) VisualTransformation.None else PasswordVisualTransformation(),
                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password),
                modifier = Modifier.fillMaxWidth(),
                isError = passwordError != null
            )
            if (passwordError != null) {
                Text(
                    text = passwordError ?: "",
                    color = MaterialTheme.colorScheme.error,
                    style = MaterialTheme.typography.bodySmall,
                    modifier = Modifier.padding(start = 16.dp)
                )
            }
            Spacer(modifier = Modifier.height(20.dp))
            if (isLoading) {
                CircularProgressIndicator(modifier = Modifier.align(Alignment.CenterHorizontally))
            } else {
                Button(
                    onClick = {
                        scope.launch {
                            if (!validate()) return@launch
                            isLoading = true
                            try {
                                if (isLogin) {
                                    val user = authService.signIn(email.trim(), password.trim())
                                    if (user != null) {
                                        if (!user.isEmailVerified) {
                                            navController.popBackStack()
                                            navController.navigate(Routes.VerifyEmailScreen.route)
                                        } else {
                                            FirebaseFirestore.getInstance().collection("users")
                                                .document(user.uid)
                                                .update("emailVerified", true)
                                                .await()
                                            navController.popBackStack()
                                            navController.navigate(Routes.Home.route)
                                        }
                                    }
                                } else {
                                    authService.signUp(email.trim(), password.trim())
                                    navController.popBackStack()
                                    navController.navigate(Routes.VerifyEmailScreen.route)
                                }
                            } catch (e: Exception) {
                                snackbarHostState.showSnackbar(e.toString())
                            } finally {
                                isLoading = false
                            }
                        }
                    },
                    modifier = Modifier.fillMaxWidth()
                ) {
                    Text(if (isLogin) "Войти" else "Зарегистрироваться")
                }
                TextButton(
                    onClick = { isLogin = !isLogin },
                    modifier = Modifier.fillMaxWidth()
                ) {
                    Text(
                        text = if (isLogin) "Создать аккаунт" else "Уже есть аккаунт? Войти",
                        fontSize = 14.sp
                    )
                }
            }
        }
    }
}