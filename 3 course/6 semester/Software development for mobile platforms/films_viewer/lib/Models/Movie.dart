class Movie {
  final String id;
  String title;
  String description;
  int year;
  int duration;
  List<String> genres;
  String director;
  List<String> imageUrls;
  String posterUrl;
  List<String> favoritedBy;

  Movie({
    required this.id,
    required this.title,
    required this.description,
    required this.year,
    required this.duration,
    required this.genres,
    required this.director,
    required this.imageUrls,
    required this.posterUrl,
    required this.favoritedBy,
  });

  factory Movie.fromFirestore(Map<String, dynamic> data, String id) {
    return Movie(
      id: id,
      title: data['title'] ?? '',
      description: data['description'] ?? '',
      year: data['year'] ?? 0,
      duration: data['duration'] ?? 0,
      genres: List<String>.from(data['genres'] ?? []),
      director: data['director'] ?? '',
      imageUrls: List<String>.from(data['imageUrls'] ?? []),
      posterUrl: data['posterUrl'] ?? '',
      favoritedBy: List<String>.from(data['favoritedBy'] ?? []),
    );
  }

  Map<String, dynamic> toFirestore() {
    return {
      'title': title,
      'description': description,
      'year': year,
      'duration': duration,
      'genres': genres,
      'director': director,
      'imageUrls': imageUrls,
      'posterUrl': posterUrl,
      'favoritedBy': favoritedBy,
    };
  }

}