import UIKit
import FirebaseFirestore

class MoviesViewController: UIViewController, UITableViewDataSource, UITableViewDelegate, UISearchBarDelegate, UIPickerViewDataSource, UIPickerViewDelegate, UITextFieldDelegate {
    
    private let movieService = MovieService()
    private let userId: String
    private var listener: ListenerRegistration?
    
    private var allMovies: [Movie] = []
    private var filteredMovies: [Movie] = []
    private var genres: [String] = []
    private var searchQuery: String = ""
    private var selectedGenre: String? = ""
    
    private let searchBar = UISearchBar()
    private let genreTextField = UITextField()
    private let genrePicker = UIPickerView()
    private let tableView = UITableView()
    
    init(userId: String) {
        self.userId = userId
        super.init(nibName: nil, bundle: nil)
        self.title = "Фильмы"
    }
    
    required init?(coder: NSCoder) {
        fatalError("init(coder:) not implemented")
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        navigationItem.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: nil, action: nil)

        setupViews()
        setupGenrePicker()
        subscribeToMovies()
    }
    
    deinit {
        listener?.remove()
    }
    
    private func setupViews() {
        searchBar.delegate = self
        searchBar.placeholder = "Поиск"
        searchBar.translatesAutoresizingMaskIntoConstraints = false
        searchBar.backgroundImage = UIImage()
        searchBar.searchTextField.backgroundColor = .systemGray6
        
        genreTextField.placeholder = "Все жанры"
        genreTextField.text = "Все жанры"
        genreTextField.borderStyle = .roundedRect
        genreTextField.translatesAutoresizingMaskIntoConstraints = false
        genreTextField.textAlignment = .center
        
        let headerStack = UIStackView()
        headerStack.axis = .horizontal
        headerStack.spacing = 12
        headerStack.alignment = .center
        headerStack.translatesAutoresizingMaskIntoConstraints = false
        
        headerStack.addArrangedSubview(searchBar)
        headerStack.addArrangedSubview(genreTextField)
        
        tableView.register(MovieTableViewCell.self, forCellReuseIdentifier: MovieTableViewCell.identifier)
        tableView.dataSource = self
        tableView.delegate = self
        tableView.translatesAutoresizingMaskIntoConstraints = false
        
        view.addSubview(headerStack)
        view.addSubview(tableView)
        
        NSLayoutConstraint.activate([
            headerStack.topAnchor.constraint(equalTo: view.layoutMarginsGuide.topAnchor, constant: 8),
            headerStack.leadingAnchor.constraint(equalTo: view.leadingAnchor, constant: 16),
            headerStack.trailingAnchor.constraint(equalTo: view.trailingAnchor, constant: -16),
            
            genreTextField.widthAnchor.constraint(equalToConstant: 150),
            genreTextField.heightAnchor.constraint(equalToConstant: 40),
            
            tableView.topAnchor.constraint(equalTo: headerStack.bottomAnchor, constant: 12),
            tableView.leadingAnchor.constraint(equalTo: view.leadingAnchor),
            tableView.trailingAnchor.constraint(equalTo: view.trailingAnchor),
            tableView.bottomAnchor.constraint(equalTo: view.safeAreaLayoutGuide.bottomAnchor)
        ])
        
        searchBar.setContentHuggingPriority(.defaultLow, for: .horizontal)
        genreTextField.setContentHuggingPriority(.required, for: .horizontal)
    }
    
    private func setupGenrePicker() {
        genrePicker.dataSource = self
        genrePicker.delegate = self
        genreTextField.inputView = genrePicker
        genreTextField.delegate = self
        genreTextField.tintColor = .clear
        
        let toolbar = UIToolbar()
        toolbar.sizeToFit()
        let doneButton = UIBarButtonItem(barButtonSystemItem: .done, target: self, action: #selector(donePicker))
        toolbar.setItems([doneButton], animated: false)
        genreTextField.inputAccessoryView = toolbar
    }
    
    // MARK: - UITextFieldDelegate
    func textField(_ textField: UITextField, shouldChangeCharactersIn range: NSRange, replacementString string: String) -> Bool {
        return false // Блокируем ручной ввод текста
    }
        
    func textFieldShouldBeginEditing(_ textField: UITextField) -> Bool {
        return true // Разрешаем показ пикера при тапе
    }
    
    @objc private func donePicker() {
        view.endEditing(true)
    }
    
    private func subscribeToMovies() {
        listener = movieService.getMoviesStream { [weak self] movies in
            guard let self = self else { return }
            self.allMovies = movies
            self.applyFilters()
            let genresSet = Set(movies.flatMap { $0.genres })
            self.genres = Array(genresSet).sorted()
            DispatchQueue.main.async {
                self.genrePicker.reloadAllComponents()
                self.tableView.reloadData()
            }
        }
    }
    
    private func applyFilters() {
        filteredMovies = allMovies.filter { movie in
            let matchesSearch = searchQuery.isEmpty ? true : movie.title.lowercased().contains(searchQuery.lowercased())
            let matchesGenre = (selectedGenre == nil || selectedGenre!.isEmpty) || movie.genres.contains(selectedGenre!)
            return matchesSearch && matchesGenre
        }
    }
    
    // MARK: - UISearchBarDelegate
    func searchBar(_ searchBar: UISearchBar, textDidChange searchText: String) {
        searchQuery = searchText
        applyFilters()
        tableView.reloadData()
    }
    
    // MARK: - UITableViewDataSource & Delegate
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return filteredMovies.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        guard let cell = tableView.dequeueReusableCell(withIdentifier: MovieTableViewCell.identifier, for: indexPath) as? MovieTableViewCell else {
            return UITableViewCell()
        }
        let movie = filteredMovies[indexPath.row]
        cell.configure(with: movie, userId: userId)
        cell.favoriteButtonAction = { [weak self] in
            self?.toggleFavorite(movieId: movie.id, at: indexPath)
        }
        return cell
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        tableView.deselectRow(at: indexPath, animated: true)
        let movie = filteredMovies[indexPath.row]
        let detailVC = MovieDetailViewController(movie: movie, userId: userId)
        
        navigationController?.pushViewController(detailVC, animated: true)
    }
    
    private func toggleFavorite(movieId: String, at indexPath: IndexPath) {
        let movie = filteredMovies[indexPath.row]
        var updatedMovie = movie
        if updatedMovie.favoritedBy.contains(userId) {
            updatedMovie.favoritedBy.removeAll(where: { $0 == userId })
        } else {
            updatedMovie.favoritedBy.append(userId)
        }
        filteredMovies[indexPath.row] = updatedMovie
        tableView.reloadRows(at: [indexPath], with: .automatic)
        movieService.toggleFavorite(movieId: movieId, userId: userId) { error in
            if let error = error {
                DispatchQueue.main.async {
                    self.filteredMovies[indexPath.row] = movie
                    self.tableView.reloadRows(at: [indexPath], with: .automatic)
                    self.showAlert(message: "Ошибка обновления избранного: \(error.localizedDescription)")
                }
            }
        }
    }
    
    // MARK: - UIPickerViewDataSource & Delegate
    func numberOfComponents(in pickerView: UIPickerView) -> Int { 1 }
    
    func pickerView(_ pickerView: UIPickerView, numberOfRowsInComponent component: Int) -> Int {
        return genres.count + 1
    }
    
    func pickerView(_ pickerView: UIPickerView, titleForRow row: Int, forComponent component: Int) -> String? {
        return row == 0 ? "Все жанры" : genres[row - 1]
    }
    
    func pickerView(_ pickerView: UIPickerView, didSelectRow row: Int, inComponent component: Int) {
        if row == 0 {
            selectedGenre = nil
            genreTextField.text = "Все жанры"
        } else {
            selectedGenre = genres[row - 1]
            genreTextField.text = selectedGenre
        }
        applyFilters()
        tableView.reloadData()
    }
    
    private func showAlert(message: String) {
         let alert = UIAlertController(title: "Ошибка", message: message, preferredStyle: .alert)
         alert.addAction(UIAlertAction(title: "OK", style: .default))
         present(alert, animated: true)
    }
}
