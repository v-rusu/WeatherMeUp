using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace WeatherMeUp
{

	public interface WeatherManagerDelegate {
		void weatherManagerDidUpdate (WeatherManager manager);
	}

	public class WeatherData {
		public double tempMin;
		public double tempMax;
		public string condition;
		public DateTime date;
	}

	public class WeatherManager
	{
		static public string kMorningKey = "kMorningKey";
		static public string kAfternoonKey = "kAfternoonKey";
		static public string kEveningKey = "kEveningKey";

		public WeatherManagerDelegate dlg;

		public Dictionary<String, WeatherData> weatherDict = new Dictionary<String, WeatherData>();

		public WeatherManager (double lat, double lon)
		{
			string url = "http://api.openweathermap.org/data/2.5/forecast?lat=" + lat + "&lon=" + lon + "&appid=a09b9ff27d6b70ff1306cb519afe2e15";

			// HTTP web request
			var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.Method = "GET";
			httpWebRequest.BeginGetResponse(new AsyncCallback(GetResponseCallback), httpWebRequest);
		}

		void GetResponseCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				HttpWebRequest myrequest = (HttpWebRequest)asynchronousResult.AsyncState;
				using (HttpWebResponse response = (HttpWebResponse)myrequest.EndGetResponse(asynchronousResult))
				{
					System.IO.Stream responseStream = response.GetResponseStream();
					using (var reader = new System.IO.StreamReader(responseStream))
					{
						string data = reader.ReadToEnd();
						System.Diagnostics.Debug.WriteLine (data);
						JObject weatherObject = JsonConvert.DeserializeObject(data) as JObject;
						JArray weatherDataList = weatherObject["list"] as JArray;

						weatherDict.Add(kMorningKey, weatherDataFromJObject(weatherDataList[3] as JObject));
						weatherDict.Add(kAfternoonKey, mergeWeatherData(weatherDataFromJObject(weatherDataList[4] as JObject), weatherDataFromJObject(weatherDataList[5] as JObject)));
						weatherDict.Add(kEveningKey, weatherDataFromJObject(weatherDataList[6] as JObject));

						dlg.weatherManagerDidUpdate(this);
					}

					responseStream.Dispose();
				}
			}
			catch (Exception e)
			{
				//FUCK THE Exception
			}
		}

		public WeatherData weatherDataFromJObject(JObject obj) {
			return new WeatherData {
				tempMin = (double)obj["main"]["temp_min"],
				tempMax = (double)obj["main"]["temp_max"],
				condition = (string)obj["weather"][0]["main"],
				date = FromUnixTime((long)obj["dt"])
			};
		}

		public WeatherData mergeWeatherData(WeatherData wd1, WeatherData wd2) {
			return new WeatherData {
				tempMin = Math.Min(wd1.tempMin, wd2.tempMin),
				tempMax = Math.Max(wd1.tempMax, wd2.tempMax),
				condition = wd2.condition,
				date = wd2.date
			};
		}

		public DateTime FromUnixTime(long unixTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddSeconds(unixTime);
		}
	}
}

