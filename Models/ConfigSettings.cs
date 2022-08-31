using System.Net;

namespace RSSFeeder_2.Models
{
    public class ConfigSettings
    {
        public string RssUrl { get; set; }
        public TimeSpan UpdateTime { get; set; }
        public string IPAddress { get; }
        public int Port { get; }
        public NetworkCredential Credentials { get; }

        public ConfigSettings(string rssUrl, string updateTime, string ipAddress, string port, string username,
            string password)
        {
            RssUrl = rssUrl;
            UpdateTime = new TimeSpan(0, 0, int.Parse(updateTime));
            IPAddress = ipAddress;
            int temp;
            int.TryParse(port, out temp);
            Port = temp;
            Credentials = new NetworkCredential(username, password);
        }
    }
}