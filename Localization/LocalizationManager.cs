﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;

namespace My_Weather.Localization
{
    public class LocalizationManager
    {
        private LocalizationManager()
        {
        }

        private static LocalizationManager _localizationManager;

        public static LocalizationManager Instance => _localizationManager ?? (_localizationManager = new LocalizationManager());

        // declaring an event using built-in EventHandler
        public event EventHandler CultureChanged;

        public CultureInfo CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set
            {
                if (Equals(value, Thread.CurrentThread.CurrentUICulture))
                    return;
                Thread.CurrentThread.CurrentUICulture = value;
                CultureInfo.DefaultThreadCurrentUICulture = value;
                OnCultureChanged();
            }
        }

        public IEnumerable<CultureInfo> Cultures => LocalizationProvider?.Cultures ?? Enumerable.Empty<CultureInfo>();

        public ILocalizationProvider LocalizationProvider { get; set; }

        private void OnCultureChanged()
        {
            CultureChanged?.Invoke(this, EventArgs.Empty);
        }

        public object Localize(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "[NULL]";
            var localizedValue = LocalizationProvider?.Localize(key);
            return localizedValue ?? $"[{key}]";
        }
    }
}

