using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace WeatherMeUp.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, LocationManagerDelegate, WeatherManagerDelegate
	{
		LocationManager locationManager = new LocationManager ();
		WeatherManager weatherManager;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			LoadApplication (new App ());

			System.Diagnostics.Debug.WriteLine ("MA FUT PE XAMARIN!");
			locationManager.startLocationServices ();
			locationManager.dlg = this;

			return base.FinishedLaunching (app, options);
		}

		public void locationManagerDidUpdateLocation (LocationManager manager) {
			System.Diagnostics.Debug.WriteLine ("Long: {0}, Lat: {1}", manager.longitude, manager.latitude);

			weatherManager = new WeatherManager (manager.latitude, manager.longitude);
			weatherManager.dlg = this;

			manager.stopLocationServices ();
		}

		public void weatherManagerDidUpdate (WeatherManager manager) {
			System.Diagnostics.Debug.WriteLine ("Morning conditions: " + manager.weatherDict [WeatherManager.kMorningKey].condition);
			System.Diagnostics.Debug.WriteLine ("Afternoon conditions: " + manager.weatherDict [WeatherManager.kAfternoonKey].condition);
			System.Diagnostics.Debug.WriteLine ("Evening conditions: " + manager.weatherDict [WeatherManager.kEveningKey].condition);
		}
	}
}

