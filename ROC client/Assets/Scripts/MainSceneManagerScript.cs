using UnityEngine;


public class MainSceneManagerScript : MonoBehaviour {

    //List of GameObjects needed
    private GameObject gameMenu;
    private GameObject gpsObject;

    private bool ingameMenuToggle = false;
    private bool gpsToggle = false;

    private void Start () {
        gameMenu = GameObject.Find("Menu");
        gameMenu.SetActive(ingameMenuToggle);
        gpsObject = GameObject.Find("GPSObject");
        gpsObject.SetActive(gpsToggle);
    }

    //Functions checking that the menu button is not called
    private void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) { // Escape button in key setup
            ingameMenuToggle = !ingameMenuToggle;
            gameMenu.SetActive(ingameMenuToggle);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            gpsToggle = !gpsToggle;
            gpsObject.SetActive(gpsToggle);
        }
    }

    public void ChangeToMenuScene()
    {
        Application.LoadLevel("MenuScene");
    }

}