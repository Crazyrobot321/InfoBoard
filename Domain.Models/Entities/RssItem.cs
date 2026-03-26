using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Entities
{
    public class RssItem
    {
        public string Title { get; set; } = string.Empty;
        public string PublishDate { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
    }
}
