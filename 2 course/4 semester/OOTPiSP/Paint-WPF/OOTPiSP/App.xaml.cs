using System.Windows;

namespace OOTPiSP;

public partial class App
{
    public App()
    {
        FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false; //Для ввода . в TextBox
        SplashScreen splashScreen = new("Assets/bg.jpg");
        splashScreen.Show(true, false);
    }
}