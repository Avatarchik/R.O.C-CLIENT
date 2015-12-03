using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngine.UI;
using OpenCvSharp;

public class MainSceneManagerScript : MonoBehaviour {

    bool ingameMenuToggle = false;
    GameObject gameMenu;
    Mat testMat;
    Texture2D testTexture;

    // Use this for initialization
    void Start () {
        testTexture = new Texture2D(1980, 1024);

        gameMenu = GameObject.Find("Menu");
        gameMenu.SetActive(ingameMenuToggle);
        testImageOpencv();
    }

    void testImageOpencv()
    {
        testTexture = new Texture2D(1980, 1024);
        testMat = new Mat(Application.dataPath + "/testpattern.png", ImreadModes.Unchanged);
        if (testMat.Empty()) //check whether the image is loaded or not
        {
            print("Fail to load image");
        }
        byte[] imgdata = testMat.ToBytes(".png");
        testTexture.LoadImage(imgdata);
        GameObject.Find("Image").GetComponent<Image>().sprite = Sprite.Create(testTexture, new UnityEngine.Rect(0, 0, 1920, 1080), Vector2.zero);
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
