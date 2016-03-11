using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DataTransferScript : MonoBehaviour {

    Toggle gpsToggle;

    // Use this for initialization
    void Start () {
   }
	
    void Awake()
    {
        gpsToggle = GameObject.Find("GpsToggle").GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void SendMenuGps()
    {
        print(gpsToggle.isOn);
    }
}
