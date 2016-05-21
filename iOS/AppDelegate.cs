using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace WeatherMeUp.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, LocationManagerDelegate
	{
		LocationManager locationManager = new LocationManager ();

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
		}
	}
}

