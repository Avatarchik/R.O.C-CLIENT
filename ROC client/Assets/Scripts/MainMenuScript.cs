using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ProgressBar;

public class MainMenuScript : MonoBehaviour {

    NetworkScript networkScript;
    GameObject loadingPanel;
    AsyncOperation async = null;

    // Use this for initialization
    void Start () {
        networkScript = GameObject.Find("ROCNetworkManager").GetComponent<NetworkScript>();
        loadingPanel = GameObject.Find("LoadingPanel");
        loadingPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
    }

    //TODO alex_m : Add a check that the board can be accessed via a ping
    public void PassToMainScene()
    {
        if (GameObject.Find("PortField").GetComponent<InputField>().text != "" && GameObject.Find("IpField").GetComponent<InputField>().text != "")
        {
            if (networkScript.SetUpNetwork() == -1)
                return;
            StartCoroutine("loading");
        }
    }

   private IEnumerator loading()
    {
        loadingPanel.SetActive(true);
        async = Application.LoadLevelAsync("MainScene");
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress < 0.9f)
            {
                GameObject.Find("ProgressBarLabelAbove").GetComponent<ProgressBarBehaviour>().Value = async.progress * 100.0f;
            }
            else
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
