using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using My_Weather.Localization;

namespace My_Weather
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IEnumerable<CultureInfo> _cultureInfos;
        private CultureInfo _currentCulture;
        public event PropertyChangedEventHandler PropertyChanged;

        //Событие, на изменение свойства      
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); //Проверка, изменяется или свойство
        }

        public IEnumerable<CultureInfo> CultureInfos
        {
            get { return _cultureInfos ?? (_cultureInfos = LocalizationManager.Instance.Cultures); }
            set
            //Проверка, изменилась ли _cultureInfos
            {
                if (Equals(value, _cultureInfos)) return;
                _cultureInfos = value;
                OnPropertyChanged();
            }
        }

        public CultureInfo CurrentCulture
        {
            get { return _currentCulture ?? (_currentCulture = LocalizationManager.Instance.CurrentCulture); }
            set
            {
                if (Equals(value, _currentCulture)) return;
                _currentCulture = value;
                LocalizationManager.Instance.CurrentCulture = value;
                OnPropertyChanged();
            }
        }

    }
}
