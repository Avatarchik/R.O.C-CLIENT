using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GPSScript : MonoBehaviour {

    //List of GameObjects needed
    private Toggle gpsToggle;

    private float latitude;
    private float longitude;

    private GoogleMapLocation[] homeMapLocations = new GoogleMapLocation[1];
    private GoogleMapLocation[] goMapLocations = new GoogleMapLocation[1];
    private GoogleMapMarker[] mapMarker = new GoogleMapMarker[2];


    private CommunicationManagerScript comScript;
    // Use this for initialization
    void Start () {
        goMapLocations[0] = null;
        mapMarker[1] = null;
        comScript = GameObject.Find("ManagerObject").GetComponent<CommunicationManagerScript>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
            DisplayClickCoordinates();
    }

    public void InitGps()
    {
        GoogleMap googleMap;

        if ((googleMap = GameObject.Find("GPSRawImage").GetComponent<GoogleMap>()) == null)
            return;
        googleMap.mapType = GoogleMap.MapType.RoadMap;

        GoogleMapMarker homeMarkerType = new GoogleMapMarker();
        homeMarkerType.color = GoogleMapColor.red;
        homeMarkerType.size = GoogleMapMarker.GoogleMapMarkerSize.Mid;
        homeMarkerType.locations = homeMapLocations;

        mapMarker[0] = homeMarkerType;
        googleMap.markers = mapMarker;
    }

    public void SetGpsMap(float latitude, float longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;

        GoogleMap googleMap;

        if ((googleMap = GameObject.Find("GPSRawImage").GetComponent<GoogleMap>()) == null)
            return;
        Debug.Log("Recentering google map : x = " + latitude + ", y = " + longitude);
        googleMap.centerLocation.latitude = this.latitude;
        googleMap.centerLocation.longitude = this.longitude;

        GoogleMapLocation homeLocation = new GoogleMapLocation();
        homeLocation.latitude = this.latitude;
        homeLocation.longitude = this.longitude;
        homeLocation.address = "";

        homeMapLocations[0] = homeLocation;
    }
    
    public void DisplayClickCoordinates()
    {
        GoogleMap googleMap;

        Debug.Log("x : " + Input.mousePosition.x + " - y : " + Input.mousePosition.y);
        if ((googleMap = GameObject.Find("GPSRawImage").GetComponent<GoogleMap>()) == null)
            return;

        float lat = googleMap.centerLocation.latitude;
        float lng = googleMap.centerLocation.longitude;
        int zoom = googleMap.zoom;
        int width = Screen.width;
        int height = Screen.height;
        float mouseX = Screen.width - Input.mousePosition.x;
        float mouseY = Screen.height - Input.mousePosition.y;

        float x, y, s, tiles, centerPointx, centerPointy, mousePointx, mousePointy, mouseLat, mouseLng;
        x = mouseX - (width / 2);
        y = mouseY - (height / 2);
        s = (float) Math.Min(Math.Max(Math.Sin(lat * (((float)Math.PI) / 180)), -.9999), .9999);
        tiles = 1 << zoom;

        centerPointx = 128 + lng * (256 / 360);
        centerPointy = (float)(128 + 0.5 * Math.Log((1 + s) / (1 - s)) * -(256 / (2 * Math.PI)));
        mousePointx = (centerPointx * tiles) + x;
        mousePointy = (centerPointy * tiles) + y;
        mouseLat = (float) ((2 * Math.Atan(Math.Exp(((mousePointy / tiles) - 128) / -(256 / (2 * Math.PI)))) - Math.PI / 2) / (Math.PI / 180));
        float between1 = (mousePointx / tiles) - 128;
        float between3 = 0.71111111111111111111111111111111111f;
        mouseLng = (between1 / between3);
        mouseLng = (float)((2 * Math.Atan(Math.Exp(((mousePointx / tiles) - 128) / -(256 / (2 * Math.PI)))) - Math.PI / 2) / (Math.PI / 180)) + googleMap.centerLocation.longitude;
        Debug.Log("xfinal = " + mouseLat + " yfinal = "  + mouseLng + " - " + between1 + " - " + between3);
        addMarker(mouseLat, mouseLng);
        RefreshGps();

        comScript.SendCoordinates(x, y);
    }

    private void addMarker(float x, float y)
    {
        GoogleMapLocation add = new GoogleMapLocation();
        add.latitude = x;
        add.longitude = y;
        add.address = "";

        goMapLocations[0] = add;

        GoogleMapMarker homeMarkerType = new GoogleMapMarker();
        homeMarkerType.color = GoogleMapColor.green;
        homeMarkerType.size = GoogleMapMarker.GoogleMapMarkerSize.Small;
        homeMarkerType.locations = goMapLocations;

        mapMarker[1] = homeMarkerType;


    }

    public void RefreshGps()
    {
       GoogleMap googleMap;

        if ((googleMap = GameObject.Find("GPSRawImage").GetComponent<GoogleMap>()) == null)
            return;
        googleMap.Refresh();
    }
}
