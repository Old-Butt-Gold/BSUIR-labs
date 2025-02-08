import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import '../../Models/UserProfile.dart';
import '../../Routes.dart';
import '../../Services/AuthService.dart';
import '../../Services/ProfileService.dart';

class ProfileScreen extends StatefulWidget {
  final String userId;
  final String email;

  const ProfileScreen({super.key, required this.userId, required this.email});

  @override
  _ProfileScreenState createState() => _ProfileScreenState();
}

class _ProfileScreenState extends State<ProfileScreen> {
  final _auth = AuthService();
  final _profileService = ProfileService();

  late UserProfile _currentProfile;
  late TextEditingController _firstNameController;
  late TextEditingController _lastNameController;
  late TextEditingController _birthDateController;
  late TextEditingController _phoneController;
  late TextEditingController _addressController;
  late TextEditingController _countryController;
  late TextEditingController _cityController;
  late TextEditingController _bioController;

  bool _isLoading = true;
  final DateFormat _dateFormatter = DateFormat('dd.MM.yyyy');

  @override
  void initState() {
    super.initState();

    _firstNameController = TextEditingController();
    _lastNameController = TextEditingController();
    _birthDateController = TextEditingController();
    _phoneController = TextEditingController();
    _addressController = TextEditingController();
    _countryController = TextEditingController();
    _cityController = TextEditingController();
    _bioController = TextEditingController();

    _fetchProfile();
  }

  Future<void> _fetchProfile() async {
    try {
      final profile =
      await _profileService.getProfileStream(widget.userId).first;
      setState(() {
        _currentProfile = profile;
        _firstNameController.text = profile.firstName;
        _lastNameController.text = profile.lastName;
        _birthDateController.text = profile.birthDate;
        _phoneController.text = profile.phone;
        _addressController.text = profile.address;
        _countryController.text = profile.country;
        _cityController.text = profile.city;
        _bioController.text = profile.bio;
        _isLoading = false;
      });
    } catch (e) {
      setState(() => _isLoading = false);
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Ошибка загрузки профиля: $e')),
      );
    }
  }

  Future<void> _saveProfile() async {
    final updatedProfile = UserProfile(
      id: _currentProfile.id,
      email: _currentProfile.email,
      firstName: _firstNameController.text,
      lastName: _lastNameController.text,
      birthDate: _birthDateController.text,
      phone: _phoneController.text,
      address: _addressController.text,
      country: _countryController.text,
      city: _cityController.text,
      bio: _bioController.text,
      gender: _currentProfile.gender,
    );

    try {
      await _profileService.updateProfile(updatedProfile);
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Профиль успешно обновлен')),
      );
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Ошибка обновления: $e')),
      );
    }
  }

  @override
  void dispose() {
    _firstNameController.dispose();
    _lastNameController.dispose();
    _birthDateController.dispose();
    _phoneController.dispose();
    _addressController.dispose();
    _countryController.dispose();
    _cityController.dispose();
    _bioController.dispose();
    super.dispose();
  }

  Widget _buildField({
    required String label,
    IconData? icon,
    TextEditingController? controller,
    bool readOnly = false,
    String? initialValue,
    int maxLines = 1,
    VoidCallback? onTap,
  }) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: TextFormField(
        controller: controller,
        initialValue: initialValue,
        readOnly: readOnly,
        maxLines: maxLines,
        onTap: onTap,
        decoration: InputDecoration(
          labelText: label,
          suffixIcon: icon != null ? Icon(icon) : null,
          border: const OutlineInputBorder(),
          filled: readOnly,
          enabled: !readOnly,
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return const Scaffold(
        body: Center(child: CircularProgressIndicator()),
      );
    }

    return Scaffold(
      appBar: AppBar(
        title: const Text('Профиль'),
        actions: [
          IconButton(
            icon: const Icon(Icons.save),
            onPressed: _saveProfile,
          ),
        ],
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          child: ListView(
            children: [
              _buildReadOnlyField('Почта', widget.email, Icons.email),
              const SizedBox(height: 16),
              _buildTextField('Имя', _firstNameController, Icons.account_circle),
              _buildTextField('Фамилия', _lastNameController, Icons.account_circle),
              _buildGenderDropdown(),
              _buildBirthDateField(),
              _buildTextField('Телефон', _phoneController, Icons.phone),
              _buildTextField('Адрес', _addressController, Icons.house),
              _buildTextField('Страна', _countryController, Icons.map),
              _buildTextField('Город', _cityController, Icons.location_city),
              _buildTextField('Биография', _bioController, Icons.account_circle, maxLines: 5),
              const SizedBox(height: 20),
              ElevatedButton(
                onPressed: () async {
                  await _auth.signOut();
                  Navigator.pushReplacementNamed(context, Routes.authWrapperScreen);
                },
                style: ElevatedButton.styleFrom(backgroundColor: Colors.red),
                child: const Text('Выйти из системы'),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildTextField(String label, TextEditingController controller, IconData icon, {int maxLines = 1}) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: TextFormField(
        controller: controller,
        decoration: InputDecoration(
          suffixIcon: Icon(icon),
          labelText: label,
          border: const OutlineInputBorder(),
        ),
        maxLines: maxLines,
      ),
    );
  }

  Widget _buildReadOnlyField(String label, String value, IconData icon) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: TextFormField(
        initialValue: value,
        decoration: InputDecoration(
          labelText: label,
          suffixIcon: Icon(icon),
          border: const OutlineInputBorder(),
          filled: true,
          enabled: false,
        ),
      ),
    );
  }

  Widget _buildBirthDateField() {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: TextFormField(
        controller: _birthDateController,
        decoration: const InputDecoration(
          labelText: 'Дата рождения',
          border: OutlineInputBorder(),
          suffixIcon: Icon(Icons.calendar_today),
        ),
        readOnly: true,
        onTap: () async {
          DateTime initialDate;
          try {
            initialDate = _dateFormatter.parse(_birthDateController.text);
          } catch (e) {
            initialDate = DateTime.now();
          }

          final pickedDate = await showDatePicker(
            context: context,
            initialDate: initialDate,
            firstDate: DateTime(1900),
            lastDate: DateTime.now(),
          );

          if (pickedDate != null) {
            setState(() {
              _birthDateController.text = _dateFormatter.format(pickedDate);
            });
          }
        },
      ),
    );
  }

  Widget _buildGenderDropdown() {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: DropdownButtonFormField<String>(
        value: _currentProfile.gender,
        decoration: const InputDecoration(
          labelText: 'Пол',
          border: OutlineInputBorder(),
        ),
        items: const [
          DropdownMenuItem(value: 'Не указан', child: Text('Не указан')),
          DropdownMenuItem(value: 'Мужской', child: Text('Мужской')),
          DropdownMenuItem(value: 'Женский', child: Text('Женский')),
        ],
        onChanged: (value) {
          setState(() {
            _currentProfile.gender = value!;
          });
        },
      ),
    );
  }
}
