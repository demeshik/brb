using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BRB
{
    public sealed partial class AccountInfo : Page
    {
        private string fioname = string.Empty;
        public AccountInfo()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += AccountInfo_BackRequested;
        }
        private void AccountInfo_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
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
                fioname = e.Parameter.ToString();
            }
            GetInfo();
        }
        public async void GetInfo()
        {
            Client person = await DBWorker.GetInformationAsync(fioname);
            name.Text += fioname;
            loginname.Text += person.Login;
            acccode.Text += person.AccCode;
            cashnem.Text += person.Cash;
            blockaccount.Text += person.Block.ToString();
            progressbar.Visibility = Visibility.Collapsed;
            panel.Visibility = Visibility.Visible;
        }
    }
}
