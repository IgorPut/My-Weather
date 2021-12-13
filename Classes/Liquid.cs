using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    class Liquid
    {
        public static string LiquidKind (float rain, float snow, float ice)
        {
            HashSet<string> liquid = new HashSet<string>();
            string cul = Properties.Resources.Name;
            string liquidKind/* = ""*/;

            if (rain > 0)
            {
                switch (cul)
                {
                    case "be-BE":
                        liquid.Add("дождж");
                        break;
                    case "ru-RU":
                        liquid.Add("дождж");
                        break;
                    default:
                        liquid.Add("rain");
                        break;
                }
            }

            if (snow > 0)
            {
                switch (cul)
                {
                    case "be-BE":
                        liquid.Add("снег");
                        break;
                    case "ru-RU":
                        liquid.Add("снег");
                        break;
                    default:
                        liquid.Add("snow");
                        break;
                }
            }
            //UnitType 3 'ice'
            if (ice > 0)
            {
                switch (cul)
                {
                    case "be-BE":
                        liquid.Add("шэрань");
                        break;
                    case "ru-RU":
                        liquid.Add("изморозь");
                        break;
                    default:
                        liquid.Add("ice");
                        break;
                }
            }

            liquidKind = string.Join(", ", liquid);
            return liquidKind;

        }
    }
}
