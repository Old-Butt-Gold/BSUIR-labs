import 'package:cached_network_image/cached_network_image.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:flutter/material.dart';

import '../../Models/Movie.dart';
import 'DetailRow.dart';

class MovieDetailScreen extends StatefulWidget {
  final Movie movie;
  final String userId;
  final Function onFavoritePressed;

  const MovieDetailScreen({
    super.key,
    required this.movie,
    required this.userId,
    required this.onFavoritePressed,
  });

  @override
  _MovieDetailScreenState createState() => _MovieDetailScreenState();
}

class _MovieDetailScreenState extends State<MovieDetailScreen> {
  late bool isFavorite;
  int _currentIndex = 0;

  @override
  void initState() {
    super.initState();
    isFavorite = widget.movie.favoritedBy.contains(widget.userId);
  }

  void _toggleFavorite() async {
    setState(() {
      isFavorite = !isFavorite;
    });
    await widget.onFavoritePressed();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
          title: FittedBox(
            fit: BoxFit.cover,
            child: Text(widget.movie.title),
          ),
        actions: [
          IconButton(
            icon: Icon(isFavorite ? Icons.favorite : Icons.favorite_border),
            color: Colors.red,
            onPressed: _toggleFavorite,
          ),
        ],
      ),
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              CarouselSlider(
                items: widget.movie.imageUrls.map((url) {
                  return CachedNetworkImage(
                    imageUrl: url,
                    height: double.infinity,
                    fit: BoxFit.cover,
                    width: double.infinity,
                    placeholder: (context, url) => Container(color: Colors.grey[300]),
                    errorWidget: (context, url, error) => const Icon(Icons.error),
                  );
                }).toList(),
                options: CarouselOptions(
                  height: 500,
                  autoPlay: true,
                  viewportFraction: 1.0,
                  onPageChanged: (index, reason) {
                    setState(() {
                      _currentIndex = index;
                    });
                  },
                ),
              ),
              const SizedBox(height: 8),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: widget.movie.imageUrls.asMap().entries.map((entry) {
                  return GestureDetector(
                    onTap: () {
                      setState(() {
                        _currentIndex = entry.key;
                      });
                    },
                    child: Container(
                      margin: const EdgeInsets.symmetric(horizontal: 6),
                      width: _currentIndex == entry.key ? 12 : 8,
                      height: _currentIndex == entry.key ? 12 : 8,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        color: _currentIndex == entry.key ? Colors.red : Colors.grey,
                        boxShadow: [
                          if (_currentIndex == entry.key)
                            const BoxShadow(
                              color: Colors.black26,
                              blurRadius: 4,
                              spreadRadius: 2,
                            ),
                        ],
                      ),
                    ),
                  );
                }).toList(),
              ),
              const SizedBox(height: 20),
              Wrap(
                spacing: 8,
                children: widget.movie.genres.map((genre) {
                  return Chip(
                    label: Text(genre, style: const TextStyle(color: Colors.white, fontWeight: FontWeight.bold)),
                    backgroundColor: Colors.red.shade500,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(10),
                      side: BorderSide(color: Colors.red.shade200),
                    ),
                  );
                }).toList(),
              ),
              const SizedBox(height: 20),
              DetailRow(title: 'Режиссер', value: widget.movie.director),
              DetailRow(title: 'Год выпуска', value: widget.movie.year.toString()),
              DetailRow(title: 'Продолжительность', value: '${widget.movie.duration} мин'),
              const SizedBox(height: 20),
              Text('Описание', style: Theme.of(context).textTheme.headlineMedium),
              const SizedBox(height: 8),
              Text(widget.movie.description, style: Theme.of(context).textTheme.bodyMedium),
            ],
          ),
        ),
      ),
    );
  }
}
