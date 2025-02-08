package com.example.films_viewer_kotlin

sealed class Routes(val route: String) {
    object Home : Routes("/home")
    object AuthScreen : Routes("/authScreen")
    object AuthWrapperScreen : Routes("/authWrapperScreen")
    object VerifyEmailScreen : Routes("/verifyEmailScreen")
    object MovieDetail : Routes("/movieDetail")
}