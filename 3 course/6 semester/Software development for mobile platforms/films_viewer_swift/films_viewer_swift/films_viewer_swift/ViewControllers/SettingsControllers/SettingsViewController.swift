import UIKit

class SettingsViewController: UIViewController {
    
    private let authService = AuthService()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        setupViews()
    }
    
    private func setupViews() {
        let deleteButton = UIButton(type: .system)
        deleteButton.setTitle("Удалить аккаунт", for: .normal)
        deleteButton.backgroundColor = .systemBlue
        deleteButton.setTitleColor(.white, for: .normal)
        deleteButton.layer.cornerRadius = 8
        deleteButton.heightAnchor.constraint(equalToConstant: 44).isActive = true
        deleteButton.addTarget(self, action: #selector(showDeleteUserDialog), for: .touchUpInside)
        
        let createButton = UIButton(type: .system)
        createButton.setTitle("Создать нового пользователя", for: .normal)
        createButton.backgroundColor = .systemBlue
        createButton.setTitleColor(.white, for: .normal)
        createButton.layer.cornerRadius = 8
        createButton.heightAnchor.constraint(equalToConstant: 44).isActive = true
        createButton.addTarget(self, action: #selector(showCreateUserDialog), for: .touchUpInside)
        
        let stackView = UIStackView(arrangedSubviews: [deleteButton, createButton])
        stackView.axis = .vertical
        stackView.spacing = 20
        stackView.alignment = .fill
        stackView.translatesAutoresizingMaskIntoConstraints = false
        
        view.addSubview(stackView)
        NSLayoutConstraint.activate([
            stackView.centerYAnchor.constraint(equalTo: view.centerYAnchor),
            stackView.leadingAnchor.constraint(equalTo: view.leadingAnchor, constant: 24),
            stackView.trailingAnchor.constraint(equalTo: view.trailingAnchor, constant: -24)
        ])
    }
    
    @objc private func showDeleteUserDialog() {
        let alert = UIAlertController(title: "Удаление аккаунта",
                                      message: "Введите ваш пароль для подтверждения удаления аккаунта.",
                                      preferredStyle: .alert)
        alert.addTextField { textField in
            textField.placeholder = "Пароль"
            textField.isSecureTextEntry = true
        }
        alert.addAction(UIAlertAction(title: "Отмена", style: .cancel))
        alert.addAction(UIAlertAction(title: "Удалить", style: .destructive, handler: { [weak self] _ in
            guard let password = alert.textFields?.first?.text, !password.isEmpty else { return }
            self?.authService.deleteUser(password: password) { error in
                DispatchQueue.main.async {
                    if let error = error {
                        self?.showAlert(message: "Ошибка: \(error.localizedDescription)")
                    } else if let sceneDelegate = UIApplication.shared.connectedScenes.first?.delegate as? SceneDelegate {
                        sceneDelegate.setRootViewController(AuthWrapperViewController())
                    }
                }
            }
        }))
        present(alert, animated: true)
    }
    
    @objc private func showCreateUserDialog() {
        let alert = UIAlertController(title: "Создать нового пользователя",
                                      message: nil,
                                      preferredStyle: .alert)
        alert.addTextField { textField in
            textField.placeholder = "Email"
        }
        alert.addTextField { textField in
            textField.placeholder = "Пароль"
            textField.isSecureTextEntry = true
        }
        alert.addAction(UIAlertAction(title: "Отмена", style: .cancel))
        alert.addAction(UIAlertAction(title: "Создать", style: .default, handler: { _ in
            guard let email = alert.textFields?[0].text, !email.isEmpty,
                  let password = alert.textFields?[1].text, password.count >= 6 else { return }
            AuthService().signUp(email: email, password: password) { result in
                DispatchQueue.main.async {
                    switch result {
                    case .success(_):
                        let successAlert = UIAlertController(title: "Успех",
                                                               message: "Пользователь создан. Проверьте почту для подтверждения.",
                                                               preferredStyle: .alert)
                        successAlert.addAction(UIAlertAction(title: "ОК", style: .default))
                        self.present(successAlert, animated: true)
                    case .failure(let error):
                        self.showAlert(message: "Ошибка при создании пользователя: \(error.localizedDescription)")
                    }
                }
            }
        }))
        present(alert, animated: true)
    }
    
    private func showAlert(message: String) {
        let alert = UIAlertController(title: "Информация",
                                      message: message,
                                      preferredStyle: .alert)
        alert.addAction(UIAlertAction(title: "ОК", style: .default))
        present(alert, animated: true)
    }
}
