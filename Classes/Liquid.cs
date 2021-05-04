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
            string liquidKind = "";

            if (rain > 0)
            {
                switch (cul)
                {
                    case "be":
                        liquid.Add("дождж");
                        break;
                    case "ru":
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
                    case "be":
                        liquid.Add("снег");
                        break;
                    case "ru":
                        liquid.Add("снег");
                        break;
                    default:
                        liquid.Add("snow");
                        break;
                }
            }

            if (ice > 0)
            {
                switch (cul)
                {
                    case "be":
                        liquid.Add("град");
                        break;
                    case "ru":
                        liquid.Add("град");
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
