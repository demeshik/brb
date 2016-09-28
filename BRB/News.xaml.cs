using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace BRB
{
    public sealed partial class News : Page
    {
        public News()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested +=News_BackRequested;
            Drwa();
        }
        private void News_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
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
        public async void Drwa()
        {
            int k = 1;
            await RSS.Download();

            for (int i = 0; i < RSS.titlesList.Count; i++)
            {
                TextBlock textElement = new TextBlock();
                textElement.TextWrapping = TextWrapping.Wrap;
                textElement.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                textElement.Text = k + " Заголовок:\n" + RSS.titlesList[i] + "\n";

                InlineCollection inline = textElement.Inlines;
                inline.Add(new Run
                {
                    Text = "Дата публикации:\n" + RSS.datesList[i].LocalDateTime.ToLocalTime().ToString() + "\n",
                });

                inline.Add(new Run
                {
                    Text = "Ссылка:",
                    Foreground = new SolidColorBrush(Windows.UI.Colors.Red)
                });

                HyperlinkButton button = new HyperlinkButton();
                button.Content = "Перейти";
                button.Foreground= new SolidColorBrush(Windows.UI.Colors.White); 
                button.NavigateUri = new Uri(RSS.linksList[i]);
                StackPanel st = new StackPanel();
                st.Children.Add(textElement);
                st.Children.Add(button);
                st.Margin = new Thickness(5, 10, 0, 0);
                listSlider.Items.Add(st);
                k++;
            }

        }
    }
}
