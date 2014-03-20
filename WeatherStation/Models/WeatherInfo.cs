using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WeatherStation.Models
{
    public class TemperatureInfo
    {
        public float Temperature { get; set; }
        public long DateTime { get; set; }
    }

    public class AirpressureInfo
    {
        public float Airpressure { get; set; }
        public long DateTime { get; set; }
    }

    public class HumidityInfo
    {
        public float Humidity { get; set; }
        public long DateTime { get; set; }
    }

    public class WindInfo
    {
        public string WindDirection { get; set; }
    }

    public class WeatherInfoSet
    {
        private enum Wind_Directions
        {
            North = 0, ENE = 1, NE = 2, NNE = 3, East = 4, ESE = 5, SE = 6, SSE = 7, South = 8, SSW = 9, SW = 10, WSW = 11, West = 12, WNW = 13, NW = 14, NNW = 15
        };

        private enum Seasons
        {
            SPRING = 0, SUMMER = 1, AUTUMN = 2, WINTER = 3
        };


        private string[,] pressure_rising_table = new string[,] { { "clear", "clear", "clear", "sleet" }, // NORTH

                                                                  { "cloudy", "partlycloudy", "partlycloudy", "cloudy" }, // ENE
                                                                  { "cloudy", "partlycloudy", "partlycloudy", "cloudy" }, // NE
                                                                  { "cloudy", "partlycloudy", "partlycloudy", "cloudy" }, // NNE
                                                                  
                                                                  { "cloudy", "cloudy", "partlycloudy", "snow" }, // EAST
                                                                  
                                                                  { "clear", "clear", "clear", "snow" }, // ESE
                                                                  { "clear", "clear", "clear", "snow" }, // SE
                                                                  { "clear", "clear", "clear", "snow" }, // SSE
                                                                  
                                                                  { "clear", "clear", "clear", "hazy" }, // SOUTH
                                                                  
                                                                  { "clear", "partlycloudy", "partlycloudy", "partlycloudy" }, // SSW
                                                                  { "clear", "partlycloudy", "partlycloudy", "partlycloudy" }, // SW
                                                                  { "clear", "partlycloudy", "partlycloudy", "partlycloudy" }, // WSW
                                                                  
                                                                  { "clear", "partlycloudy", "clear", "partlycloudy" }, // WEST
                                                                  
                                                                  { "partlycloudy", "partlycloudy", "partlycloudy", "sleet" },   // WNW
                                                                  { "partlycloudy", "partlycloudy", "partlycloudy", "sleet" },   // NW
                                                                  { "partlycloudy", "partlycloudy", "partlycloudy", "sleet" } }; // NNW


        private string[,] pressure_dropping_table = new string[,] { { "sleet", "tstorms", "rain", "snow" }, // NORTH

                                                                    { "rain", "rain", "partlycloudy", "snow" }, // ENE
                                                                    { "rain", "rain", "partlycloudy", "snow" }, // NE
                                                                    { "rain", "rain", "partlycloudy", "snow" }, // NNE

                                                                    { "rain", "rain", "partlycloudy", "snow" }, // EAST
                                                                    
                                                                    { "rain", "tstorms", "rain", "snow" }, // ESE
                                                                    { "rain", "tstorms", "rain", "snow" }, // SE
                                                                    { "rain", "tstorms", "rain", "snow" }, // SSE

                                                                    { "rain", "tstorms", "rain", "partlycloudy" }, // SOUTH
                                                                    
                                                                    { "rain", "rain", "rain", "partlycloudy" }, // SSW
                                                                    { "rain", "rain", "rain", "partlycloudy" }, // SW
                                                                    { "rain", "rain", "rain", "partlycloudy" }, // WSW

                                                                    { "rain", "rain", "rain", "partlycloudy" }, // WEST
                                                                    
                                                                    { "tstorms", "tstorms", "rain", "tstorms" },   // WNW
                                                                    { "tstorms", "tstorms", "rain", "tstorms" },   // NW
                                                                    { "tstorms", "tstorms", "rain", "tstorms" } }; // NNW

        public List<TemperatureInfo> TemperatureInfoList { get; set; }
        public List<AirpressureInfo> AirpressureInfoList { get; set; }
        public List<HumidityInfo> HumidityInfoList { get; set; }

        public List<SelectListItem> PullDownInfoList { get; set; }

        public string JsonTemperatureInfoList { get; set; }
        public string JsonAirpressureInfoList { get; set; }
        public string JsonHumidityInfoList { get; set; }

        public WindInfo Windinfo { get; set; }
        public string WeathertypeIconUrl { get; set; }

        public string MySelection { get; set; }

        public string WeatherImageURL;

        public WeatherInfoSet()
        {
            AirpressureInfoList = new List<AirpressureInfo>();
            HumidityInfoList = new List<HumidityInfo>();
            TemperatureInfoList = new List<TemperatureInfo>();

            PullDownInfoList = new List<SelectListItem>();
        }

        public void UpdateWeatherType()
        {
            Seasons current_season = Seasons.SPRING;
            Wind_Directions current_wind_direction;
            string weathertype = "clear";

            if (DateTime.Now.Month >= 3 && DateTime.Now.Month <= 5)
                current_season = Seasons.SPRING;
            else if (DateTime.Now.Month >= 6 && DateTime.Now.Month <= 8)
                current_season = Seasons.SUMMER;
            else if (DateTime.Now.Month >= 9 && DateTime.Now.Month <= 11)
                current_season = Seasons.AUTUMN;
            else if (DateTime.Now.Month >= 12 || DateTime.Now.Month <= 2)
                current_season = Seasons.WINTER;

            current_wind_direction = (Wind_Directions)Enum.Parse(typeof(Wind_Directions), this.Windinfo.WindDirection);

            if (this.AirpressureInfoList.Count > 0)
            {
                if (this.AirpressureInfoList.First().Airpressure > this.AirpressureInfoList.Last().Airpressure)
                    weathertype = this.pressure_dropping_table[(int)current_wind_direction, (int)current_season];
                else
                    weathertype = this.pressure_rising_table[(int)current_wind_direction, (int)current_season];
            }

            this.WeathertypeIconUrl = String.Format("http://icons.wxug.com/i/c/a/{0}.gif", weathertype);
        }
    }
}