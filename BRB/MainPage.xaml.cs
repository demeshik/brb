using System;
using System.Text.RegularExpressions;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace BRB
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            if (IsInternet())
            {
                this.InitializeComponent();
                if (String.Equals(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily, "Windows.Mobile"))
                    VisualStateManager.GoToState(this, "MobileLaylout", true);
            }
            else
            {
                Help.Message("Нет соединения с Интернетом, пожалуйста, попробуйте позже.", "Ошибка");
            }
        }
        private async void run_Click(object sender, RoutedEventArgs e)
        {
            string rezult = string.Empty;
            prog.IsActive = true;
            prog.Visibility = Visibility.Visible;
            if (String.Equals(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily, "Windows.Mobile"))
            {
                rezult = await DBWorker.CheckClient(login.Text, password.Password);
                if (rezult == null)
                {
                    Help.Message("Такой пользователь не найден в базе данных. Пожалуйста, проверьте введенные данные и попробуйте еще раз.", "Ошибка");
                    password.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    login.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                }
                else
                    Frame.Navigate(typeof(MenuUser), rezult);
            }
            else
            {
                rezult = await DBWorker.CheckManager(login.Text, password.Password);
                if (rezult == null)
                {
                    Help.Message("Такой пользователь не найден в базе данных, либо у вас нет доступа. Пожалуйста, проверьте введенные данные и попробуйте еще раз.", "Ошибка");
                    password.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    login.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                }
                else
                    Frame.Navigate(typeof(ManagerMenu), rezult);
            }
            prog.IsActive = false;
            prog.Visibility = Visibility.Collapsed;

        }
        private void login_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex reg = new Regex(@"[а-яА-ЯёЁ]+$");
            Match match = reg.Match(login.Text);
            if (match.Success) 
            {
                login.Background = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                login.Background = new SolidColorBrush(Windows.UI.Colors.White);
            }
        }
        public static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }
    }

}
