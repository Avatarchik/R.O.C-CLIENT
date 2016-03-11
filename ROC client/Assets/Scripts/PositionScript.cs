using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PositionScript : MonoBehaviour {

    private float rotationSpeed = 0.5F;

    private string XAngleText = "X : ";
    private float XAngle = 180.0F;
    private string YAngleText = "Y : ";
    private float YAngle = 0;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        RotateModel(Input.GetAxis("Vertical") * rotationSpeed, Input.GetAxis("Horizontal") * rotationSpeed);
    }

    public void RotateModel(float x, float y)
    {
        XAngle = XAngle + x;
        YAngle = YAngle + y;

        if (GameObject.Find("Menu"))

        GameObject.Find("Position3DObject").GetComponent<Transform>().transform.eulerAngles = new Vector3(XAngle, YAngle, 0.0f);
        GameObject.Find("XText").GetComponent<Text>().text = XAngleText + (XAngle - 180).ToString();
        GameObject.Find("YText").GetComponent<Text>().text = YAngleText + (YAngle).ToString();
    }
}
