using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using WeatherStation.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Drawing;


namespace WeatherStation.Database
{
    public class WeatherStationDatabase
    {
        private MySql.Data.MySqlClient.MySqlConnection Mycon;

        public void OpenDatabase()
        {
            Mycon = new MySqlConnection("server=eu-cdbr-azure-west-b.cloudapp.net;user id=bc156d741860ea; password=0b704a62;" +
                                        "database=ictweataqjswbn7u; Pooling=true; Min Pool Size=0; Max Pool Size=3;");

            if (Mycon.State != ConnectionState.Open)
            {
                try
                {
                    Mycon.Open();
                }
                catch (MySqlException ex)
                {
                    throw (ex);
                }
            }
        }

        public void CloseDatabase()
        {
            if (Mycon.State == ConnectionState.Open)
            {
                try
                {
                    Mycon.Close();
                }
                catch (MySqlException ex)
                {
                    throw (ex);
                }
            }
        }

        public void SetWindData(WeatherInfoSet weatherinfoset)
        {
            string ApiKey = "3e36ad78b6a9da44";
            String url = String.Format("http://api.wunderground.com/api/{0}/conditions/q/Netherlands/Eelde.json", ApiKey);
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var json = reader.ReadToEnd();
                    JObject searchResult = JObject.Parse(json);
                    string wind_direction = searchResult["current_observation"]["wind_dir"].ToString();

                    weatherinfoset.Windinfo = new WindInfo() { WindDirection = wind_direction };                    
                }
            }
        }

        public void SetTemperatureData(WeatherInfoSet weatherinfoset)
        {
            TimeSpan span = new TimeSpan(0, 3, 0, 0, 0);

            DateTime now = DateTime.Now.ToUniversalTime().Subtract(span);

            string datetimeframe_start_str = now.ToString("yyyy") + '-' + now.ToString("MM") + '-' + now.ToString("dd") +
                                             ' ' + now.ToString("HH") + ':' + now.ToString("mm") + ':' + now.ToString("ss");

            string query = String.Format("SELECT * FROM temperaturedata WHERE datetime >= '{0}'", datetimeframe_start_str);

            MySqlCommand myCommand = new MySqlCommand(query);
   
            myCommand.Connection = Mycon;

            MySqlDataReader reader = myCommand.ExecuteReader();

            while (reader.Read())
            {
                DateTime date = (DateTime)reader[0];
                float temperature = (float)reader[1];
                
                // JavaScript Date constructor accepts number of milliseconds since Unix epoch (1 January 1970 00:00:00 UTC). 
                long datetimemin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                long datetime = (date.ToUniversalTime().Ticks - datetimemin) / 10000;

                weatherinfoset.TemperatureInfoList.Add(new TemperatureInfo() { Temperature = temperature, DateTime = datetime });
            }

            weatherinfoset.JsonTemperatureInfoList = JsonConvert.SerializeObject(weatherinfoset.TemperatureInfoList);

            reader.Close();
        }

        public void SetHumidityData(WeatherInfoSet weatherinfoset)
        {
            TimeSpan span = new TimeSpan(0, 3, 0, 0, 0);

            DateTime now = DateTime.Now.ToUniversalTime().Subtract(span);

            string datetimeframe_start_str = now.ToString("yyyy") + '-' + now.ToString("MM") + '-' + now.ToString("dd") +
                                             ' ' + now.ToString("HH") + ':' + now.ToString("mm") + ':' + now.ToString("ss");

            string query = String.Format("SELECT * FROM humiditydata WHERE datetime >= '{0}'", datetimeframe_start_str);

            MySqlCommand myCommand = new MySqlCommand(query);

            myCommand.Connection = Mycon;

            MySqlDataReader reader = myCommand.ExecuteReader();

            while (reader.Read())
            {
                DateTime date = (DateTime)reader[0];
                float humidity = (float)reader[1];

                // JavaScript Date constructor accepts number of milliseconds since Unix epoch (1 January 1970 00:00:00 UTC). 
                long datetimemin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                long datetime = (date.ToUniversalTime().Ticks - datetimemin) / 10000;

                weatherinfoset.HumidityInfoList.Add(new HumidityInfo() { Humidity = humidity, DateTime = datetime });
            }

            weatherinfoset.JsonHumidityInfoList = JsonConvert.SerializeObject(weatherinfoset.HumidityInfoList);

            reader.Close();
        }

        public void SetAirpressureData(WeatherInfoSet weatherinfoset)
        {
            TimeSpan span = new TimeSpan(0, 3, 0, 0, 0);

            DateTime now = DateTime.Now.ToUniversalTime().Subtract(span);

            string datetimeframe_start_str = now.ToString("yyyy") + '-' + now.ToString("MM") + '-' + now.ToString("dd") +
                                             ' ' + now.ToString("HH") + ':' + now.ToString("mm") + ':' + now.ToString("ss");

            string query = String.Format("SELECT * FROM airpressuredata WHERE datetime >= '{0}'", datetimeframe_start_str);

            MySqlCommand myCommand = new MySqlCommand(query);
            myCommand.Connection = Mycon;

            MySqlDataReader reader = myCommand.ExecuteReader();

            while (reader.Read())
            {
                DateTime date = (DateTime)reader[0];
                float airpressure = (float)reader[1];

                // JavaScript Date constructor accepts number of milliseconds since Unix epoch (1 January 1970 00:00:00 UTC). 
                long datetimemin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                long datetime = (date.ToUniversalTime().Ticks - datetimemin) / 10000;

                weatherinfoset.AirpressureInfoList.Add(new AirpressureInfo() { Airpressure = airpressure, DateTime = datetime });
            }

            weatherinfoset.JsonAirpressureInfoList = JsonConvert.SerializeObject(weatherinfoset.AirpressureInfoList);

            reader.Close();
        }

        public void SetImageData(WeatherInfoSet weatherinfoset)
        {
            string query = String.Format("SELECT image FROM imagedata");

            MySqlCommand myCommand = new MySqlCommand(query);
            myCommand.Connection = Mycon;

            MySqlDataReader reader = myCommand.ExecuteReader();

            while (reader.Read())
            {
                long bytesize = reader.GetBytes(0, 0, null, 0, 0);
                byte[] ImageBytes = new byte[bytesize];
                reader.GetBytes(0, 0, ImageBytes, 0, (int)bytesize);
                weatherinfoset.WeatherImageURL = "data:image/jpg;base64," + Convert.ToBase64String(ImageBytes);
            }

            reader.Close();
        }
    }
}