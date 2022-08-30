namespace RSSFeeder_2.Models
{
    public class RssItem
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string PubDate { get; set; }
        public string Description { get; set; }

        public RssItem(string link, string title, string pubDate, string description)
        {
            Link = link;
            Title = title;
            PubDate = /*pubDate*/DateTime.Parse(pubDate).ToString("F");
            Description = description;
        }
    }
}
