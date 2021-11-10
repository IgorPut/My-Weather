using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class Language
    {
        //Функция возвращает общепринятую аббревиатуру языка, в зависимости от культуры. 
        private static string NameLanguage()
        {
            string cul = Properties.Resources.Name;
            switch (cul)
            {
                case "be-BE":
                    return "ru-RU";
                case "ru-RU":
                    return "ru-RU";
                default:
                    return "en-US";
            }
        }

        public static string nameLanguage = NameLanguage();
    }
}
