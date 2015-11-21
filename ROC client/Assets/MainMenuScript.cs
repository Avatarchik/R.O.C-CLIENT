using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

    NetworkScript networkScript;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        networkScript = GameObject.Find("ROCNetworkManager").GetComponent<NetworkScript>();
        networkScript.HandleTasks();
    }

    //TODO alex_m : Add a check that the board can be accessed via a ping
    public void PassToMainScene()
    {
        if (GameObject.Find("PortField").GetComponent<InputField>().text != "" && GameObject.Find("IpField").GetComponent<InputField>().text != "")
        {
            networkScript.SetUpNetwork();
            Application.LoadLevel("MainScene");
        }
    }
}
