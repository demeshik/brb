using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace BRB
{
    public static class RSS
    {
        public static List<string> titlesList = new List<string>();
        public static List<string> linksList = new List<string>();
        public static List<DateTimeOffset> datesList = new List<DateTimeOffset>();
        public async static Task Download()
        {
            Uri uri = new Uri("http://www.nbrb.by/RSS/?p=News");
            SyndicationClient client = new SyndicationClient();
            SyndicationFeed feed = await client.RetrieveFeedAsync(uri);
            foreach (SyndicationItem item in feed.Items)
            {
                titlesList.Add(item.Title.Text);
                foreach(var link in item.Links)
                {
                    linksList.Add(link.NodeValue);
                }
                datesList.Add(item.PublishedDate);   
            }
        }
    }
}
