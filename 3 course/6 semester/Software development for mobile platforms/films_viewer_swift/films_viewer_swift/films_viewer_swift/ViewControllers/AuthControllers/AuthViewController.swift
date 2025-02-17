import UIKit
import FirebaseFirestore

class AuthViewController: UIViewController {
    
    private let authService = AuthService()
    
    // MARK: - UI Элементы
    
    private let logoImageView: UIImageView = {
        let iv = UIImageView()
        iv.image = UIImage(systemName: "person.circle.fill")
        iv.tintColor = .systemPurple
        iv.contentMode = .scaleAspectFit
        iv.translatesAutoresizingMaskIntoConstraints = false
        return iv
    }()
    
    private let emailTextField: UITextField = {
        let tf = UITextField()
        tf.placeholder = "Email"
        tf.borderStyle = .roundedRect
        tf.autocapitalizationType = .none
        let imageView = UIImageView(image: UIImage(systemName: "envelope"))
        imageView.tintColor = .gray
        tf.leftView = imageView
        tf.leftViewMode = .always
        return tf
    }()
    
    private let passwordTextField: UITextField = {
        let tf = UITextField()
        tf.placeholder = "Пароль"
        tf.borderStyle = .roundedRect
        tf.isSecureTextEntry = true
        let imageView = UIImageView(image: UIImage(systemName: "lock"))
        imageView.tintColor = .gray
        tf.leftView = imageView
        tf.leftViewMode = .always
        return tf
    }()
    
    private let actionButton: UIButton = {
        let btn = UIButton(type: .system)
        btn.setTitle("Войти", for: .normal)
        btn.backgroundColor = .systemPurple
        btn.tintColor = .white
        btn.layer.cornerRadius = 8
        btn.heightAnchor.constraint(equalToConstant: 44).isActive = true
        return btn
    }()
    
    private let toggleButton: UIButton = {
        let btn = UIButton(type: .system)
        btn.setTitle("Создать аккаунт", for: .normal)
        return btn
    }()
    
    private var isLogin = true
    
    // MARK: - Жизненный цикл
    
    override func viewDidLoad() {
        super.viewDidLoad()
        view.backgroundColor = .systemBackground
        setupViews()
    }
    
    // MARK: - Настройка UI
    
    private func setupViews() {
        let stackView = UIStackView(arrangedSubviews: [
            logoImageView,
            emailTextField,
            passwordTextField,
            actionButton,
            toggleButton
        ])
        stackView.axis = .vertical
        stackView.spacing = 16
        stackView.alignment = .fill
        stackView.translatesAutoresizingMaskIntoConstraints = false
        
        view.addSubview(stackView)
        
        NSLayoutConstraint.activate([
            logoImageView.heightAnchor.constraint(equalToConstant: 100),
            stackView.leadingAnchor.constraint(equalTo: view.leadingAnchor, constant: 24),
            stackView.trailingAnchor.constraint(equalTo: view.trailingAnchor, constant: -24),
            stackView.centerYAnchor.constraint(equalTo: view.centerYAnchor)
        ])
        
        actionButton.addTarget(self, action: #selector(handleAction), for: .touchUpInside)
        toggleButton.addTarget(self, action: #selector(handleToggle), for: .touchUpInside)
    }
    
    // MARK: - Действия
    
    @objc private func handleAction() {
        guard let email = emailTextField.text, !email.isEmpty,
              let password = passwordTextField.text, !password.isEmpty else {
            showAlert(message: "Заполните email и пароль")
            return
        }
        
        if isLogin {
            authService.signIn(email: email, password: password) { [weak self] result in
                guard let self = self else { return }
                switch result {
                case .success(let user):
                    if !user.isEmailVerified {
                        self.setRootViewController(VerifyEmailViewController())
                    } else {
                        Firestore.firestore().collection("users").document(user.uid)
                            .updateData(["emailVerified": true])
                        self.setRootViewController(MainPageViewController())
                    }
                case .failure(let error):
                    self.showAlert(message: error.localizedDescription)
                }
            }
        } else {
            authService.signUp(email: email, password: password) { [weak self] result in
                guard let self = self else { return }
                switch result {
                case .success(_):
                    self.setRootViewController(AuthWrapperViewController())
                case .failure(let error):
                    self.showAlert(message: error.localizedDescription)
                }
            }
        }
    }
    
    @objc private func handleToggle() {
        isLogin.toggle()
        actionButton.setTitle(isLogin ? "Войти" : "Зарегистрироваться", for: .normal)
        toggleButton.setTitle(isLogin ? "Создать аккаунт" : "Уже есть аккаунт? Войти", for: .normal)
    }
    
    private func setRootViewController(_ vc: UIViewController) {
        if let sceneDelegate = UIApplication.shared.connectedScenes.first?.delegate as? SceneDelegate {
            sceneDelegate.setRootViewController(vc)
        }
    }
    
    private func showAlert(message: String) {
        let alert = UIAlertController(title: "Ошибка", message: message, preferredStyle: .alert)
        alert.addAction(UIAlertAction(title: "ОК", style: .default))
        present(alert, animated: true)
    }
}
