import UIKit
import FirebaseFirestore

class VerifyEmailViewController: UIViewController {
    
    private let authService = AuthService()
    
    // MARK: - UI Элементы
    
    private let mailIcon: UIImageView = {
        let iv = UIImageView()
        iv.image = UIImage(systemName: "envelope.open.fill")
        iv.tintColor = .systemBlue
        iv.contentMode = .scaleAspectFit
        iv.translatesAutoresizingMaskIntoConstraints = false
        return iv
    }()
    
    private let infoLabel: UILabel = {
        let label = UILabel()
        label.text = "Проверьте вашу почту и подтвердите email."
        label.textAlignment = .center
        label.font = UIFont.systemFont(ofSize: 18)
        label.numberOfLines = 0
        return label
    }()
    
    private let checkButton: UIButton = {
        let btn = UIButton(type: .system)
        btn.setTitle("Проверить подтверждение", for: .normal)
        btn.setImage(UIImage(systemName: "checkmark.circle"), for: .normal)
        btn.tintColor = .white
        btn.backgroundColor = .systemGreen
        btn.layer.cornerRadius = 8
        btn.heightAnchor.constraint(equalToConstant: 44).isActive = true
        btn.imageEdgeInsets = UIEdgeInsets(top: 0, left: -8, bottom: 0, right: 0)
        return btn
    }()
    
    private let resendButton: UIButton = {
        let btn = UIButton(type: .system)
        btn.setTitle("Отправить повторно", for: .normal)
        btn.setImage(UIImage(systemName: "arrow.clockwise"), for: .normal)
        btn.tintColor = .white
        btn.backgroundColor = .systemOrange
        btn.layer.cornerRadius = 8
        btn.heightAnchor.constraint(equalToConstant: 44).isActive = true
        btn.imageEdgeInsets = UIEdgeInsets(top: 0, left: -8, bottom: 0, right: 0)
        return btn
    }()
    
    private let signOutButton: UIButton = {
        let btn = UIButton(type: .system)
        btn.setTitle("Выйти", for: .normal)
        btn.tintColor = .systemRed
        return btn
    }()
    
    // MARK: - Жизненный цикл
    
    override func viewDidLoad() {
        super.viewDidLoad()
        view.backgroundColor = .systemBackground
        setupViews()
    }
    
    // MARK: - Настройка UI
    
    private func setupViews() {
        let stackView = UIStackView(arrangedSubviews: [
            mailIcon,
            infoLabel,
            checkButton,
            resendButton,
            signOutButton
        ])
        stackView.axis = .vertical
        stackView.spacing = 20
        stackView.alignment = .center
        stackView.translatesAutoresizingMaskIntoConstraints = false
        
        view.addSubview(stackView)
        
        NSLayoutConstraint.activate([
            mailIcon.heightAnchor.constraint(equalToConstant: 100),
            mailIcon.widthAnchor.constraint(equalToConstant: 100),
            
            stackView.leadingAnchor.constraint(equalTo: view.leadingAnchor, constant: 24),
            stackView.trailingAnchor.constraint(equalTo: view.trailingAnchor, constant: -24),
            stackView.centerYAnchor.constraint(equalTo: view.centerYAnchor)
        ])
        
        [checkButton, resendButton].forEach { button in
            button.translatesAutoresizingMaskIntoConstraints = false
            NSLayoutConstraint.activate([
                button.leadingAnchor.constraint(equalTo: stackView.leadingAnchor),
                button.trailingAnchor.constraint(equalTo: stackView.trailingAnchor)
            ])
        }
        
        checkButton.addTarget(self, action: #selector(checkVerification), for: .touchUpInside)
        resendButton.addTarget(self, action: #selector(resendVerification), for: .touchUpInside)
        signOutButton.addTarget(self, action: #selector(signOut), for: .touchUpInside)
    }
    
    // MARK: - Действия
    
    @objc private func checkVerification() {
        authService.checkEmailVerification { [weak self] isVerified in
            guard let self = self, let user = self.authService.currentUser else { return }
            if isVerified {
                Firestore.firestore().collection("users").document(user.uid)
                    .updateData(["emailVerified": true])
                self.setRootViewController(MainPageViewController())
            } else {
                self.showAlert(message: "Email пока не подтвержден.")
            }
        }
    }
    
    @objc private func resendVerification() {
        authService.resendVerificationEmail { [weak self] error in
            if let error = error {
                self?.showAlert(message: error.localizedDescription)
            } else {
                self?.showAlert(message: "Письмо отправлено повторно")
            }
        }
    }
    
    @objc private func signOut() {
        authService.signOut { [weak self] error in
            if let error = error {
                self?.showAlert(message: error.localizedDescription)
            } else {
                self?.setRootViewController(AuthWrapperViewController())
            }
        }
    }
    
    private func setRootViewController(_ vc: UIViewController) {
        if let sceneDelegate = UIApplication.shared.connectedScenes.first?.delegate as? SceneDelegate {
            sceneDelegate.setRootViewController(vc)
        }
    }
    
    private func showAlert(message: String) {
        let alert = UIAlertController(title: "Информация", message: message, preferredStyle: .alert)
        alert.addAction(UIAlertAction(title: "ОК", style: .default))
        present(alert, animated: true)
    }
}
