using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace WeatherMeUp.Droid
{
	[Activity (Label = "WeatherMeUp.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, DroidLocationManagerDelegate, WeatherManagerDelegate
	{
		DroidLocationManager locationManager;
		WeatherManager weatherManager;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			
			LoadApplication (new App ());

			locationManager = new DroidLocationManager(this);
			locationManager.startLocationServices ();
			locationManager.dlg = this;
		}

		public void locationManagerDidUpdateLocation (DroidLocationManager manager) {
			System.Diagnostics.Debug.WriteLine ("Lat: {0}, Long: {1}", manager.location.Latitude, manager.location.Longitude);

			weatherManager = new WeatherManager (manager.location.Latitude, manager.location.Longitude);
			weatherManager.dlg = this;

			manager.stopLocationServices ();
		}

		public void weatherManagerDidUpdate (WeatherManager manager) {
			System.Diagnostics.Debug.WriteLine ("Morning conditions: " + manager.weatherDict [WeatherManager.kMorningKey].condition);
			System.Diagnostics.Debug.WriteLine ("Afternoon conditions: " + manager.weatherDict [WeatherManager.kAfternoonKey].condition);
			System.Diagnostics.Debug.WriteLine ("Evening conditions: " + manager.weatherDict [WeatherManager.kEveningKey].condition);


			System.Diagnostics.Debug.WriteLine ("Morning Time: {0}", manager.weatherDict [WeatherManager.kMorningKey].date);
			System.Diagnostics.Debug.WriteLine ("AN Time: {0}", manager.weatherDict [WeatherManager.kAfternoonKey].date);
			System.Diagnostics.Debug.WriteLine ("EV Time: {0}", manager.weatherDict [WeatherManager.kEveningKey].date);
		}
	}
}

