using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour {

    private float rotationSpeed = 0.5F;

    private float XAngle = 0.0F;
    private float YAngle = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RotateVehicleModel(Input.GetAxis("VerticalMovement") * rotationSpeed);
        RotateCameraModel(Input.GetAxis("Horizontal") * rotationSpeed);
    }

    public void RotateVehicleModel(float x)
    {
        XAngle = XAngle + x;

        GameObject.Find("MovementVehicleRawImage").GetComponent<Transform>().transform.eulerAngles = new Vector3(0.0f, 0.0f, XAngle);
    }

    public void RotateCameraModel(float y)
    {
        YAngle -= y;

        GameObject.Find("MovementCameraRawImage").GetComponent<Transform>().transform.eulerAngles = new Vector3(0.0f, 0.0f, YAngle);
    }
}
