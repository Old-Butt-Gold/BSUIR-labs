import Foundation
import FirebaseAuth
import FirebaseFirestore

class AuthService {
    private let auth = Auth.auth()
    private let db = Firestore.firestore()
    private let profileService = ProfileService()
    
    var currentUser: User? {
        return auth.currentUser
    }
    
    func signIn(email: String, password: String, completion: @escaping (Result<User, Error>) -> Void) {
        auth.signIn(withEmail: email, password: password) { authResult, error in
            if let user = authResult?.user {
                completion(.success(user))
            } else if let error = error {
                completion(.failure(error))
            }
        }
    }
    
    func signUp(email: String, password: String, completion: @escaping (Result<User, Error>) -> Void) {
        auth.createUser(withEmail: email, password: password) { authResult, error in
            if let user = authResult?.user {

                let profile = UserProfile.empty(userId: user.uid, email: email)
                self.profileService.createInitialProfile(profile: profile) { error in
                    if let error = error {
                        completion(.failure(error))
                        return
                    }
    
                    user.sendEmailVerification(completion: nil)
                    completion(.success(user))
                }
            } else if let error = error {
                completion(.failure(error))
            }
        }
    }
    
    func checkEmailVerification(completion: @escaping (Bool) -> Void) {
        auth.currentUser?.reload(completion: { error in
            let isVerified = self.auth.currentUser?.isEmailVerified ?? false
            completion(isVerified)
        })
    }
    
    func resendVerificationEmail(completion: @escaping (Error?) -> Void) {
        auth.currentUser?.sendEmailVerification(completion: completion)
    }
    
    func deleteUser(password: String, completion: @escaping (Error?) -> Void) {
        guard let user = auth.currentUser, let email = user.email else {
            completion(NSError(domain: "", code: 0, userInfo: [NSLocalizedDescriptionKey: "Пользователь не найден"]))
            return
        }
        let credential = EmailAuthProvider.credential(withEmail: email, password: password)
        user.reauthenticate(with: credential) { authResult, error in
            if let error = error {
                completion(error)
                return
            }

            self.db.collection("users").document(user.uid).delete { error in
                if let error = error {
                    completion(error)
                    return
                }

                self.db.collection("movies")
                    .whereField("favoritedBy", arrayContains: user.uid)
                    .getDocuments { snapshot, error in
                        if let snapshot = snapshot {
                            let batch = self.db.batch()
                            for document in snapshot.documents {
                                var favorites = document.data()["favoritedBy"] as? [String] ?? []
                                favorites.removeAll(where: { $0 == user.uid })
                                batch.updateData(["favoritedBy": favorites], forDocument: document.reference)
                            }
                            batch.commit { batchError in
                                if let batchError = batchError {
                                    completion(batchError)
                                    return
                                }

                                user.delete { error in
                                    if let error = error {
                                        completion(error)
                                    } else {
                                        self.signOut { _ in
                                            completion(nil)
                                        }
                                    }
                                }
                            }
                        } else {
                            completion(error)
                        }
                    }
            }
        }
    }
    
    func signOut(completion: @escaping (Error?) -> Void) {
        do {
            try auth.signOut()
            completion(nil)
        } catch let signOutError {
            completion(signOutError)
        }
    }
}
