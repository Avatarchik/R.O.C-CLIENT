using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    GameObject gpsMenu;
    GameObject gyroscopeMenu;
    GameObject movementMenu;

    public bool gyroscopeToggle = true;
    public bool movementToggle = true;

    // Use this for initialization
    void Start () {
        movementMenu = GameObject.Find("MovementObject");
        gyroscopeMenu = GameObject.Find("PositionObject");
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
        gyroscopeMenu.SetActive(gyroscopeToggle);
    }

    public void MovementActivate(bool activate)
    {
        movementToggle = !movementToggle;
        movementMenu.SetActive(movementToggle);
    }
}
