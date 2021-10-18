using System.Collections.Generic;
using System.Globalization;
using My_Weather.Properties;

namespace My_Weather.Localization
{
    public class ResxLocalizationProvider : ILocalizationProvider
    {
        private IEnumerable<CultureInfo> _cultures;

        public object Localize(string key)
        {
            return Resources.ResourceManager.GetObject(key);
        }

        public IEnumerable<CultureInfo> Cultures => _cultures ?? (_cultures = new List<CultureInfo>
        {
            new CultureInfo("be-BE"),
            new CultureInfo("en-US"),
            new CultureInfo("ru-RU"),
        });
    }
}
