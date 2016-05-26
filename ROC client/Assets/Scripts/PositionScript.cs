using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PositionScript : MonoBehaviour {

    //List of GameObjects needed
    private Toggle positionToggle;

    private string XAngleText = "X : ";
    private float XAngle = 180.0F;
    private string YAngleText = "Y : ";
    private float YAngle = 0;

    private void Start() {
        positionToggle = GameObject.Find("PositionToggle").GetComponent<Toggle>();
    }

    public void RotateModel(float x, float y)
    {
        XAngle = XAngle + x;
        YAngle = YAngle + y;

        if (positionToggle.isOn == true)
        {
            GameObject.Find("Position3DObject").GetComponent<Transform>().transform.eulerAngles = new Vector3(XAngle, YAngle, 0.0f);
            GameObject.Find("XText").GetComponent<Text>().text = XAngleText + (XAngle - 180).ToString();
            GameObject.Find("YText").GetComponent<Text>().text = YAngleText + (YAngle).ToString();
        }
    }

    public float getXAngle()
    {
        return XAngle;
    }

    public float getYAngle()
    {
        return YAngle;
    }

}
