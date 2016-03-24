using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour {

    //List of GameObjects needed
    private Toggle movementToggle;

    private float XAngle = 0.0F;
    private float YAngle = 0;

    private void Start()
    {
        movementToggle = GameObject.Find("MovementToggle").GetComponent<Toggle>();
    }

    public void RotateVehicleModel(float x)
    {
        XAngle = XAngle + x;

        if (movementToggle.isOn == true)
            GameObject.Find("MovementVehicleRawImage").GetComponent<Transform>().transform.eulerAngles = new Vector3(0.0f, 0.0f, XAngle);
    }

    public void RotateCameraModel(float y)
    {
        YAngle -= y;

        if (movementToggle.isOn == true)
            GameObject.Find("MovementCameraRawImage").GetComponent<Transform>().transform.eulerAngles = new Vector3(0.0f, 0.0f, YAngle);
    }
}
