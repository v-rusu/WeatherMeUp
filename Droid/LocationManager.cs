using System;
using System.Collections.Generic;
using System.Linq;

using Android.Locations;
using Android.OS;
using Android.Content;

namespace WeatherMeUp.Droid
{
	public interface DroidLocationManagerDelegate {
		void locationManagerDidUpdateLocation (DroidLocationManager manager);
	}

	public class DroidLocationManager: Java.Lang.Object, ILocationListener
	{
		private LocationManager locationManager;
		private Context ctx;

		public DroidLocationManagerDelegate dlg;

		public Location location;

		public DroidLocationManager ()
		{
			
		}

		public DroidLocationManager (Context ctx)
		{
			this.ctx = ctx;
		}
			
		public void startLocationServices() {
			locationManager = ctx.GetSystemService (Context.LocationService) as LocationManager;

			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

			String locationProvider;
			if (acceptableLocationProviders.Any ()) {
				locationProvider = acceptableLocationProviders.First ();
			} else {
				locationProvider = "";
			}

			locationManager.RequestLocationUpdates (locationProvider, 0, 0, this);
		}

		public void stopLocationServices() {
			locationManager.RemoveUpdates (this);
		}

		public void OnLocationChanged (Location location) {
			this.location = location;

			dlg.locationManagerDidUpdateLocation(this);
		}

		public void OnProviderDisabled (string provider) {}
		public void OnProviderEnabled (string provider) {}
		public void OnStatusChanged (string provider, Availability status, Bundle extras) {}
	}
}

