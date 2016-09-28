using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace BRB
{
    public sealed partial class ManagerMenu : Page
    {
        public ManagerMenu()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += ExitRequested;
        }
        private void ExitRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                {
                    e.Handled = true;
                    rootFrame.GoBack();
                }
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                Title.Text += e.Parameter.ToString() + " !";
            }
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
        }

        private void StackPanel_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            searchOperations.Visibility = Visibility.Visible;
        }
        private void searchOperations_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            searchOperations.Visibility = Visibility.Collapsed;
        }

        private void StackPanel_PointerMoved_1(object sender, PointerRoutedEventArgs e)
        {
            StackPanel panel = sender as StackPanel;
            foreach (var item in panel.Children)
            {
                TextBlock block = item as TextBlock;
                block.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            }
        }
        private void StackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Color gray = Color.FromArgb(255, 147, 146, 146);
            StackPanel panel = sender as StackPanel;
            foreach (var item in panel.Children)
            {
                TextBlock block = item as TextBlock;
                block.Foreground = new SolidColorBrush(gray);
            }
        }

        private void Accounts_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(AccountsWork));
        }
        private async void DateSearch(object sender, TappedRoutedEventArgs e)
        {
            List<string> parametrs = new List<string>();
            parametrs.Add("Дата");
            DatePicker picker = new DatePicker();
            SearchDialog.Content = picker;
            ContentDialogResult rezult = await SearchDialog.ShowAsync();
            if (rezult == ContentDialogResult.Primary)
            {
                DateTimeOffset time = picker.Date;
                parametrs.Add(time.Date.ToString("d.MM.yyyy"));
                Frame.Navigate(typeof(SearchOperations), parametrs);
            }
        }
        private async void OperationSearch(object sender, TappedRoutedEventArgs e)
        {
            List<string> parametrs = new List<string>();
            parametrs.Add("Операция");
            TextBox box = new TextBox() { PlaceholderText = "Введите название операции(перевод, мобильная связь и т.д.)" };
            SearchDialog.Content = box;
            ContentDialogResult rezult = await SearchDialog.ShowAsync();
            if (rezult == ContentDialogResult.Primary)
            {
                parametrs.Add(box.Text);
                Frame.Navigate(typeof(SearchOperations), parametrs);
            }
        }
        private async void IdSearch(object sender, TappedRoutedEventArgs e)
        {
            List<string> parametrs = new List<string>();
            parametrs.Add("ID");
            TextBox box = new TextBox() { PlaceholderText = "Введите ID пользователя" };
            SearchDialog.Content = box;
            ContentDialogResult rezult = await SearchDialog.ShowAsync();
            if (rezult == ContentDialogResult.Primary)
            {
                parametrs.Add(box.Text);
                Frame.Navigate(typeof(SearchOperations), parametrs);
            }
        }

        private void TextColorChange(object sender, PointerRoutedEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            block.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
        }
        private void DefaultColor(object sender, PointerRoutedEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            Color gray = Color.FromArgb(255, 147, 146, 146);
            block.Foreground = new SolidColorBrush(gray);
        }

        private void Repotrs_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Report));
        }
    }
}
