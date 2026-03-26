using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Models.Entities
{
    public enum ResetInterval
    {
        Daily,
        Weekly,
        Once
    }
    public partial class GameTimeItem : BasePropertyChanged
    {
        public string Name { get; set; } = string.Empty;
        public string Game { get; set; } = string.Empty;
        public DateTime BaseResetTime { get; set; }
        public ResetInterval Interval { get; set; }
        public bool IsSelected { get; set; }

        private string _displayTime = string.Empty;
        public string DisplayTime
        {
            get => _displayTime;
            set { _displayTime = value; OnPropertyChanged(nameof(DisplayTime));}
        }
    }
}
