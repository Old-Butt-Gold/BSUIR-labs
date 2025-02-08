package com.example.films_viewer_kotlin.models

data class UserProfile(
    val id: String,
    val email: String,
    var firstName: String,
    var lastName: String,
    var birthDate: String,
    var phone: String,
    var address: String,
    var country: String,
    var city: String,
    var bio: String,
    var gender: String
) {

    fun toFirestore(): Map<String, Any> {
        return mapOf(
            "email" to email,
            "firstName" to firstName,
            "lastName" to lastName,
            "birthDate" to birthDate,
            "phone" to phone,
            "address" to address,
            "country" to country,
            "city" to city,
            "bio" to bio,
            "gender" to gender
        )
    }

    companion object {
        fun empty(userId: String, email: String = ""): UserProfile {
            return UserProfile(
                id = userId,
                email = email,
                firstName = "",
                lastName = "",
                birthDate = "",
                phone = "",
                address = "",
                country = "",
                city = "",
                bio = "",
                gender = "Не указан"
            )
        }

        fun fromFirestore(data: Map<String, Any?>, id: String): UserProfile {
            return UserProfile(
                id = id,
                email = data["email"] as? String ?: "",
                firstName = data["firstName"] as? String ?: "",
                lastName = data["lastName"] as? String ?: "",
                birthDate = data["birthDate"] as? String ?: "",
                phone = data["phone"] as? String ?: "",
                address = data["address"] as? String ?: "",
                country = data["country"] as? String ?: "",
                city = data["city"] as? String ?: "",
                bio = data["bio"] as? String ?: "",
                gender = data["gender"] as? String ?: "Не указан"
            )
        }
    }
}
