using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Models.Entities
{
    //Behövs inte göra en ny PropertyChanged i varje fil då detta är "basklassen"
    public class BasePropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
