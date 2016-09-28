using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace BRB
{
    public sealed partial class MenuUser : Page
    {
        private string name = null;
        public MenuUser()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                title_menuUser.Text = $"Добро пожаловать, {e.Parameter.ToString()}";
                name = e.Parameter.ToString();
            }
        }
        private void NumIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(AccountInfo), name);
        }
        private void News_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(News));
        }
        private void Payments_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.BlockCheck)
                Help.Message("Ваш счет заблокирован. Вам недоступны операции. Обратитесь в службу поддержки банка.", "Ошибка");
            else
                Frame.Navigate(typeof(TransferPage), name);
        }
        private void History_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(History), name);
        }
        private void Settings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings), name);
        }

        private void Contacts_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Contacts));
        }
    }
}
