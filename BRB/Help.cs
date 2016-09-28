using System;
using Windows.UI.Popups;

namespace BRB
{
    public static class Help
    {
        public static async void Message(string Content, string Title)
        {
            MessageDialog mes = new MessageDialog(Content, Title);
            await mes.ShowAsync();
        }
    }
}