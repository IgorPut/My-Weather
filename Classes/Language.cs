namespace My_Weather.Classes
{
    class Language
    {
        //Свойство NameLanguage принимает значение значение общепринятой аббревиатуры языка, в зависимости от культуры. 
        //Используется в http запросах
        //Для языков, не перечисленных в switch, принимает значение en-US
        private static string _nameLang;
        public static string NameLanguage
        {
            get
            {
                switch (_nameLang)
                {
                    //case "be-BE":
                    //    return "en-US";
                    case "ru-RU":
                        return "ru-RU";
                    default:
                        return "en-US";
                }
            }

            set
            {
                _nameLang = value;
            }
        }
    }
}
