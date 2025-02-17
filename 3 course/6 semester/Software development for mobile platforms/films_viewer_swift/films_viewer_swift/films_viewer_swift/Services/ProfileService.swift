import Foundation
import FirebaseFirestore

class ProfileService {
    private let db = Firestore.firestore()
    
    func getProfileStream(userId: String, completion: @escaping (UserProfile?) -> Void) -> ListenerRegistration {
        let listener = db.collection("users").document(userId).addSnapshotListener { snapshot, error in
            if let snapshot = snapshot, snapshot.exists, let data = snapshot.data() {
                let profile = UserProfile(id: snapshot.documentID, data: data)
                completion(profile)
            } else {
                completion(UserProfile.empty(userId: userId))
            }
        }
        return listener
    }
    
    func updateProfile(profile: UserProfile, completion: @escaping (Error?) -> Void) {
        db.collection("users").document(profile.id).updateData(profile.toFirestore(), completion: completion)
    }
    
    func createInitialProfile(profile: UserProfile, completion: @escaping (Error?) -> Void) {
        let docRef = db.collection("users").document(profile.id)
        docRef.getDocument { (document, error) in
            if let document = document, document.exists {
                completion(nil)
            } else {
                var data = profile.toFirestore()
                data["emailVerified"] = false
                data["createdAt"] = FieldValue.serverTimestamp()
                docRef.setData(data, completion: completion)
            }
        }
    }
}
