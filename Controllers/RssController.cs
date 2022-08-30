using MarrubiumShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using RSSFeeder_2.Models;
using System;
using System.Runtime;
using System.Xml;
using System.Xml.Linq;

namespace RSSFeeder_2.Controllers
{
    public class RssController : Controller
    {
        private ConfigSettings _configSettings;

        public RssController()
        {
            XDocument xdoc = XDocument.Load("config_settings.xml");
            XElement? settings = xdoc.Element("settings");
            if (settings is null)
                throw new NullReferenceException("Invalid config settings \".XML\" file!");
            var rssUrl = settings.Element("rss_url");
            var updateTime = settings.Element("update_time");
            var host = settings.Element("host"); 
            var port = settings.Element("port");
            var username = settings.Element("user_name");
            var password = settings.Element("password");
            
            if (rssUrl is null || updateTime is null || host is null || port is null || username is null || password is null)
                throw new ArgumentNullException("Invalid config arguments!");
            
            _configSettings = new ConfigSettings(
                rssUrl.Value,
                updateTime.Value,
                host.Value,
                port.Value,
                username.Value,
                password.Value);
        }

        public IActionResult Main()
        {
            return View();
        }

        [HttpPut("put-items-json")]
        public IActionResult GetItemsJson()
        {
            var rssUrl = HttpContext.Request.Headers["RssUrl"].ToString();
            if (rssUrl is null)
                throw new BadHttpRequestException("Invalid request data!");
            XmlTextReader xmlReader = new XmlTextReader(rssUrl);
            XmlUrlResolver resolver = new XmlUrlResolver();
            //WebProxy webProxy = new WebProxy(_configSettings.Host, _configSettings.Port);
            //webProxy.Credentials = _configSettings.Credentials;
            //resolver.Proxy = webProxy;
            xmlReader.XmlResolver = resolver;
            XDocument document = XDocument.Load(xmlReader);
            var items = GetItemsFromXml(document);
            return Json(items, JsonDefaultOptions.Serializer);
        }
        
        [HttpGet("get-rss-settings")]
        public IActionResult GetRssSettings()
        {
            return Json(new
            {
                RssUrl = _configSettings.RssUrl,
                UpdateTime = _configSettings.UpdateTime.Seconds
            }, JsonDefaultOptions.Serializer);
        }

        private List<RssItem> GetItemsFromXml(XDocument document)
        {
            List<RssItem> items = new List<RssItem>();
            var root = document.Root;
            if (root is null)
                throw new NullReferenceException("Invalid XML format!");
            var channel = root.Element("channel");
            if (channel is null)
                throw new NullReferenceException("Element with name \"channel\" is not found!");
            foreach (var item in channel.Elements("item"))
            {
                var link = item.Element("link");
                var title = item.Element("title");
                var pubDate = item.Element("pubDate");
                var description = item.Element("description");
                if(link is null || title is null || pubDate is null || description is null)
                    throw new ArgumentNullException("Invalid item arguments!");
                items.Add(new RssItem(
                    link.Value,
                    title.Value,
                    pubDate.Value,
                    description.Value));
            }
            return items;
        }    
    }
}