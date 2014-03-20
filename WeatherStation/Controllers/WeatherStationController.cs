using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WeatherStation.Database;
using WeatherStation.Models;

namespace WeatherStation.Controllers
{
    public class WeatherStationController : Controller
    {
        [HandleError(ExceptionType = typeof(MySqlException), View = "DatabaseError")]
        public ActionResult TemperatureGraph()
        {
            WeatherInfoSet weatherinfoset = new WeatherInfoSet()
            { 
                PullDownInfoList =  { new SelectListItem() { Text = "Temperature", Value = Url.Action("TemperatureGraph", "WeatherStation") },
                                      new SelectListItem() { Text = "Humidity", Value = Url.Action("HumidityGraph", "WeatherStation") },
                                      new SelectListItem() { Text = "Air Pressure", Value = Url.Action("AirpressureGraph", "WeatherStation") } }
            };

            WeatherStationDatabase weatherdatabase = new WeatherStationDatabase();

            try
            {
                weatherdatabase.OpenDatabase();
                weatherdatabase.SetTemperatureData(weatherinfoset);
                weatherdatabase.SetHumidityData(weatherinfoset);
                weatherdatabase.SetAirpressureData(weatherinfoset);
                weatherdatabase.SetImageData(weatherinfoset);
            }
            catch (MySqlException ex)
            {
                throw (ex);
            }

            ViewBag.WeatherUndergroundAvailable = true;

            try
            {
                weatherdatabase.SetWindData(weatherinfoset);
            }
            catch (System.Net.WebException)
            {
                ViewBag.WeatherUndergroundAvailable = false;
            }

            if (ViewBag.WeatherUndergroundAvailable)
                weatherinfoset.UpdateWeatherType();

            try
            {
                weatherdatabase.CloseDatabase();
            }
            catch (MySqlException ex)
            {
                throw (ex);
            }

            Response.AddHeader("Refresh", "60");
            
            return View(weatherinfoset);
        }

        [HandleError(ExceptionType = typeof(MySqlException), View = "DatabaseError")]
        public ActionResult HumidityGraph()
        {
            WeatherInfoSet weatherinfoset = new WeatherInfoSet()
            {
                PullDownInfoList =  { new SelectListItem() { Text = "Humidity", Value = Url.Action("HumidityGraph", "WeatherStation") },
                                      new SelectListItem() { Text = "Temperature", Value = Url.Action("TemperatureGraph", "WeatherStation") },
                                      new SelectListItem() { Text = "Air Pressure", Value = Url.Action("AirpressureGraph", "WeatherStation") } }
            };

            WeatherStationDatabase weatherdatabase = new WeatherStationDatabase();

            try
            {
                weatherdatabase.OpenDatabase();
                weatherdatabase.SetTemperatureData(weatherinfoset);
                weatherdatabase.SetHumidityData(weatherinfoset);
                weatherdatabase.SetAirpressureData(weatherinfoset);
                weatherdatabase.SetImageData(weatherinfoset);
            }
            catch (MySqlException ex)
            {
                throw (ex);
            }

            ViewBag.WeatherUndergroundAvailable = true;

            try
            {
                weatherdatabase.SetWindData(weatherinfoset);
            }
            catch (System.Net.WebException)
            {
                ViewBag.WeatherUndergroundAvailable = false;
            }

            if (ViewBag.WeatherUndergroundAvailable)
                weatherinfoset.UpdateWeatherType();

            try
            {
                weatherdatabase.CloseDatabase();
            }
            catch (MySqlException ex)
            {
                throw (ex);
            }

            Response.AddHeader("Refresh", "60");

            return View(weatherinfoset);
        }

        [HandleError(ExceptionType = typeof(MySqlException), View = "DatabaseError")]
        public ActionResult AirpressureGraph()
        {
            WeatherInfoSet weatherinfoset = new WeatherInfoSet()
            {
                PullDownInfoList =  { new SelectListItem() { Text = "Air Pressure", Value = Url.Action("AirpressureGraph", "WeatherStation") },
                                      new SelectListItem() { Text = "Humidity", Value = Url.Action("HumidityGraph", "WeatherStation") },
                                      new SelectListItem() { Text = "Temperature", Value = Url.Action("TemperatureGraph", "WeatherStation") } },
            };

            WeatherStationDatabase weatherdatabase = new WeatherStationDatabase();

            try
            {
                weatherdatabase.OpenDatabase();
                weatherdatabase.SetTemperatureData(weatherinfoset);
                weatherdatabase.SetHumidityData(weatherinfoset);
                weatherdatabase.SetAirpressureData(weatherinfoset);
                weatherdatabase.SetImageData(weatherinfoset);
            }
            catch (MySqlException ex)
            {
                throw (ex);
            }

            ViewBag.WeatherUndergroundAvailable = true;

            try
            {
                weatherdatabase.SetWindData(weatherinfoset);
            }
            catch (System.Net.WebException)
            {
                ViewBag.WeatherUndergroundAvailable = false;
            }

            if (ViewBag.WeatherUndergroundAvailable)
                weatherinfoset.UpdateWeatherType();

            try
            {
                weatherdatabase.CloseDatabase();
            }
            catch (MySqlException ex)
            {
                throw (ex);
            }

            Response.AddHeader("Refresh", "60");

            return View(weatherinfoset);
        }

    }
}
