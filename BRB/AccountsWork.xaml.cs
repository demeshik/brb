using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace BRB
{
    public sealed partial class AccountsWork : Page
    {
        private int checkEdit = 0;
        private StackPanel ChoosePanel;
        public AccountsWork()
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
            GetClients();
        }

        private async void GetClients()
        {
            Ring.IsActive = true;
            Ring.Visibility = Visibility.Visible;
            List<Client> clients = await DBWorker.GetClientsAsync();
            FillView(clients);
            Ring.IsActive = false;
            Ring.Visibility = Visibility.Collapsed;
            AccountLabel.Visibility = Visibility.Visible;
            ChooseLabel.Visibility = Visibility.Visible;
            AddDeleteGrid.Visibility = Visibility.Visible;
            EditBlockGrid.Visibility = Visibility.Visible;
        }
        private void FillView(List<Client> list)
        {
            Grid grid = new Grid();
            ColumnDefinition column1 = new ColumnDefinition();
            ColumnDefinition column2 = new ColumnDefinition();
            ColumnDefinition column3 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);
            grid.ColumnDefinitions.Add(column3);
            int i = 0;
            foreach(var client in list)
            {
                if (i == 3)
                {
                    i = 0;
                    ClientsView.Items.Add(grid);
                    grid = new Grid();
                    column1 = new ColumnDefinition();
                    column2 = new ColumnDefinition();
                    column3 = new ColumnDefinition();
                    grid.ColumnDefinitions.Add(column1);
                    grid.ColumnDefinitions.Add(column2);
                    grid.ColumnDefinitions.Add(column3);

                    StackPanel panel = PanelFill(client);
                    grid.Children.Add(panel);
                    Grid.SetColumn(panel, i);
                    i++; 
                }
                else
                {
                    StackPanel panel = PanelFill(client);
                    grid.Children.Add(panel);
                    Grid.SetColumn(panel, i);
                    i++; 
                }
            }
            ClientsView.Items.Add(grid);
        }   
        private StackPanel PanelFill(Client clnt)
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Center };
            panel.Tapped += Panel_Tapped;
            TextBlock id = new TextBlock() { Text = "ID: " + clnt.ID, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            TextBlock name = new TextBlock() { Text = "Имя: " + clnt.Name, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            TextBlock code = new TextBlock() { Text = "Номер счета: " + clnt.AccCode, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            TextBlock cash = new TextBlock() { Text = "Сумма на счету: " + clnt.Cash, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            TextBlock block = new TextBlock() { Text = "Блокирован: " + clnt.Block, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.WrapWholeWords, Foreground = new SolidColorBrush(Windows.UI.Colors.White) };
            panel.Children.Add(id);
            panel.Children.Add(name);
            panel.Children.Add(code);
            panel.Children.Add(cash);
            panel.Children.Add(block);
            return panel;
        }
        private void OperationsColor(Windows.UI.Color color)
        {
            foreach(var textbox in ClearPanel.Children)
            {
                TextBlock text = textbox as TextBlock;
                text.Foreground = new SolidColorBrush(color);
            }
            foreach (var textbox in EditPanel.Children)
            {
                TextBlock text = textbox as TextBlock;
                text.Foreground = new SolidColorBrush(color);
            }
            foreach (var textbox in BlockPanel.Children)
            {
                TextBlock text = textbox as TextBlock;
                text.Foreground = new SolidColorBrush(color);
            }
        }
        private async void CheckAgreeRefresh()
        {
            var messDialog = new MessageDialog("Информация в аккаунте была изменена. Перезагрузить список?");
            messDialog.Commands.Add(new UICommand(
                "Да",
                new UICommandInvokedHandler(this.AgreeCommandHandler)));
            messDialog.Commands.Add(new UICommand(
                "Нет",
                new UICommandInvokedHandler(this.AgreeCommandHandler)));
            messDialog.DefaultCommandIndex = 0;
            messDialog.CancelCommandIndex = 1;
            await messDialog.ShowAsync();
        }
        private bool CheckFieldsBack()
        {
            StackPanel panel = AddEditDialog.Content as StackPanel;
            foreach (var element in panel.Children)
            {
                if (element.ToString() == "Windows.UI.Xaml.Controls.TextBox")
                {
                    TextBox box = (TextBox)element;
                    SolidColorBrush brush = (SolidColorBrush)box.BorderBrush;
                    if (brush.Color == Windows.UI.Colors.Red)
                        return true;
                }

            }
            return false;
        }
        private bool CheckFieldsText()
        {
            StackPanel panel = AddEditDialog.Content as StackPanel;
            foreach (var element in panel.Children)
            {
                if (element.ToString() == "Windows.UI.Xaml.Controls.TextBox")
                {
                    TextBox box = (TextBox)element;
                    if (box.Text == string.Empty) 
                        return true;
                }

            }
            return false;
        }
        private void ClearBorders()
        {
            foreach (var item in ClientsView.Items)
            {
                Grid grd = item as Grid;
                foreach (var gridChild in grd.Children)
                {
                    StackPanel spanel = gridChild as StackPanel;
                    if (spanel.BorderBrush != null)
                        spanel.ClearValue(StackPanel.BorderBrushProperty);
                }
            }
        }
        private void ClearBackGround()
        {
            StackPanel panel = AddEditDialog.Content as StackPanel;
            foreach (var element in panel.Children)
            {
                if (element.ToString() == "Windows.UI.Xaml.Controls.TextBox")
                {
                    TextBox box = (TextBox)element;
                    box.BorderBrush = new SolidColorBrush(Windows.UI.Colors.White);
                    box.BorderThickness = new Thickness(1);
                }

            }
        }   
        private void ClearFields()
        {
            AddID.Text = string.Empty;
            AddName.Text = string.Empty;
            AddLogin.Text = string.Empty;
            AddPass.Text = string.Empty;
            AddCode.Text = string.Empty;
            AddCash.Text = string.Empty;
        }

        private void Panel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel panel = sender as StackPanel;
            if (panel.BorderBrush != null)
            {
                panel.ClearValue(StackPanel.BorderBrushProperty);
                ChoosePanel = null;
                ((TextBlock)BlockPanel.Children[1]).Text = "Блокировка";
                OperationsColor(Windows.UI.Colors.Gray);
                return;
            }
            ClearBorders();
            ChoosePanel = panel;
            string checkBlock = ((TextBlock)panel.Children[4]).Text.Remove(0, 12);
            if (checkBlock == "True")
                ((TextBlock)BlockPanel.Children[1]).Text = "Разблокировать";
            else
                ((TextBlock)BlockPanel.Children[1]).Text = "Заблокировать";
            panel.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
            panel.BorderThickness = new Thickness(5);
            OperationsColor(Windows.UI.Colors.White);
        }
        private async void Add_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bool rez;
            AddEditDialog.IsPrimaryButtonEnabled = false;
            ClearBackGround();
            ContentDialogResult rezult = await AddEditDialog.ShowAsync();
            if (rezult == ContentDialogResult.Primary)
            {
                Client person = new Client();
                person.ID = int.Parse(AddID.Text);
                person.Name = AddName.Text;
                person.Login = AddLogin.Text;
                person.Password = AddPass.Text;
                person.AccCode = AddCode.Text;
                person.Cash = AddCash.Text;
                person.Block = false;

                ProgRing.Visibility = Visibility.Visible;
                ProgRing.IsActive = true;
                rez = await DBWorker.AddClientAsync(person);

                ProgRing.IsActive = false;
                ProgRing.Visibility = Visibility.Collapsed;
                ClearFields();
                CheckAgreeRefresh();
            }
            else
            {
                ClearFields();
            }
        }
        private async void Clear_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ChoosePanel == null)
                e.Handled = true;
            else
            {
                var messDialog = new MessageDialog("Вы действительно хотите удалить аккаунт пользователя?");
                messDialog.Commands.Add(new UICommand(
                    "Да",
                    new UICommandInvokedHandler(this.DeleteCommandHandler)));
                messDialog.Commands.Add(new UICommand(
                    "Нет",
                    new UICommandInvokedHandler(this.DeleteCommandHandler)));
                messDialog.DefaultCommandIndex = 0;
                messDialog.CancelCommandIndex = 1;
                await messDialog.ShowAsync();
            }
        }
        private async void Edit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string oldID = string.Empty;
            if (ChoosePanel == null)
                e.Handled = true;
            else
            {
                checkEdit = 5;
                AddID.Text = ((TextBlock)ChoosePanel.Children[0]).Text.Remove(0, 4);
                oldID = ((TextBlock)ChoosePanel.Children[0]).Text.Remove(0, 4);
                AddName.Text = ((TextBlock)ChoosePanel.Children[1]).Text.Remove(0, 5);
                AddLogin.IsEnabled = false;
                AddPass.IsEnabled = false;
                AddCode.Text = ((TextBlock)ChoosePanel.Children[2]).Text.Remove(0, 13);
                AddCash.Text = ((TextBlock)ChoosePanel.Children[3]).Text.Remove(0, 16);
                ClearBackGround();
                AddEditDialog.IsPrimaryButtonEnabled = true;
                ContentDialogResult rezult = await AddEditDialog.ShowAsync();
                if (rezult == ContentDialogResult.Primary)
                {
                    checkEdit = 0;
                    Client person = new Client();
                    person.ID = int.Parse(AddID.Text);
                    person.Name = AddName.Text;
                    person.Login = string.Empty;
                    person.Password = string.Empty;
                    person.AccCode = AddCode.Text;
                    person.Cash = AddCash.Text;
                    person.Block = bool.Parse(((TextBlock)ChoosePanel.Children[4]).Text.Remove(0, 12));

                    ProgRing.Visibility = Visibility.Visible;
                    ProgRing.IsActive = true;
                    bool rez = await DBWorker.UpdateAccoutnManagAsync(person, oldID);

                    ProgRing.IsActive = false;
                    ProgRing.Visibility = Visibility.Collapsed;
                    AddLogin.IsEnabled = true;
                    AddPass.IsEnabled = true;
                    ClearFields();
                    CheckAgreeRefresh();
                }
                else
                {
                    checkEdit = 0;
                    AddLogin.IsEnabled = true;
                    AddPass.IsEnabled = true;
                    ClearFields();
                }
            }
        }
        private async void Block_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ChoosePanel == null)
                e.Handled = true;
            else
            {
                bool rezult;
                ProgRing.Visibility = Visibility.Visible;
                ProgRing.IsActive = true;
                TextBlock NameBox = (TextBlock)ChoosePanel.Children[1];
                string name = NameBox.Text.Remove(0, 5);
                if (((TextBlock)BlockPanel.Children[1]).Text == "Заблокировать")
                    rezult = await DBWorker.BlockAccountAsync(name, true);
                else
                    rezult = await DBWorker.BlockAccountAsync(name, false);
                ProgRing.IsActive = false;
                ProgRing.Visibility = Visibility.Collapsed;
                CheckAgreeRefresh();
            }
        }

        private async void DeleteCommandHandler(IUICommand command)
        {
            if (command.Label == "Да")
            {
                TextBlock NameBox = (TextBlock)ChoosePanel.Children[1];
                string name = NameBox.Text.Remove(0, 5);
                TextBlock IdBox = (TextBlock)ChoosePanel.Children[0];
                int ID = int.Parse(IdBox.Text.Remove(0, 4));
                bool x = await DBWorker.DeleteAccountAsync(name, ID);
                CheckAgreeRefresh();
            }
        }
        private async void AgreeCommandHandler(IUICommand command)
        {
            if (command.Label == "Да")
            {
                ClientsView.Items.Clear();
                List<Client> clients = await DBWorker.GetClientsAsync();
                FillView(clients);
                OperationsColor(Windows.UI.Colors.Gray);
                ChoosePanel = null;
                ((TextBlock)BlockPanel.Children[1]).Text = "Блокировка";
            }
            else
            {
                ClearBorders();
                OperationsColor(Windows.UI.Colors.Gray);
                ChoosePanel = null;
                ((TextBlock)BlockPanel.Children[1]).Text = "Блокировка";
            }
        }
        private void LoginCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (checkEdit == 0) 
            {
                if (CheckFieldsBack() || CheckFieldsText())
                    AddEditDialog.IsPrimaryButtonEnabled = false;
                else
                    AddEditDialog.IsPrimaryButtonEnabled = true;
            }
            TextBox block = sender as TextBox;
            Regex reg;
            reg = new Regex(@"[\W|а-яА-ЯёЁ]+$");
            if ((reg.Match(block.Text)).Success || block.Text.Length > 12)
            {
                block.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                block.BorderThickness = new Thickness(3);
            }
            else
            {
                block.BorderThickness = new Thickness(0);
            }
        }
        private void AddPass_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (checkEdit == 0) 
            {
                if (CheckFieldsBack() || CheckFieldsText())
                    AddEditDialog.IsPrimaryButtonEnabled = false;
                else
                    AddEditDialog.IsPrimaryButtonEnabled = true;
            }
            TextBox block = sender as TextBox;
            Regex reg;
            reg = new Regex(@"[\W|а-яА-ЯёЁ]+$");
            if ((reg.Match(block.Text)).Success || block.Text.Length > 20)
            {
                block.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                block.BorderThickness = new Thickness(3);
            }
            else
            {
                block.BorderThickness = new Thickness(0);
            }
        }
        private void AddCash_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (checkEdit==0)
            {
                if (CheckFieldsBack() || CheckFieldsText())
                    AddEditDialog.IsPrimaryButtonEnabled = false;
                else
                    AddEditDialog.IsPrimaryButtonEnabled = true;
            }
            TextBox block = sender as TextBox;
            Regex reg;
            reg = new Regex(@"[\D]+$");
            if ((reg.Match(block.Text)).Success)
            {
                block.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                block.BorderThickness = new Thickness(3);
            }
            else
            {
                block.BorderThickness = new Thickness(0);
            }
        }
        private void AddName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (checkEdit == 0) 
            {
                if (CheckFieldsBack() || CheckFieldsText())
                    AddEditDialog.IsPrimaryButtonEnabled = false;
                else
                    AddEditDialog.IsPrimaryButtonEnabled = true;
            }
            TextBox block = sender as TextBox;
            if (block.Text.Length > 50)
            {
                block.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                block.BorderThickness = new Thickness(3);
            }
            else
            {
                block.BorderThickness = new Thickness(0);
            }
        }
        private void AddID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (checkEdit==0)
            {
                if (CheckFieldsBack() || CheckFieldsText())
                    AddEditDialog.IsPrimaryButtonEnabled = false;
                else
                    AddEditDialog.IsPrimaryButtonEnabled = true;
            }
            int nowID;
            if (AddID.Text == string.Empty)
            {
                AddID.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                AddID.BorderThickness = new Thickness(3);
            }
            else
            {
                Regex reg;
                reg = new Regex(@"[\D]+$");
                if ((reg.Match(AddID.Text)).Success)
                {
                    AddID.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                    AddID.BorderThickness = new Thickness(3);
                }
                else
                {
                    nowID = int.Parse(AddID.Text);
                    var s = App.IdsList.Exists(o => o == nowID);
                    if (s)
                    {
                        AddID.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                        AddID.BorderThickness = new Thickness(3);
                    }
                    else
                    {
                        AddID.BorderThickness = new Thickness(0);
                    }
                }
            }

        }
    }
}