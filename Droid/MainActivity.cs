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
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, DroidLocationManagerDelegate
	{
		DroidLocationManager locationManager;

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
		}
	}
}

