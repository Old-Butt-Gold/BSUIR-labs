package com.example.films_viewer_kotlin.models

data class Movie(
    val id: String,
    var title: String,
    var description: String,
    var year: Int,
    var duration: Int,
    var genres: List<String>,
    var director: String,
    var imageUrls: List<String>,
    var posterUrl: String,
    var favoritedBy: List<String>
) {
    fun toFirestore(): Map<String, Any> {
        return mapOf(
            "title" to title,
            "description" to description,
            "year" to year,
            "duration" to duration,
            "genres" to genres,
            "director" to director,
            "imageUrls" to imageUrls,
            "posterUrl" to posterUrl,
            "favoritedBy" to favoritedBy
        )
    }

    companion object {
        fun fromFirestore(data: Map<String, Any?>, id: String): Movie {
            return Movie(
                id = id,
                title = data["title"] as? String ?: "",
                description = data["description"] as? String ?: "",
                year = (data["year"] as? Number)?.toInt() ?: 0,
                duration = (data["duration"] as? Number)?.toInt() ?: 0,
                genres = (data["genres"] as? List<*>)?.mapNotNull { it as? String } ?: emptyList(),
                director = data["director"] as? String ?: "",
                imageUrls = (data["imageUrls"] as? List<*>)?.mapNotNull { it as? String } ?: emptyList(),
                posterUrl = data["posterUrl"] as? String ?: "",
                favoritedBy = (data["favoritedBy"] as? List<*>)?.mapNotNull { it as? String } ?: emptyList()
            )
        }
    }
}
