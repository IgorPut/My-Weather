using System.Collections.Generic;

namespace My_Weather.Classes
{
   
    internal class Liquid
    {
        public HashSet<string> liquidNames = new HashSet<string>(), liquidVals = new HashSet<string>();
        //Наименования осадков в дневном и ночном прогнозе
        public Liquid (float rainVal, string rainUnit, float snowVal, string snowUnit, float iceVal, string iceUnit)
        {
            HashSet<string> liquid = new HashSet<string>();
            string cul = Properties.Resources.Name;
            string liquidKind, liquidName;

            if (rainVal > 0)
            {
                switch (cul)
                {
                    case "be-BE":
                        liquid.Add("дождж");
                        liquidName = "дождж";
                        break;
                    case "ru-RU":
                        liquid.Add("дождь");
                        liquidName = "дождь";
                        break;
                    default:
                        liquid.Add("rain");
                        liquidName = "rain";
                        break;
                }
                liquidNames.Add(liquidName);
                liquidVals.Add(rainVal.ToString() + rainUnit);
            }

            if (snowVal > 0)
            {
                switch (cul)
                {
                    case "be-BE":
                        liquid.Add("снег");
                        liquidName = "снег";
                        break;
                    case "ru-RU":
                        liquid.Add("снег");
                        liquidName = "снег";
                        break;
                    default:
                        liquid.Add("snow");
                        liquidName = "snow";
                        break;
                }
                liquidNames.Add(liquidName);
                liquidVals.Add(rainVal.ToString() + rainUnit);
            }

            //UnitType 3 'ice'
            if (iceVal > 0)
            {
                switch (cul)
                {
                    case "be-BE":
                        liquid.Add("шэрань");
                        liquidName = "шэрань";
                        break;
                    case "ru-RU":
                        liquid.Add("изморозь");
                        liquidName = "изморозь";
                        break;
                    default:
                        liquid.Add("ice");
                        liquidName = "ice";
                        break;
                }
                liquidNames.Add(liquidName);
                liquidVals.Add(rainVal.ToString() + rainUnit);
            }

            liquidKind = string.Join(", ", liquid);
            //return liquidKind;
        }
    }
}
