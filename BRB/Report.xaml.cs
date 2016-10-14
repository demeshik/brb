using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace BRB
{
    public sealed partial class Report : Page
    {
        public Report()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }
        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
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
            Frame rootFrame = Window.Current.Content as Frame;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
        }

        private async void FillTapped(object sender, TappedRoutedEventArgs e)
        {
            listview.Items.Clear();
            Ring.Visibility = Visibility.Visible;
            Ring.IsActive = true;
            if (String.Equals(((TextBlock)sender).Name, "Day"))
            {
                List<OperationClass> operations = await DBWorker.GetOperations("Дата", DateTime.Today.ToString("dd.MM.yyyy"));
                if (operations.Count == 0)
                    NoOperations();
                else
                    FillView(operations);
            }
            if (String.Equals(((TextBlock)sender).Name, "Yesterday"))
            {
                List<OperationClass> operations = await DBWorker.GetOperations("Дата", DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0, 0)).ToString("dd.MM.yyyy"));
                if (operations.Count == 0)
                    NoOperations();
                else
                    FillView(operations);
            }
            if (String.Equals(((TextBlock)sender).Name, "Week"))
            {
                DateTime firstDate = DateTime.Today.Subtract(new TimeSpan(7, 0, 0, 0, 0));
                List<DateTime> dateList = new List<DateTime>();
                dateList.Add(firstDate);
                dateList.Add(DateTime.Today);
                List<OperationClass> operations = await DBWorker.GetOperations("", dateList);
                if (operations.Count == 0)
                    NoOperations();
                else
                    FillView(operations);
            }
            if (String.Equals(((TextBlock)sender).Name, "Month"))
            {
                DateTime firstDate = DateTime.Today.Subtract(new TimeSpan(30, 12, 0, 0, 0));
                List<DateTime> dateList = new List<DateTime>();
                dateList.Add(firstDate);
                dateList.Add(DateTime.Today);
                List<OperationClass> operations = await DBWorker.GetOperations("", dateList);
                if (operations.Count == 0)
                    NoOperations();
                else
                    FillView(operations);
            }
            if (String.Equals(((TextBlock)sender).Name, "All"))
            {
                List<OperationClass> operations = await DBWorker.GetOperations();
                if (operations.Count == 0)
                    NoOperations();
                else
                    FillView(operations);
            }
        }
        private void FillView(List<OperationClass> operations)
        {
            int k = 1;
            foreach (OperationClass operation in operations)
            {
                StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
                TextBlock number = new TextBlock() { Text = k.ToString() + ". ", TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White), Margin = new Thickness(0, 0, 20, 0) };
                TextBlock id = new TextBlock() { Text = "ID: " + operation.ID.ToString() + "; ", TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White), Margin = new Thickness(0, 0, 20, 0) };
                TextBlock name = new TextBlock() { Text = "Тип: " + operation.TypeOperation + "; ", TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White), Margin = new Thickness(0, 0, 20, 0) };
                TextBlock subtype = new TextBlock() { Text = "Подтип: " + operation.SubType + "; ", TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White), Margin = new Thickness(0, 0, 20, 0) };
                TextBlock code = new TextBlock() { Text = "Получатель: " + operation.AccountCode + "; ", TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White), Margin = new Thickness(0, 0, 20, 0) };
                TextBlock cash = new TextBlock() { Text = "Сумма: " + operation.Amount + "; ", TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White), Margin = new Thickness(0, 0, 20, 0) };
                TextBlock date = new TextBlock() { Text = "Дата: " + operation.Date.ToString("d.MM.yyyy"), TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White), Margin = new Thickness(0, 0, 20, 0) };
                panel.Children.Add(number);
                panel.Children.Add(id);
                panel.Children.Add(name);
                panel.Children.Add(subtype);
                panel.Children.Add(code);
                panel.Children.Add(cash);
                panel.Children.Add(date);

                listview.Items.Add(panel);
                k++;
            }
            Ring.Visibility = Visibility.Collapsed;
            Ring.IsActive = false;
            listview.Visibility = Visibility.Visible;
        }
        private void NoOperations()
        {
            TextBlock block = new TextBlock() { Foreground = new SolidColorBrush(Windows.UI.Colors.Gray), FontSize=30, Text = "В этот промежуток времени, операций не проводилось" };
            listview.Items.Add(block);
            Ring.Visibility = Visibility.Collapsed;
            Ring.IsActive = false;
            listview.Visibility = Visibility.Visible;
        }
    }
}
