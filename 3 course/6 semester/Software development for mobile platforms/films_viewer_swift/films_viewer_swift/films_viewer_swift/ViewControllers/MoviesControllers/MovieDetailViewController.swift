import UIKit

class MovieDetailViewController: UIViewController {
    private let movie: Movie
    private let userId: String
    private var isFavorite: Bool
    private let movieService = MovieService()
    
    private let scrollView = UIScrollView()
    private let contentStackView = UIStackView()
    
    private let sliderScrollView = UIScrollView()
    private let pageControl = UIPageControl()
    
    init(movie: Movie, userId: String) {
        self.movie = movie
        self.userId = userId
        self.isFavorite = movie.favoritedBy.contains(userId)
        super.init(nibName: nil, bundle: nil)
        self.hidesBottomBarWhenPushed = true
    }
    
    required init?(coder: NSCoder) {
        fatalError("init(coder:) not implemented")
    }
    
    override func viewDidLoad() {
         super.viewDidLoad()
         setupNavigationBar()
         setupViews()
    }
    
    private func setupNavigationBar() {
         let titleLabel = UILabel()
         titleLabel.text = movie.title
         titleLabel.font = UIFont.boldSystemFont(ofSize: 17)
         titleLabel.textColor = .label
         titleLabel.textAlignment = .center
         titleLabel.adjustsFontSizeToFitWidth = true
         titleLabel.minimumScaleFactor = 0.5
         titleLabel.numberOfLines = 1
            
         navigationItem.titleView = titleLabel
        
         let favoriteItem = UIBarButtonItem(image: UIImage(systemName: isFavorite ? "heart.fill" : "heart"),
                                              style: .plain,
                                              target: self,
                                              action: #selector(toggleFavorite))
         favoriteItem.tintColor = .red
         navigationItem.rightBarButtonItem = favoriteItem
    }
    
    private func setupViews() {
         scrollView.translatesAutoresizingMaskIntoConstraints = false
         view.addSubview(scrollView)
         NSLayoutConstraint.activate([
              scrollView.topAnchor.constraint(equalTo: view.topAnchor),
              scrollView.leadingAnchor.constraint(equalTo: view.leadingAnchor),
              scrollView.trailingAnchor.constraint(equalTo: view.trailingAnchor),
              scrollView.bottomAnchor.constraint(equalTo: view.bottomAnchor)
         ])
         
         contentStackView.axis = .vertical
         contentStackView.spacing = 16
         contentStackView.translatesAutoresizingMaskIntoConstraints = false
         scrollView.addSubview(contentStackView)
         NSLayoutConstraint.activate([
              contentStackView.topAnchor.constraint(equalTo: scrollView.topAnchor, constant: 16),
              contentStackView.leadingAnchor.constraint(equalTo: view.leadingAnchor, constant: 16),
              contentStackView.trailingAnchor.constraint(equalTo: view.trailingAnchor, constant: -16),
              contentStackView.bottomAnchor.constraint(equalTo: scrollView.bottomAnchor, constant: -16)
         ])
         
         setupSlider()
         

         pageControl.numberOfPages = movie.imageUrls.count
         pageControl.currentPage = 0
         pageControl.currentPageIndicatorTintColor = .red
         pageControl.pageIndicatorTintColor = .lightGray
         pageControl.translatesAutoresizingMaskIntoConstraints = false
         contentStackView.addArrangedSubview(pageControl)
         
         let genresStack = UIStackView()
         genresStack.axis = .horizontal
         genresStack.spacing = 8
         genresStack.alignment = .center
         for genre in movie.genres {
             let chip = createChip(with: genre)
             genresStack.addArrangedSubview(chip)
         }
         contentStackView.addArrangedSubview(genresStack)
         
         contentStackView.addArrangedSubview(createDetailRow(title: "Режиссер", value: movie.director))
         contentStackView.addArrangedSubview(createDetailRow(title: "Год выпуска", value: "\(movie.year)"))
         contentStackView.addArrangedSubview(createDetailRow(title: "Продолжительность", value: "\(movie.duration) мин"))
         
         let descriptionTitle = UILabel()
         descriptionTitle.text = "Описание"
         descriptionTitle.font = UIFont.systemFont(ofSize: 20, weight: .bold)
         contentStackView.addArrangedSubview(descriptionTitle)
         
         let descriptionLabel = UILabel()
         descriptionLabel.text = movie.description
         descriptionLabel.numberOfLines = 0
         descriptionLabel.lineBreakMode = .byWordWrapping
         descriptionLabel.font = UIFont.systemFont(ofSize: 16)
         contentStackView.addArrangedSubview(descriptionLabel)
    }
    
    private func setupSlider() {
         sliderScrollView.isPagingEnabled = true
         sliderScrollView.showsHorizontalScrollIndicator = false
         sliderScrollView.delegate = self
         sliderScrollView.translatesAutoresizingMaskIntoConstraints = false
         contentStackView.addArrangedSubview(sliderScrollView)
         NSLayoutConstraint.activate([
              sliderScrollView.heightAnchor.constraint(equalToConstant: 500)
         ])
         
         let sliderContentView = UIView()
         sliderContentView.translatesAutoresizingMaskIntoConstraints = false
         sliderScrollView.addSubview(sliderContentView)
         NSLayoutConstraint.activate([
              sliderContentView.topAnchor.constraint(equalTo: sliderScrollView.topAnchor),
              sliderContentView.bottomAnchor.constraint(equalTo: sliderScrollView.bottomAnchor),
              sliderContentView.leadingAnchor.constraint(equalTo: sliderScrollView.leadingAnchor),
              sliderContentView.trailingAnchor.constraint(equalTo: sliderScrollView.trailingAnchor),
              sliderContentView.heightAnchor.constraint(equalTo: sliderScrollView.heightAnchor)
         ])
         
         var previousImageView: UIImageView? = nil
         for urlString in movie.imageUrls {
             let imageView = UIImageView()
             imageView.contentMode = .scaleAspectFill
             imageView.clipsToBounds = true
             imageView.translatesAutoresizingMaskIntoConstraints = false
             sliderContentView.addSubview(imageView)
             
             if let url = URL(string: urlString) {
                 URLSession.shared.dataTask(with: url) { data, _, _ in
                     if let data = data, let image = UIImage(data: data) {
                         DispatchQueue.main.async {
                             imageView.image = image
                         }
                     }
                 }.resume()
             }
             
             NSLayoutConstraint.activate([
                 imageView.topAnchor.constraint(equalTo: sliderContentView.topAnchor),
                 imageView.bottomAnchor.constraint(equalTo: sliderContentView.bottomAnchor),
                 imageView.widthAnchor.constraint(equalTo: sliderScrollView.widthAnchor)
             ])
             
             if let previous = previousImageView {
                 imageView.leadingAnchor.constraint(equalTo: previous.trailingAnchor).isActive = true
             } else {
                 imageView.leadingAnchor.constraint(equalTo: sliderContentView.leadingAnchor).isActive = true
             }
             previousImageView = imageView
         }
         if let lastImageView = previousImageView {
              lastImageView.trailingAnchor.constraint(equalTo: sliderContentView.trailingAnchor).isActive = true
         }
    }
    
    private func createChip(with text: String) -> UIView {
        let label = UILabel()
        label.text = text
        label.textColor = .white
        label.font = UIFont.systemFont(ofSize: 14, weight: .bold)
        label.textAlignment = .center
        label.translatesAutoresizingMaskIntoConstraints = false
        
        let chip = UIView()
        chip.backgroundColor = UIColor.red.withAlphaComponent(0.8)
        chip.layer.cornerRadius = 10
        chip.layer.borderWidth = 1
        chip.layer.borderColor = UIColor.red.withAlphaComponent(0.5).cgColor
        chip.translatesAutoresizingMaskIntoConstraints = false
        chip.addSubview(label)
        
        NSLayoutConstraint.activate([
            label.topAnchor.constraint(equalTo: chip.topAnchor, constant: 4),
            label.bottomAnchor.constraint(equalTo: chip.bottomAnchor, constant: -4),
            label.leadingAnchor.constraint(equalTo: chip.leadingAnchor, constant: 8),
            label.trailingAnchor.constraint(equalTo: chip.trailingAnchor, constant: -8)
        ])
        
        return chip
    }
    
    private func createDetailRow(title: String, value: String) -> UIView {
        let container = UIView()
        container.translatesAutoresizingMaskIntoConstraints = false

        let titleLabel = UILabel()
        titleLabel.text = "\(title):"
        titleLabel.font = UIFont.systemFont(ofSize: 16, weight: .bold)
        titleLabel.numberOfLines = 0
        titleLabel.translatesAutoresizingMaskIntoConstraints = false

        let valueLabel = UILabel()
        valueLabel.text = value
        valueLabel.font = UIFont.systemFont(ofSize: 16)
        valueLabel.numberOfLines = 0
        valueLabel.translatesAutoresizingMaskIntoConstraints = false

        container.addSubview(titleLabel)
        container.addSubview(valueLabel)

        NSLayoutConstraint.activate([
            titleLabel.topAnchor.constraint(equalTo: container.topAnchor),
            titleLabel.leadingAnchor.constraint(equalTo: container.leadingAnchor),
            titleLabel.bottomAnchor.constraint(lessThanOrEqualTo: container.bottomAnchor),
            
            valueLabel.topAnchor.constraint(equalTo: container.topAnchor),
            valueLabel.leadingAnchor.constraint(equalTo: titleLabel.trailingAnchor, constant: 8),
            valueLabel.trailingAnchor.constraint(equalTo: container.trailingAnchor),
            valueLabel.bottomAnchor.constraint(equalTo: container.bottomAnchor)
        ])
        
        titleLabel.setContentHuggingPriority(.defaultHigh, for: .horizontal)
        valueLabel.setContentHuggingPriority(.defaultLow, for: .horizontal)

        return container
    }

    
    @objc private func toggleFavorite() {
         isFavorite.toggle()
         navigationItem.rightBarButtonItem?.image = UIImage(systemName: isFavorite ? "heart.fill" : "heart")
         movieService.toggleFavorite(movieId: movie.id, userId: userId) { error in
              DispatchQueue.main.async {
                   if let error = error {
                        self.showAlert(message: "Ошибка обновления избранного: \(error.localizedDescription)")
                        self.isFavorite.toggle()
                        self.navigationItem.rightBarButtonItem?.image = UIImage(systemName: self.isFavorite ? "heart.fill" : "heart")
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

extension MovieDetailViewController: UIScrollViewDelegate {
    func scrollViewDidScroll(_ scrollView: UIScrollView) {
         if scrollView == sliderScrollView {
              let page = Int(round(scrollView.contentOffset.x / view.bounds.width))
              pageControl.currentPage = page
         }
    }
}
