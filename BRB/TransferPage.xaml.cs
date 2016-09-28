using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace BRB
{
    public sealed partial class TransferPage : Page
    {
        private string fio = null;
        public TransferPage()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += Transfer_BackRequested;
        }
        private void Transfer_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                fio = e.Parameter.ToString();
            }
        }
        private void listOperations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
            Operation.Text = lbi.Content.ToString();
            listOperations.Visibility = Visibility.Collapsed;
        }
        private void Operation_Tapped(object sender, TappedRoutedEventArgs e)
        {
            listOperations.Visibility = Visibility.Visible;
        }
        private void toggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleSwitch.IsOn)
                Operation.Visibility = Visibility.Visible;
            else
                Operation.Visibility = Visibility.Collapsed;
        }
        private async void OK_Tapped(object sender, TappedRoutedEventArgs e)
        {
         
            if (Number.Text.Length > 12)
            {
                e.Handled = true;
                Help.Message("Проверьте правильность номера счета", "Ошибка");
            }
            else
            {
                OperationClass operation = new OperationClass();
                if (toggleSwitch.IsOn)
                {
                    operation.TypeOperation = toggleSwitch.OnContent.ToString();
                    operation.SubType = Operation.Text;
                }
                else
                {
                    operation.TypeOperation = toggleSwitch.OffContent.ToString();
                    operation.SubType = null;
                }
                operation.AccountCode = Number.Text;
                operation.Amount = Count.Text;
                operation.Date = DateTime.Now;
                bool check = false;
                progressBar.Visibility = Visibility.Visible;
                check = await DBWorker.PrepareOperationAsync(fio, operation);
                progressBar.Visibility = Visibility.Collapsed;
                if (check)
                {
                    Number.Text = "";
                    Count.Text = "";
                    Help.Message("Операция выполена успешно.", "Сообщение");
                }
            }
        }

        private void NumberChanged(object sender, TextChangedEventArgs e)
        {
            if (Number.Text.Length > 12)
                Number.Background = new SolidColorBrush(Windows.UI.Colors.Red);
            else
                Number.Background = new SolidColorBrush(Windows.UI.Colors.White);
        }
    }
}
