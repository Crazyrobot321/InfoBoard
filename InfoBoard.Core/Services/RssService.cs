using Domain.Models.Entities;
using InfoBoard.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;

namespace InfoBoard.Core.Services
{
    public class RssService : IRssService
    {
        private readonly HttpClient _httpClient;

        public RssService(HttpClient httpClient)  // Vi tar in HttpClient via DI (standard i MAUI)
        {
            _httpClient = httpClient;
        }
        public async Task<List<RssItem>> GetSvtNews()
        {
            try
            {
                var xmlString = await _httpClient.GetStringAsync("https://www.svt.se/rss.xml");
                var doc = XDocument.Parse(xmlString);

                // Vi letar upp alla <item> taggar i XML-filen
                var items = doc.Descendants("item").Select(item =>
                {
                    var title = item.Element("title")?.Value;
                    var pubDateStr = item.Element("pubDate")?.Value;
                    var link = item.Element("link")?.Value;

                    string formattedTime = "00:00";
                    if (DateTime.TryParse(pubDateStr, out var dt))
                    {
                        formattedTime = dt.ToLocalTime().ToString("HH:mm");
                    }

                    return new RssItem
                    {
                        Title = title,
                        PublishDate = formattedTime,
                        Link = link
                    };
                }).Take(10).ToList(); // Vi tar bara de 10 första nyheterna

                return items;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"RSS-fel: {ex.Message}");
                return new List<RssItem> 
                { 
                    new RssItem 
                    { 
                        Title = "Kunde inte ladda nyheter" 
                    } 
                };
            }
        }
    }
}
