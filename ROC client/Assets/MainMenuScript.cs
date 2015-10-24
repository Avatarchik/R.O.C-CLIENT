using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PassToMainScene()
    {
        if (GameObject.Find("PortField").GetComponent<InputField>().text != "" && GameObject.Find("IpField").GetComponent<InputField>().text != "")
            Application.LoadLevel("MainScene");

    }
}
