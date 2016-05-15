using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FpsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

    float deltaTime = 0.0f;

    // Update is called once per frame
    void Update () {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        GameObject.Find("FPSText").GetComponent<Text>().text = string.Format("FPS : {0:0.}", fps);
        GameObject.Find("MSText").GetComponent<Text>().text = string.Format("MS : {0:0.0}", msec);
    }


}
