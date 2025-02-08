class UserProfile {
  final String id;
  final String email;
  String firstName;
  String lastName;
  String birthDate;
  String phone;
  String address;
  String country;
  String city;
  String bio;
  String gender;

  UserProfile({
    required this.id,
    required this.email,
    required this.firstName,
    required this.lastName,
    required this.birthDate,
    required this.phone,
    required this.address,
    required this.country,
    required this.city,
    required this.bio,
    required this.gender,
  });

  factory UserProfile.empty(String userId, [String email = '']) => UserProfile(
    id: userId,
    email: email,
    firstName: '',
    lastName: '',
    birthDate: '',
    phone: '',
    address: '',
    country: '',
    city: '',
    bio: '',
    gender: 'Не указан',
  );

  factory UserProfile.fromFirestore(Map<String, dynamic> data, String id) {
    return UserProfile(
      id: id,
      email: data['email'] ?? '',
      firstName: data['firstName'] ?? '',
      lastName: data['lastName'] ?? '',
      birthDate: data['birthDate'] ?? '',
      phone: data['phone'] ?? '',
      address: data['address'] ?? '',
      country: data['country'] ?? '',
      city: data['city'] ?? '',
      bio: data['bio'] ?? '',
      gender: data['gender'] ?? 'Не указан',
    );
  }

  Map<String, dynamic> toFirestore() {
    return {
      'email': email,
      'firstName': firstName,
      'lastName': lastName,
      'birthDate': birthDate,
      'phone': phone,
      'address': address,
      'country': country,
      'city': city,
      'bio': bio,
      'gender': gender,
    };
  }
}