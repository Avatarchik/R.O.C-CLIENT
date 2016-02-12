using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngine.UI;

public class MainSceneManagerScript : MonoBehaviour {

    bool ingameMenuToggle = false;
    GameObject gameMenu;
    Texture2D testTexture;

    // Use this for initialization
    void Start () {
        testTexture = new Texture2D(1980, 1024);

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
    }
}
