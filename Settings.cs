using System.ComponentModel;
using System.Configuration;
using System.Windows;

namespace My_Weather.Properties
{
    // Этот класс позволяет обрабатывать определенные события в классе параметров:
    //  Событие SettingChanging возникает перед изменением значения параметра.
    //  Событие PropertyChanged возникает после изменения значения параметра.
    //  Событие SettingsLoaded возникает после загрузки значений параметров.
    //  Событие SettingsSaving возникает перед сохранением значений параметров.
    internal sealed partial class Settings
    {
        public Settings()
        {
            // // Для добавления обработчиков событий для сохранения и изменения параметров раскомментируйте приведенные ниже строки:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            //this.SettingsSaving += this.SettingsSavingEventHandler;
            //
            //SettingsLoaded += SettingsLoadedEventHandler;
        }

        private void SettingsLoadedEventHandler(object sender, SettingsLoadedEventArgs e)

        {
            //MessageBox.Show("Setting Loaded");
        }

        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
            // Добавьте здесь код для обработки события SettingChangingEvent.
        }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
            // Добавьте здесь код для обработки события SettingsSaving.
        }
    }
}
