import UIKit
import FirebaseFirestore

class AuthWrapperViewController: UIViewController {
    
    private let authService = AuthService()
    
    override func viewDidLoad() {
        super.viewDidLoad()

        if let user = authService.currentUser {
            Firestore.firestore().collection("users").document(user.uid).getDocument { snapshot, error in
                if snapshot?.exists == true {
                    if user.isEmailVerified {
                        self.setRootViewController(MainPageViewController())
                    } else {
                        self.setRootViewController(VerifyEmailViewController())
                    }
                } else {
                    self.setRootViewController(AuthViewController())
                }
            }
        } else {
            self.setRootViewController(AuthViewController())
        }
    }
    
    private func setRootViewController(_ vc: UIViewController) {
        if let sceneDelegate = UIApplication.shared.connectedScenes.first?.delegate as? SceneDelegate {
            sceneDelegate.setRootViewController(vc)
        }
    }
}
