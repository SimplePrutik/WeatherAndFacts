using System;

namespace JSONProperties.Weather
{
    [Serializable]
    public class Period
    {
        public string name;
        public int temperature;
        public string temperatureUnit;
        public string icon;
    }
}