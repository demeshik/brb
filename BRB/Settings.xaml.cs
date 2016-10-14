using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BRB
{
    public sealed partial class Settings : Page
    {
        private string fio = string.Empty;
        private string newLogin = string.Empty;
        private string newPass = string.Empty;
        private string chaLog = string.Empty;
        private string chaPass1 = string.Empty;
        private string chaPass2 = string.Empty;
        public Settings()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += Settings_BackRequested;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.BlockCheck)
                block.Text = "Разблокировать";
            else
                block.Text = "Заблокировать";
            if (e.Parameter != null)
            {
                fio = e.Parameter.ToString();
            }
        }
        private void Settings_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }
        private async void LoginChanged(object sender, TappedRoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "Изменение",
                RequestedTheme = ElementTheme.Dark,
                MaxWidth = this.ActualWidth 
            };

            StackPanel panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = "Введите новый логин(не более 10 символов, латиница):",
                TextWrapping = TextWrapping.Wrap,

            });

            TextBox log = new TextBox();
            log.PlaceholderText = "";
            log.TextWrapping = TextWrapping.Wrap;
            log.TextChanged += delegate
            {
                chaLog = log.Text;
            };
            panel.Children.Add(log);

            dialog.Content = panel;

            dialog.PrimaryButtonText = "OK";
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;

            dialog.SecondaryButtonText = "Cancel";
            dialog.SecondaryButtonClick += delegate {};

            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                loginRing.Visibility = Visibility.Visible;
                loginRing.IsActive = true;
                bool rezult = await DBWorker.UpdateInformation(fio, "Login", newLogin);
                loginRing.Visibility = Visibility.Collapsed;
                loginRing.IsActive = false;
                Help.Message("Данные успешно обновлены", "Поздравляем");
                logChecked.Visibility = Visibility.Visible;
            }

        }
        private async void pass_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var dialog2 = new ContentDialog()
            {
                Title = "Изменение",
                RequestedTheme = ElementTheme.Dark,
                MaxWidth = this.ActualWidth
            };

            StackPanel panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = "Введите новый пароль(не более 20 символов, латиница+цифры):",
                TextWrapping = TextWrapping.Wrap,

            });

            PasswordBox pass1 = new PasswordBox();
            pass1.PasswordChanged += delegate
            {
                chaPass1 = pass1.Password;
            };
            panel.Children.Add(pass1);

            panel.Children.Add(new TextBlock
            {
                Text = "Повторите пароль:",
                TextWrapping = TextWrapping.Wrap,
            });

            PasswordBox pass2 = new PasswordBox();
            pass2.PasswordChanged += delegate
            {
                chaPass2 = pass2.Password;
            };
            panel.Children.Add(pass2);

            dialog2.Content = panel;

            dialog2.PrimaryButtonText = "OK";
            dialog2.PrimaryButtonClick += Dialog2_PrimaryButtonClick;

            dialog2.SecondaryButtonText = "Cancel";
            dialog2.SecondaryButtonClick += delegate {

            };

            ContentDialogResult result = await dialog2.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                passRing.Visibility = Visibility.Visible;
                passRing.IsActive = true;
                bool rezult = await DBWorker.UpdateInformation(fio, "Pass", newPass);
                passRing.Visibility = Visibility.Collapsed;
                passRing.IsActive = false;
                Help.Message("Данные успешно обновлены", "Поздравляем");
                paassCheck.Visibility = Visibility.Visible;
            }
        }
        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string value = string.Empty;
            Regex reg = new Regex(@"[а-яА-ЯёЁ]+$");
            Match match = reg.Match(chaLog);
            StackPanel panel = (StackPanel)sender.Content;
            foreach (var child in panel.Children)
            {
                if (child.ToString() == "Windows.UI.Xaml.Controls.TextBox")
                {
                    TextBox box = (TextBox)child;
                    value = box.Text;
                }
            }
            if (match.Success || chaLog.Length > 10 || value == string.Empty) 
            {
                args.Cancel = true;
                sender.Background = new SolidColorBrush(Windows.UI.Colors.DarkRed);
                sender.Title = "Ошибка";
                logChecked.Visibility = Visibility.Collapsed;
            }
            else
            {
                newLogin = chaLog;
            }
        }
        private void Dialog2_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Regex reg = new Regex(@"[а-яА-ЯёЁ]+$");
            Match match = reg.Match(chaPass1);
            Match match2 = reg.Match(chaPass2);
            bool check = String.Equals(chaPass1, chaPass2);
            if (match.Success || match2.Success || chaPass1.Length > 20 || !check || chaPass1.Length == 0) 
            {
                args.Cancel = true;
                sender.Background = new SolidColorBrush(Windows.UI.Colors.DarkRed);
                sender.Title = "Ошибка";
                paassCheck.Visibility = Visibility.Collapsed;
            }
            else
            {
                newPass = chaPass1;
            }
        }

        private async void block_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bool rezult = false;
            ring.Visibility = Visibility.Visible;
            ring.IsActive = true;
            if (block.Text == "Заблокировать")
            {
                rezult = await DBWorker.BlockAccount(fio, true);
                ring.Visibility = Visibility.Collapsed;
                ring.IsActive = false;
                if (rezult)
                {
                    App.BlockCheck = true;
                    Help.Message("Аккаут заблокирован.", "");
                    block.Text = "Разблокировать";
                }
                else
                    Help.Message("Что-то пошло не так, попробуйте еще раз", "Ошибка");    
            }
            else
            {
                rezult = await DBWorker.BlockAccount(fio, false);
                ring.Visibility = Visibility.Collapsed;
                ring.IsActive = false;
                if (rezult)
                {
                    App.BlockCheck = false;
                    Help.Message("Аккаут разблокирован.", "");
                    block.Text = "Заблокировать";
                }
                else
                    Help.Message("Что-то пошло не так, попробуйте еще раз", "Ошибка");
            }
        }
    }
}
