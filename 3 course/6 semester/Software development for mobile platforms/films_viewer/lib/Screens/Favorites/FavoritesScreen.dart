import 'dart:async';

import 'package:films_viewer/Services/MovieService.dart';
import 'package:flutter/material.dart';

import '../Movies/MovieCard.dart';
import '../../Models/Movie.dart';

class FavoritesScreen extends StatefulWidget {
  final String userId;
  final ScrollController scrollController;
  final MovieService movieService;

  const FavoritesScreen({
    super.key,
    required this.userId,
    required this.scrollController,
    required this.movieService,
  });

  @override
  State<FavoritesScreen> createState() => _FavoritesScreenState();
}

class _FavoritesScreenState extends State<FavoritesScreen> {
  late StreamSubscription<List<Movie>> _subscription;
  List<Movie> _movies = [];

  @override
  void initState() {
    super.initState();
    _subscription = widget.movieService
        .getFavoriteMovies(widget.userId)
        .listen((movies) => setState(() => _movies = movies));
  }

  @override
  void dispose() {
    _subscription.cancel();
    super.dispose();
  }

  Future<void> _toggleFavorite(String movieId) async {
    final index = _movies.indexWhere((m) => m.id == movieId);
    if (index == -1) return;

    final oldMovie = _movies[index];
    setState(() => _movies.removeAt(index));

    try {
      await widget.movieService.toggleFavorite(movieId, widget.userId);
    } catch (e) {
      setState(() => _movies.insert(index, oldMovie));
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Ошибка обновления избранного')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_movies.isEmpty) {
      return const Center(child: Text('Нет избранных фильмов'));
    }

    return ListView.builder(
      controller: widget.scrollController,
      itemCount: _movies.length,
      itemBuilder: (context, index) {
        final movie = _movies[index];
        return MovieCard(
          movie: movie,
          userId: widget.userId,
          onFavoritePressed: () => _toggleFavorite(movie.id),
        );
      },
    );
  }
}
