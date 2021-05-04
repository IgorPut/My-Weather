using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class UvIndex
    {
        public static string UV_Category(int categoryValue, string category)
        {
            string cul = Properties.Resources.Name;
            switch (categoryValue)
            {
                case 1:
                    switch (cul)
                    {
                        case "be":
                            return "Нiзкi";
                        default:
                            return category;
                    }
                case 2:
                    switch (cul)
                    {
                        case "be":
                            return "Умеран.";
                        default:
                            return category;
                    }
                case 3:
                    switch (cul)
                    {
                        case "be":
                            return "Выс.";
                        default:
                            return category;
                    }
                default:
                    return category;
            }
        }

    }
}
