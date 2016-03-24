using UnityEngine;


public class MainSceneManagerScript : MonoBehaviour {

    //List of GameObjects needed
    private GameObject gameMenu;

    bool ingameMenuToggle = false;

    private void Start () {
        gameMenu = GameObject.Find("Menu");
        gameMenu.SetActive(ingameMenuToggle);
    }

    //Functions checking that the menu button is not called
    private void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) { // Escape button in key setup
            ingameMenuToggle = !ingameMenuToggle;
            gameMenu.SetActive(ingameMenuToggle);
        }
    }

    public void ChangeToMenuScene()
    {
        Application.LoadLevel("MenuScene");
    }

}