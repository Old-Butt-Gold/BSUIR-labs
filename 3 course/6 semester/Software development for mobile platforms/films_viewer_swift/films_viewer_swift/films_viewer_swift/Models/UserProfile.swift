import Foundation

struct UserProfile {
    let id: String
    let email: String
    var firstName: String
    var lastName: String
    var birthDate: String
    var phone: String
    var address: String
    var country: String
    var city: String
    var bio: String
    var gender: String
    
    init(id: String, data: [String: Any]) {
        self.id = id
        self.email = data["email"] as? String ?? ""
        self.firstName = data["firstName"] as? String ?? ""
        self.lastName = data["lastName"] as? String ?? ""
        self.birthDate = data["birthDate"] as? String ?? ""
        self.phone = data["phone"] as? String ?? ""
        self.address = data["address"] as? String ?? ""
        self.country = data["country"] as? String ?? ""
        self.city = data["city"] as? String ?? ""
        self.bio = data["bio"] as? String ?? ""
        self.gender = data["gender"] as? String ?? "Не указан"
    }
    
    func toFirestore() -> [String: Any] {
        return [
            "email": email,
            "firstName": firstName,
            "lastName": lastName,
            "birthDate": birthDate,
            "phone": phone,
            "address": address,
            "country": country,
            "city": city,
            "bio": bio,
            "gender": gender
        ]
    }
    
    static func empty(userId: String, email: String = "") -> UserProfile {
        return UserProfile(id: userId, data: [
            "email": email,
            "firstName": "",
            "lastName": "",
            "birthDate": "",
            "phone": "",
            "address": "",
            "country": "",
            "city": "",
            "bio": "",
            "gender": "Не указан"
        ])
    }
}
