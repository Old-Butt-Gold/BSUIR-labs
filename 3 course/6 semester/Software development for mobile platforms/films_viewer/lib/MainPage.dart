import 'package:films_viewer/Services/AuthService.dart';
import 'package:flutter/material.dart';

import 'Screens/Favorites/FavoritesScreen.dart';
import 'Screens/Movies/MoviesScreen.dart';
import 'Services/MovieService.dart';
import 'Screens/Profile/ProfileScreen.dart';
import 'Screens/Settings/SettingsScreen.dart';

class MainPage extends StatefulWidget {
  const MainPage({super.key});

  @override
  _MainPageState createState() => _MainPageState();
}

class _MainPageState extends State<MainPage> {
  int _currentIndex = 0;
  bool _isBottomBarVisible = true;

  late final List<ScrollController> _scrollControllers;
  late final MovieService _movieService;
  late final AuthService _authService;

  late final String _userId;
  late final List<Widget> _screens;

  @override
  void initState() {
    super.initState();

    _movieService = MovieService();

    _authService = AuthService();

    _userId = _authService.currentUser!.uid;

    _scrollControllers = List.generate(4, (_) => ScrollController());

    _screens = [
      MoviesScreen(
        scrollController: _scrollControllers[0],
        userId: _userId,
        movieService: _movieService,
      ),
      FavoritesScreen(
        scrollController: _scrollControllers[1],
        userId: _userId,
        movieService: _movieService,
      ),
      ProfileScreen(
        userId: _userId,
        email: _authService.currentUser!.email!,
      ),
      const SettingsScreen(),
    ];
  }

  @override
  void dispose() {
    for (var controller in _scrollControllers) {
      controller.dispose();
    }
    super.dispose();
  }

  bool _notificationListener(ScrollNotification notification) {
    if (notification is ScrollUpdateNotification) {
      if (notification.scrollDelta! > 0 && _isBottomBarVisible) {
        setState(() {
          _isBottomBarVisible = false;
        });
      } else if (notification.scrollDelta! < 0 && !_isBottomBarVisible) {
        setState(() {
          _isBottomBarVisible = true;
        });
      }
    }
    return false;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      // Чтобы получать события нажатия даже по пустым областям,
      // оборачиваем тело в GestureDetector с behavior: opaque
      body: GestureDetector(
        behavior: HitTestBehavior.opaque,
        onTap: () {
          if (!_isBottomBarVisible) {
            setState(() {
              _isBottomBarVisible = true;
            });
          }
        },
        child: NotificationListener<ScrollNotification>(
          onNotification: _notificationListener,
          child: IndexedStack(
            index: _currentIndex,
            children: _screens,
          ),
        ),
      ),
      bottomNavigationBar: AnimatedOpacity(
        curve: Curves.easeInOut,
        opacity: _isBottomBarVisible ? 1.0 : 0.0,
        duration: Duration(milliseconds: 300),
        child: _isBottomBarVisible
            ? BottomNavigationBar(
          currentIndex: _currentIndex,
          onTap: (index) => setState(() => _currentIndex = index),
          items: _bottomBarItems(),
          selectedItemColor: bottomNavItems[_currentIndex]['color'] as Color,
          unselectedItemColor: Colors.grey,
          selectedLabelStyle: TextStyle(fontWeight: FontWeight.bold),
          unselectedLabelStyle: TextStyle(fontWeight: FontWeight.normal),
        )
            : SizedBox.shrink(),
      ),
    );
  }

  final bottomNavItems = [
    {'label': 'Фильмы', 'icon': Icons.movie, 'color': Colors.orange},
    {'label': 'Избранное', 'icon': Icons.favorite, 'color': Colors.redAccent},
    {'label': 'Профиль', 'icon': Icons.person, 'color': Colors.greenAccent},
    {'label': 'Настройки', 'icon': Icons.settings, 'color': Colors.blueAccent},
  ];

  List<BottomNavigationBarItem> _bottomBarItems() {
    return bottomNavItems
        .map((item) =>
        BottomNavigationBarItem(
          label: item['label'] as String,
          icon: Icon(item['icon'] as IconData, color: item['color'] as Color),
        ))
        .toList();
  }
}