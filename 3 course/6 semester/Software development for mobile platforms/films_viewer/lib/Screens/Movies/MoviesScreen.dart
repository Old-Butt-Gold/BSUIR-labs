import 'dart:async';
import 'package:films_viewer/Services/MovieService.dart';
import 'package:flutter/material.dart';
import '../../Models/Movie.dart';
import 'MovieCard.dart';

class MoviesScreen extends StatefulWidget {
  final ScrollController scrollController;
  final String userId;
  final MovieService movieService;

  const MoviesScreen({
    super.key,
    required this.scrollController,
    required this.userId,
    required this.movieService,
  });

  @override
  State<MoviesScreen> createState() => _MoviesScreenState();
}

class _MoviesScreenState extends State<MoviesScreen> {
  late StreamSubscription<List<Movie>> _subscription;
  List<Movie> _allMovies = [];
  List<Movie> _filteredMovies = [];
  List<String> _genres = [];
  String _searchQuery = "";
  String? _selectedGenre;

  @override
  void initState() {
    super.initState();
    _subscription = widget.movieService.getMoviesStream().listen((movies) {
      setState(() {
        _allMovies = movies;
        _applyFilters();

        _genres = _allMovies
            .expand((movie) => movie.genres)
            .toSet()
            .toList()
          ..sort();
      });
    });
  }

  @override
  void dispose() {
    _subscription.cancel();
    super.dispose();
  }

  void _applyFilters() {
    _filteredMovies = _allMovies.where((movie) {
      return movie.title.toLowerCase().contains(_searchQuery.toLowerCase())
          && (_selectedGenre == null || movie.genres.contains(_selectedGenre));
    }).toList();
  }

  void _onSearchChanged(String query) {
    setState(() {
      _searchQuery = query;
      _applyFilters();
    });
  }

  void _onGenreSelected(String? genre) {
    setState(() {
      _selectedGenre = genre;
      _applyFilters();
    });
  }

  Future<void> _toggleFavorite(String movieId) async {
    final index = _filteredMovies.indexWhere((m) => m.id == movieId);
    if (index == -1) return;

    final oldMovie = _filteredMovies[index];
    final newFavoritedBy = List<String>.from(oldMovie.favoritedBy);
    final isFavorite = newFavoritedBy.contains(widget.userId);

    setState(() {
      if (isFavorite) {
        newFavoritedBy.remove(widget.userId);
      } else {
        newFavoritedBy.add(widget.userId);
      }
      _filteredMovies[index].favoritedBy = newFavoritedBy;
    });

    try {
      await widget.movieService.toggleFavorite(movieId, widget.userId);
    } catch (e) {
      setState(() => _filteredMovies[index] = oldMovie);
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Ошибка обновления избранного')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return SafeArea(
        child: GestureDetector(
          onTap: () => FocusManager.instance.primaryFocus?.unfocus(),
          child: Column(
            children: [
              Padding(
                padding: const EdgeInsets.only(
                    top: 4, left: 16, right: 16, bottom: 4),
                child: Row(
                  children: [
                    Expanded(
                      child: TextField(
                        decoration: const InputDecoration(
                          labelText: 'Поиск',
                          prefixIcon: Icon(Icons.search),
                          border: OutlineInputBorder(),
                        ),
                        onChanged: _onSearchChanged,
                      ),
                    ),
                    const SizedBox(width: 12),
                    DropdownButton<String?>(
                      value: _selectedGenre,
                      hint: const Text("Все жанры"),
                      onChanged: _onGenreSelected,
                      items: [
                        const DropdownMenuItem(
                            value: null, child: Text("Все жанры")),
                        ..._genres.map((genre) =>
                            DropdownMenuItem(value: genre, child: Text(genre))),
                      ],
                    ),
                  ],
                ),
              ),
              Expanded(
                child: SingleChildScrollView(
                  child: ListView.builder(
                    controller: widget.scrollController,
                    shrinkWrap: true,
                    physics: const NeverScrollableScrollPhysics(),
                    itemCount: _filteredMovies.length,
                    itemBuilder: (context, index) {
                      final movie = _filteredMovies[index];
                      return MovieCard(
                        movie: movie,
                        userId: widget.userId,
                        onFavoritePressed: () => _toggleFavorite(movie.id),
                      );
                    },
                  ),
                ),
              ),
            ],
          ),
        )
    );
  }
}