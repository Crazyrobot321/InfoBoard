using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Entities
{
    public class NewServer
    {
        public string GameName { get; set; } = string.Empty;
        public string ResetName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Today;
        public TimeSpan Time { get; set; } = DateTime.Now.TimeOfDay;
        public ResetInterval Interval { get; set; }
    }
}
