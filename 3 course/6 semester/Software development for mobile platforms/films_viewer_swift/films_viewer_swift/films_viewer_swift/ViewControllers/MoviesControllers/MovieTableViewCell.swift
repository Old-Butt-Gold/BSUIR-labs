import UIKit

class MovieTableViewCell: UITableViewCell {
    static let identifier = "MovieCell"
    
    private let posterImageView = UIImageView()
    private let titleLabel = UILabel()
    private let genresLabel = UILabel()
    private let detailsLabel = UILabel()
    let favoriteButton = UIButton(type: .system)
    
    var favoriteButtonAction: (() -> Void)?
    
    override init(style: UITableViewCell.CellStyle, reuseIdentifier: String?) {
         super.init(style: style, reuseIdentifier: reuseIdentifier)
         setupViews()
    }
    
    required init?(coder: NSCoder) {
         fatalError("init(coder:) not implemented")
    }
    
    private func setupViews() {
         posterImageView.contentMode = .scaleAspectFill
         posterImageView.clipsToBounds = true
         posterImageView.layer.cornerRadius = 8
         posterImageView.translatesAutoresizingMaskIntoConstraints = false
         
         titleLabel.font = UIFont.systemFont(ofSize: 20, weight: .semibold)
         titleLabel.numberOfLines = 0
         titleLabel.lineBreakMode = .byWordWrapping
         titleLabel.translatesAutoresizingMaskIntoConstraints = false
         
         genresLabel.font = UIFont.systemFont(ofSize: 14)
         genresLabel.textColor = .gray
         genresLabel.numberOfLines = 0
         genresLabel.lineBreakMode = .byWordWrapping
         genresLabel.translatesAutoresizingMaskIntoConstraints = false
         
         detailsLabel.font = UIFont.systemFont(ofSize: 12)
         detailsLabel.textColor = .gray
         detailsLabel.numberOfLines = 0
         detailsLabel.lineBreakMode = .byWordWrapping
         detailsLabel.translatesAutoresizingMaskIntoConstraints = false
         
         favoriteButton.tintColor = .red
         favoriteButton.translatesAutoresizingMaskIntoConstraints = false
         favoriteButton.addTarget(self, action: #selector(favoriteTapped), for: .touchUpInside)
         
         contentView.addSubview(posterImageView)
         contentView.addSubview(titleLabel)
         contentView.addSubview(genresLabel)
         contentView.addSubview(detailsLabel)
         contentView.addSubview(favoriteButton)
         
         NSLayoutConstraint.activate([
              posterImageView.leadingAnchor.constraint(equalTo: contentView.leadingAnchor, constant: 16),
              posterImageView.topAnchor.constraint(equalTo: contentView.topAnchor, constant: 16),
              posterImageView.bottomAnchor.constraint(equalTo: contentView.bottomAnchor, constant: -16),
              posterImageView.widthAnchor.constraint(equalToConstant: 120),
              posterImageView.heightAnchor.constraint(equalToConstant: 180),
              
              titleLabel.topAnchor.constraint(equalTo: posterImageView.topAnchor),
              titleLabel.leadingAnchor.constraint(equalTo: posterImageView.trailingAnchor, constant: 16),
              titleLabel.trailingAnchor.constraint(equalTo: favoriteButton.leadingAnchor, constant: -8),
              
              genresLabel.topAnchor.constraint(equalTo: titleLabel.bottomAnchor, constant: 8),
              genresLabel.leadingAnchor.constraint(equalTo: titleLabel.leadingAnchor),
              genresLabel.trailingAnchor.constraint(equalTo: titleLabel.trailingAnchor),
              
              detailsLabel.topAnchor.constraint(equalTo: genresLabel.bottomAnchor, constant: 8),
              detailsLabel.leadingAnchor.constraint(equalTo: titleLabel.leadingAnchor),
              detailsLabel.trailingAnchor.constraint(equalTo: titleLabel.trailingAnchor),
              
              favoriteButton.topAnchor.constraint(equalTo: contentView.topAnchor, constant: 16),
              favoriteButton.trailingAnchor.constraint(equalTo: contentView.trailingAnchor, constant: -16),
              favoriteButton.widthAnchor.constraint(equalToConstant: 40),
              favoriteButton.heightAnchor.constraint(equalToConstant: 40)
         ])
        
         titleLabel.setContentHuggingPriority(.defaultHigh, for: .vertical)
         genresLabel.setContentHuggingPriority(.defaultHigh, for: .vertical)
         detailsLabel.setContentHuggingPriority(.defaultHigh, for: .vertical)
    }
    
    func configure(with movie: Movie, userId: String) {
         titleLabel.text = movie.title
         genresLabel.text = movie.genres.joined(separator: ", ")
         detailsLabel.text = "\(movie.year) • \(movie.duration) мин"
         let isFavorite = movie.favoritedBy.contains(userId)
         let imageName = isFavorite ? "heart.fill" : "heart"
         favoriteButton.setImage(UIImage(systemName: imageName), for: .normal)
         
         if let url = URL(string: movie.posterUrl) {
              URLSession.shared.dataTask(with: url) { data, _, _ in
                  if let data = data, let image = UIImage(data: data) {
                      DispatchQueue.main.async {
                          self.posterImageView.image = image
                      }
                  }
              }.resume()
         } else {
              posterImageView.image = nil
         }
    }
    
    @objc private func favoriteTapped() {
         favoriteButtonAction?()
    }
}
