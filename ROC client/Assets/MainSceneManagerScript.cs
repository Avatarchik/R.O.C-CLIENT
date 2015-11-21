using UnityEngine;
using System.Collections;

public class MainSceneManagerScript : MonoBehaviour {

    bool ingameMenuToggle = false;
    GameObject gameMenu;

	// Use this for initialization
	void Start () {
        gameMenu = GameObject.Find("Menu");
        gameMenu.SetActive(ingameMenuToggle);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M)) // M button in key setup
        {
            ingameMenuToggle = !ingameMenuToggle;
            gameMenu.SetActive(ingameMenuToggle);
        }
      //print(Input.GetAxis("Horizontal"));
    }
}
