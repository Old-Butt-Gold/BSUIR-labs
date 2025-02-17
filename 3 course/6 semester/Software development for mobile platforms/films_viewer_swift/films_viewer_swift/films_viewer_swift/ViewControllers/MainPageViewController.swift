import UIKit

class MainPageViewController: UITabBarController {
    
    override func viewDidLoad() {
        super.viewDidLoad()

        let moviesVC = MoviesViewController(userId: AuthService().currentUser?.uid ?? "")
        moviesVC.tabBarItem = UITabBarItem(title: "Фильмы",
                                            image: UIImage(systemName: "film"),
                                            selectedImage: UIImage(systemName:"film")?.withTintColor(.orange, renderingMode:.alwaysOriginal))
        moviesVC.tabBarItem.setTitleTextAttributes([.foregroundColor: UIColor.orange], for: .selected)

        let favoritesVC = FavoritesViewController(userId: AuthService().currentUser?.uid ?? "")
        favoritesVC.tabBarItem = UITabBarItem(title: "Избранное",
                                                image: UIImage(systemName: "heart"),
                                                selectedImage: UIImage(systemName:"heart")?.withTintColor(.red, renderingMode:.alwaysOriginal))
        favoritesVC.tabBarItem.setTitleTextAttributes([.foregroundColor: UIColor.red], for: .selected)

        let authService = AuthService()
        let userId = authService.currentUser?.uid ?? ""
        let email = authService.currentUser?.email ?? ""
        let profileVC = ProfileViewController(userId: userId, email: email)
        
        profileVC.tabBarItem = UITabBarItem(title: "Профиль",
                                            image: UIImage(systemName: "person"),
                                            selectedImage: UIImage(systemName:"person")?.withTintColor(.green, renderingMode:.alwaysOriginal))
        profileVC.tabBarItem.setTitleTextAttributes([.foregroundColor: UIColor.green], for: .selected)
        
        let settingsVC = SettingsViewController()
        settingsVC.tabBarItem = UITabBarItem(title: "Настройки",
                                                image: UIImage(systemName: "gear"),
                                                selectedImage: UIImage(systemName:"gear")?.withTintColor(.blue, renderingMode:.alwaysOriginal))
        settingsVC.tabBarItem.setTitleTextAttributes([.foregroundColor: UIColor.blue], for: .selected)
        и
        viewControllers = [moviesVC, favoritesVC, profileVC, settingsVC].map {UINavigationController(rootViewController: $0) }
        
        tabBar.unselectedItemTintColor = .lightGray
        
    }
}
