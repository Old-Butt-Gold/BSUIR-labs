import Foundation

struct Movie {
    let id: String
    var title: String
    var description: String
    var year: Int
    var duration: Int
    var genres: [String]
    var director: String
    var imageUrls: [String]
    var posterUrl: String
    var favoritedBy: [String]
    
    init(id: String, data: [String: Any]) {
        self.id = id
        self.title = data["title"] as? String ?? ""
        self.description = data["description"] as? String ?? ""
        self.year = data["year"] as? Int ?? 0
        self.duration = data["duration"] as? Int ?? 0
        self.genres = data["genres"] as? [String] ?? []
        self.director = data["director"] as? String ?? ""
        self.imageUrls = data["imageUrls"] as? [String] ?? []
        self.posterUrl = data["posterUrl"] as? String ?? ""
        self.favoritedBy = data["favoritedBy"] as? [String] ?? []
    }
    
    func toFirestore() -> [String: Any] {
        return [
            "title": title,
            "description": description,
            "year": year,
            "duration": duration,
            "genres": genres,
            "director": director,
            "imageUrls": imageUrls,
            "posterUrl": posterUrl,
            "favoritedBy": favoritedBy
        ]
    }
}
