using System.Net;

namespace RSSFeeder_2.Models
{
    public class ConfigSettings
    {
        public string RssUrl { get; set; }
        public TimeSpan UpdateTime { get; set; }
        public string Host { get; }
        public int Port { get; }
        public NetworkCredential Credentials { get; }

        public ConfigSettings(string rssUrl, string updateTime, string host, string port, string username,
            string password)
        {
            RssUrl = rssUrl;
            UpdateTime = new TimeSpan(0, 0, int.Parse(updateTime));
            Host = host;
            Port = int.Parse(port);
            Credentials = new NetworkCredential(username, password);
        }
    }
}