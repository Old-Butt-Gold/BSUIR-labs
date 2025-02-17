import Foundation
import FirebaseFirestore

class MovieService {
    private let db = Firestore.firestore()
    
    func getMoviesStream(completion: @escaping ([Movie]) -> Void) -> ListenerRegistration {
        let listener = db.collection("movies").addSnapshotListener { snapshot, error in
            guard let documents = snapshot?.documents else {
                print("Нет документов или произошла ошибка: \(error?.localizedDescription ?? "неизвестная ошибка")")
                completion([])
                return
            }
            let movies = documents.map { doc in
                return Movie(id: doc.documentID, data: doc.data())
            }
            completion(movies)
        }
        return listener
    }
    
    func getFavoriteMovies(userId: String, completion: @escaping ([Movie]) -> Void) -> ListenerRegistration {
        let listener = db.collection("movies")
            .whereField("favoritedBy", arrayContains: userId)
            .addSnapshotListener { snapshot, error in
                guard let documents = snapshot?.documents else {
                    print("Ошибка при получении избранных фильмов: \(error?.localizedDescription ?? "неизвестная ошибка")")
                    completion([])
                    return
                }
                let movies = documents.map { doc in
                    return Movie(id: doc.documentID, data: doc.data())
                }
                completion(movies)
            }
        return listener
    }
    
    func toggleFavorite(movieId: String, userId: String, completion: @escaping (Error?) -> Void) {
        let docRef = db.collection("movies").document(movieId)
        docRef.getDocument { (document, error) in
            guard let document = document, document.exists, let data = document.data() else {
                completion(error)
                return
            }
            
            let favoritedBy = data["favoritedBy"] as? [String] ?? []
            if favoritedBy.contains(userId) {
                docRef.updateData(["favoritedBy": FieldValue.arrayRemove([userId])], completion: completion)
            } else {
                docRef.updateData(["favoritedBy": FieldValue.arrayUnion([userId])], completion: completion)
            }
        }
    }
    
    func addMovie(movie: Movie, completion: @escaping (Error?) -> Void) {
        db.collection("movies").addDocument(data: movie.toFirestore(), completion: completion)
    }
}
