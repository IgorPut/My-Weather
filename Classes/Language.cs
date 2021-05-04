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
                case "be":
                    return "ru-ru";
                case "ru":
                    return "ru-ru";
                default:
                    return "en-us";
            }
        }

        public static string nameLanguage = NameLanguage();
    }
}
