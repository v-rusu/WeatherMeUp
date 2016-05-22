using System;
using CoreLocation;
using Foundation;

namespace WeatherMeUp.iOS
{
	public interface LocationManagerDelegate {
		void locationManagerDidUpdateLocation (LocationManager manager);
	}

	public class LocationManager: CLLocationManagerDelegate
	{
		private CLLocationManager locationManager;

		public LocationManagerDelegate dlg;

		public double latitude;
		public double longitude;

		public LocationManager ()
		{
		}

		public void startLocationServices() {
			if (null == locationManager) {
				locationManager = new CLLocationManager();
			}

			locationManager.RequestWhenInUseAuthorization ();

			locationManager.Delegate = this;
			locationManager.DesiredAccuracy = CLLocation.AccuracyKilometer;
			locationManager.DistanceFilter = 500;

			locationManager.StartUpdatingLocation ();
		}

		public void stopLocationServices() {
			locationManager.StopUpdatingLocation();
		}

		override public void LocationsUpdated (CLLocationManager manager, CLLocation[] locations) {
			CLLocation location = locations.GetValue (locations.Length - 1) as CLLocation;

			latitude = location.Coordinate.Latitude;
			longitude = location.Coordinate.Longitude;

			dlg.locationManagerDidUpdateLocation (this);
		}
	}
}

