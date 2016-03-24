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
        GetComponent<Text>().text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    }


}
