import UIKit
import FirebaseFirestore

class FavoritesViewController: UIViewController, UITableViewDataSource, UITableViewDelegate {
    
    private let movieService = MovieService()
    private let userId: String
    private var listener: ListenerRegistration?
    private var movies: [Movie] = []
    
    private let tableView = UITableView()
    
    init(userId: String) {
         self.userId = userId
         super.init(nibName: nil, bundle: nil)
         self.title = "Избранное"
    }
    
    required init?(coder: NSCoder) {
         fatalError("init(coder:) not implemented")
    }
    
    override func viewDidLoad() {
         super.viewDidLoad()
        
        navigationItem.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: nil, action: nil)
        
         setupTableView()
         subscribeToFavorites()
    }
    
    deinit {
         listener?.remove()
    }
    
    private func setupTableView() {
         tableView.translatesAutoresizingMaskIntoConstraints = false
         tableView.register(MovieTableViewCell.self, forCellReuseIdentifier: MovieTableViewCell.identifier)
         tableView.dataSource = self
         tableView.delegate = self
         view.addSubview(tableView)
         NSLayoutConstraint.activate([
             tableView.topAnchor.constraint(equalTo: view.topAnchor),
             tableView.leadingAnchor.constraint(equalTo: view.leadingAnchor),
             tableView.trailingAnchor.constraint(equalTo: view.trailingAnchor),
             tableView.bottomAnchor.constraint(equalTo: view.bottomAnchor)
         ])
    }
    
    private func subscribeToFavorites() {
         listener = movieService.getFavoriteMovies(userId: userId) { [weak self] movies in
             self?.movies = movies
             DispatchQueue.main.async {
                  self?.tableView.reloadData()
             }
         }
    }
    
    // MARK: - UITableViewDataSource & Delegate
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
         if movies.isEmpty {
             let label = UILabel(frame: tableView.bounds)
             label.text = "Нет избранных фильмов"
             label.textAlignment = .center
             tableView.backgroundView = label
         } else {
             tableView.backgroundView = nil
         }
         return movies.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
         guard let cell = tableView.dequeueReusableCell(withIdentifier: MovieTableViewCell.identifier, for: indexPath) as? MovieTableViewCell else {
             return UITableViewCell()
         }
         let movie = movies[indexPath.row]
         cell.configure(with: movie, userId: userId)
         cell.favoriteButtonAction = { [weak self] in
             self?.toggleFavorite(movieId: movie.id, at: indexPath)
         }
         return cell
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
         tableView.deselectRow(at: indexPath, animated: true)
         let movie = movies[indexPath.row]
         let detailVC = MovieDetailViewController(movie: movie, userId: userId)
         navigationController?.pushViewController(detailVC, animated: true)
    }
    
    private func toggleFavorite(movieId: String, at indexPath: IndexPath) {
         let movie = movies[indexPath.row]
         let oldMovie = movie
         movies.remove(at: indexPath.row)
         tableView.deleteRows(at: [indexPath], with: .automatic)
         movieService.toggleFavorite(movieId: movieId, userId: userId) { error in
             if let error = error {
                 DispatchQueue.main.async {
                     self.movies.insert(oldMovie, at: indexPath.row)
                     self.tableView.insertRows(at: [indexPath], with: .automatic)
                     self.showAlert(message: "Ошибка обновления избранного: \(error.localizedDescription)")
                 }
             }
         }
    }
    
    private func showAlert(message: String) {
         let alert = UIAlertController(title: "Ошибка", message: message, preferredStyle: .alert)
         alert.addAction(UIAlertAction(title: "OK", style: .default))
         present(alert, animated: true)
    }
}
