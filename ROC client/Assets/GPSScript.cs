using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GPSScript : MonoBehaviour {

    //List of GameObjects needed
    private Toggle gpsToggle;

    private float latitude;
    private float longitude;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetGpsMap(float latitude, float longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshGpsPosition()
    {
        GoogleMap googleMap;

        if ((googleMap = GameObject.Find("GPSRawImage").GetComponent<GoogleMap>()) == null)
            return;
        Debug.Log("Recentering google map : x = " + latitude + ", y = " + longitude);
        googleMap.centerLocation.latitude = this.latitude;
        googleMap.centerLocation.longitude = this.longitude;
        googleMap.mapType = GoogleMap.MapType.RoadMap;

        GoogleMapLocation home = new GoogleMapLocation();
        home.latitude = (float) 48.85052;
        home.longitude = (float) 2.346743;

        GoogleMapLocation[] mapLocations = new GoogleMapLocation[1];
        mapLocations[0] = home;      

        GoogleMapMarker add = new GoogleMapMarker();
        add.color = GoogleMapColor.red;
        add.size = GoogleMapMarker.GoogleMapMarkerSize.Mid;
        add.locations = mapLocations;

        GoogleMapMarker[] mapMarker = new GoogleMapMarker[1];
        mapMarker[0] = add;

        googleMap.markers = mapMarker;
    }
}
