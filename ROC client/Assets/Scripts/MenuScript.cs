using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    private GameObject gpsMenu;
    private GameObject gyroscopeObject;
    private GameObject movementObject;

    private bool gyroscopeToggle = true;
    private bool movementToggle = true;

    // Use this for initialization
    void Start () {
        movementObject = GameObject.Find("MovementObject");
        gyroscopeObject = GameObject.Find("PositionObject");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GpsActivate(bool activate)
    {
    }

    public void GyroscopeActivate(bool activate)
    {
        gyroscopeToggle = !gyroscopeToggle;
        gyroscopeObject.SetActive(gyroscopeToggle);
    }

    public void MovementActivate(bool activate)
    {
        movementToggle = !movementToggle;
        movementObject.SetActive(movementToggle);
    }
}
