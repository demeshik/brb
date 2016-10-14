using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BRB
{
    public sealed partial class History : Page
    {
        string name = string.Empty;
        public History()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += History_BackRequested;
        }
        private void History_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
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
                name = e.Parameter.ToString();
            }
            Fill();
        }
        public async void Fill()
        {
            List<OperationClass> operations = null;
            operations = await DBWorker.GetHistory(name);
            if (operations.Count == 0)
            {
                TextBlock text = new TextBlock();
                text.Text = "История операций пуста.";
                text.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);
                text.HorizontalAlignment = HorizontalAlignment.Center;
                text.VerticalAlignment = VerticalAlignment.Center;
                text.FontSize = 24;
                listView.Items.Add(text);
            }
            else
            {
                int i = 1;
                foreach(var oper in operations)
                {
                    TextBlock title = new TextBlock();
                    title.Text = i + ". " + oper.TypeOperation;
                    title.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);

                    TextBlock subtype = new TextBlock();
                    if (oper.TypeOperation == "Оплата")
                        subtype.Text = "Тип: " + oper.SubType;
                    else
                        subtype.Text = "Тип: ---";
                    subtype.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);

                    TextBlock acccode = new TextBlock();
                    acccode.Text = "Номер счета:" + oper.AccountCode;
                    acccode.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);

                    TextBlock cash = new TextBlock();
                    cash.Text = "Сумма: " + oper.Amount;
                    cash.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);

                    TextBlock date = new TextBlock();
                    date.Text = "Дата: " + oper.Date.ToString("d MMM yyyy") + "\n";
                    date.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);
                    StackPanel stack = new StackPanel();
                    stack.Children.Add(title);
                    stack.Children.Add(subtype);
                    stack.Children.Add(acccode);
                    stack.Children.Add(cash);
                    stack.Children.Add(date);
                    listView.Items.Add(stack);
                    i++;
                }
            }
            progressbar.Visibility = Visibility.Collapsed;
        }
    }
}
