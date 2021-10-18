using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace My_Weather.Localization
{
    public class KeyLocalizationListener : INotifyPropertyChanged
    {
        public KeyLocalizationListener(string key, object[] args)
        {
            Key = key;
            Args = args;
            LocalizationManager.Instance.CultureChanged += OnCultureChanged;
        }

        private string Key { get; }

        private object[] Args { get; }

        public object Value
        {
            get
            {
                var value = LocalizationManager.Instance.Localize(Key);
                if (value is string && Args != null)
                    value = string.Format((string)value, Args);
                return value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnCultureChanged(object sender, EventArgs eventArgs)
        {
            // Уведомляем привязку об изменении строки
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }

        ~KeyLocalizationListener()
        {
            LocalizationManager.Instance.CultureChanged -= OnCultureChanged;
        }
    }
}
