using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;


namespace BRB
{
    public sealed partial class Contacts : Page
    {
        public Contacts()
        { 
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += News_BackRequested;
            FillRule(); 
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
        public void FillRule()
        {
            TextBlock textElement = new TextBlock();
            textElement.TextWrapping = TextWrapping.Wrap;
            textElement.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            InlineCollection inline = textElement.Inlines;

            inline.Add(new Run
            {
                Text = "Сайт:",
                Foreground = new SolidColorBrush(Windows.UI.Colors.Red)
            });

            HyperlinkButton button = new HyperlinkButton();
            button.Content = "Перейти";
            button.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            button.NavigateUri = new Uri("https://www.nbrb.by/");
            StackPanel.Children.Add(textElement);
            StackPanel.Children.Add(button);

            TextBlock newText = new TextBlock();
            newText.TextWrapping = TextWrapping.Wrap;
            newText.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            InlineCollection newInline = newText.Inlines;

            newInline.Add(new Run
            {
                Text = "Электронное обращение:",
                Foreground = new SolidColorBrush(Windows.UI.Colors.Red)
            });

            HyperlinkButton newButton = new HyperlinkButton();
            newButton.Content = "Перейти";
            newButton.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            newButton.NavigateUri = new Uri("https://www.nbrb.by/NbrbApplications/");
            StackPanel.Children.Add(newText);
            StackPanel.Children.Add(newButton);

            TextBlock newText2 = new TextBlock();
            newText2.TextWrapping = TextWrapping.Wrap;
            newText2.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            newText2.Text = "Электронное обращение к администрации: email@nbrb.by";
            
            StackPanel.Children.Add(newText2);

            TextBlock newText3 = new TextBlock();
            newText3.TextWrapping = TextWrapping.Wrap;
            newText3.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            newText3.Text = "Контакт-центр: +375 17 306-00-02";

            StackPanel.Children.Add(newText3);

        }

    }
}
