using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BRB
{
    public sealed partial class SearchOperations : Page
    {
        public SearchOperations()
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
            List<string> parames = e.Parameter as List<string>;
            if (String.Equals(parames[0],"Дата")) 
            {
                Title.Text = "Поиск по дате " + parames[1];
                GetOperationsDate(parames[1]);
            }
            if (String.Equals(parames[0], "Операция"))
            {
                Title.Text = "Поиск по операции: " + parames[1];
                GetOperationsOper(parames[1]);
            }
            if (String.Equals(parames[0], "ID"))
            {
                Title.Text = "Поиск по ID: " + parames[1];
                GetOperationsID(parames[1]);
            }
        }

        public async void GetOperationsDate(string Date)
        {
            Ring.Visibility = Visibility.Visible;
            Ring.IsActive = true;
            List<OperationClass> operations = await DBWorker.GetOperationsAsync("Дата", Date);
            FillList(operations);
            Ring.Visibility = Visibility.Collapsed;
            Ring.IsActive = false;
        }
        public async void GetOperationsOper(string Operation)
        {
            Ring.Visibility = Visibility.Visible;
            Ring.IsActive = true;
            List<OperationClass> operations = await DBWorker.GetOperationsAsync("Операция", Operation);
            FillList(operations);
            Ring.Visibility = Visibility.Collapsed;
            Ring.IsActive = false;
        }
        public async void GetOperationsID(string ID)
        {
            Ring.Visibility = Visibility.Visible;
            Ring.IsActive = true;
            List<OperationClass> operations = await DBWorker.GetOperationsAsync("ID", ID);
            if (operations.Count == 0)
                Help.Message("Операций для такого ID не найдено. Проверьте все данные и повторите попытку", "Ошибка");
            else
                FillList(operations);
            Ring.Visibility = Visibility.Collapsed;
            Ring.IsActive = false;
        }
        public void FillList(List<OperationClass> operations)
        {
            Grid grid = new Grid();
            ColumnDefinition column1 = new ColumnDefinition();
            ColumnDefinition column2 = new ColumnDefinition();
            ColumnDefinition column3 = new ColumnDefinition();
            RowDefinition row1 = new RowDefinition();
            RowDefinition row2 = new RowDefinition();
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);
            grid.ColumnDefinitions.Add(column3);
            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);
            int i = 0, j = 0, k = 0;
            foreach (var operation in operations)
            {
                if (i == 3)
                {
                    if (k == 6)
                    {
                        k = 0;
                        i = 0;
                        j = 0;
                        OperationsView.Items.Add(grid);
                        grid = new Grid();
                        column1 = new ColumnDefinition();
                        column2 = new ColumnDefinition();
                        column3 = new ColumnDefinition();
                        row1 = new RowDefinition();
                        row2 = new RowDefinition();
                        grid.ColumnDefinitions.Add(column1);
                        grid.ColumnDefinitions.Add(column2);
                        grid.ColumnDefinitions.Add(column3);
                        grid.RowDefinitions.Add(row1);
                        grid.RowDefinitions.Add(row2);

                        StackPanel panel = PanelFill(operation);
                        grid.Children.Add(panel);
                        Grid.SetColumn(panel, i);
                        Grid.SetRow(panel, j);
                        i++;
                    }
                    else
                    {
                        i = 0;
                        j++;
                        StackPanel panel = PanelFill(operation);
                        grid.Children.Add(panel);
                        Grid.SetColumn(panel, i);
                        Grid.SetRow(panel, j);
                        i++;
                        k++;
                    }
                }
                else
                {
                    StackPanel panel = PanelFill(operation);
                    grid.Children.Add(panel);
                    Grid.SetColumn(panel, i);
                    Grid.SetRow(panel, j);
                    i++;k++;
                }
            }
            OperationsView.Items.Add(grid);
        }   
        private StackPanel PanelFill(OperationClass oper)
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Center };
            //panel.Tapped += Panel_Tapped;
            var s = panel.Parent;
            TextBlock id = new TextBlock() { Text = "ID: " + oper.ID, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            TextBlock name = new TextBlock() { Text = "Тип операции: " + oper.TypeOperation, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            TextBlock code = new TextBlock() { Text = "Подтип: " + oper.SubType, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            TextBlock cash = new TextBlock() { Text = "Получатель: " + oper.AccountCode, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            TextBlock block = new TextBlock() { Text = "Сумма: " + oper.Amount, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            TextBlock date = new TextBlock() { Text = "Дата: " + oper.Date.ToString("d.MM.yyyy"), TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            panel.Children.Add(id);
            panel.Children.Add(name);
            panel.Children.Add(code);
            panel.Children.Add(cash);
            panel.Children.Add(block);
            panel.Children.Add(date);
            return panel;
        }
    }
}
